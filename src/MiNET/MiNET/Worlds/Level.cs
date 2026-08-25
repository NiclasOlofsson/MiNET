#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using fNbt;
using log4net;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Entities;
using MiNET.Entities.Hostile;
using MiNET.Entities.Passive;
using MiNET.Entities.World;
using MiNET.Items;
using MiNET.Net;
using MiNET.Sounds;
using MiNET.Utils;
using MiNET.Utils.Diagnostics;
using MiNET.Utils.IO;
using MiNET.Utils.Nbt;
using MiNET.Utils.Vectors;

namespace MiNET.Worlds
{
	public class Level : IBlockAccess, ILevelMetricsSource
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Level));

		public static readonly BlockCoordinates Up = new BlockCoordinates(0, 1, 0);
		public static readonly BlockCoordinates Down = new BlockCoordinates(0, -1, 0);
		public static readonly BlockCoordinates South = new BlockCoordinates(0, 0, 1);
		public static readonly BlockCoordinates North = new BlockCoordinates(0, 0, -1);
		public static readonly BlockCoordinates East = new BlockCoordinates(1, 0, 0);
		public static readonly BlockCoordinates West = new BlockCoordinates(-1, 0, 0);

		public IWorldProvider WorldProvider { get; set; }


		public PlayerLocation SpawnPoint { get; set; } = null;

		public ConcurrentDictionary<long, Player> Players { get; private set; } = new ConcurrentDictionary<long, Player>();

//TODO: Need to protect this, not threadsafe
		public ConcurrentDictionary<long, Entity> Entities { get; private set; } = new ConcurrentDictionary<long, Entity>();

//TODO: Need to protect this, not threadsafe
		public List<BlockEntity> BlockEntities { get; private set; } = new List<BlockEntity>();

//TODO: Need to protect this, not threadsafe
		public ConcurrentDictionary<BlockCoordinates, long> BlockWithTicks { get; private set; } = new ConcurrentDictionary<BlockCoordinates, long>();

//TODO: Need to protect this, not threadsafe
		public string LevelId { get; private set; }

		public string LevelName { get; private set; }
		public Dimension Dimension { get; set; } = Dimension.Overworld;

		public GameMode GameMode { get; private set; }
		public bool IsSurvival => GameMode == GameMode.Survival;
		public bool HaveDownfall { get; set; }
		public Difficulty Difficulty { get; set; }
		public bool AutoSmelt { get; set; } = false;
		/// <summary>Ticks on the level clock. Stored by <see cref="Clock" />; this is a shorthand for it.</summary>
		public long WorldTime
		{
			get => Clock.Time;
			set => Clock.Time = value;
		}

		/// <summary>Position within the current day, derived from <see cref="Clock" />.</summary>
		public long CurrentWorldCycleTime => Clock.TimeOfDay;

		/// <summary>Age of the world in ticks. Unrelated to the clock: it never pauses and never wraps.</summary>
		public long TickTime { get; set; }
		public int SkylightSubtracted { get; set; }
		public long StartTimeInTicks { get; private set; }
		public bool EnableBlockTicking { get; set; } = false;
		public bool EnableChunkTicking { get; set; } = false;

		public bool AllowBuild { get; set; } = true;
		public bool AllowBreak { get; set; } = true;

		public EntityManager EntityManager { get; protected set; }
		public InventoryManager InventoryManager { get; protected set; }
		public EntitySpawnManager EntitySpawnManager { get; protected set; }

		/// <summary>The level's clock. One per level; it reads and writes <see cref="WorldTime" />.</summary>
		public WorldClock Clock { get; protected set; }

		public int ViewDistance { get; set; }

		/// <summary>
		///     Entity relevance radius in blocks, 2D. Movement broadcast is culled to recipients
		///     within this distance of the mover; 0 disables culling and keeps the legacy
		///     all-to-all broadcast path untouched. Per level: level types and plugins set it
		///     here, the config key EntityRelevanceRadius only seeds the default.
		/// </summary>
		public int EntityRelevanceRadius { get; set; } = Config.GetProperty("EntityRelevanceRadius", 0);

		public Random Random { get; private set; }

		public int SaveInterval { get; set; } = 300;
		public int UnloadInterval { get; set; } = -1;

		// What StartGame tells a joining client about this world. The defaults are the values
		// vanilla BDS 1.26.34 sends, taken from a decoded capture. Zero is not a neutral default
		// for most of them: it is a smaller tick range, an unlimited world of size zero, and a
		// broadcast mode the client does not expect.
		public long Seed { get; set; } = 12345;

		// 2 = infinite, which is what vanilla reports even for a flat world. 1 is "flat", and the
		// client uses this to decide how it treats the world edge.
		public int GeneratorType { get; set; } = 2;

		// Vanilla names the spawn biome. An empty string leaves the client with no biome for the
		// point it spawns at.
		public string SpawnBiomeName { get; set; } = "minecraft:plains";
		public short SpawnBiomeType { get; set; }

		public bool AchievementsDisabled { get; set; } = true;
		public float RainLevel { get; set; }
		public float LightningLevel { get; set; }
		public bool IsMultiplayer { get; set; } = true;
		public bool BroadcastToLan { get; set; } = true;
		// Enum-typed, not int: the client rejects a broadcast setting outside GamePublishSetting.
		public LevelSettings.GamePublishSetting XboxLiveBroadcastMode { get; set; } = LevelSettings.GamePublishSetting.Nomultiplay;
		public LevelSettings.GamePublishSetting PlatformBroadcastMode { get; set; } = LevelSettings.GamePublishSetting.Nomultiplay;
		public bool UseMsaGamertagsOnly { get; set; } = true;
		public bool IsTexturepacksRequired { get; set; }
		public bool BonusChest { get; set; }
		public bool MapEnabled { get; set; }
		public bool IsTrial { get; set; }
		public int ServerChunkTickRange { get; set; } = 4;
		public int LimitedWorldWidth { get; set; } = 16;
		public int LimitedWorldLength { get; set; } = 16;
		public int MovementRewindHistorySize { get; set; } = 40;
		public int EnchantmentSeed { get; set; } = 123456;

		public Level(LevelManager levelManager, string levelId, IWorldProvider worldProvider, EntityManager entityManager, GameMode gameMode = GameMode.Survival, Difficulty difficulty = Difficulty.Normal, int viewDistance = 11)
		{
			Random = new Random();

			LevelManager = levelManager;
			EntityManager = entityManager;
			InventoryManager = new InventoryManager(this);
			EntitySpawnManager = new EntitySpawnManager(this);
			Clock = new WorldClock(this);
			LevelId = levelId;
			GameMode = gameMode;
			Difficulty = difficulty;
			ViewDistance = viewDistance;
			WorldProvider = worldProvider;
		}

		public LevelManager LevelManager { get; }
		public Level NetherLevel { get; set; }
		public Level TheEndLevel { get; set; }
		public Level OverworldLevel { get; set; }

		public void Initialize()
		{
			//IsWorldTimeStarted = false;
			WorldProvider.Initialize();

			SpawnPoint = SpawnPoint ?? new PlayerLocation(WorldProvider.GetSpawnPoint());
			TickTime = WorldProvider.GetTime();
			Clock.Time = WorldProvider.GetDayTime();
			LevelName = WorldProvider.GetName();

			// Pre-warming is worth it on a real server, where the cost is paid once and every player
			// arriving at spawn benefits. Switching it off leaves caching itself alone, so chunks
			// are simply cached as they are first read.
			//
			// Backgrounded and parallel, which is the original shape restored twice over: the 2015
			// pre-cache ran fire-and-forget on the thread pool and startup never waited for it,
			// a property silently lost in the dimension rework; and the load path is single-thread
			// ~1ms a column while every other core idles at startup. A player joining before the
			// warm finishes just loads their columns on demand, same as with warming off.
			if (WorldProvider.IsCaching && Config.GetProperty("PreWarmChunks", true))
			{
				var centre = new ChunkCoordinates(SpawnPoint) / 8;
				int radius = ViewDistance;

				Task.Run(() =>
				{
					try
					{
						Stopwatch chunkLoading = Stopwatch.StartNew();

						var disc = new List<ChunkCoordinates>();
						int radiusSquared = radius * radius;
						for (int x = -radius; x <= radius; x++)
						{
							for (int z = -radius; z <= radius; z++)
							{
								if (x * x + z * z > radiusSquared) continue;

								disc.Add(new ChunkCoordinates(centre.X + x, centre.Z + z));
							}
						}

						int loaded = 0;
						Parallel.ForEach(disc, coordinates =>
						{
							ChunkColumn column = GetChunk(coordinates);
							if (column == null) return;

							// Warms the active delivery mode's seed and its blobs beside the
							// column itself; the packet has to go back, nobody is listening.
							// Warms what a join will send, so the seed built here is the one reused.
							column.CreateCachedPushChunk().PutPool();
							Interlocked.Increment(ref loaded);
						});

						Log.Info($"World pre-cache {loaded} chunks completed in {chunkLoading.ElapsedMilliseconds}ms");
					}
					catch (Exception e)
					{
						Log.Error($"World pre-cache failed for {LevelId}", e);
					}
				});
			}

			if (Dimension == Dimension.Overworld)
			{
				if (Config.GetProperty("CheckForSafeSpawn", true))
				{
					// Snap the spawn to the ground exactly. GetHeight is the first air block, which is
					// where feet belong: SpawnPoint is a feet position like KnownPosition, and the eye
					// offset is added only on the wire. Clearance would be a drop at every join, and a
					// tolerance band would let a spawn below the surface stand.
					var height = GetHeight((BlockCoordinates) SpawnPoint);
					if (height > 0 && SpawnPoint.Y != height) SpawnPoint.Y = height;
					Log.Debug($"Checking for safe spawn, ground height {height}, spawn Y {SpawnPoint.Y}");
				}

				if (LevelManager != null && WorldProvider.HaveNether())
				{
					NetherLevel = LevelManager.GetDimension(this, Dimension.Nether);
				}
				if (LevelManager != null && WorldProvider.HaveTheEnd())
				{
					TheEndLevel = LevelManager.GetDimension(this, Dimension.TheEnd);
				}
			}

			//SpawnPoint.Y = 20;

			StartTimeInTicks = DateTime.UtcNow.Ticks;

			// Registered here rather than in the constructor: the tags read Dimension and
			// WorldProvider, and both are settled by the time a level starts ticking, not by the time
			// it is constructed.
			_metricTags = new TagList {{"levelType", LevelType}, {"dimension", DimensionName}};
			_levelTickTags = new TagList {{"level", LevelId}};
			EngineMetrics.RegisterLevel(this);

			_tickTimer = new Stopwatch();
			_tickTimer.Restart();

			if (EnableLevelTicking) _tickerHighPrecisionTimer = new HighPrecisionTimer(50, WorldTick, false, false);
			else Log.Warn($"Level {LevelId} runs without a tick: no clock, no block or entity ticking, and no Player.OnTick.");
		}

		/// <summary>
		///     Whether this level runs the 50ms world tick at all. On for any world where anything
		///     happens.
		///     <para>
		///         Off is for a level that is only a place to stand: nothing grows, nothing moves,
		///         nothing is saved. It also stops <see cref="Player.OnTick" />, and with it hunger,
		///         effects, portal detection, popups and the adaptive chunk radius, so a level that
		///         turns this off owns whatever periodic work it still needs.
		///     </para>
		/// </summary>
		public bool EnableLevelTicking { get; set; } = Config.GetProperty("EnableLevelTicking", true);

		private TagList _metricTags;
		private TagList _levelTickTags;

		/// <inheritdoc />
		public string LevelType => WorldProvider?.GetType().Name ?? "None";

		/// <inheritdoc />
		public string DimensionName => Dimension.ToString();

		/// <inheritdoc />
		public int MetricPlayerCount => PlayerCount;

		/// <inheritdoc />
		public int MetricEntityCount => Entities.Count;

		private void _tickerHighPrecisionTimer_Tick()
		{
			WorldTick(null);
		}

		private HighPrecisionTimer _tickerHighPrecisionTimer;

		public virtual void Close()
		{
			WorldProvider?.SaveChunks();

			NetherLevel?.Close();
			TheEndLevel?.Close();

			_tickerHighPrecisionTimer?.Dispose();
			_tickerHighPrecisionTimer = null;

			_relevanceMatrix?.Dispose();
			_relevanceMatrix = null;
			_relevanceSlots.Clear();
			_relevanceSlotOwners.Clear();

			EngineMetrics.UnregisterLevel(this);

			foreach (var entity in Entities.Values.ToArray())
			{
				entity.DespawnEntity();
			}

			Entities.Clear();

			foreach (Player player in Players.Values.ToArray())
			{
				player.Disconnect("Unexpected player lingering on close of level: " + player.Username);
			}

			Players.Clear();

			BlockEntities.Clear();

			BlockWithTicks.Clear();
			BlockWithTicks = null;
			BlockEntities = null;
			Players = null;
			Entities = null;

			if (WorldProvider is AnvilWorldProvider provider)
			{
				foreach (var chunk in provider._chunkCache)
				{
					provider._chunkCache.TryRemove(chunk.Key, out var waste);
					if (waste == null) continue;

					foreach (var c in waste)
					{
						c.PutPool();
					}

					waste.ClearCache();
				}
			}

			WorldProvider = null;

			Log.Info("Closed level: " + LevelId);
		}

		/// <summary>
		///     Packs one already-encoded packet into its own wrapper. PrepareSend merges everything
		///     queued between flushes into a single wrapper, so this is how a packet is kept as its
		///     own payload. Public because plugins need it for the same reason the server does.
		/// </summary>
		public static McpeWrapper CreateMcpeBatch(ReadOnlyMemory<byte> bytes)
		{
			return BatchUtils.CreateBatchPacket(bytes, CompressionLevel.Optimal, true);
		}

		public static McpeWrapper CreateMcpeBatch(ReadOnlySequence<byte> bytes)
		{
			return BatchUtils.CreateBatchPacket(bytes, CompressionLevel.Optimal, true);
		}

		private object _playerWriteLock = new object();

		public virtual void AddPlayer(Player newPlayer, bool spawn)
		{
			if (newPlayer.Username == null) throw new ArgumentNullException(nameof(newPlayer.Username));

			EntityManager.AddEntity(newPlayer);

			lock (_playerWriteLock)
			{
				if (!newPlayer.IsConnected)
				{
					Log.Error("Tried to add player that was already disconnected.");
					return;
				}

				if (Players.TryAdd(newPlayer.EntityId, newPlayer))
				{
					foreach (Entity entity in Entities.Values.ToArray())
					{
						entity.SpawnToPlayers(new[] {newPlayer});
					}

					SpawnToAll(newPlayer);
				}

				newPlayer.IsSpawned = spawn;
			}

			OnPlayerAdded(new LevelEventArgs(newPlayer, this));
		}

		public event EventHandler<LevelEventArgs> PlayerAdded;

		protected virtual void OnPlayerAdded(LevelEventArgs e)
		{
			PlayerAdded?.Invoke(this, e);
		}

		public event EventHandler<LevelEventArgs> PlayerRemoved;

		protected virtual void OnPlayerRemoved(LevelEventArgs e)
		{
			PlayerRemoved?.Invoke(this, e);
		}

		public void SpawnToAll(Player newPlayer)
		{
			lock (_playerWriteLock)
			{
				// The player list keeps us from moving this completely to player.
				// It's simply to slow and bad.

				// AddPlayer has already put the joiner in Players, so this is everyone else.
				// Reading the level's players and then appending the joiner (as this used to)
				// listed them twice, and every join sent a roster with one duplicate record.
				Player[] others = GetAllPlayers().Where(p => p != newPlayer).ToArray();

				// The roster the joiner gets, self first then everyone else. Order is what vanilla
				// BDS 1.26.34 sends: a second bot joining an occupied server receives [self, other],
				// not the level's own ordering (verified with two clients against BDS).
				var roster = new List<Player>(others.Length + 1) {newPlayer};
				roster.AddRange(others);


				// Encoded and compressed here, on this thread, so the send lane that services
				// the session has nothing to do but ship bytes. It still keeps its place in the
				// sequence: PrepareSend closes the pending batch before it queues a finished wrapper.
				// That order is not cosmetic. A roster that overtakes StartGame is dropped, and the
				// joining player then sees the others in the world with no rows in the player list.
				//
				// Assembled from each player's cached record slices as a segment chain, compressed
				// straight from the chain: a full roster costs three pointer segments per player
				// instead of a re-serialization of every record, and the contiguous multi-megabyte
				// encode a large roster used to require never exists. Batched because the client
				// refuses a player list past 1000 records (see MaxRecordsPerPacket) with a packet
				// violation that kills the connection.
				foreach (ReadOnlySequence<byte> rosterBatch in PlayerListRosterBuilder.BuildAddedBatches(roster))
				{
					newPlayer.SendPacket(CreateMcpeBatch(rosterBatch));
				}

				// One record, the joiner, to everyone already connected. This is how another client
				// learns their skin, so it cannot be skipped: AddPlayer below only references the
				// identity, it does not carry an appearance.
				var playerList = McpePlayerList.CreateObject();
				playerList.records = McpePlayerList.Added(newPlayer);
				RelayBroadcast(newPlayer, roster.ToArray(), CreateMcpeBatch(playerList.EncodeAsMemory()));
				playerList.PutPool();

				// The entity halves only when relevance culling is off. Under culling the joiner
				// enters the matrix on the next tick and the entered transitions spawn exactly
				// the pairs in range, both directions. The player list above stays global either
				// way; the client needs the roster record (skin, identity) regardless of distance.
				if (EntityRelevanceRadius == 0)
				{
					newPlayer.SpawnToPlayers(others);

					foreach (Player spawnedPlayer in others)
					{
						spawnedPlayer.SpawnToPlayers(new[] {newPlayer});
					}
				}
			}
		}

		public virtual void RemovePlayer(Player player, bool despawn = true)
		{
			if (Players == null) return; // Closing down the level sets players to null;
			if (Entities == null) return; // Closing down the level sets players to null;

			lock (_playerWriteLock)
			{
				Player removed;
				if (Players.TryRemove(player.EntityId, out removed))
				{
					player.IsSpawned = false;
					if (despawn) DespawnFromAll(player);

					foreach (Entity entity in Entities.Values.ToArray())
					{
						entity.DespawnFromPlayers(new[] {removed});
					}

					// Returns the skin-store refcount this player's cached roster record holds.
					player.InvalidateRosterSlices();
				}
			}

			OnPlayerRemoved(new LevelEventArgs(player, this));
		}

		public void DespawnFromAll(Player player)
		{
			lock (_playerWriteLock)
			{
				var spawnedPlayers = GetAllPlayers();

				// The leaver's own screen is always cleaned: on a level transfer this is what
				// removes the old level's players from their client (removes for entities that
				// were never spawned there are ignored).
				foreach (Player spawnedPlayer in spawnedPlayers)
				{
					spawnedPlayer.DespawnFromPlayers(new[] {player});
				}

				// The survivors' half only when relevance culling is off. Under culling the next
				// tick's roster sync reads the leaver's row and despawns them from exactly the
				// clients that hold the entity.
				if (EntityRelevanceRadius == 0) player.DespawnFromPlayers(spawnedPlayers);

				McpePlayerList playerListMessage = McpePlayerList.CreateObject();
				playerListMessage.records = McpePlayerList.Removed(spawnedPlayers);
				player.SendPacket(CreateMcpeBatch(playerListMessage.EncodeAsMemory()));
				playerListMessage.records = null;
				playerListMessage.PutPool();

				McpePlayerList playerList = McpePlayerList.CreateObject();
				playerList.records = McpePlayerList.Removed(player);
				RelayBroadcast(player, CreateMcpeBatch(playerList.EncodeAsMemory()));
				playerList.records = null;
				playerList.PutPool();
			}
		}

		public void AddEntity(Entity entity)
		{
			lock (Entities)
			{
				EntityManager.AddEntity(entity);

				if (Entities.TryAdd(entity.EntityId, entity))
				{
					entity.SpawnToPlayers(GetAllPlayers());
				}
				else
				{
					throw new Exception("Entity existed in the players list when it should not");
				}
			}
		}

		public void RemoveEntity(Entity entity)
		{
			lock (Entities)
			{
				if (!Entities.TryRemove(entity.EntityId, out entity)) return; // It's ok. Holograms destroy this play..
				entity.DespawnFromPlayers(GetAllPlayers());
			}
		}


		/// <summary>
		///     A name is one seat: a new login under a name evicts whoever already holds it. The
		///     usual case is a crashed or stale session whose sweep has not fired yet - the
		///     returning player IS the same person, and making them wait out the sweep timeout
		///     serves nobody. Matched on name alone and guarded by reference identity: ClientIds
		///     are not unique across clients (the bot fleet reuses them run to run), so they can
		///     distinguish nothing.
		/// </summary>
		public void RemoveDuplicatePlayers(Player newPlayer)
		{
			foreach (Player existing in GetAllPlayers())
			{
				if (ReferenceEquals(existing, newPlayer)) continue;
				if (!newPlayer.Username.Equals(existing.Username, StringComparison.InvariantCultureIgnoreCase)) continue;

				Log.Info($"Evicting existing session for {existing.Username} on new login");
				existing.Disconnect("You logged in from another location.", false);
			}
		}

		public virtual void BroadcastTitle(string text, TitleType type = TitleType.Title, int fadeIn = 6, int fadeOut = 6, int stayTime = 20, Player sender = null, Player[] sendList = null)
		{
			var mcpeSetTitle = McpeSetTitle.CreateObject();
			mcpeSetTitle.fadeInTime = fadeIn;
			mcpeSetTitle.stayTime = stayTime;
			mcpeSetTitle.fadeOutTime = fadeOut;
			mcpeSetTitle.type = (int) type;
			mcpeSetTitle.text = text;

			RelayBroadcast(sender, sendList, mcpeSetTitle);
		}

		public virtual void BroadcastMessage(string text, MessageType type = MessageType.Chat, Player sender = null, Player[] sendList = null, bool needsTranslation = false, string[] parameters = null)
		{
			if (type == MessageType.Chat || type == MessageType.Raw)
			{
				foreach (var line in text.Split(new string[] {"\n", Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries))
				{
					McpeText message = McpeText.CreateObject();
					message.type = (byte) type;
					message.source = sender == null ? "" : sender.Username;
					message.message = line;
					message.needsTranslation = needsTranslation;
					message.parameters = parameters;
					RelayBroadcast(sendList, message);
				}
			}
			else
			{
				McpeText message = McpeText.CreateObject();
				message.type = (byte) type;
				message.source = sender == null ? "" : sender.Username;
				message.message = text;
				message.needsTranslation = needsTranslation;
				message.parameters = parameters;
				RelayBroadcast(sendList, message);
			}
		}

		private object _tickSync = new object();
		private Stopwatch _tickTimer = new Stopwatch();
		public long LastTickProcessingTime = 0;
		public long AvarageTickProcessingTime = 50;
		public int PlayerCount { get; private set; }

		public Profiler _profiler = new Profiler();

		private void WorldTick(object sender)
		{
			//if (_tickTimer.ElapsedMilliseconds < 40 && LastTickProcessingTime < 50)
			//{
			//	if (Log.IsDebugEnabled) Log.Warn($"World tick came too fast: {_tickTimer.ElapsedMilliseconds} ms");
			//	return;
			//}

			// Not debug-gated: a late world tick is a real service event (every player perceives it
			// as a global stall), and it self-rate-limits to at most one line per tick.
			if (_tickTimer.ElapsedMilliseconds >= 65) Log.Warn($"Time between world tick too long: {_tickTimer.ElapsedMilliseconds} ms. Last processing time={LastTickProcessingTime}, Avarage={AvarageTickProcessingTime}");

			// Drift, not duration: how late this tick STARTED against its 50ms schedule. A slow timer
			// and a slow tick body are opposite faults, and one duration number conflates them.
			EngineMetrics.RecordTickLag(_tickTimer.Elapsed.TotalMilliseconds - 50, _metricTags);

			Measurement worldTickMeasurement = _profiler.Begin("World tick");

			_tickTimer.Restart();

			try
			{
				TickTime++;

				Player[] players = GetSpawnedPlayers();

				Clock.Tick();

				// Vanilla's cadence: one clock sync every 256 ticks, and no SetTime at all. BDS
				// 1.26.34 never sends SetTime, running clock or frozen.
				if (!Clock.Paused && TickTime % 256 == 0)
				{
					Clock.BroadcastState();
				}

				SkylightSubtracted = CalculateSkylightSubtracted(WorldTime);

				// Save dirty chunks
				if (TickTime % (SaveInterval * 20) == 0)
				{
					long saveStartedAt = Stopwatch.GetTimestamp();
					WorldProvider.SaveChunks();
					EngineMetrics.RecordSave(saveStartedAt, _metricTags);
				}

				// Unload chunks not needed
				if (UnloadInterval > 0 && TickTime % (UnloadInterval * 20) == 0)
				{
					var cacheProvider = WorldProvider as ICachingWorldProvider;
					int removed = cacheProvider?.UnloadChunks(players, (ChunkCoordinates) (BlockCoordinates) SpawnPoint, ViewDistance) ?? 0;
					if (removed > 0) Log.Warn($"Unloaded {removed} chunks, {cacheProvider?.GetCachedChunks().Length} chunks remain cached");
				}

				var blockAndChunkTickMeasurement = worldTickMeasurement?.Begin("Block and chunk tick");

				Entity[] entities = Entities.Values.OrderBy(e => e.EntityId).ToArray();
				if (EnableChunkTicking || EnableBlockTicking)
				{
					if (EnableChunkTicking) EntitySpawnManager.DespawnMobs(TickTime);

					List<EntitySpawnManager.SpawnState> chunksWithinRadiusOfPlayer = new List<EntitySpawnManager.SpawnState>();
					foreach (var player in players)
					{
						BlockCoordinates bCoord = (BlockCoordinates) player.KnownPosition;

						chunksWithinRadiusOfPlayer = GetChunkCoordinatesForTick(new ChunkCoordinates(bCoord), chunksWithinRadiusOfPlayer, 17, Random); // Should actually be 15
					}

					if (chunksWithinRadiusOfPlayer.Count > 0)
					{
						bool canSpawnPassive = false;
						bool canSpawnHostile = false;

						if (DoMobspawning)
						{
							canSpawnPassive = TickTime % 400 == 0;

							var effectiveChunkCount = Math.Max(17 * 17, chunksWithinRadiusOfPlayer.Count);
							int entityPassiveCount = 0;
							int entityHostileCount = 0;
							foreach (var entity in entities)
							{
								if (entity is PassiveMob)
								{
									entityPassiveCount++;
								}
								else if (entity is HostileMob)
								{
									entityHostileCount++;
								}
							}


							var passiveCap = EntitySpawnManager.CapPassive * (effectiveChunkCount / 289f);
							canSpawnPassive = canSpawnPassive && entityPassiveCount < passiveCap;
							canSpawnPassive = canSpawnPassive || entityPassiveCount < passiveCap * 0.20; // Custom to get instant spawn when no mobs
							canSpawnHostile = entityHostileCount < EntitySpawnManager.CapHostile * (effectiveChunkCount / 289f);
						}

						var state = chunksWithinRadiusOfPlayer;

						Parallel.ForEach(state, spawnState =>
						{
							Random random = new Random(spawnState.Seed);

							ChunkColumn chunk = GetChunk(new ChunkCoordinates(spawnState.ChunkX, spawnState.ChunkZ), true);
							if (chunk == null) return; // Not loaded

							if (DoMobspawning)
							{
								int x = random.Next(16);
								int z = random.Next(16);

								var height = chunk.GetHeight(x, z);

								var chunkTickMeasurement = blockAndChunkTickMeasurement?.Begin("Chunk tick");

								var maxValue = (((height + 1) >> 4) + 1) * 16 - 1;
								var ySpawn = random.Next(maxValue);
								var spawnCoordinates = new BlockCoordinates(x + spawnState.ChunkX * 16, ySpawn, z + spawnState.ChunkZ * 16);
								var spawnBlock = GetBlock(spawnCoordinates, chunk);
								if (spawnBlock.IsTransparent)
								{
									// Entity spawning, only one attempt per chunk
									EntitySpawnManager.AttemptMobSpawn(spawnCoordinates, random, canSpawnPassive, canSpawnHostile);
								}

								chunkTickMeasurement?.End();
							}

							if (EnableBlockTicking && RandomTickSpeed > 0)
							{
								for (int s = 0; s < 16; s++)
								{
									for (int i = 0; i < RandomTickSpeed; i++)
									{
										int x = random.Next(16);
										int y = random.Next(16);
										int z = random.Next(16);

										var blockTickMeasurement = blockAndChunkTickMeasurement?.Begin("Block tick");

										var blockCoordinates = new BlockCoordinates(x + spawnState.ChunkX * 16, y + s * 16, z + spawnState.ChunkZ * 16);
										var block = GetBlock(blockCoordinates, chunk);
										//Stopwatch sw = Stopwatch.StartNew();
										block.OnTick(this, true);
										//if(sw.ElapsedMilliseconds > 50)
										//{
										//	if (Log.IsDebugEnabled) Log.Warn($"Took a long time ({sw.ElapsedMilliseconds}) with block tick on {block}");
										//}
										blockTickMeasurement?.End();
									}
								}
							}
						});
					}
				}

				blockAndChunkTickMeasurement?.End();

				var blockUpdateMeasurement = worldTickMeasurement?.Begin("Block update tick");

				// Block updates
				foreach (KeyValuePair<BlockCoordinates, long> blockEvent in BlockWithTicks)
				{
					try
					{
						if (blockEvent.Value <= TickTime)
						{
							if (BlockWithTicks.TryRemove(blockEvent.Key, out _)) GetBlock(blockEvent.Key).OnTick(this, false);
						}
					}
					catch (Exception e)
					{
						Log.Warn("Block ticking", e);
					}
				}

				blockUpdateMeasurement?.End();

				var blockEntityMeasurement = worldTickMeasurement?.Begin("Block entity tick");
				// Block entity updates
				foreach (BlockEntity blockEntity in BlockEntities.ToArray())
				{
					blockEntity.OnTick(this);
				}

				blockEntityMeasurement?.End();

				var entityMeasurement = worldTickMeasurement?.Begin("Entity tick");

				// Entity updates
				foreach (Entity entity in entities)
				{
					entity.OnTick(entities);
				}

				entityMeasurement?.End();

				PlayerCount = players.Length;

				// Player tick
				var playerMeasurement = worldTickMeasurement?.Begin("Player tick");

				foreach (var player in players)
				{
					if (player.IsSpawned) player.OnTick(entities);
				}

				playerMeasurement?.End();

				// Send player movements
				BroadCastMovement(players, entities);

				if (Log.IsDebugEnabled && _tickTimer.ElapsedMilliseconds >= 50) Log.Error($"World tick too too long: {_tickTimer.ElapsedMilliseconds} ms");
			}
			catch (Exception e)
			{
				Log.Error("World ticking", e);
			}
			finally
			{
				LastTickProcessingTime = _tickTimer.ElapsedMilliseconds;
				AvarageTickProcessingTime = (AvarageTickProcessingTime * 9 + _tickTimer.ElapsedMilliseconds) / 10L;

				// MSPT, twice: aggregated by level type for percentiles across the server, and by level
				// identity so one misbehaving world is nameable rather than merely visible.
				double tickMillis = _tickTimer.Elapsed.TotalMilliseconds;
				EngineMetrics.RecordTick(tickMillis, _metricTags);
				EngineMetrics.RecordLevelTick(tickMillis, _levelTickTags);

				worldTickMeasurement?.End();
			}
		}

		public int GetSubtractedLight(BlockCoordinates coordinates)
		{
			return GetSubtractedLight(coordinates, SkylightSubtracted);
		}

		public int GetSubtractedLight(BlockCoordinates coordinates, int amount)
		{
			var skyLight = GetSkyLight(coordinates) - amount;
			var blockLight = GetBlockLight(coordinates);

			return (int) Math.Max(skyLight, blockLight);
		}

		public int CalculateSkylightSubtracted(long worldTime)
		{
			float f = CalculateCelestialAngle(worldTime);
			double f1 = 1.0F - (Math.Cos(f * ((float) Math.PI * 2F)) * 2.0F + 0.5F);
			f1 = BiomeUtils.Clamp((float) f1, 0.0F, 1.0F);
			f1 = 1.0F - f1;
			//f1 = (float)((double)f1 * (1.0D - (double)(this.getRainStrength(p_72967_1_) * 5.0F) / 16.0D));
			//f1 = (float)((double)f1 * (1.0D - (double)(this.getThunderStrength(p_72967_1_) * 5.0F) / 16.0D));
			f1 = 1.0F - f1;
			return (int) (f1 * 11.0F);
		}

		public float CalculateCelestialAngle(long worldTime)
		{
			int i = (int) (worldTime % 24000L);
			float f = ((float) i) / 24000.0F - 0.25F;

			if (f < 0.0F)
			{
				++f;
			}

			if (f > 1.0F)
			{
				--f;
			}

			float f1 = 1.0F - (float) ((Math.Cos((double) f * Math.PI) + 1.0D) / 2.0D);
			f = f + (f1 - f) / 3.0F;
			return f;
		}

		public Player[] GetSpawnedPlayers()
		{
			if (Players == null) return new Player[0]; // HACK

			return Players.Values.Where(player => player.IsSpawned).ToArray();
		}

		public Player[] GetAllPlayers()
		{
			if (Players == null) return new Player[0]; // HACK

			return Players.Values.ToArray();
		}

		public Entity[] GetEntites()
		{
			lock (Entities)
			{
				return Entities.Values.ToArray();
			}
		}

		private IEnumerable<Player> GetStaledPlayers(Player[] players)
		{
			DateTime now = DateTime.UtcNow;
			TimeSpan span = TimeSpan.FromSeconds(300);
			return players.Where(player => (now - player.LastUpdatedTime) > span);
		}

		private DateTime _lastSendTime = DateTime.UtcNow;
		private DateTime _lastBroadcast = DateTime.UtcNow;

		// One identity per level for the move roster's CoalesceKey: every roster batch wholly
		// supersedes the previous one (it carries fresh positions for every mover), so a send lane
		// that still holds two may drop the older unsent. Per level, not global, so two levels'
		// rosters can never supersede each other in a lane serving a player mid-transfer.
		private readonly object _moveRosterCoalesceKey = new object();

		// Relevance culling state, only alive while EntityRelevanceRadius > 0. The matrix and the
		// per-tick scratch collections belong to the tick thread. Coalesce keys are per group
		// hash and persist across ticks, so a lane can supersede last tick's batch for the same
		// audience with this tick's.
		// Relevance cells are 16 blocks. Coarser cells mean fewer groups but a wider band of
		// early/late spawns around the radius; a chunk-sized cell keeps that band at most one
		// chunk while collapsing a crowded area to tens of groups.
		private const int RelevanceCellBits = 4;

		private static float QuantizeToCellCenter(float coordinate)
		{
			return ((((int) coordinate) >> RelevanceCellBits) << RelevanceCellBits) + (1 << (RelevanceCellBits - 1));
		}

		private RelevanceMatrix _relevanceMatrix;
		private readonly Dictionary<Player, int> _relevanceSlots = new Dictionary<Player, int>();
		private readonly Dictionary<int, Player> _relevanceSlotOwners = new Dictionary<int, Player>();
		private readonly Dictionary<ulong, object> _relevanceCoalesceKeys = new Dictionary<ulong, object>();
		private readonly HashSet<Player> _relevanceRosterScratch = new HashSet<Player>();
		private readonly List<Player> _relevanceGoneScratch = new List<Player>();
		private readonly List<Player> _relevanceViewerScratch = new List<Player>();
		private readonly List<(Player Player, int Slot)> _relevanceMoverScratch = new List<(Player, int)>();
		private readonly Dictionary<ulong, List<Player>> _relevanceGroups = new Dictionary<ulong, List<Player>>();
		private readonly List<(List<Player> Members, object CoalesceKey)> _relevanceGroupWork = new List<(List<Player>, object)>();
		private int[] _relevancePlayerSlotScratch = Array.Empty<int>();
		private ulong[] _relevanceHashScratch = Array.Empty<ulong>();
		private Packet[] _relevanceMoverPackets = Array.Empty<Packet>();

		// Groups persist across ticks: at walking speed a player crosses a 16-block cell every
		// ~3.7s, so the group structure is near-identical tick to tick and rebuilding it 20
		// times a second is waste. The rebuild fires on movement, not on a timer: when any
		// player has drifted more than a cell from where they stood at the last regroup (only
		// cell crossings can change the structure), or immediately when the roster changes (a
		// departed player must leave the member lists before the next send).
		private const float RegroupDeviationBlocks = 16f;
		private float[] _relevanceAnchorX = Array.Empty<float>();
		private float[] _relevanceAnchorZ = Array.Empty<float>();
		private bool _relevanceForceRegroup;
		private readonly Dictionary<Player, List<Player>> _relevanceSpawnScratch = new Dictionary<Player, List<Player>>();
		private readonly Dictionary<Player, List<Player>> _relevanceDespawnScratch = new Dictionary<Player, List<Player>>();
		private readonly Stack<List<Player>> _relevanceGroupPool = new Stack<List<Player>>();

		protected virtual void BroadCastMovement(Player[] players, Entity[] entities)
		{
			DateTime now = DateTime.UtcNow;

			// The culled path runs ahead of the legacy early-outs on purpose: its roster sync
			// (slot allocation, departure despawns) must run even when zero or one player is
			// left, or the last leaver's slot never frees and their entity lingers on screen.
			if (EntityRelevanceRadius > 0)
			{
				BroadCastMovementCulled(players, now);
				return;
			}

			if (_relevanceMatrix != null)
			{
				// Radius went back to 0 at runtime: drop the culling state so a later re-enable
				// starts from a clean matrix instead of a stale roster. Players that were culled
				// at the moment of the switch stay unspawned on each other's clients until they
				// cross paths with a rejoin or level change; the legacy path assumes global
				// spawn and never repairs it.
				_relevanceMatrix.Dispose();
				_relevanceMatrix = null;
				_relevanceSlots.Clear();
				_relevanceSlotOwners.Clear();
				_relevanceCoalesceKeys.Clear();
				_relevanceGroups.Clear();
				_relevanceGroupWork.Clear();
				_relevanceForceRegroup = true;
			}

			if (players.Length == 0) return;

			if (players.Length <= 1 && entities.Length == 0) return;

			//if (now - _lastBroadcast < TimeSpan.FromMilliseconds(50)) return;

			DateTime lastSendTime = _lastSendTime;
			_lastSendTime = DateTime.UtcNow;

			// Stamped before the roster is walked, so broadcast.build covers everything the tick thread
			// spends here: building the records, compressing them, and encoding the wrapper.
			long buildStartedAt = Stopwatch.GetTimestamp();

			//using (MemoryStream stream = new MemoryStream())
			{
				int playerMoveCount = 0;
				int entiyMoveCount = 0;

				List<Packet> movePackets = new List<Packet>();

				foreach (var player in players)
				{
					if (now - player.LastUpdatedTime <= now - lastSendTime)
					{
						var knownPosition = (PlayerLocation) player.KnownPosition.Clone();

						var move = McpeMovePlayer.CreateObject();
						move.runtimeEntityId = player.EntityId;
						move.position = new Vector3(knownPosition.X, knownPosition.Y + 1.62f, knownPosition.Z);
						move.rotation = new Vector2(knownPosition.Pitch, knownPosition.Yaw);
						move.headYaw = knownPosition.HeadYaw;
						move.mode = player.Vehicle == 0 ? McpeMovePlayer.PositionMode.Normal : McpeMovePlayer.PositionMode.Onlyheadrot;
						move.onGround = !player.IsGliding && player.IsOnGround;
						move.ridingRuntimeEntityId = player.Vehicle;
						movePackets.Add(move);
						playerMoveCount++;
					}
				}

				//foreach (var entity in entities)
				//{
				//	if (entity.LastUpdatedTime >= lastSendTime)
				//	{
				//		{
				//			McpeMoveEntity moveEntity = McpeMoveEntity.CreateObject();
				//			moveEntity.entityId = entity.EntityId;
				//			moveEntity.position = (PlayerLocation)entity.KnownPosition.Clone();
				//			moveEntity.position.Y += entity.PositionOffset;
				//			byte[] bytes = moveEntity.Encode();
				//			BatchUtils.WriteLength(stream, bytes.Length);
				//			stream.Write(bytes, 0, bytes.Length);
				//			moveEntity.PutPool();
				//		}
				//		{
				//			McpeSetEntityMotion entityMotion = McpeSetEntityMotion.CreateObject();
				//			entityMotion.entityId = entity.EntityId;
				//			entityMotion.velocity = entity.Velocity;
				//			byte[] bytes = entityMotion.Encode();
				//			BatchUtils.WriteLength(stream, bytes.Length);
				//			stream.Write(bytes, 0, bytes.Length);
				//			entityMotion.PutPool();
				//		}
				//		entiyMoveCount++;
				//	}
				//}

				if (playerMoveCount == 0 && entiyMoveCount == 0) return;

				if (players.Length == 1 && entiyMoveCount == 0) return;

				if (movePackets.Count == 0) return;

				//McpeWrapper batch = BatchUtils.CreateBatchPacket(new Memory<byte>(stream.GetBuffer(), 0, (int) stream.Length), CompressionLevel.Optimal, false);
				var batch = McpeWrapper.CreateObject(players.Length);
				batch.CoalesceKey = _moveRosterCoalesceKey;
				batch.SetPayload(Compression.CompressPacketsForWrapper(movePackets));
				batch.EncodeAsMemory();

				// The compressed size is what decides the fragment count, and so the datagram rate:
				// this batch goes to every player, so bytes here multiply by players.Length on the wire.
				EngineMetrics.RecordBroadcast(playerMoveCount + entiyMoveCount, batch.payload.Length, buildStartedAt, _metricTags);

				// Inline on the tick thread: SendPacket only enqueues (NetherNetSession's send lane
				// does the transport work), so a work item per recipient buys no parallelism and
				// costs an allocation and a pool dispatch each.
				foreach (Player player in players) player.SendPacket(batch);
				_lastBroadcast = DateTime.UtcNow;
			}
		}

		/// <summary>
		///     The movement broadcast under relevance culling. One matrix pass over the spawned
		///     roster; the transition stream drives entity spawn/despawn (the player list stays
		///     global, handled at join/leave as always); then recipients grouped by row hash:
		///     everyone with the same visible set shares one compressed batch, so the
		///     one-compression economy of the legacy roster survives per spatial cluster instead
		///     of per level. Transitions are applied BEFORE the movement fan-out so a client
		///     always holds the AddPlayer entity before the first movement for it arrives.
		/// </summary>
		private void BroadCastMovementCulled(Player[] players, DateTime now)
		{
			RelevanceMatrix matrix = _relevanceMatrix ??= new RelevanceMatrix(EntityRelevanceRadius, players.Length);
			matrix.Radius = EntityRelevanceRadius;

			// Sync the slot roster with the spawned players: newcomers get a slot and enter the
			// matrix (their entered pairs after Compute are what spawns them on nearby clients).
			// The departed are despawned from their final audience, read from the row BEFORE the
			// slot is scrubbed, then give the slot back.
			_relevanceRosterScratch.Clear();
			foreach (Player player in players)
			{
				_relevanceRosterScratch.Add(player);
				if (!_relevanceSlots.ContainsKey(player))
				{
					int slot = matrix.AllocateSlot(QuantizeToCellCenter(player.KnownPosition.X), QuantizeToCellCenter(player.KnownPosition.Z));
					_relevanceSlots[player] = slot;
					_relevanceSlotOwners[slot] = player;
					_relevanceForceRegroup = true;
				}
			}
			if (_relevanceSlots.Count != players.Length)
			{
				_relevanceGoneScratch.Clear();
				foreach (KeyValuePair<Player, int> entry in _relevanceSlots)
				{
					if (!_relevanceRosterScratch.Contains(entry.Key)) _relevanceGoneScratch.Add(entry.Key);
				}
				foreach (Player gone in _relevanceGoneScratch)
				{
					int slot = _relevanceSlots[gone];
					_relevanceViewerScratch.Clear();
					foreach (int viewerSlot in matrix.EnumerateRow(slot))
					{
						_relevanceViewerScratch.Add(_relevanceSlotOwners[viewerSlot]);
					}
					if (_relevanceViewerScratch.Count > 0) gone.DespawnFromPlayers(_relevanceViewerScratch.ToArray());

					matrix.FreeSlot(slot);
					_relevanceSlots.Remove(gone);
					_relevanceSlotOwners.Remove(slot);
					_relevanceForceRegroup = true;
				}
			}

			if (players.Length == 0) return;

			DateTime lastSendTime = _lastSendTime;
			_lastSendTime = now;

			// Stamped before the roster is walked, so broadcast.build covers everything the tick
			// thread spends here: building the records, compressing them, and encoding the wrapper.
			long buildStartedAt = Stopwatch.GetTimestamp();

			long matrixStartedAt = Stopwatch.GetTimestamp();

			// Positions are read live, no snapshot: whatever value KnownPosition holds at the
			// moment of reading is fine, it can already have advanced by the time the packet is
			// built, and that matters not (ruling). The matrix sees cell centers, not exact
			// positions. Exact distances give every viewer a subtly unique visible set (rim
			// membership differs per block), which measured at 400 spread players as ~380
			// single-member groups, one compression each, and a 172ms tick. Quantized, every
			// viewer in a cell holds a bit-identical row, so groups collapse to occupied cells,
			// and boundary transitions get cell hysteresis instead of flickering on the exact
			// radius. Movement packets still carry the exact position; only relevance is
			// cell-grained.
			if (_relevancePlayerSlotScratch.Length < players.Length)
			{
				_relevancePlayerSlotScratch = new int[players.Length];
				_relevanceHashScratch = new ulong[players.Length];
			}
			_relevanceMoverScratch.Clear();
			float maxDeviationSq = 0f;
			for (int i = 0; i < players.Length; i++)
			{
				Player player = players[i];
				int slot = _relevanceSlots[player];
				_relevancePlayerSlotScratch[i] = slot;
				PlayerLocation knownPosition = player.KnownPosition;
				matrix.SetPosition(slot, QuantizeToCellCenter(knownPosition.X), QuantizeToCellCenter(knownPosition.Z));

				// Drift since the last regroup, the signal that decides whether the group
				// structure gets rebuilt this pass.
				if (slot < _relevanceAnchorX.Length)
				{
					float dx = knownPosition.X - _relevanceAnchorX[slot];
					float dz = knownPosition.Z - _relevanceAnchorZ[slot];
					float deviationSq = dx * dx + dz * dz;
					if (deviationSq > maxDeviationSq) maxDeviationSq = deviationSq;
				}
				else
				{
					_relevanceForceRegroup = true;
				}

				if (now - player.LastUpdatedTime <= now - lastSendTime)
				{
					_relevanceMoverScratch.Add((player, slot));
				}
			}

			matrix.Compute();

			// The transition stream, grouped per moving entity so one entity crossing into a
			// crowd spawns with one call. A pair that entered gets the entity spawned on the
			// viewer's client; a pair that left gets it despawned. Teleports and dimension
			// changes need nothing special: a position jump is just a batch of lefts and
			// entereds here.
			int transitionCount = 0;
			foreach ((int viewerSlot, int entitySlot, bool entered) in matrix.EnumerateTransitions())
			{
				Player viewer = _relevanceSlotOwners[viewerSlot];
				Player entity = _relevanceSlotOwners[entitySlot];
				Dictionary<Player, List<Player>> map = entered ? _relevanceSpawnScratch : _relevanceDespawnScratch;
				if (!map.TryGetValue(entity, out List<Player> audience))
				{
					audience = _relevanceGroupPool.Count > 0 ? _relevanceGroupPool.Pop() : new List<Player>();
					map[entity] = audience;
				}
				audience.Add(viewer);
				transitionCount++;
			}

			EngineMetrics.RecordRelevance(matrixStartedAt, matrix.PairCount, transitionCount, _metricTags);

			if (_relevanceSpawnScratch.Count > 0 || _relevanceDespawnScratch.Count > 0)
			{
				foreach (KeyValuePair<Player, List<Player>> entry in _relevanceDespawnScratch)
				{
					entry.Key.DespawnFromPlayers(entry.Value.ToArray());
					entry.Value.Clear();
					_relevanceGroupPool.Push(entry.Value);
				}
				foreach (KeyValuePair<Player, List<Player>> entry in _relevanceSpawnScratch)
				{
					entry.Key.SpawnToPlayers(entry.Value.ToArray());
					entry.Value.Clear();
					_relevanceGroupPool.Push(entry.Value);
				}
				_relevanceSpawnScratch.Clear();
				_relevanceDespawnScratch.Clear();
			}

			if (_relevanceMoverScratch.Count == 0) return;

			// After Compute the matrix is a frozen snapshot and everything below is pure
			// derivation from it, so each per-element pass fans out as its own Parallel.For with
			// index-aligned writes; only the dictionary group-by and the pool bookkeeping stay
			// serial. Capped below the pinned logical count on purpose throughout: the session
			// send lanes that drain the queues run on the same cores.
			var buildParallelism = new ParallelOptions {MaxDegreeOfParallelism = 4};

			// The group structure only changes when someone crosses a cell, so it is rebuilt on
			// the movement signal, not per tick: when any player drifted more than a cell since
			// the anchors were last stamped, or on any roster change. In between, movers ride
			// the standing groups; the audience drift is bounded by one cell and the client's
			// interpolation swallows it.
			if (_relevanceForceRegroup || _relevanceGroupWork.Count == 0 || maxDeviationSq > RegroupDeviationBlocks * RegroupDeviationBlocks)
			{
				foreach (List<Player> group in _relevanceGroups.Values)
				{
					group.Clear();
					_relevanceGroupPool.Push(group);
				}
				_relevanceGroups.Clear();
				_relevanceGroupWork.Clear();

				// Row hashes, one per player, parallel: a pure read of the matrix rows.
				Parallel.For(0, players.Length, buildParallelism, i => { _relevanceHashScratch[i] = matrix.GetRowHashWithSelf(_relevancePlayerSlotScratch[i]); });

				// Group the recipients by row-plus-self hash: a mutually visible cluster shares
				// one hash and so one compressed batch, the legacy one-compression economy per
				// spatial cluster. The shared batch carries every mover in the cluster, so
				// members get their own movement echoed back, same as the legacy all-to-all
				// always did.
				for (int i = 0; i < players.Length; i++)
				{
					ulong hash = _relevanceHashScratch[i];
					if (!_relevanceGroups.TryGetValue(hash, out List<Player> group))
					{
						group = _relevanceGroupPool.Count > 0 ? _relevanceGroupPool.Pop() : new List<Player>();
						_relevanceGroups[hash] = group;
					}
					group.Add(players[i]);
				}

				// Snapshot the groups and resolve their coalesce keys on the tick thread; the
				// key dictionary is tick-thread state and must not be touched from the parallel
				// builds. The snapshot lives until the next rebuild.
				foreach (KeyValuePair<ulong, List<Player>> entry in _relevanceGroups)
				{
					if (!_relevanceCoalesceKeys.TryGetValue(entry.Key, out object coalesceKey))
					{
						coalesceKey = new object();
						_relevanceCoalesceKeys[entry.Key] = coalesceKey;
					}
					_relevanceGroupWork.Add((entry.Value, coalesceKey));
				}

				// Fresh anchors: deviation is measured from here until the next rebuild.
				for (int i = 0; i < players.Length; i++)
				{
					int slot = _relevancePlayerSlotScratch[i];
					if (slot >= _relevanceAnchorX.Length)
					{
						int newSize = Math.Max(slot + 64, _relevanceAnchorX.Length * 2);
						Array.Resize(ref _relevanceAnchorX, newSize);
						Array.Resize(ref _relevanceAnchorZ, newSize);
					}
					PlayerLocation position = players[i].KnownPosition;
					_relevanceAnchorX[slot] = position.X;
					_relevanceAnchorZ[slot] = position.Z;
				}

				// The coalesce keys follow the live group hashes; when the clusters dissolve,
				// the orphaned keys go too, or a long-lived level accumulates one per hash ever
				// seen.
				if (_relevanceCoalesceKeys.Count > 2 * _relevanceGroups.Count + 8)
				{
					foreach (ulong key in _relevanceCoalesceKeys.Keys)
					{
						if (!_relevanceGroups.ContainsKey(key)) _relevanceCoalesceKeys.Remove(key);
					}
				}

				_relevanceForceRegroup = false;
			}

			// Every mover's packet is built and encoded exactly ONCE per pass, in parallel (each
			// mover independent, pool and encode concurrent-safe, index-aligned writes); the
			// groups then assemble their payloads from the cached frame bytes. Building packets
			// per group measured at 400 players as ~130k packet encodes per pass, which alone
			// blew the tick budget.
			if (_relevanceMoverPackets.Length < _relevanceMoverScratch.Count) _relevanceMoverPackets = new Packet[_relevanceMoverScratch.Count];
			Parallel.For(0, _relevanceMoverScratch.Count, buildParallelism, m =>
			{
				(Player mover, int _) = _relevanceMoverScratch[m];
				PlayerLocation position = mover.KnownPosition;
				var move = McpeMovePlayer.CreateObject();
				move.runtimeEntityId = mover.EntityId;
				move.position = new Vector3(position.X, position.Y + 1.62f, position.Z);
				move.rotation = new Vector2(position.Pitch, position.Yaw);
				move.headYaw = position.HeadYaw;
				move.mode = mover.Vehicle == 0 ? McpeMovePlayer.PositionMode.Normal : McpeMovePlayer.PositionMode.Onlyheadrot;
				move.onGround = !mover.IsGliding && mover.IsOnGround;
				move.ridingRuntimeEntityId = mover.Vehicle;
				move.EncodeAsMemory();
				_relevanceMoverPackets[m] = move;
			});

			// The per-group build (frame assembly, wrapper, compression, enqueue) costs ~50us,
			// and the group count grows with how spread the population is (~1650 at 2000
			// wandering players), which made this loop most of the tick at scale. Every group is
			// independent, and everything the body touches is either read-only for the duration
			// (the matrix after Compute, the cached mover bytes, the slot map) or thread-safe on
			// its own (packet pools, the stream manager, metrics, SendPacket's session enqueue),
			// so the groups fan out across the server's cores. Parallel.For joins before the
			// pooled mover packets are returned.
			int groupCount = 0;
			Parallel.For(0, _relevanceGroupWork.Count, buildParallelism, i =>
			{
				(List<Player> members, object coalesceKey) = _relevanceGroupWork[i];
				int representative = _relevanceSlots[members[0]];

				using MemoryStream frames = MiNetServer.MemoryStreamManager.GetStream();
				int moveCount = 0;
				for (int m = 0; m < _relevanceMoverScratch.Count; m++)
				{
					// The group's shared set is row(representative) plus the representative
					// itself; every member's row-plus-self hashes to the same set.
					int slot = _relevanceMoverScratch[m].Slot;
					if (slot != representative && !matrix.IsRelevant(representative, slot)) continue;

					ReadOnlyMemory<byte> bs = _relevanceMoverPackets[m].EncodeAsMemory();
					BatchUtils.WriteLength(frames, bs.Length);
					frames.Write(bs.Span);
					moveCount++;
				}

				if (moveCount == 0) return;
				Interlocked.Increment(ref groupCount);

				var batch = McpeWrapper.CreateObject(members.Count);
				batch.CoalesceKey = coalesceKey;
				batch.SetPayload(Compression.CompressIntoPooledStream(new ReadOnlyMemory<byte>(frames.GetBuffer(), 0, (int) frames.Length), false, CompressionLevel.Fastest));
				batch.EncodeAsMemory();

				EngineMetrics.RecordBroadcast(moveCount, batch.payload.Length, buildStartedAt, _metricTags);
				EngineMetrics.RecordBroadcastRecipients(members.Count, _metricTags);

				foreach (Player member in members) member.SendPacket(batch);
			});

			for (int m = 0; m < _relevanceMoverScratch.Count; m++)
			{
				_relevanceMoverPackets[m].PutPool();
				_relevanceMoverPackets[m] = null;
			}

			if (groupCount > 0) EngineMetrics.RecordBroadcastGroups(groupCount, _metricTags);

			_lastBroadcast = DateTime.UtcNow;
		}

		public void RelayBroadcast<T>(T message) where T : Packet<T>, new()
		{
			RelayBroadcast(null, GetAllPlayers(), message);
		}

		public void RelayBroadcast<T>(Player source, T message) where T : Packet<T>, new()
		{
			RelayBroadcast(source, GetAllPlayers(), message);
		}

		public void RelayBroadcast<T>(Player[] sendList, T message) where T : Packet<T>, new()
		{
			RelayBroadcast(null, sendList ?? GetAllPlayers(), message);
		}

		public void RelayBroadcast<T>(Player source, Player[] sendList, T message) where T : Packet<T>, new()
		{
			if (message == null) return;

			if (!message.IsPooled)
			{
				try
				{
					throw new ArgumentException($"Trying to broadcast a message of type {message.GetType().Name} that isn't pooled. Please use CreateObject and not the constructor.");
				}
				catch (Exception e)
				{
					Log.Fatal("Broadcast", e);
					throw;
				}
			}

			if (sendList == null || sendList.Length == 0)
			{
				message.PutPool();
				return;
			}

			if (message.ReferenceCounter == 1 && sendList.Length > 1)
			{
				message.AddReferences(sendList.Length - 1);
			}

			if (sendList.Length == 1)
			{
				Player player = sendList.First();

				if (source != null && player == source)
				{
					message.PutPool();
					return;
				}

				player.SendPacket(message);
			}
			else
			{
				Parallel.ForEach(sendList, player =>
				{
					if (source != null && player == source)
					{
						message.PutPool();
						return;
					}

					player.SendPacket(message);
				});
			}
		}

		public List<EntitySpawnManager.SpawnState> GetChunkCoordinatesForTick(ChunkCoordinates chunkPosition, List<EntitySpawnManager.SpawnState> chunksUsed, double radius, Random random)
		{
			{
				List<EntitySpawnManager.SpawnState> newOrders = new List<EntitySpawnManager.SpawnState>();

				int centerX = chunkPosition.X;
				int centerZ = chunkPosition.Z;

				int halfRadius = (int) Math.Floor(radius / 2f);

				for (double x = -halfRadius; x <= halfRadius; ++x)
				{
					for (double z = -halfRadius; z <= halfRadius; ++z)
					{
						int chunkX = (int) (x + centerX);
						int chunkZ = (int) (z + centerZ);
						EntitySpawnManager.SpawnState index = new EntitySpawnManager.SpawnState(chunkX, chunkZ, random.Next());
						newOrders.Add(index);
					}
				}

				return newOrders.Union(chunksUsed).ToList();
			}
		}

		/// <summary>
		///     How much the player's facing direction reorders the radial sweep. 0 is pure inside-out by
		///     distance; higher values pull the cone in front of the player forward. At 1.0 a column
		///     directly ahead outranks one directly behind at up to 1.41x its distance, so the order
		///     stays fundamentally radial and near columns behind you still beat far ones in front.
		/// </summary>
		public static double ChunkDirectionBias { get; set; } = 1.0;

		/// <param name="viewYawDegrees">
		///     Minecraft yaw the player is facing, snapshotted when the sweep is computed. NaN orders
		///     purely by distance.
		/// </param>
		/// <summary>
		///     The columns a player at <paramref name="chunkPosition" /> should hold, nearest first,
		///     as ordinary packets rather than finished wrappers.
		///     <para>
		///         A skeleton is biomes and a byte, so wrapping each one alone cost a compress and a
		///         send per column and denied the send lane any chance to batch them: 81 columns was
		///         81 wrappers of forty bytes. Handed back as packets they coalesce like everything
		///         else, and one deflate stream sees all the skeletons together, which is the case it
		///         is good at.
		///     </para>
		///     <para>
		///         <paramref name="chunksUsed" /> is what this player already holds. Membership is the
		///         whole test: a column in here is never yielded again, whatever has been written to it
		///         since, because block writes reach the client as UpdateBlock. The only way back in is
		///         leaving the disc and re-entering it.
		///     </para>
		/// </summary>
		public IEnumerable<(ChunkCoordinates Coordinates, McpeLevelChunk Chunk)> GenerateChunks(ChunkCoordinates chunkPosition, Dictionary<ChunkCoordinates, long> chunksUsed, double radius, Func<Vector3> getCurrentPositionAction = null, double viewYawDegrees = double.NaN, bool prune = true, bool cachedPush = false)
		{
			lock (chunksUsed)
			{
				var newOrders = new Dictionary<ChunkCoordinates, double>();

				double radiusSquared = Math.Pow(radius, 2);

				int centerX = chunkPosition.X;
				int centerZ = chunkPosition.Z;

				// Minecraft yaw: 0 faces +Z, and it turns toward -X, so this is the unit vector the
				// player is looking along on the horizontal plane.
				bool directional = ChunkDirectionBias > 0 && !double.IsNaN(viewYawDegrees);
				double yawRadians = viewYawDegrees * Math.PI / 180d;
				double lookX = directional ? -Math.Sin(yawRadians) : 0;
				double lookZ = directional ? Math.Cos(yawRadians) : 0;

				for (double x = -radius; x <= radius; ++x)
				{
					for (double z = -radius; z <= radius; ++z)
					{
						var distance = (x * x) + (z * z);
						if (distance > radiusSquared)
						{
							continue;
						}
						int chunkX = (int) (x + centerX);
						int chunkZ = (int) (z + centerZ);
						var index = new ChunkCoordinates(chunkX, chunkZ);

						// Squared distance scaled by how far off the view direction this column sits:
						// 1.0 straight ahead, 1 + bias directly behind. Multiplying rather than adding
						// keeps the sweep radial - the penalty grows with distance, so it never
						// promotes a far column ahead over a near one behind.
						double cost = distance;
						if (directional && distance > 0)
						{
							double length = Math.Sqrt(distance);
							double alignment = (x * lookX + z * lookZ) / length; // -1 behind, 1 ahead
							cost *= 1d + ChunkDirectionBias * (1d - alignment) / 2d;
						}

						newOrders[index] = cost;
					}
				}

				// Pruned to exactly the published area. The client evicts columns outside the area
				// the publisher update declares and never re-requests them on its own (verified: a
				// client walked out and back requests nothing without a fresh skeleton), so every
				// column outside the publish boundary must be forgotten here to be re-sent on
				// return. The sweep set IS the published area - same centre, same ChunkRadius the
				// publisher multiplies by 16 - which keeps the two aligned by construction. Never
				// prune narrower than the publish area shrinks: a column the client dropped but the
				// server still remembers is a permanent hole. A first-block pass over the join
				// burst radius is NOT the published area and passes prune: false, or it would
				// forget the whole outer view here and re-send it every pass.
				if (prune)
				{
					foreach (ChunkCoordinates coordinates in chunksUsed.Keys.ToArray())
					{
						if (!newOrders.ContainsKey(coordinates)) chunksUsed.Remove(coordinates);
					}
				}

				foreach (var pair in newOrders.OrderBy(pair => pair.Value))
				{
					// Held by the client already. Content changes since then travelled as UpdateBlock,
					// so a second push would only make the client rebuild a column it has.
					if (chunksUsed.ContainsKey(pair.Key)) continue;

					if (WorldProvider == null) continue;

					if (getCurrentPositionAction != null)
					{
						var currentPos = getCurrentPositionAction();
						var coords = new ChunkCoordinates(currentPos);
						if(coords.DistanceTo(pair.Key) > radius) continue;
					}
					ChunkColumn chunkColumn = GetChunk(pair.Key);
					McpeLevelChunk chunk = null;
					if (chunkColumn != null)
					{
						// The caller said which form it wants. Push hands the client every hash the
						// column has and asks nothing of it; a skeleton announces the biomes and
						// leaves the client to request the sections it actually needs.
						chunk = cachedPush ? chunkColumn.CreateCachedPushChunk() : chunkColumn.CreateSkeletonChunk();

						chunksUsed[pair.Key] = chunkColumn.Version;
					}

					yield return (pair.Key, chunk);
				}
			}
		}

		public Block GetBlock(PlayerLocation location)
		{
			return GetBlock((BlockCoordinates) location);
		}

		public Block GetBlock(int x, int y, int z)
		{
			return GetBlock(new BlockCoordinates(x, y, z));
		}

		/// <summary>
		///     The runtime id at a position: chunk to sub-chunk to palette read, no block instance
		///     built. This is the probe form for per-tick queries (portal, water, suffocation);
		///     <see cref="GetBlock(BlockCoordinates, ChunkColumn)" /> stays for callers that need a
		///     block to interact with. An unloaded chunk reads as air.
		/// </summary>
		public int GetRuntimeIdAt(BlockCoordinates blockCoordinates)
		{
			ChunkColumn chunk = GetChunk(new ChunkCoordinates(blockCoordinates.X >> 4, blockCoordinates.Z >> 4));
			// Outside the world reads as air. The sub-chunk lookup clamps, so without this a query
			// above or below the build range is answered with a real block from the top or bottom of
			// the column, which is exactly what the per-tick checks calling this would hit.
			if (chunk == null || !ChunkColumn.IsInsideWorld(blockCoordinates.Y)) return BlockFactory.AirRuntimeId;

			return chunk.GetBlockRuntimeId(blockCoordinates.X & 0x0f, blockCoordinates.Y, blockCoordinates.Z & 0x0f);
		}

		public Block GetBlock(BlockCoordinates blockCoordinates, ChunkColumn tryChunk = null)
		{
			ChunkColumn chunk = null;

			var chunkCoordinates = new ChunkCoordinates(blockCoordinates.X >> 4, blockCoordinates.Z >> 4);
			if (tryChunk != null && tryChunk.X == chunkCoordinates.X && tryChunk.Z == chunkCoordinates.Z)
			{
				chunk = tryChunk;
			}
			else
			{
				chunk = GetChunk(chunkCoordinates);
			}
			// Above or below the world reads as air, the same as a column that is not loaded. The
			// sub-chunk lookup clamps, so without this an entity high above the world or falling out
			// of the bottom is answered with a real block from the top or bottom of the column.
			if (chunk == null || !ChunkColumn.IsInsideWorld(blockCoordinates.Y))
				return new Air
				{
					Coordinates = blockCoordinates,
					SkyLight = 15
				};

			var block = chunk.GetBlockObject(blockCoordinates.X & 0x0f, blockCoordinates.Y, blockCoordinates.Z & 0x0f);
			byte blockLight = chunk.GetBlocklight(blockCoordinates.X & 0x0f, blockCoordinates.Y, blockCoordinates.Z & 0x0f);
			byte skyLight = chunk.GetSkylight(blockCoordinates.X & 0x0f, blockCoordinates.Y, blockCoordinates.Z & 0x0f);
			byte biomeId = chunk.GetBiome(blockCoordinates.X & 0x0f, blockCoordinates.Z & 0x0f);

			//Block block = BlockFactory.GetBlockById(bid);
			block.Coordinates = blockCoordinates;
			//block.Metadata = metadata;
			block.BlockLight = blockLight;
			block.SkyLight = skyLight;
			block.BiomeId = biomeId;

			return block;
		}

		public bool IsBlock(int x, int y, int z, int blockId)
		{
			return IsBlock(new BlockCoordinates(x, y, z), blockId);
		}

		public bool IsBlock(BlockCoordinates blockCoordinates, int blockId)
		{
			ChunkColumn chunk = GetChunk(blockCoordinates);
			if (chunk == null || !ChunkColumn.IsInsideWorld(blockCoordinates.Y)) return false;

			return chunk.GetBlockId(blockCoordinates.X & 0x0f, blockCoordinates.Y, blockCoordinates.Z & 0x0f) == blockId;
		}

		/// <summary>
		///     Which block stands here, asked by name so that it holds for every state the block has.
		///     Nothing is built to answer it.
		/// </summary>
		public bool IsBlock(BlockCoordinates blockCoordinates, string name)
		{
			ChunkColumn chunk = GetChunk(blockCoordinates);
			if (chunk == null || !ChunkColumn.IsInsideWorld(blockCoordinates.Y)) return false;

			return BlockFactory.GetBlockName(chunk.GetBlockRuntimeId(blockCoordinates.X & 0x0f, blockCoordinates.Y, blockCoordinates.Z & 0x0f)) == name;
		}

		public bool IsAir(BlockCoordinates blockCoordinates)
		{
			ChunkColumn chunk = GetChunk(blockCoordinates);
			if (chunk == null || !ChunkColumn.IsInsideWorld(blockCoordinates.Y)) return true;

			return BlockFactory.IsAir(chunk.GetBlockRuntimeId(blockCoordinates.X & 0x0f, blockCoordinates.Y, blockCoordinates.Z & 0x0f));
		}

		/// <summary>Whether a full solid cube stands here. For "can something walk through it", ask <see cref="BlocksMovement" />.</summary>
		public bool IsSolid(BlockCoordinates blockCoordinates)
		{
			ChunkColumn chunk = GetChunk(blockCoordinates);
			if (chunk == null || !ChunkColumn.IsInsideWorld(blockCoordinates.Y)) return false;

			return BlockFactory.IsSolid(chunk.GetBlockRuntimeId(blockCoordinates.X & 0x0f, blockCoordinates.Y, blockCoordinates.Z & 0x0f));
		}

		public bool IsNotBlockingSkylight(BlockCoordinates blockCoordinates)
		{
			ChunkColumn chunk = GetChunk(blockCoordinates);
			if (chunk == null || !ChunkColumn.IsInsideWorld(blockCoordinates.Y)) return true;

			return BlockFactory.SkyLightPasses(chunk.GetBlockRuntimeId(blockCoordinates.X & 0x0f, blockCoordinates.Y, blockCoordinates.Z & 0x0f));
		}

		public bool IsTransparent(BlockCoordinates blockCoordinates)
		{
			ChunkColumn chunk = GetChunk(blockCoordinates);
			if (chunk == null || !ChunkColumn.IsInsideWorld(blockCoordinates.Y)) return true;

			return BlockFactory.IsTransparent(chunk.GetBlockRuntimeId(blockCoordinates.X & 0x0f, blockCoordinates.Y, blockCoordinates.Z & 0x0f));
		}

		public int GetHeight(BlockCoordinates blockCoordinates)
		{
			ChunkColumn chunk = GetChunk(blockCoordinates);
			if (chunk == null) return 256;

			return chunk.GetHeight(blockCoordinates.X & 0x0f, blockCoordinates.Z & 0x0f);
		}

		public byte GetSkyLight(BlockCoordinates blockCoordinates)
		{
			ChunkColumn chunk = GetChunk(blockCoordinates);

			if (chunk == null || !ChunkColumn.IsInsideWorld(blockCoordinates.Y)) return 15;

			return chunk.GetSkylight(blockCoordinates.X & 0x0f, blockCoordinates.Y, blockCoordinates.Z & 0x0f);
		}

		public byte GetBlockLight(BlockCoordinates blockCoordinates)
		{
			ChunkColumn chunk = GetChunk(blockCoordinates);

			if (chunk == null || !ChunkColumn.IsInsideWorld(blockCoordinates.Y)) return 15;

			return chunk.GetBlocklight(blockCoordinates.X & 0x0f, blockCoordinates.Y, blockCoordinates.Z & 0x0f);
		}

		public byte GetBiomeId(BlockCoordinates blockCoordinates)
		{
			ChunkColumn chunk = GetChunk(blockCoordinates);

			if (chunk == null) return 0;

			return chunk.GetBiome(blockCoordinates.X & 0x0f, blockCoordinates.Z & 0x0f);
		}

		public ChunkColumn GetChunk(BlockCoordinates blockCoordinates, bool cacheOnly = false)
		{
			return GetChunk((ChunkCoordinates) blockCoordinates, cacheOnly);
		}

		public ChunkColumn GetChunk(ChunkCoordinates chunkCoordinates, bool cacheOnly = false)
		{
			long startedAt = Stopwatch.GetTimestamp();
			var chunk = WorldProvider.GenerateChunkColumn(chunkCoordinates, cacheOnly);
			EngineMetrics.RecordChunkLoad(startedAt, _metricTags);

			if (chunk == null) Log.Debug($"Got <null> chunk at {chunkCoordinates}");

			// Stamped here rather than in each provider: the column goes on the wire carrying its
			// dimension, and the client drops one that does not match the dimension it is in.
			if (chunk != null) chunk.Dimension = Dimension;

			return chunk;
		}

		public void SetBlock(Block block, bool broadcast = true, bool applyPhysics = true, bool calculateLight = true, ChunkColumn possibleChunk = null)
		{
			// The column's own range, not zero. Everything from WorldMinY up is addressable, and
			// guarding at zero silently dropped every block in the deepslate range.
			if (!ChunkColumn.IsInsideWorld(block.Coordinates.Y)) return;

			var chunkCoordinates = new ChunkCoordinates(block.Coordinates.X >> 4, block.Coordinates.Z >> 4);
			ChunkColumn chunk = possibleChunk != null && possibleChunk.X == chunkCoordinates.X && possibleChunk.Z == chunkCoordinates.Z ? possibleChunk : GetChunk(chunkCoordinates);


			if (!OnBlockPlace(new BlockPlaceEventArgs(null, this, block, null)))
			{
				return;
			}

			chunk.SetBlock(block.Coordinates.X & 0x0f, block.Coordinates.Y, block.Coordinates.Z & 0x0f, block);
			if (calculateLight && chunk.GetHeight(block.Coordinates.X & 0x0f, block.Coordinates.Z & 0x0f) <= block.Coordinates.Y + 1)
			{
				// WorldMaxY, not WorldHeight: the argument is a world Y to start scanning down from,
				// and WorldHeight is a block count, 384 against a ceiling of 320.
				chunk.RecalcHeight(block.Coordinates.X & 0x0f, block.Coordinates.Z & 0x0f, Math.Min(ChunkColumn.WorldMaxY, block.Coordinates.Y + 1));
			}

			if (applyPhysics) ApplyPhysics(block.Coordinates.X, block.Coordinates.Y, block.Coordinates.Z);

			// We should not ignore creative. Need to investigate.
			if (GameMode != GameMode.Creative && calculateLight /* && block.LightLevel > 0*/)
			{
				if (Dimension == Dimension.Overworld) new SkyLightCalculations().Calculate(this, block.Coordinates);

				block.BlockLight = (byte) block.LightLevel;
				chunk.SetBlocklight(block.Coordinates.X & 0x0f, block.Coordinates.Y, block.Coordinates.Z & 0x0f, (byte) block.LightLevel);
				BlockLightCalculations.Calculate(this, block.Coordinates);
			}

			if (broadcast)
			{
				var message = McpeUpdateBlock.CreateObject();
				message.blockRuntimeId = BlockFactory.GetNetworkId(block);
				message.coordinates = block.Coordinates;
				message.blockPriority = 0xb;
				RelayBroadcast(message);
			}

			block.BlockAdded(this);
		}

		private void CalculateSkyLight(int x, int y, int z)
		{
			DoLight(x, y, z);
			DoLight(x - 1, y, z);
			DoLight(x + 1, y, z);
			DoLight(x, y, z - 1);
			DoLight(x, y, z + 1);
			DoLight(x - 1, y, z - 1);
			DoLight(x - 1, y, z + 1);
			DoLight(x + 1, y, z - 1);
			DoLight(x + 1, y, z + 1);
		}

		private void DoLight(int x, int y, int z)
		{
			//Block block = GetBlock(x, y, z);
			//if (block is Air) return;
			//new SkyLightCalculations().Calculate(this, block);
		}

		public void SetBlockLight(Block block)
		{
			ChunkColumn chunk = GetChunk(new ChunkCoordinates(block.Coordinates.X >> 4, block.Coordinates.Z >> 4));
			chunk.SetBlocklight(block.Coordinates.X & 0x0f, block.Coordinates.Y, block.Coordinates.Z & 0x0f, block.BlockLight);
		}

		public void SetBlockLight(BlockCoordinates coordinates, byte blockLight)
		{
			ChunkColumn chunk = GetChunk(coordinates);
			chunk?.SetBlocklight(coordinates.X & 0x0f, coordinates.Y, coordinates.Z & 0x0f, blockLight);
		}

		public void SetBiomeId(BlockCoordinates coordinates, byte biomeId)
		{
			ChunkColumn chunk = GetChunk(coordinates);
			chunk?.SetBiome(coordinates.X & 0x0f, coordinates.Z & 0x0f, biomeId);
		}

		public void SetSkyLight(Block block)
		{
			ChunkColumn chunk = GetChunk(new ChunkCoordinates(block.Coordinates.X >> 4, block.Coordinates.Z >> 4));
			chunk.SetSkyLight(block.Coordinates.X & 0x0f, block.Coordinates.Y, block.Coordinates.Z & 0x0f, block.SkyLight);
		}

		public void SetSkyLight(BlockCoordinates coordinates, byte skyLight)
		{
			ChunkColumn chunk = GetChunk(coordinates);
			chunk?.SetSkyLight(coordinates.X & 0x0f, coordinates.Y, coordinates.Z & 0x0f, skyLight);
		}

		public void SetAir(BlockCoordinates blockCoordinates, bool broadcast = true)
		{
			SetAir(blockCoordinates.X, blockCoordinates.Y, blockCoordinates.Z, broadcast);
		}

		public void SetAir(int x, int y, int z, bool broadcast = true)
		{
			var air = new Air {Coordinates = new BlockCoordinates(x, y, z)};
			SetBlock(air, broadcast);
		}

		public BlockEntity GetBlockEntity(BlockCoordinates blockCoordinates)
		{
			var blockEntity = BlockEntities.FirstOrDefault(entity => entity.Coordinates == blockCoordinates);
			if (blockEntity != null)
			{
				return blockEntity;
			}

			ChunkColumn chunk = GetChunk(new ChunkCoordinates(blockCoordinates.X >> 4, blockCoordinates.Z >> 4));

			NbtCompound nbt = chunk?.GetBlockEntity(blockCoordinates);
			if (nbt == null) return null;

			if (!nbt.TryGet("id", out NbtString idTag)) return null;

			blockEntity = BlockEntityFactory.GetBlockEntityById(idTag.StringValue);
			if (blockEntity == null) return null;

			blockEntity.Coordinates = blockCoordinates;
			blockEntity.SetCompound(nbt);

			return blockEntity;
		}

		public void SetBlockEntity(BlockEntity blockEntity, bool broadcast = true)
		{
			ChunkColumn chunk = GetChunk(new ChunkCoordinates(blockEntity.Coordinates.X >> 4, blockEntity.Coordinates.Z >> 4));
			chunk.SetBlockEntity(blockEntity.Coordinates, blockEntity.GetCompound());

			if (blockEntity.UpdatesOnTick)
			{
				BlockEntities.RemoveAll(entity => entity.Coordinates == blockEntity.Coordinates);
				BlockEntities.Add(blockEntity);
			}

			if (!broadcast) return;

			Nbt nbt = new Nbt
			{
				NbtFile = new NbtFile
				{
					BigEndian = false,
					UseVarInt = true,
					RootTag = blockEntity.GetCompound()
				}
			};

			if (Log.IsDebugEnabled) Log.Debug($"Nbt: {nbt.NbtFile.RootTag}");

			var entityData = McpeBlockEntityData.CreateObject();
			entityData.namedtag = nbt;
			entityData.coordinates = blockEntity.Coordinates;

			RelayBroadcast(entityData);
		}

		public void RemoveBlockEntity(BlockCoordinates blockCoordinates)
		{
			// Before anything else, and whether or not the chunk still holds the tag: an inventory left
			// in the manager's cache outlives the block it belonged to.
			InventoryManager.RemoveInventory(blockCoordinates);

			ChunkColumn chunk = GetChunk(new ChunkCoordinates(blockCoordinates.X >> 4, blockCoordinates.Z >> 4));
			var nbt = chunk.GetBlockEntity(blockCoordinates);

			if (nbt == null) return;

			var blockEntity = BlockEntities.FirstOrDefault(entity => entity.Coordinates == blockCoordinates);
			if (blockEntity != null)
			{
				BlockEntities.Remove(blockEntity);
			}

			chunk.RemoveBlockEntity(blockCoordinates);
		}

		public event EventHandler<BlockPlaceEventArgs> BlockPlace;

		public virtual bool OnBlockPlace(BlockPlaceEventArgs e)
		{
			BlockPlace?.Invoke(this, e);

			return !e.Cancel;
		}

		public void Interact(Player player, Item itemInHand, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			Block target = GetBlock(blockCoordinates);
			if (!player.IsSneaking && target.Interact(this, player, blockCoordinates, face, faceCoords)) return; // Handled in block interaction

			Log.Debug($"Item in hand: {itemInHand}");
			if (itemInHand is ItemBlock)
			{
				Block block = GetBlock(blockCoordinates);
				if (!block.IsReplaceable)
				{
					block = GetBlock(itemInHand.GetNewCoordinatesFromFace(blockCoordinates, face));
				}

				if (!AllowBuild || player.GameMode == GameMode.Spectator || !OnBlockPlace(new BlockPlaceEventArgs(player, this, target, block)))
				{
					// Revert

					player.SendPlayerInventory();

					var message = McpeUpdateBlock.CreateObject();
					message.blockRuntimeId = BlockFactory.GetNetworkId(block);
					message.coordinates = block.Coordinates;
					message.blockPriority = 0xb;
					player.SendPacket(message);

					return;
				}
			}

			itemInHand.PlaceBlock(this, player, blockCoordinates, face, faceCoords);
		}

		public event EventHandler<BlockBreakEventArgs> BlockBreak;

		protected virtual bool OnBlockBreak(BlockBreakEventArgs e)
		{
			BlockBreak?.Invoke(this, e);

			return !e.Cancel;
		}

		public void BreakBlock(Player player, BlockCoordinates blockCoordinates, BlockFace face = BlockFace.None)
		{
			Block block = GetBlock(blockCoordinates);
			BlockEntity blockEntity = GetBlockEntity(blockCoordinates);

			Item inHand = player.Inventory.GetItemInHand();
			bool canBreak = inHand.BreakBlock(this, player, block, blockEntity);

			if (!canBreak || !AllowBreak || player.GameMode == GameMode.Spectator || !OnBlockBreak(new BlockBreakEventArgs(player, this, block, null)))
			{
				// Revert

				RevertBlockAction(player, block, blockEntity);
			}
			else
			{
				BreakBlock(player, block, blockEntity, inHand, face);

				player.Inventory.DamageItemInHand(ItemDamageReason.BlockBreak, null, block);
				player.HungerManager.IncreaseExhaustion(0.025f);
				player.ExperienceManager.AddExperience(block.GetExperiencePoints());
			}
		}

		private static void RevertBlockAction(Player player, Block block, BlockEntity blockEntity)
		{
			var message = McpeUpdateBlock.CreateObject();
			message.blockRuntimeId = BlockFactory.GetNetworkId(block);
			message.coordinates = block.Coordinates;
			message.blockPriority = 0xb;
			player.SendPacket(message);

			// Revert block entity if exists
			if (blockEntity != null)
			{
				Nbt nbt = new Nbt
				{
					NbtFile = new NbtFile
					{
						BigEndian = false,
						RootTag = blockEntity.GetCompound()
					}
				};

				var entityData = McpeBlockEntityData.CreateObject();
				entityData.namedtag = nbt;
				entityData.coordinates = blockEntity.Coordinates;

				player.SendPacket(entityData);
			}
		}

		public void BreakBlock(Player player, Block block, BlockEntity blockEntity = null, Item tool = null, BlockFace face = BlockFace.None)
		{
			block.BreakBlock(this, face);
			var drops = new List<Item>();
			drops.AddRange(block.GetDrops(tool ?? new ItemAir()));

			if (blockEntity != null)
			{
				RemoveBlockEntity(block.Coordinates);
				drops.AddRange(blockEntity.GetDrops());
			}

			if ((player != null && player.GameMode == GameMode.Survival) || (player == null && GameMode == GameMode.Survival))
			{
				foreach (Item drop in drops)
				{
					DropItem(block.Coordinates, drop);
				}
			}
		}


		public virtual void DropItem(Vector3 coordinates, Item drop)
		{
			if (GameMode == GameMode.Creative) return;

			if (drop == null) return;
			if (drop.IsAir) return;
			if (drop.Count == 0) return;

			if (AutoSmelt) drop = drop.GetSmelt() ?? drop;

			Random random = new Random();
			var itemEntity = new ItemEntity(this, drop)
			{
				KnownPosition =
				{
					X = (float) coordinates.X + 0.5f,
					Y = (float) coordinates.Y + 0.5f,
					Z = (float) coordinates.Z + 0.5f
				},
				Velocity = new Vector3((float) (random.NextDouble() * 0.005), (float) (random.NextDouble() * 0.20), (float) (random.NextDouble() * 0.005))
			};

			itemEntity.SpawnEntity();
		}

		public void ApplyPhysics(int x, int y, int z)
		{
			DoPhysics(x - 1, y, z);
			DoPhysics(x + 1, y, z);
			DoPhysics(x, y - 1, z);
			DoPhysics(x, y + 1, z);
			DoPhysics(x, y, z - 1);
			DoPhysics(x, y, z + 1);
		}

		private void DoPhysics(int x, int y, int z)
		{
			Block block = GetBlock(x, y, z);
			if (block is Air) return;
			block.DoPhysics(this);
		}

		public void ScheduleBlockTick(Block block, int tickRate)
		{
			if (BlockWithTicks.ContainsKey(block.Coordinates)) return;
			BlockWithTicks[block.Coordinates] = TickTime + tickRate;
		}

		public void CancelBlockTick(Block block)
		{
			BlockWithTicks.TryRemove(block.Coordinates, out _);
		}

		public bool TryGetEntity<T>(long targetEntityId, out T entity) where T : class
		{
			entity = null;

			if (Players.TryGetValue(targetEntityId, out var player))
			{
				entity = player as T;
			}
			else if (Entities.TryGetValue(targetEntityId, out var ent))
			{
				entity = ent as T;
			}

			return entity != null;
		}


		public ChunkColumn[] GetLoadedChunks()
		{
			var cacheProvider = WorldProvider as ICachingWorldProvider;
			if (cacheProvider != null)
			{
				return cacheProvider.GetCachedChunks();
			}

			return new ChunkColumn[0];
		}

		public void ClearLoadedChunks()
		{
			var cacheProvider = WorldProvider as ICachingWorldProvider;
			cacheProvider?.ClearCachedChunks();
		}

		/// <summary>
		///     Spawns a lightning bolt centred on <paramref name="position" />. The client anchors the
		///     bolt's visual at a corner of its footprint rather than at the entity position, so a
		///     bolt sent at a target's exact coordinates lands about half a block off it. The
		///     correction is empirical: it was found by striking a known target with each of the four
		///     half-block offsets and seeing which one landed on it.
		/// </summary>
		public void StrikeLightning(Vector3 position)
		{
			Vector3 centred = position - new Vector3(0.5f, 0, 0.5f);
			new Lightning(this) {KnownPosition = new PlayerLocation(centred)}.SpawnEntity();
		}

		public void MakeSound(Sound sound)
		{
			sound.Spawn(this);
		}

		public bool DrowningDamage { get; set; } = true;
		public bool CommandblockOutput { get; set; } = true;
		public bool DoTiledrops { get; set; } = true;
		public bool DoMobloot { get; set; } = true;
		public bool KeepInventory { get; set; } = true;
		public bool DoDaylightcycle { get; set; } = true;
		public bool DoMobspawning { get; set; } = true;
		public bool DoEntitydrops { get; set; } = true;
		public bool DoFiretick { get; set; } = true;
		public bool DoWeathercycle { get; set; } = true;
		public bool Pvp { get; set; } = true;
		public bool Falldamage { get; set; } = true;
		public bool Firedamage { get; set; } = true;
		public bool Mobgriefing { get; set; } = true;
		public bool ShowCoordinates { get; set; } = true;
		public bool NaturalRegeneration { get; set; } = true;
		public bool TntExplodes { get; set; } = true;
		public bool SendCommandfeedback { get; set; } = true;
		public int RandomTickSpeed { get; set; } = 1; // Bedrock default (Java uses 3)
		public bool RecipesUnlock { get; set; } = true;
		public bool DoLimitedCrafting { get; set; } = false;
		public int PlayerWaypoints { get; set; } = 1;
		public bool Locatorbar { get; set; } = true;
		public bool ShowDaysPlayed { get; set; } = false;
		public int MaxCommandChainLength { get; set; } = 65535;
		public bool DoInsomnia { get; set; } = true;
		public bool CommandblocksEnabled { get; set; } = true;
		public bool DoImmediateRespawn { get; set; } = false;
		public bool ShowDeathMessages { get; set; } = true;
		public int FunctionCommandLimit { get; set; } = 10000;
		public int SpawnRadius { get; set; } = 10;
		public bool ShowTags { get; set; } = true;
		public bool FreezeDamage { get; set; } = true;
		public bool RespawnBlocksExplode { get; set; } = true;
		public bool ShowBorderEffect { get; set; } = true;
		public bool ShowRecipeMessages { get; set; } = true;
		public int PlayersSleepingPercentage { get; set; } = 100;
		public bool ProjectilesCanBreakBlocks { get; set; } = true;
		public bool TntExplosionDropDecay { get; set; } = false;

		public virtual void BroadcastGameRules()
		{
			McpeGameRulesChanged gameRulesChanged = McpeGameRulesChanged.CreateObject();
			gameRulesChanged.rules = GetGameRules();
			RelayBroadcast(gameRulesChanged);
		}

		public void SetGameRule(GameRulesEnum rule, bool value)
		{
			switch (rule)
			{
				case GameRulesEnum.DrowningDamage:
					DrowningDamage = value;
					break;
				case GameRulesEnum.CommandblockOutput:
					CommandblockOutput = value;
					break;
				case GameRulesEnum.DoTiledrops:
					DoTiledrops = value;
					break;
				case GameRulesEnum.DoMobloot:
					DoMobloot = value;
					break;
				case GameRulesEnum.KeepInventory:
					KeepInventory = value;
					break;
				case GameRulesEnum.DoDaylightcycle:
					DoDaylightcycle = value;
					break;
				case GameRulesEnum.DoMobspawning:
					DoMobspawning = value;
					break;
				case GameRulesEnum.DoEntitydrops:
					DoEntitydrops = value;
					break;
				case GameRulesEnum.DoFiretick:
					DoFiretick = value;
					break;
				case GameRulesEnum.DoWeathercycle:
					DoWeathercycle = value;
					break;
				case GameRulesEnum.Pvp:
					Pvp = value;
					break;
				case GameRulesEnum.Falldamage:
					Falldamage = value;
					break;
				case GameRulesEnum.Firedamage:
					Firedamage = value;
					break;
				case GameRulesEnum.Mobgriefing:
					Mobgriefing = value;
					break;
				case GameRulesEnum.ShowCoordinates:
					ShowCoordinates = value;
					break;
				case GameRulesEnum.NaturalRegeneration:
					NaturalRegeneration = value;
					break;
				case GameRulesEnum.TntExplodes:
					TntExplodes = value;
					break;
				case GameRulesEnum.SendCommandfeedback:
					SendCommandfeedback = value;
					break;
				case GameRulesEnum.RecipesUnlock:
					RecipesUnlock = value;
					break;
				case GameRulesEnum.DoLimitedCrafting:
					DoLimitedCrafting = value;
					break;
				case GameRulesEnum.Locatorbar:
					Locatorbar = value;
					break;
				case GameRulesEnum.ShowDaysPlayed:
					ShowDaysPlayed = value;
					break;
				case GameRulesEnum.DoInsomnia:
					DoInsomnia = value;
					break;
				case GameRulesEnum.CommandblocksEnabled:
					CommandblocksEnabled = value;
					break;
				case GameRulesEnum.DoImmediateRespawn:
					DoImmediateRespawn = value;
					break;
				case GameRulesEnum.ShowDeathmessages:
					ShowDeathMessages = value;
					break;
				case GameRulesEnum.ShowTags:
					ShowTags = value;
					break;
				case GameRulesEnum.FreezeDamage:
					FreezeDamage = value;
					break;
				case GameRulesEnum.RespawnBlocksExplode:
					RespawnBlocksExplode = value;
					break;
				case GameRulesEnum.ShowBorderEffect:
					ShowBorderEffect = value;
					break;
				case GameRulesEnum.ShowRecipeMessages:
					ShowRecipeMessages = value;
					break;
				case GameRulesEnum.ProjectilesCanBreakBlocks:
					ProjectilesCanBreakBlocks = value;
					break;
				case GameRulesEnum.TntExplosionDropDecay:
					TntExplosionDropDecay = value;
					break;
			}
		}

		public void SetGameRule(GameRulesEnum rule, int value)
		{
			switch (rule)
			{
				case GameRulesEnum.RandomTickSpeed:
					RandomTickSpeed = value;
					break;
				case GameRulesEnum.PlayerWaypoints:
					PlayerWaypoints = value;
					break;
				case GameRulesEnum.MaxCommandChainLength:
					MaxCommandChainLength = value;
					break;
				case GameRulesEnum.FunctionCommandLimit:
					FunctionCommandLimit = value;
					break;
				case GameRulesEnum.SpawnRadius:
					SpawnRadius = value;
					break;
				case GameRulesEnum.PlayersSleepingPercentage:
					PlayersSleepingPercentage = value;
					break;
			}
		}


		public bool GetGameRule(GameRulesEnum rule)
		{
			switch (rule)
			{
				case GameRulesEnum.DrowningDamage:
					return DrowningDamage;
				case GameRulesEnum.CommandblockOutput:
					return CommandblockOutput;
				case GameRulesEnum.DoTiledrops:
					return DoTiledrops;
				case GameRulesEnum.DoMobloot:
					return DoMobloot;
				case GameRulesEnum.KeepInventory:
					return KeepInventory;
				case GameRulesEnum.DoDaylightcycle:
					return DoDaylightcycle;
				case GameRulesEnum.DoMobspawning:
					return DoMobspawning;
				case GameRulesEnum.DoEntitydrops:
					return DoEntitydrops;
				case GameRulesEnum.DoFiretick:
					return DoFiretick;
				case GameRulesEnum.DoWeathercycle:
					return DoWeathercycle;
				case GameRulesEnum.Pvp:
					return Pvp;
				case GameRulesEnum.Falldamage:
					return Falldamage;
				case GameRulesEnum.Firedamage:
					return Firedamage;
				case GameRulesEnum.Mobgriefing:
					return Mobgriefing;
				case GameRulesEnum.ShowCoordinates:
					return ShowCoordinates;
				case GameRulesEnum.NaturalRegeneration:
					return NaturalRegeneration;
				case GameRulesEnum.TntExplodes:
					return TntExplodes;
				case GameRulesEnum.SendCommandfeedback:
					return SendCommandfeedback;
				case GameRulesEnum.RecipesUnlock:
					return RecipesUnlock;
				case GameRulesEnum.DoLimitedCrafting:
					return DoLimitedCrafting;
				case GameRulesEnum.Locatorbar:
					return Locatorbar;
				case GameRulesEnum.ShowDaysPlayed:
					return ShowDaysPlayed;
				case GameRulesEnum.DoInsomnia:
					return DoInsomnia;
				case GameRulesEnum.CommandblocksEnabled:
					return CommandblocksEnabled;
				case GameRulesEnum.DoImmediateRespawn:
					return DoImmediateRespawn;
				case GameRulesEnum.ShowDeathmessages:
					return ShowDeathMessages;
				case GameRulesEnum.ShowTags:
					return ShowTags;
				case GameRulesEnum.FreezeDamage:
					return FreezeDamage;
				case GameRulesEnum.RespawnBlocksExplode:
					return RespawnBlocksExplode;
				case GameRulesEnum.ShowBorderEffect:
					return ShowBorderEffect;
				case GameRulesEnum.ShowRecipeMessages:
					return ShowRecipeMessages;
				case GameRulesEnum.ProjectilesCanBreakBlocks:
					return ProjectilesCanBreakBlocks;
				case GameRulesEnum.TntExplosionDropDecay:
					return TntExplosionDropDecay;
			}

			return false;
		}

		public virtual GameRules GetGameRules()
		{
			// The full 1.26.34 rule set, exact wire names (camelCase) and order. Every value
			// comes from the level's properties (defaults match vanilla); no literals here.
			GameRules rules = new GameRules();
			rules.Add(new GameRule<bool>("commandBlockOutput", CommandblockOutput));
			rules.Add(new GameRule<bool>("doDayLightCycle", DoDaylightcycle));
			rules.Add(new GameRule<bool>("doEntityDrops", DoEntitydrops));
			rules.Add(new GameRule<bool>("doFireTick", DoFiretick));
			rules.Add(new GameRule<bool>("recipesUnlock", RecipesUnlock));
			rules.Add(new GameRule<bool>("doLimitedCrafting", DoLimitedCrafting));
			rules.Add(new GameRule<bool>("doMobLoot", DoMobloot));
			rules.Add(new GameRule<bool>("doMobSpawning", DoMobspawning));
			rules.Add(new GameRule<bool>("doTileDrops", DoTiledrops));
			rules.Add(new GameRule<bool>("doWeatherCycle", DoWeathercycle));
			rules.Add(new GameRule<bool>("drowningDamage", DrowningDamage));
			rules.Add(new GameRule<bool>("fallDamage", Falldamage));
			rules.Add(new GameRule<bool>("fireDamage", Firedamage));
			rules.Add(new GameRule<bool>("keepInventory", KeepInventory));
			rules.Add(new GameRule<bool>("mobGriefing", Mobgriefing));
			rules.Add(new GameRule<bool>("pvp", Pvp));
			rules.Add(new GameRule<bool>("showCoordinates", ShowCoordinates));
			rules.Add(new GameRule<int>("playerWaypoints", PlayerWaypoints));
			rules.Add(new GameRule<bool>("locatorbar", Locatorbar));
			rules.Add(new GameRule<bool>("showDaysPlayed", ShowDaysPlayed));
			rules.Add(new GameRule<bool>("naturalRegeneration", NaturalRegeneration));
			rules.Add(new GameRule<bool>("tntExplodes", TntExplodes));
			rules.Add(new GameRule<bool>("sendCommandFeedback", SendCommandfeedback));
			rules.Add(new GameRule<int>("maxCommandChainLength", MaxCommandChainLength));
			rules.Add(new GameRule<bool>("doInsomnia", DoInsomnia));
			rules.Add(new GameRule<bool>("commandBlocksEnabled", CommandblocksEnabled));
			rules.Add(new GameRule<int>("randomTickSpeed", RandomTickSpeed));
			rules.Add(new GameRule<bool>("doImmediateRespawn", DoImmediateRespawn));
			rules.Add(new GameRule<bool>("showDeathMessages", ShowDeathMessages));
			rules.Add(new GameRule<int>("functionCommandLimit", FunctionCommandLimit));
			rules.Add(new GameRule<int>("spawnRadius", SpawnRadius));
			rules.Add(new GameRule<bool>("showTags", ShowTags));
			rules.Add(new GameRule<bool>("freezeDamage", FreezeDamage));
			rules.Add(new GameRule<bool>("respawnBlocksExplode", RespawnBlocksExplode));
			rules.Add(new GameRule<bool>("showBorderEffect", ShowBorderEffect));
			rules.Add(new GameRule<bool>("showRecipeMessages", ShowRecipeMessages));
			rules.Add(new GameRule<int>("playersSleepingPercentage", PlayersSleepingPercentage));
			rules.Add(new GameRule<bool>("projectilesCanBreakBlocks", ProjectilesCanBreakBlocks));
			rules.Add(new GameRule<bool>("tntExplosionDropDecay", TntExplosionDropDecay));
			return rules;
		}

		public void BroadcastSound(BlockCoordinates position, LevelSoundEventType sound, int blockId = 0, Player sender = null)
		{
			var packet = McpeLevelSoundEvent.CreateObject();
			packet.position = position;
			// TODO: Wire format is a sound name string since protocol 993; verify the
			// name mapping against BDS traces when the server side is brought to 1001.
			packet.soundId = sound.ToString();
			packet.blockId = blockId;
			RelayBroadcast(sender, packet);
		}
	}

	public class LevelEventArgs : EventArgs
	{
		public Player Player { get; set; }
		public Level Level { get; set; }

		public LevelEventArgs(Player player, Level level)
		{
			Player = player;
			Level = level;
		}
	}

	public class LevelCancelEventArgs : LevelEventArgs
	{
		public bool Cancel { get; set; }

		public LevelCancelEventArgs(Player player, Level level) : base(player, level)
		{
		}
	}

	public class BlockPlaceEventArgs : LevelCancelEventArgs
	{
		public Block TargetBlock { get; private set; }
		public Block ExistingBlock { get; private set; }

		public BlockPlaceEventArgs(Player player, Level level, Block targetBlock, Block existingBlock) : base(player, level)
		{
			TargetBlock = targetBlock;
			ExistingBlock = existingBlock;
		}
	}


	public class BlockBreakEventArgs : LevelCancelEventArgs
	{
		public Block Block { get; private set; }
		public List<Item> Drops { get; private set; }

		public BlockBreakEventArgs(Player player, Level level, Block block, List<Item> drops) : base(player, level)
		{
			Block = block;
			Drops = drops;
		}
	}
}