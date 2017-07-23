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
// The Original Code is Niclas Olofsson.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
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
using System.Threading;
using System.Threading.Tasks;
using fNbt;
using log4net;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Entities;
using MiNET.Entities.World;
using MiNET.Items;
using MiNET.Net;
using MiNET.Sounds;
using MiNET.Utils;

namespace MiNET.Worlds
{
	public class Level
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Level));

		public static readonly BlockCoordinates Up = new BlockCoordinates(0, 1, 0);
		public static readonly BlockCoordinates Down = new BlockCoordinates(0, -1, 0);
		public static readonly BlockCoordinates West = new BlockCoordinates(0, 0, 1);
		public static readonly BlockCoordinates East = new BlockCoordinates(0, 0, -1);
		public static readonly BlockCoordinates South = new BlockCoordinates(1, 0, 0);
		public static readonly BlockCoordinates North = new BlockCoordinates(-1, 0, 0);

		public IWorldProvider WorldProvider { get; protected set; }

		private int _worldDayCycleTime = 24000;

		public PlayerLocation SpawnPoint { get; set; }
		public ConcurrentDictionary<long, Player> Players { get; private set; } //TODO: Need to protect this, not threadsafe
		public ConcurrentDictionary<long, Entity> Entities { get; private set; } //TODO: Need to protect this, not threadsafe
		public List<BlockEntity> BlockEntities { get; private set; } //TODO: Need to protect this, not threadsafe
		public ConcurrentDictionary<BlockCoordinates, long> BlockWithTicks { get; private set; } //TODO: Need to protect this, not threadsafe
		public string LevelId { get; private set; }
		public string LevelName { get; private set; }
		public Dimension Dimension { get; set; } = Dimension.Overworld;

		public GameMode GameMode { get; private set; }
		public bool IsSurvival => GameMode == GameMode.Survival;
		public bool HaveDownfall { get; set; }
		public Difficulty Difficulty { get; set; }
		public bool AutoSmelt { get; set; } = false;
		public long CurrentWorldTime { get; set; }
		public long TickTime { get; set; }
		public long StartTimeInTicks { get; private set; }
		public bool IsWorldTimeStarted { get; set; } = false;
		public bool EnableBlockTicking { get; set; } = false;
		public bool EnableChunkTicking { get; set; } = false;

		public bool AllowBuild { get; set; } = true;
		public bool AllowBreak { get; set; } = true;

		public EntityManager EntityManager { get; private set; }
		public InventoryManager InventoryManager { get; private set; }
		public EntitySpawnManager EntitySpawnManager { get; private set; }

		public int ViewDistance { get; set; }

		public Random Random { get; private set; }

		public Level(LevelManager levelManager, string levelId, IWorldProvider worldProvider, EntityManager entityManager, GameMode gameMode = GameMode.Survival, Difficulty difficulty = Difficulty.Normal, int viewDistance = 11)
		{
			Random = new Random();

			LevelManager = levelManager;
			EntityManager = entityManager;
			InventoryManager = new InventoryManager(this);
			EntitySpawnManager = new EntitySpawnManager(this);
			SpawnPoint = null;
			Players = new ConcurrentDictionary<long, Player>();
			Entities = new ConcurrentDictionary<long, Entity>();
			BlockEntities = new List<BlockEntity>();
			BlockWithTicks = new ConcurrentDictionary<BlockCoordinates, long>();
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
			CurrentWorldTime = WorldProvider.GetTime();
			LevelName = WorldProvider.GetName();

			if (WorldProvider.IsCaching)
			{
				Stopwatch chunkLoading = new Stopwatch();
				chunkLoading.Start();
				// Pre-cache chunks for spawn coordinates
				int i = 0;
				foreach (var chunk in GenerateChunks(new ChunkCoordinates(SpawnPoint), new Dictionary<Tuple<int, int>, McpeWrapper>(), ViewDistance))
				{
					if (chunk != null) i++;
				}
				Log.InfoFormat("World pre-cache {0} chunks completed in {1}ms", i, chunkLoading.ElapsedMilliseconds);
			}

			//if (Dimension == Dimension.Overworld)
			//{
			//	if (Config.GetProperty("CheckForSafeSpawn", false))
			//	{
			//		var height = GetHeight((BlockCoordinates) SpawnPoint);
			//		if (height > SpawnPoint.Y) SpawnPoint.Y = height;
			//		Log.Debug("Checking for safe spawn");
			//	}

			//	NetherLevel = LevelManager.GetDimension(this, Dimension.Nether);
			//	TheEndLevel = LevelManager.GetDimension(this, Dimension.TheEnd);
			//}

			StartTimeInTicks = DateTime.UtcNow.Ticks;

			_tickTimer = new Stopwatch();
			_tickTimer.Restart();
			_tickerHighPrecisionTimer = new HighPrecisionTimer(50, WorldTick);
		}

		private void _tickerHighPrecisionTimer_Tick()
		{
			WorldTick(null);
		}

		private HighPrecisionTimer _tickerHighPrecisionTimer;
		private MultiMediaTimer _mmTickTimer = null;

		public virtual void Close()
		{
			NetherLevel?.Close();
			TheEndLevel?.Close();

			_tickerHighPrecisionTimer.Dispose();

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

			AnvilWorldProvider provider = WorldProvider as AnvilWorldProvider;
			if (provider != null)
			{
				foreach (var chunk in provider._chunkCache)
				{
					chunk.Value?.ClearCache();
				}
			}

			WorldProvider = null;

			Log.Info("Closed level: " + LevelId);
		}

		internal static McpeWrapper CreateMcpeBatch(byte[] bytes)
		{
			return BatchUtils.CreateBatchPacket(bytes, 0, (int) bytes.Length, CompressionLevel.Optimal, true);
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
					SpawnToAll(newPlayer);

					foreach (Entity entity in Entities.Values.ToArray())
					{
						entity.SpawnToPlayers(new[] {newPlayer});
					}
				}

				newPlayer.IsSpawned = spawn;
			}
		}

		public void SpawnToAll(Player newPlayer)
		{
			lock (_playerWriteLock)
			{
				// The player list keeps us from moving this completely to player.
				// It's simply to slow and bad.

				Player[] players = GetAllPlayers();
				List<Player> spawnedPlayers = players.ToList();
				spawnedPlayers.Add(newPlayer);

				Player[] sendList = spawnedPlayers.ToArray();

				McpePlayerList playerListMessage = McpePlayerList.CreateObject();
				playerListMessage.records = new PlayerAddRecords(spawnedPlayers);
				newPlayer.SendPackage(CreateMcpeBatch(playerListMessage.Encode()));
				playerListMessage.records = null;
				playerListMessage.PutPool();

				McpePlayerList playerList = McpePlayerList.CreateObject();
				playerList.records = new PlayerAddRecords {newPlayer};
				RelayBroadcast(newPlayer, sendList, CreateMcpeBatch(playerList.Encode()));
				playerList.records = null;
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
				player.SendPackage(CreateMcpeBatch(playerListMessage.Encode()));
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

		public virtual void BroadcastMessage(string text, MessageType type = MessageType.Chat, Player sender = null, Player[] sendList = null)
		{
			if (type == MessageType.Chat || type == MessageType.Raw)
			{
				foreach (var line in text.Split(new string[] {"\n", Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries))
				{
					McpeText message = McpeText.CreateObject();
					message.type = (byte) type;
					message.source = sender == null ? "" : sender.Username;
					message.message = line;
					RelayBroadcast(sendList, message);
				}
			}
			else
			{
				McpeText message = McpeText.CreateObject();
				message.type = (byte) type;
				message.source = sender == null ? "" : sender.Username;
				message.message = text;
				RelayBroadcast(sendList, message);
			}
		}

		private object _tickSync = new object();
		private Stopwatch _tickTimer = Stopwatch.StartNew();
		public long LastTickProcessingTime = 0;
		public long AvarageTickProcessingTime = 50;
		public int PlayerCount { get; private set; }

		private void WorldTick(object sender)
		{
			if (_tickTimer.ElapsedMilliseconds < 40)
			{
				if (Log.IsDebugEnabled) Log.Warn($"World tick came too fast: {_tickTimer.ElapsedMilliseconds} ms");
				return;
			}

			if (Log.IsDebugEnabled && _tickTimer.ElapsedMilliseconds >= 65) Log.Error($"Time between World tick took too long: {_tickTimer.ElapsedMilliseconds} ms");

			_tickTimer.Restart();
			try
			{
				TickTime++;

				Player[] players = GetSpawnedPlayers();

				if (IsWorldTimeStarted) CurrentWorldTime++;
				if (CurrentWorldTime > _worldDayCycleTime) CurrentWorldTime = 0;
				if (IsWorldTimeStarted && TickTime%100 == 0)
				{
					McpeSetTime message = McpeSetTime.CreateObject();
					message.time = (int) CurrentWorldTime;
					RelayBroadcast(message);
				}

				if (EnableChunkTicking || EnableBlockTicking)
				{
					if (EnableChunkTicking) EntitySpawnManager.DespawnMobs(TickTime);

					List<Tuple<int, int>> chunksWithinRadiusOfPlayer = new List<Tuple<int, int>>();
					foreach (var player in players)
					{
						BlockCoordinates bCoord = (BlockCoordinates) player.KnownPosition;

						chunksWithinRadiusOfPlayer = GetChunkCoordinatesForTick(new ChunkCoordinates(bCoord), chunksWithinRadiusOfPlayer, 8); // Should actually be 15
					}

					ThreadPool.QueueUserWorkItem(state =>
					{
						Parallel.ForEach((List<Tuple<int, int>>) state, coord =>
						{
							var random = new Random();
							for (int s = 0; s < 16; s++)
							{
								for (int i = 0; i < 3; i++)
								{
									int x = random.Next(16);
									int y = random.Next(16);
									int z = random.Next(16);

									var blockCoordinates = new BlockCoordinates(x + coord.Item1*16, y + s*16, z + coord.Item2*16);
									var height = GetHeight(blockCoordinates);
									if (height > 0 && s*16 > height) continue;

									if (IsAir(blockCoordinates))
									{
										if (i == 0 && EnableChunkTicking)
										{
											// Entity spawning, only one attempt per chunk
											var numberOfLoadedChunks = ((List<Tuple<int, int>>) state).Count;
											EntitySpawnManager.AttemptMobSpawn(TickTime, blockCoordinates, numberOfLoadedChunks);
										}

										continue;
									}

									if (EnableBlockTicking)
									{
										GetBlock(blockCoordinates).OnTick(this, true);
									}
								}
							}
						});
					}, chunksWithinRadiusOfPlayer);
				}

				// Block updates
				foreach (KeyValuePair<BlockCoordinates, long> blockEvent in BlockWithTicks)
				{
					try
					{
						if (blockEvent.Value <= TickTime)
						{
							long value;
							BlockWithTicks.TryRemove(blockEvent.Key, out value);
							GetBlock(blockEvent.Key).OnTick(this, false);
						}
					}
					catch (Exception e)
					{
						Log.Warn("Block ticking", e);
					}
				}

				// Block entity updates
				foreach (BlockEntity blockEntity in BlockEntities.ToArray())
				{
					blockEntity.OnTick(this);
				}

				// Entity updates
				Entity[] entities = Entities.Values.ToArray();
				foreach (Entity entity in entities)
				{
					entity.OnTick();
				}

				PlayerCount = players.Length;

				// Player tick
				foreach (var player in players)
				{
					if (player.IsSpawned) player.OnTick();
				}

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
				AvarageTickProcessingTime = ((AvarageTickProcessingTime*9) + _tickTimer.ElapsedMilliseconds)/10L;
			}
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

			using (MemoryStream stream = MiNetServer.MemoryStreamManager.GetStream())
			{
				int playerMoveCount = 0;
				int entiyMoveCount = 0;

				foreach (var player in players)
				{
					if (now - player.LastUpdatedTime <= now - lastSendTime)
					{
						PlayerLocation knownPosition = player.KnownPosition;

						McpeMovePlayer move = McpeMovePlayer.CreateObject();
						move.runtimeEntityId = player.EntityId;
						move.x = knownPosition.X;
						move.y = knownPosition.Y + 1.62f;
						move.z = knownPosition.Z;
						move.yaw = knownPosition.Yaw;
						move.pitch = knownPosition.Pitch;
						move.headYaw = knownPosition.HeadYaw;
						move.mode = 0;
						byte[] bytes = move.Encode();
						BatchUtils.WriteLength(stream, bytes.Length);
						stream.Write(bytes, 0, bytes.Length);
						move.PutPool();
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

				McpeWrapper batch = BatchUtils.CreateBatchPacket(stream.GetBuffer(), 0, (int) stream.Length, CompressionLevel.Optimal, false);
				batch.AddReferences(players.Length - 1);
				batch.Encode();
				foreach (var player in players)
				{
					MiNetServer.FastThreadPool.QueueUserWorkItem(() => player.SendPackage(batch));
				}
				_lastBroadcast = DateTime.UtcNow;
			}
		}

		public void RelayBroadcast<T>(T message) where T : Package<T>, new()
		{
			RelayBroadcast(null, GetAllPlayers(), message);
		}

		public void RelayBroadcast<T>(Player source, T message) where T : Package<T>, new()
		{
			RelayBroadcast(source, GetAllPlayers(), message);
		}

		public void RelayBroadcast<T>(Player[] sendList, T message) where T : Package<T>, new()
		{
			RelayBroadcast(null, sendList ?? GetAllPlayers(), message);
		}

		public void RelayBroadcast<T>(Player source, Player[] sendList, T message) where T : Package<T>, new()
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

				player.SendPackage(message);
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

					player.SendPackage(message);
				});
			}
		}

		public List<Tuple<int, int>> GetChunkCoordinatesForTick(ChunkCoordinates chunkPosition, List<Tuple<int, int>> chunksUsed, double radius)
		{
			{
				List<Tuple<int, int>> newOrders = new List<Tuple<int, int>>();

				double radiusSquared = Math.Pow(radius, 2);

				int centerX = chunkPosition.X;
				int centerZ = chunkPosition.Z;

				for (double x = -radius; x <= radius; ++x)
				{
					for (double z = -radius; z <= radius; ++z)
					{
						var distance = (x*x) + (z*z);
						if (distance > radiusSquared)
						{
							continue;
						}
						int chunkX = (int) (x + centerX);
						int chunkZ = (int) (z + centerZ);
						Tuple<int, int> index = new Tuple<int, int>(chunkX, chunkZ);
						newOrders.Add(index);
					}
				}

				return newOrders.Union(chunksUsed).ToList();
			}
		}

		public IEnumerable<McpeWrapper> GenerateChunks(ChunkCoordinates chunkPosition, Dictionary<Tuple<int, int>, McpeWrapper> chunksUsed, double radius)
		{
			lock (chunksUsed)
			{
				Dictionary<Tuple<int, int>, double> newOrders = new Dictionary<Tuple<int, int>, double>();

				double radiusSquared = Math.Pow(radius, 2);

				int centerX = chunkPosition.X;
				int centerZ = chunkPosition.Z;

				for (double x = -radius; x <= radius; ++x)
				{
					for (double z = -radius; z <= radius; ++z)
					{
						var distance = (x*x) + (z*z);
						if (distance > radiusSquared)
						{
							continue;
						}
						int chunkX = (int) (x + centerX);
						int chunkZ = (int) (z + centerZ);
						Tuple<int, int> index = new Tuple<int, int>(chunkX, chunkZ);
						newOrders[index] = distance;
					}
				}

				//if (newOrders.Count > viewArea)
				//{
				//	foreach (var pair in newOrders.OrderByDescending(pair => pair.Value))
				//	{
				//		if (newOrders.Count <= viewArea) break;
				//		newOrders.Remove(pair.Key);
				//	}
				//}

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

					ChunkColumn chunkColumn = WorldProvider.GenerateChunkColumn(new ChunkCoordinates(pair.Key.Item1, pair.Key.Item2));
					McpeWrapper chunk = null;
					if (chunkColumn != null)
					{
						chunk = chunkColumn.GetBatch();
					}

					chunksUsed.Add(pair.Key, chunk);

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
			if (tryChunk != null && tryChunk.x == chunkCoordinates.X && tryChunk.z == chunkCoordinates.Z)
			{
				chunk = tryChunk;
			}
			else
			{
				chunk = WorldProvider.GenerateChunkColumn(chunkCoordinates);
			}
			if (chunk == null) return new Air {Coordinates = blockCoordinates, SkyLight = 15};

			byte bid = chunk.GetBlock(blockCoordinates.X & 0x0f, blockCoordinates.Y & 0xff, blockCoordinates.Z & 0x0f);
			byte metadata = chunk.GetMetadata(blockCoordinates.X & 0x0f, blockCoordinates.Y & 0xff, blockCoordinates.Z & 0x0f);
			byte blockLight = chunk.GetBlocklight(blockCoordinates.X & 0x0f, blockCoordinates.Y & 0xff, blockCoordinates.Z & 0x0f);
			byte skyLight = chunk.GetSkylight(blockCoordinates.X & 0x0f, blockCoordinates.Y & 0xff, blockCoordinates.Z & 0x0f);
			byte biomeId = chunk.GetBiome(blockCoordinates.X & 0x0f, blockCoordinates.Z & 0x0f);

			Block block = BlockFactory.GetBlockById(bid);
			block.Coordinates = blockCoordinates;
			block.Metadata = metadata;
			block.BlockLight = blockLight;
			block.SkyLight = skyLight;
			block.BiomeId = biomeId;

			return block;
		}

		public bool IsBlock(BlockCoordinates blockCoordinates, int blockId)
		{
			ChunkColumn chunk = GetChunk(blockCoordinates);
			if (chunk == null) return false;

			return chunk.GetBlock(blockCoordinates.X & 0x0f, blockCoordinates.Y & 0xff, blockCoordinates.Z & 0x0f) == blockId;
		}

		public bool IsAir(BlockCoordinates blockCoordinates)
		{
			ChunkColumn chunk = GetChunk(blockCoordinates);
			if (chunk == null) return true;

			byte bid = chunk.GetBlock(blockCoordinates.X & 0x0f, blockCoordinates.Y & 0xff, blockCoordinates.Z & 0x0f);
			return bid == 0;
			//return bid == 0 || bid == 20 || bid == 241; // Need this for skylight calculations. Revise!
		}

		public bool IsNotBlockingSkylight(BlockCoordinates blockCoordinates)
		{
			ChunkColumn chunk = GetChunk(blockCoordinates);
			if (chunk == null) return true;

			byte bid = chunk.GetBlock(blockCoordinates.X & 0x0f, blockCoordinates.Y & 0xff, blockCoordinates.Z & 0x0f);
			return bid == 0 || bid == 20 || bid == 241; // Need this for skylight calculations. Revise!
		}

		public bool IsTransparent(BlockCoordinates blockCoordinates)
		{
			ChunkColumn chunk = GetChunk(blockCoordinates);
			if (chunk == null) return true;

			byte bid = chunk.GetBlock(blockCoordinates.X & 0x0f, blockCoordinates.Y & 0xff, blockCoordinates.Z & 0x0f);
			return BlockFactory.TransparentBlocks.Contains(bid);
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

			return chunk.GetSkylight(blockCoordinates.X & 0x0f, blockCoordinates.Y & 0xff, blockCoordinates.Z & 0x0f);
		}

		public byte GetBlockLight(BlockCoordinates blockCoordinates)
		{
			ChunkColumn chunk = GetChunk(blockCoordinates);

			if (chunk == null) return 15;

			return chunk.GetBlocklight(blockCoordinates.X & 0x0f, blockCoordinates.Y & 0xff, blockCoordinates.Z & 0x0f);
		}

		public ChunkColumn GetChunk(BlockCoordinates blockCoordinates)
		{
			return GetChunk((ChunkCoordinates) blockCoordinates);
		}

		public ChunkColumn GetChunk(ChunkCoordinates chunkCoordinates)
		{
			return WorldProvider.GenerateChunkColumn(chunkCoordinates);
		}

		public void SetBlock(int x, int y, int z, int blockId, int metadata = 0, bool broadcast = true, bool applyPhysics = true, bool calculateLight = true)
		{
			Block block = BlockFactory.GetBlockById((byte) blockId);
			block.Coordinates = new BlockCoordinates(x, y, z);
			block.Metadata = (byte) metadata;
			SetBlock(block, broadcast, applyPhysics, calculateLight);
		}


		public void SetBlock(Block block, bool broadcast = true, bool applyPhysics = true, bool calculateLight = true)
		{
			if (block.Coordinates.Y < 0) return;

			ChunkColumn chunk = WorldProvider.GenerateChunkColumn(new ChunkCoordinates(block.Coordinates.X >> 4, block.Coordinates.Z >> 4));
			chunk.SetBlock(block.Coordinates.X & 0x0f, block.Coordinates.Y & 0xff, block.Coordinates.Z & 0x0f, block.Id);
			chunk.SetMetadata(block.Coordinates.X & 0x0f, block.Coordinates.Y & 0xff, block.Coordinates.Z & 0x0f, block.Metadata);

			if (applyPhysics) ApplyPhysics(block.Coordinates.X, block.Coordinates.Y, block.Coordinates.Z);

			if (calculateLight /* && block.LightLevel > 0*/)
			{
				new SkyLightCalculations().Calculate(this, block.Coordinates);

				block.BlockLight = (byte) block.LightLevel;
				chunk.SetBlocklight(block.Coordinates.X & 0x0f, block.Coordinates.Y & 0xff, block.Coordinates.Z & 0x0f, (byte) block.LightLevel);
				BlockLightCalculations.Calculate(this, block.Coordinates);
			}

			if (!broadcast) return;

			var message = McpeUpdateBlock.CreateObject();
			message.blockId = block.Id;
			message.coordinates = block.Coordinates;
			message.blockMetaAndPriority = (byte) (0xb << 4 | (block.Metadata & 0xf));
			RelayBroadcast(message);
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
			ChunkColumn chunk = WorldProvider.GenerateChunkColumn(new ChunkCoordinates(block.Coordinates.X >> 4, block.Coordinates.Z >> 4));
			chunk.SetBlocklight(block.Coordinates.X & 0x0f, block.Coordinates.Y & 0xff, block.Coordinates.Z & 0x0f, block.BlockLight);
		}

		public void SetBlockLight(BlockCoordinates coordinates, byte blockLight)
		{
			ChunkColumn chunk = GetChunk(coordinates);
			chunk?.SetBlocklight(coordinates.X & 0x0f, coordinates.Y & 0xff, coordinates.Z & 0x0f, blockLight);
		}

		public void SetSkyLight(Block block)
		{
			ChunkColumn chunk = WorldProvider.GenerateChunkColumn(new ChunkCoordinates(block.Coordinates.X >> 4, block.Coordinates.Z >> 4));
			chunk.SetSkyLight(block.Coordinates.X & 0x0f, block.Coordinates.Y & 0xff, block.Coordinates.Z & 0x0f, block.SkyLight);
		}

		public void SetSkyLight(BlockCoordinates coordinates, byte skyLight)
		{
			ChunkColumn chunk = GetChunk(coordinates);
			chunk?.SetSkyLight(coordinates.X & 0x0f, coordinates.Y & 0xff, coordinates.Z & 0x0f, skyLight);
		}

		public void SetAir(BlockCoordinates blockCoordinates, bool broadcast = true)
		{
			SetAir(blockCoordinates.X, blockCoordinates.Y, blockCoordinates.Z, broadcast);
		}

		public void SetAir(int x, int y, int z, bool broadcast = true)
		{
			Block air = BlockFactory.GetBlockById(0);
			air.Metadata = 0;
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

			ChunkColumn chunk = WorldProvider.GenerateChunkColumn(new ChunkCoordinates(blockCoordinates.X >> 4, blockCoordinates.Z >> 4));

			NbtCompound nbt = chunk?.GetBlockEntity(blockCoordinates);
			if (nbt == null) return null;

			string id = null;
			var idTag = nbt.Get("id");
			if (idTag != null)
			{
				id = idTag.StringValue;
			}

			if (string.IsNullOrEmpty(id)) return null;

			blockEntity = BlockEntityFactory.GetBlockEntityById(id);
			if (blockEntity == null) return null;

			blockEntity.Coordinates = blockCoordinates;
			blockEntity.SetCompound(nbt);

			return blockEntity;
		}

		public void SetBlockEntity(BlockEntity blockEntity, bool broadcast = true)
		{
			ChunkColumn chunk = WorldProvider.GenerateChunkColumn(new ChunkCoordinates(blockEntity.Coordinates.X >> 4, blockEntity.Coordinates.Z >> 4));
			chunk.SetBlockEntity(blockEntity.Coordinates, blockEntity.GetCompound());

			if (blockEntity.UpdatesOnTick) BlockEntities.Add(blockEntity);

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

			var entityData = McpeBlockEntityData.CreateObject();
			entityData.namedtag = nbt;
			entityData.coordinates = blockEntity.Coordinates;

			RelayBroadcast(entityData);
		}

		public void RemoveBlockEntity(BlockCoordinates blockCoordinates)
		{
			ChunkColumn chunk = WorldProvider.GenerateChunkColumn(new ChunkCoordinates(blockCoordinates.X >> 4, blockCoordinates.Z >> 4));
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

		protected virtual bool OnBlockPlace(BlockPlaceEventArgs e)
		{
			BlockPlace?.Invoke(this, e);

			return !e.Cancel;
		}

		public void Interact(Player player, Item itemInHand, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			Block target = GetBlock(blockCoordinates);
			if (target.Interact(this, player, blockCoordinates, face, faceCoords)) return; // Handled in block interaction

			if (itemInHand is ItemBlock)
			{
				Block block = GetBlock(blockCoordinates);
				if (!block.IsReplacible)
				{
					block = GetBlock(itemInHand.GetNewCoordinatesFromFace(blockCoordinates, face));
				}

				if (!AllowBuild || player.GameMode == GameMode.Spectator || !OnBlockPlace(new BlockPlaceEventArgs(player, this, target, block)))
				{
					// Revert

					player.SendPlayerInventory();

					var message = McpeUpdateBlock.CreateObject();
					message.blockId = block.Id;
					message.coordinates = block.Coordinates;
					message.blockMetaAndPriority = (byte) (0xb << 4 | (block.Metadata & 0xf));
					player.SendPackage(message);

					return;
				}
			}

			itemInHand.UseItem(this, player, blockCoordinates, face, faceCoords);
		}

		public event EventHandler<BlockBreakEventArgs> BlockBreak;

		protected virtual bool OnBlockBreak(BlockBreakEventArgs e)
		{
			BlockBreak?.Invoke(this, e);

			return !e.Cancel;
		}

		public void BreakBlock(Player player, BlockCoordinates blockCoordinates)
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
				BreakBlock(block, blockEntity, inHand);

				player.HungerManager.IncreaseExhaustion(0.025f);
				player.AddExperience(block.GetExperiencePoints());
			}
		}

		private static void RevertBlockAction(Player player, Block block, BlockEntity blockEntity)
		{
			var message = McpeUpdateBlock.CreateObject();
			message.blockId = block.Id;
			message.coordinates = block.Coordinates;
			message.blockMetaAndPriority = (byte) (0xb << 4 | (block.Metadata & 0xf));
			player.SendPackage(message);

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

				player.SendPackage(entityData);
			}
		}

		public void BreakBlock(Block block, BlockEntity blockEntity = null, Item tool = null)
		{
			block.BreakBlock(this);
			List<Item> drops = new List<Item>();
			drops.AddRange(block.GetDrops(tool ?? new ItemAir()));

			if (blockEntity != null)
			{
				RemoveBlockEntity(block.Coordinates);
				drops.AddRange(blockEntity.GetDrops());
			}

			foreach (Item drop in drops)
			{
				DropItem(block.Coordinates, drop);
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
				Velocity = new Vector3((float) (random.NextDouble()*0.005), (float) (random.NextDouble()*0.20), (float) (random.NextDouble()*0.005))
			};

			itemEntity.SpawnEntity();
		}

		public void SetData(BlockCoordinates coordinates, byte meta)
		{
			Block block = GetBlock(coordinates);
			block.Metadata = meta;
			SetBlock(block, applyPhysics: false);
		}

		public void SetData(int x, int y, int z, byte meta)
		{
			SetData(new BlockCoordinates(x, y, z), meta);
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
			BlockWithTicks[block.Coordinates] = TickTime + tickRate;
		}

		public Entity GetEntity(long targetEntityId)
		{
			Player entity;
			Players.TryGetValue(targetEntityId, out entity);

			return entity ?? Entities.Values.FirstOrDefault(e => e.EntityId == targetEntityId);
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
			new Lightning(this)
			{
				KnownPosition = new PlayerLocation(position)
			}.SpawnEntity();
		}

		public void MakeSound(Sound sound)
		{
			sound.Spawn(this);
		}
	}

	public class LevelEventArgs : EventArgs
	{
		public Player Player { get; set; }
		public Level Level { get; set; }

		public bool Cancel { get; set; }

		public LevelEventArgs(Player player, Level level)
		{
			Player = player;
			Level = level;
		}
	}

	public class BlockPlaceEventArgs : LevelEventArgs
	{
		public Block TargetBlock { get; private set; }
		public Block ExistingBlock { get; private set; }

		public BlockPlaceEventArgs(Player player, Level level, Block targetBlock, Block existingBlock) : base(player, level)
		{
			TargetBlock = targetBlock;
			ExistingBlock = existingBlock;
		}
	}


	public class BlockBreakEventArgs : LevelEventArgs
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