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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Numerics;
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
using MiNET.Net.RakNet;
using MiNET.Sounds;
using MiNET.Utils;
using MiNET.Utils.Diagnostics;
using MiNET.Utils.IO;
using MiNET.Utils.Nbt;
using MiNET.Utils.Vectors;

namespace MiNET.Worlds
{
	public class Level : IBlockAccess
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Level));

		public static readonly BlockCoordinates Up = new BlockCoordinates(0, 1, 0);
		public static readonly BlockCoordinates Down = new BlockCoordinates(0, -1, 0);
		public static readonly BlockCoordinates South = new BlockCoordinates(0, 0, 1);
		public static readonly BlockCoordinates North = new BlockCoordinates(0, 0, -1);
		public static readonly BlockCoordinates East = new BlockCoordinates(1, 0, 0);
		public static readonly BlockCoordinates West = new BlockCoordinates(-1, 0, 0);

		public IWorldProvider WorldProvider { get; set; }

		private int _worldDayCycleTime = 24000;

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
		public long WorldTime { get; set; }
		public long CurrentWorldCycleTime { get; private set; }
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

		public int ViewDistance { get; set; }

		public Random Random { get; private set; }

		public int SaveInterval { get; set; } = 300;
		public int UnloadInterval { get; set; } = -1;

		public Level(LevelManager levelManager, string levelId, IWorldProvider worldProvider, EntityManager entityManager, GameMode gameMode = GameMode.Survival, Difficulty difficulty = Difficulty.Normal, int viewDistance = 11)
		{
			Random = new Random();

			LevelManager = levelManager;
			EntityManager = entityManager;
			InventoryManager = new InventoryManager(this);
			EntitySpawnManager = new EntitySpawnManager(this);
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
			WorldTime = WorldProvider.GetDayTime();
			LevelName = WorldProvider.GetName();

			if (WorldProvider.IsCaching)
			{
				Stopwatch chunkLoading = Stopwatch.StartNew();

				// Pre-cache chunks for spawn coordinates
				int i = 0;
				if (Dimension == Dimension.Nether)
				{
				}
				var chunkCoordinates = new ChunkCoordinates(SpawnPoint) / 8;
				foreach (var chunk in GenerateChunks(chunkCoordinates, new Dictionary<ChunkCoordinates, McpeWrapper>(), ViewDistance))
				{
					if (chunk != null) i++;
				}

				Log.Info($"World pre-cache {i} chunks completed in {chunkLoading.ElapsedMilliseconds}ms");
			}

			if (Dimension == Dimension.Overworld)
			{
				if (Config.GetProperty("CheckForSafeSpawn", false))
				{
					var height = GetHeight((BlockCoordinates) SpawnPoint);
					if (height > SpawnPoint.Y) SpawnPoint.Y = height;
					Log.Debug("Checking for safe spawn");
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

			_tickTimer = new Stopwatch();
			_tickTimer.Restart();
			_tickerHighPrecisionTimer = new HighPrecisionTimer(50, WorldTick, false, false);
		}

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

		internal static McpeWrapper CreateMcpeBatch(byte[] bytes)
		{
			return BatchUtils.CreateBatchPacket(new Memory<byte>(bytes, 0, (int) bytes.Length), CompressionLevel.Optimal, true);
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

				Player[] players = GetAllPlayers();
				var spawnedPlayers = players.ToList();
				spawnedPlayers.Add(newPlayer);

				Player[] sendList = spawnedPlayers.ToArray();

				var playerListMessage = McpePlayerList.CreateObject();
				playerListMessage.records = new PlayerAddRecords(spawnedPlayers);
				newPlayer.SendPacket(CreateMcpeBatch(playerListMessage.Encode()));
				playerListMessage.PutPool();

				var playerList = McpePlayerList.CreateObject();
				playerList.records = new PlayerAddRecords {newPlayer};
				RelayBroadcast(newPlayer, sendList, CreateMcpeBatch(playerList.Encode()));
				playerList.PutPool();

				newPlayer.SpawnToPlayers(players);

				foreach (Player spawnedPlayer in players)
				{
					spawnedPlayer.SpawnToPlayers(new[] {newPlayer});
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
				}
			}

			OnPlayerRemoved(new LevelEventArgs(player, this));
		}

		public void DespawnFromAll(Player player)
		{
			lock (_playerWriteLock)
			{
				var spawnedPlayers = GetAllPlayers();

				foreach (Player spawnedPlayer in spawnedPlayers)
				{
					spawnedPlayer.DespawnFromPlayers(new[] {player});
				}

				player.DespawnFromPlayers(spawnedPlayers);

				McpePlayerList playerListMessage = McpePlayerList.CreateObject();
				playerListMessage.records = new PlayerRemoveRecords(spawnedPlayers);
				player.SendPacket(CreateMcpeBatch(playerListMessage.Encode()));
				playerListMessage.records = null;
				playerListMessage.PutPool();

				McpePlayerList playerList = McpePlayerList.CreateObject();
				playerList.records = new PlayerRemoveRecords {player};
				RelayBroadcast(player, CreateMcpeBatch(playerList.Encode()));
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


		public void RemoveDuplicatePlayers(string username, long clientId)
		{
			//var existingPlayers = Players.Where(player => player.Value.ClientId == clientId && player.Value.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));

			//foreach (var existingPlayer in existingPlayers)
			//{
			//	Log.InfoFormat("Removing staled players on login {0}", username);
			//	existingPlayer.Value.Disconnect("Duplicate player. Crashed.", false);
			//}
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

			if (Log.IsDebugEnabled && _tickTimer.ElapsedMilliseconds >= 65) Log.Warn($"Time between world tick too long: {_tickTimer.ElapsedMilliseconds} ms. Last processing time={LastTickProcessingTime}, Avarage={AvarageTickProcessingTime}");

			Measurement worldTickMeasurement = _profiler.Begin("World tick");

			_tickTimer.Restart();

			try
			{
				TickTime++;

				Player[] players = GetSpawnedPlayers();

				if (DoDaylightcycle)
				{
					WorldTime++;
				}

				CurrentWorldCycleTime = WorldTime % _worldDayCycleTime;

				if (DoDaylightcycle && TickTime % 100 == 0)
				{
					McpeSetTime message = McpeSetTime.CreateObject();
					message.time = (int) WorldTime;
					RelayBroadcast(message);
				}

				SkylightSubtracted = CalculateSkylightSubtracted(WorldTime);

				// Save dirty chunks
				if (TickTime % (SaveInterval * 20) == 0)
				{
					WorldProvider.SaveChunks();
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

				//TODO: We don't want to trigger sending here. But right now
				// it seems better for performance since the send-tick is one for all
				// sessions, so we need to refactor that first.
				var tasks = new List<Task>();
				foreach (Player player in players)
				{
					if (player.NetworkHandler is RakSession session) tasks.Add(session.SendQueueAsync());
				}
				Task.WhenAll(tasks).Wait();

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

		protected virtual void BroadCastMovement(Player[] players, Entity[] entities)
		{
			DateTime now = DateTime.UtcNow;

			if (players.Length == 0) return;

			if (players.Length <= 1 && entities.Length == 0) return;

			//if (now - _lastBroadcast < TimeSpan.FromMilliseconds(50)) return;

			DateTime lastSendTime = _lastSendTime;
			_lastSendTime = DateTime.UtcNow;

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
						move.x = knownPosition.X;
						move.y = knownPosition.Y + 1.62f;
						move.z = knownPosition.Z;
						move.pitch = knownPosition.Pitch;
						move.yaw = knownPosition.Yaw;
						move.headYaw = knownPosition.HeadYaw;
						move.mode = (byte) (player.Vehicle == 0 ? 0 : 3);
						move.onGround = !player.IsGliding && player.IsOnGround;
						move.otherRuntimeEntityId = player.Vehicle;
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
				batch.ReliabilityHeader.Reliability = Reliability.ReliableOrdered;
				batch.payload = Compression.CompressPacketsForWrapper(movePackets);
				batch.Encode();
				foreach (Player player in players) MiNetServer.FastThreadPool.QueueUserWorkItem(() => player.SendPacket(batch));
				_lastBroadcast = DateTime.UtcNow;
			}
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

		public IEnumerable<McpeWrapper> GenerateChunks(ChunkCoordinates chunkPosition, Dictionary<ChunkCoordinates, McpeWrapper> chunksUsed, double radius, Func<Vector3> getCurrentPositionAction = null)
		{
			lock (chunksUsed)
			{
				var newOrders = new Dictionary<ChunkCoordinates, double>();

				double radiusSquared = Math.Pow(radius, 2);

				int centerX = chunkPosition.X;
				int centerZ = chunkPosition.Z;

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
						newOrders[index] = distance;
					}
				}

				foreach (var chunkKey in chunksUsed.Keys.ToArray())
				{
					if (!newOrders.ContainsKey(chunkKey))
					{
						chunksUsed.Remove(chunkKey);
					}
				}

				foreach (var pair in newOrders.OrderBy(pair => pair.Value))
				{
					if (chunksUsed.ContainsKey(pair.Key)) continue;

					if (WorldProvider == null) continue;

					if (getCurrentPositionAction != null)
					{
						var currentPos = getCurrentPositionAction();
						var coords = new ChunkCoordinates(currentPos);
						if(coords.DistanceTo(pair.Key) > radius) continue;
					}
					ChunkColumn chunkColumn = GetChunk(pair.Key);
					McpeWrapper chunk = null;
					if (chunkColumn != null)
					{
						chunk = chunkColumn.GetBatch();
						chunksUsed.Add(pair.Key, chunk);
					}

					yield return chunk;
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
			if (chunk == null)
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
			if (chunk == null) return false;

			return chunk.GetBlockId(blockCoordinates.X & 0x0f, blockCoordinates.Y, blockCoordinates.Z & 0x0f) == blockId;
		}

		public bool IsAir(BlockCoordinates blockCoordinates)
		{
			ChunkColumn chunk = GetChunk(blockCoordinates);
			if (chunk == null) return true;

			int bid = chunk.GetBlockId(blockCoordinates.X & 0x0f, blockCoordinates.Y, blockCoordinates.Z & 0x0f);
			return bid == 0;
			//return bid == 0 || bid == 20 || bid == 241; // Need this for skylight calculations. Revise!
		}

		public bool IsNotBlockingSkylight(BlockCoordinates blockCoordinates)
		{
			ChunkColumn chunk = GetChunk(blockCoordinates);
			if (chunk == null) return true;

			int bid = chunk.GetBlockId(blockCoordinates.X & 0x0f, blockCoordinates.Y, blockCoordinates.Z & 0x0f);
			return bid == 0 || bid == 20 || bid == 241; // Need this for skylight calculations. Revise!
		}

		public bool IsTransparent(BlockCoordinates blockCoordinates)
		{
			ChunkColumn chunk = GetChunk(blockCoordinates);
			if (chunk == null) return true;

			int bid = chunk.GetBlockId(blockCoordinates.X & 0x0f, blockCoordinates.Y, blockCoordinates.Z & 0x0f);
			return BlockFactory.TransparentBlocks[bid] == 1;
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

			if (chunk == null) return 15;

			return chunk.GetSkylight(blockCoordinates.X & 0x0f, blockCoordinates.Y, blockCoordinates.Z & 0x0f);
		}

		public byte GetBlockLight(BlockCoordinates blockCoordinates)
		{
			ChunkColumn chunk = GetChunk(blockCoordinates);

			if (chunk == null) return 15;

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
			var chunk = WorldProvider.GenerateChunkColumn(chunkCoordinates, cacheOnly);
			if (chunk == null) Log.Debug($"Got <null> chunk at {chunkCoordinates}");
			return chunk;
		}

		public void SetBlock(Block block, bool broadcast = true, bool applyPhysics = true, bool calculateLight = true, ChunkColumn possibleChunk = null)
		{
			if (block.Coordinates.Y < 0) return;

			var chunkCoordinates = new ChunkCoordinates(block.Coordinates.X >> 4, block.Coordinates.Z >> 4);
			ChunkColumn chunk = possibleChunk != null && possibleChunk.X == chunkCoordinates.X && possibleChunk.Z == chunkCoordinates.Z ? possibleChunk : GetChunk(chunkCoordinates);


			if (!OnBlockPlace(new BlockPlaceEventArgs(null, this, block, null)))
			{
				return;
			}

			chunk.SetBlock(block.Coordinates.X & 0x0f, block.Coordinates.Y, block.Coordinates.Z & 0x0f, block);
			if (calculateLight && chunk.GetHeight(block.Coordinates.X & 0x0f, block.Coordinates.Z & 0x0f) <= block.Coordinates.Y + 1)
			{
				chunk.RecalcHeight(block.Coordinates.X & 0x0f, block.Coordinates.Z & 0x0f, Math.Min(ChunkColumn.WorldHeight, block.Coordinates.Y + 1));
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
				message.blockRuntimeId = (uint) block.GetRuntimeId();
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
			Block air = BlockFactory.GetBlockById(0);
			air.Coordinates = new BlockCoordinates(x, y, z);
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
					message.blockRuntimeId = (uint) block.GetRuntimeId();
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
			message.blockRuntimeId = (uint) block.GetRuntimeId();
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
			if (drop.Id == 0) return;
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

		public void StrikeLightning(Vector3 position)
		{
			new Lightning(this) {KnownPosition = new PlayerLocation(position)}.SpawnEntity();
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
		public int RandomTickSpeed { get; set; } = 3;

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
			}
		}

		public void SetGameRule(GameRulesEnum rule, int value)
		{
			switch (rule)
			{
				case GameRulesEnum.DrowningDamage:
					RandomTickSpeed = value;
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
			}

			return false;
		}

		public virtual GameRules GetGameRules()
		{
			GameRules rules = new GameRules();
			rules.Add(new GameRule<bool>(GameRulesEnum.DrowningDamage, DrowningDamage));
			rules.Add(new GameRule<bool>(GameRulesEnum.CommandblockOutput, CommandblockOutput));
			rules.Add(new GameRule<bool>(GameRulesEnum.DoTiledrops, DoTiledrops));
			rules.Add(new GameRule<bool>(GameRulesEnum.DoMobloot, DoMobloot));
			rules.Add(new GameRule<bool>(GameRulesEnum.KeepInventory, KeepInventory));
			rules.Add(new GameRule<bool>(GameRulesEnum.DoDaylightcycle, DoDaylightcycle));
			rules.Add(new GameRule<bool>(GameRulesEnum.DoMobspawning, DoMobspawning));
			rules.Add(new GameRule<bool>(GameRulesEnum.DoEntitydrops, DoEntitydrops));
			rules.Add(new GameRule<bool>(GameRulesEnum.DoFiretick, DoFiretick));
			rules.Add(new GameRule<bool>(GameRulesEnum.DoWeathercycle, DoWeathercycle));
			rules.Add(new GameRule<bool>(GameRulesEnum.Pvp, Pvp));
			rules.Add(new GameRule<bool>(GameRulesEnum.Falldamage, Falldamage));
			rules.Add(new GameRule<bool>(GameRulesEnum.Firedamage, Firedamage));
			rules.Add(new GameRule<bool>(GameRulesEnum.Mobgriefing, Mobgriefing));
			rules.Add(new GameRule<bool>(GameRulesEnum.ShowCoordinates, ShowCoordinates));
			rules.Add(new GameRule<bool>(GameRulesEnum.NaturalRegeneration, NaturalRegeneration));
			rules.Add(new GameRule<bool>(GameRulesEnum.TntExplodes, TntExplodes));
			rules.Add(new GameRule<bool>(GameRulesEnum.SendCommandfeedback, SendCommandfeedback));
			rules.Add(new GameRule<bool>(GameRulesEnum.ExperimentalGameplay, true));
			return rules;
		}

		public void BroadcastSound(BlockCoordinates position, LevelSoundEventType sound, int blockId = 0, Player sender = null)
		{
			var packet = McpeLevelSoundEvent.CreateObject();
			packet.position = position;
			packet.soundId = (uint) sound;
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