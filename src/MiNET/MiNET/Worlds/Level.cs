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

		public IWorldProvider _worldProvider;
		// ReSharper disable once NotAccessedField.Local
		private Timer _levelTicker;
		private int _worldTickTime = 50;
		private int _worldDayCycleTime = 19200;
		//private int _worldDayCycleTime = 14400;

		public PlayerLocation SpawnPoint { get; set; }
		public ConcurrentDictionary<long, Player> Players { get; private set; } //TODO: Need to protect this, not threadsafe
		public ConcurrentDictionary<long, Entity> Entities { get; private set; } //TODO: Need to protect this, not threadsafe
		public List<BlockEntity> BlockEntities { get; private set; } //TODO: Need to protect this, not threadsafe
		public ConcurrentDictionary<BlockCoordinates, long> BlockWithTicks { get; private set; } //TODO: Need to protect this, not threadsafe
		public string LevelId { get; private set; }

		public GameMode GameMode { get; private set; }
		public bool IsSurvival => GameMode == GameMode.Survival;
		public bool HaveDownfall { get; set; }
		public Difficulty Difficulty { get; private set; }
		public double CurrentWorldTime { get; set; }
		public long TickTime { get; set; }
		public long StartTimeInTicks { get; private set; }
		public bool IsWorldTimeStarted { get; set; }
		public bool EnableBlockTicking { get; set; } = false;

		public bool AllowBuild { get; set; } = true;
		public bool AllowBreak { get; set; } = true;

		public EntityManager EntityManager { get; private set; }
		public InventoryManager InventoryManager { get; private set; }

		public int ViewDistance { get; set; }

		public Random Random { get; private set; }

		public Level(string levelId, IWorldProvider worldProvider, EntityManager entityManager, GameMode gameMode = GameMode.Survival, Difficulty difficulty = Difficulty.Normal, int viewDistance = 11)
		{
			Random = new Random();

			EntityManager = entityManager;
			InventoryManager = new InventoryManager(this);
			SpawnPoint = null;
			Players = new ConcurrentDictionary<long, Player>();
			Entities = new ConcurrentDictionary<long, Entity>();
			BlockEntities = new List<BlockEntity>();
			BlockWithTicks = new ConcurrentDictionary<BlockCoordinates, long>();
			LevelId = levelId;
			GameMode = gameMode;
			Difficulty = difficulty;
			ViewDistance = viewDistance;
			_worldProvider = worldProvider;
		}

		public void Initialize()
		{
			//IsWorldTimeStarted = false;
			_worldProvider.Initialize();

			SpawnPoint = SpawnPoint ?? new PlayerLocation(_worldProvider.GetSpawnPoint());
			CurrentWorldTime = _worldProvider.GetTime();

			if (_worldProvider.IsCaching)
			{
				Stopwatch chunkLoading = new Stopwatch();
				chunkLoading.Start();
				// Pre-cache chunks for spawn coordinates
				int i = 0;
				foreach (var chunk in GenerateChunks(new ChunkCoordinates(SpawnPoint), new Dictionary<Tuple<int, int>, McpeBatch>(), ViewDistance))
				{
					if (chunk != null) i++;
				}
				Log.InfoFormat("World pre-cache {0} chunks completed in {1}ms", i, chunkLoading.ElapsedMilliseconds);
			}

			if (Config.GetProperty("CheckForSafeSpawn", false))
			{
				var chunk = _worldProvider.GenerateChunkColumn(new ChunkCoordinates(SpawnPoint));
				chunk.RecalcHeight();

				var height = GetHeight((BlockCoordinates) SpawnPoint);
				if (height > SpawnPoint.Y) SpawnPoint.Y = height + 2;
				Log.Debug("Checking for safe spawn");
			}

			StartTimeInTicks = DateTime.UtcNow.Ticks;

			_tickTimer = new Stopwatch();
			_tickTimer.Restart();
			//_levelTicker = new Timer(WorldTick, null, 50, _worldTickTime); // MC worlds tick-time


			//_tickerThread = new Thread(RunWorldTick);
			//_tickerThread.Priority = ThreadPriority.Highest;
			//_tickerThread.IsBackground = true;
			//_tickerThread.Start();
			//_tickerThreadTimer.Start();

			_tickerHighPrecisionTimer = new HighPrecisionTimer(50, WorldTick);
			//_mmTickTimer = new MultiMediaTimer();
			//_mmTickTimer.Mode = TimerMode.Periodic;
			//_mmTickTimer.Period = 50;
			//_mmTickTimer.Resolution = 1;
			//_mmTickTimer.SynchronizingObject = null;
			//_mmTickTimer.Tick += WorldTick;
			//_mmTickTimer.Start();
		}

		private void _tickerHighPrecisionTimer_Tick()
		{
			WorldTick(null);
		}

		private HighPrecisionTimer _tickerHighPrecisionTimer;
		private MultiMediaTimer _mmTickTimer = null;

		public void Close()
		{
			//_levelTicker.Change(Timeout.Infinite, Timeout.Infinite);
			//WaitHandle waitHandle = new AutoResetEvent(false);
			//_levelTicker.Dispose(waitHandle);
			//WaitHandle.WaitAll(new[] {waitHandle}, TimeSpan.FromMinutes(2));
			//_levelTicker = null;

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

			AnvilWorldProvider provider = _worldProvider as AnvilWorldProvider;
			if (provider != null)
			{
				foreach (var chunk in provider._chunkCache)
				{
					chunk.Value?.ClearCache();
				}
			}

			_worldProvider = null;

			Log.Info("Closed level: " + LevelId);
		}

		internal static McpeBatch CreateMcpeBatch(byte[] bytes)
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

				Player[] players = GetSpawnedPlayers();
				List<Player> spawnedPlayers = players.ToList();

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
				var spawnedPlayers = GetSpawnedPlayers();

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
					entity.SpawnToPlayers(GetSpawnedPlayers());
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
				entity.DespawnFromPlayers(GetSpawnedPlayers());
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

		private void WorldTick(object sender, EventArgs e)
		{
			WorldTick(null);
		}

		private void WorldTick(object sender)
		{
			if (_tickTimer.ElapsedMilliseconds < 40)
			{
				if (Log.IsDebugEnabled) Log.Warn($"World tick came too fast: {_tickTimer.ElapsedMilliseconds} ms");
				return;
			}

			if (Log.IsDebugEnabled && _tickTimer.ElapsedMilliseconds >= 100) Log.Error($"Time between World tick too too long: {_tickTimer.ElapsedMilliseconds} ms");

			_tickTimer.Restart();
			try
			{
				TickTime++;

				if (IsWorldTimeStarted) CurrentWorldTime += 1.25;
				if (CurrentWorldTime > _worldDayCycleTime) CurrentWorldTime = 0;
				if (TickTime%100 == 0)
				{
					//McpeSetTime message = McpeSetTime.CreateObject();
					//message.time = (int)CurrentWorldTime;
					//message.started = (byte)(IsWorldTimeStarted ? 0x80 : 0x00);

					//RelayBroadcast(players, message);
				}

				Player[] players = GetSpawnedPlayers();
				if (EnableBlockTicking)
				{
					List<Tuple<int, int>> chunksWithinRadiusOfPlayer = new List<Tuple<int, int>>();
					foreach (var player in players)
					{
						BlockCoordinates bCoord = (BlockCoordinates) player.KnownPosition;

						chunksWithinRadiusOfPlayer = GetChunkCoordinatesForTick(new ChunkCoordinates(bCoord), chunksWithinRadiusOfPlayer, 8);
					}

					foreach (var coord in chunksWithinRadiusOfPlayer)
					{
						for (int s = 0; s < 16; s++)
						{
							for (int i = 0; i < 3; i++)
							{
								int x = Random.Next(16);
								int y = Random.Next(16);
								int z = Random.Next(16);

								var blockCoordinates = new BlockCoordinates(x + coord.Item1*16, y + s*16, z + coord.Item2*16);
								var height = GetHeight(blockCoordinates);
								if (height > 0 && s*16 > height) continue;

								if (IsAir(blockCoordinates)) continue;

								GetBlock(blockCoordinates).OnTick(this, true);
							}
						}
					}
				}
				// Block updates
				foreach (KeyValuePair<BlockCoordinates, long> blockEvent in BlockWithTicks)
				{
					if (blockEvent.Value <= TickTime)
					{
						GetBlock(blockEvent.Key).OnTick(this, false);
						long value;
						BlockWithTicks.TryRemove(blockEvent.Key, out value);
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
				//if (TickTime % 2 == 0)
				BroadCastMovement(players, entities);

				//if (TickTime%100 == 0) // Every 5 seconds
				//{
				//	var staledPlayers = GetStaledPlayers(players);
				//	foreach (var p in staledPlayers)
				//	{
				//		ThreadPool.QueueUserWorkItem(delegate(object state)
				//		{
				//			Player player = (Player) state;
				//			player.Disconnect("Staled.");
				//		}, p);
				//	}
				//}

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
			//return players.Where(player => player.NetworkSession == null);
			return players.Where(player => (now - player.LastUpdatedTime) > span);
		}

		private DateTime _lastSendTime = DateTime.UtcNow;
		private DateTime _lastBroadcast = DateTime.UtcNow;

		protected virtual void BroadCastMovement(Player[] players, Entity[] entities)
		{
			DateTime now = DateTime.UtcNow;

			if (players.Length == 0) return;

			if (players.Length <= 1 && entities.Length == 0) return;

			if (now - _lastBroadcast < TimeSpan.FromMilliseconds(50)) return;

			DateTime tickTime = _lastSendTime;
			_lastSendTime = DateTime.UtcNow;

			using (MemoryStream stream = MiNetServer.MemoryStreamManager.GetStream())
			{
				int playerMoveCount = 0;
				int entiyMoveCount = 0;

				foreach (var player in players)
				{
					if (now - player.LastUpdatedTime <= now - tickTime)
					{
						PlayerLocation knownPosition = player.KnownPosition;

						McpeMovePlayer move = McpeMovePlayer.CreateObject();
						move.entityId = player.EntityId;
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

				foreach (var entity in entities)
				{
					if (now - entity.LastUpdatedTime <= now - tickTime)
					{
						{
							McpeMoveEntity moveEntity = McpeMoveEntity.CreateObject();
							moveEntity.entityId = entity.EntityId;
							moveEntity.position = (PlayerLocation) entity.KnownPosition.Clone();
							moveEntity.position.Y += entity.PositionOffset;
							byte[] bytes = moveEntity.Encode();
							BatchUtils.WriteLength(stream, bytes.Length);
							stream.Write(bytes, 0, bytes.Length);
							moveEntity.PutPool();
						}
						{
							McpeSetEntityMotion entityMotion = McpeSetEntityMotion.CreateObject();
							entityMotion.entityId = entity.EntityId;
							entityMotion.velocity = entity.Velocity;
							byte[] bytes = entityMotion.Encode();
							BatchUtils.WriteLength(stream, bytes.Length);
							stream.Write(bytes, 0, bytes.Length);
							entityMotion.PutPool();
						}
						entiyMoveCount++;
					}
				}

				if (playerMoveCount == 0 && entiyMoveCount == 0) return;

				if (players.Length == 1 && entiyMoveCount == 0) return;

				McpeBatch batch = BatchUtils.CreateBatchPacket(stream.GetBuffer(), 0, (int) stream.Length, CompressionLevel.Optimal, false);
				batch.AddReferences(players.Length - 1);
				batch.Encode();
				//batch.ValidUntil = now + TimeSpan.FromMilliseconds(50);
				foreach (var player in players)
				{
					MiNetServer.FastThreadPool.QueueUserWorkItem(() => player.SendPackage(batch));
				}
				_lastBroadcast = DateTime.UtcNow;
			}
		}

		public void RelayBroadcast<T>(T message) where T : Package<T>, new()
		{
			RelayBroadcast(null, GetSpawnedPlayers(), message);
		}

		public void RelayBroadcast<T>(Player source, T message) where T : Package<T>, new()
		{
			RelayBroadcast(source, GetSpawnedPlayers(), message);
		}

		public void RelayBroadcast<T>(Player[] sendList, T message) where T : Package<T>, new()
		{
			RelayBroadcast(null, sendList ?? GetSpawnedPlayers(), message);
		}

		public void RelayBroadcast<T>(Player source, Player[] sendList, T message) where T : Package<T>, new()
		{
			if (message == null) return;

			if (!message.IsPooled)
			{
				//message.MakePoolable();
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

		public McpeBatch GenerateChunk(ChunkCoordinates chunkPosition)
		{
			if (_worldProvider == null) return null;

			ChunkColumn chunkColumn = _worldProvider.GenerateChunkColumn(chunkPosition);
			if (chunkColumn == null) return null;

			McpeBatch chunk = chunkColumn.GetBatch();
			if (chunk == null) return null;

			return chunk;
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

		public IEnumerable<McpeBatch> GenerateChunks(ChunkCoordinates chunkPosition, Dictionary<Tuple<int, int>, McpeBatch> chunksUsed, double radius)
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

					if (_worldProvider == null) continue;

					ChunkColumn chunkColumn = _worldProvider.GenerateChunkColumn(new ChunkCoordinates(pair.Key.Item1, pair.Key.Item2));
					McpeBatch chunk = null;
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
			return GetBlock(new BlockCoordinates((int) Math.Floor(location.X), (int) Math.Floor(location.Y), (int) Math.Floor(location.Z)));
		}

		public Block GetBlock(int x, int y, int z)
		{
			return GetBlock(new BlockCoordinates(x, y, z));
		}

		public Block GetBlock(BlockCoordinates blockCoordinates)
		{
			ChunkColumn chunk = _worldProvider.GenerateChunkColumn(new ChunkCoordinates(blockCoordinates.X >> 4, blockCoordinates.Z >> 4));
			if (chunk == null) return new Air() {Coordinates = blockCoordinates, SkyLight = 15};

			byte bid = chunk.GetBlock(blockCoordinates.X & 0x0f, blockCoordinates.Y & 0xff, blockCoordinates.Z & 0x0f);
			byte metadata = chunk.GetMetadata(blockCoordinates.X & 0x0f, blockCoordinates.Y & 0xff, blockCoordinates.Z & 0x0f);
			byte blockLight = chunk.GetBlocklight(blockCoordinates.X & 0x0f, blockCoordinates.Y & 0xff, blockCoordinates.Z & 0x0f);
			byte skyLight = chunk.GetSkylight(blockCoordinates.X & 0x0f, blockCoordinates.Y & 0xff, blockCoordinates.Z & 0x0f);

			Block block = BlockFactory.GetBlockById(bid);
			block.Coordinates = blockCoordinates;
			block.Metadata = metadata;
			block.BlockLight = blockLight;
			block.SkyLight = skyLight;

			return block;
		}

		public bool IsAir(BlockCoordinates blockCoordinates)
		{
			ChunkColumn chunk = GetChunk(blockCoordinates);
			if (chunk == null) return true;

			byte bid = chunk.GetBlock(blockCoordinates.X & 0x0f, blockCoordinates.Y & 0xff, blockCoordinates.Z & 0x0f);
			return bid == 0;
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

		private ChunkColumn GetChunk(BlockCoordinates blockCoordinates)
		{
			return _worldProvider.GenerateChunkColumn(new ChunkCoordinates(blockCoordinates.X >> 4, blockCoordinates.Z >> 4));
		}

		public void SetBlock(Block block, bool broadcast = true, bool applyPhysics = true, bool calculateLight = true)
		{
			if (block.Coordinates.Y < 0) return;

			ChunkColumn chunk = _worldProvider.GenerateChunkColumn(new ChunkCoordinates(block.Coordinates.X >> 4, block.Coordinates.Z >> 4));
			chunk.SetBlock(block.Coordinates.X & 0x0f, block.Coordinates.Y & 0xff, block.Coordinates.Z & 0x0f, block.Id);
			chunk.SetMetadata(block.Coordinates.X & 0x0f, block.Coordinates.Y & 0xff, block.Coordinates.Z & 0x0f, block.Metadata);

			if (applyPhysics) ApplyPhysics(block.Coordinates.X, block.Coordinates.Y, block.Coordinates.Z);
			if (block.LightLevel > 0)
			{
				block.BlockLight = (byte) block.LightLevel;
				chunk.SetBlocklight(block.Coordinates.X & 0x0f, block.Coordinates.Y & 0xff, block.Coordinates.Z & 0x0f, (byte) block.LightLevel);
				BlockLightCalculations.Calculate(this, block);
			}

			if (!broadcast) return;

			var message = McpeUpdateBlock.CreateObject();
			message.blockId = block.Id;
			message.coordinates = block.Coordinates;
			message.blockMetaAndPriority = (byte) (0xb << 4 | (block.Metadata & 0xf));
			RelayBroadcast(message);
		}

		public void SetBlockLight(Block block)
		{
			ChunkColumn chunk = _worldProvider.GenerateChunkColumn(new ChunkCoordinates(block.Coordinates.X >> 4, block.Coordinates.Z >> 4));
			chunk.SetBlocklight(block.Coordinates.X & 0x0f, block.Coordinates.Y & 0xff, block.Coordinates.Z & 0x0f, block.BlockLight);
		}

		public void SetSkyLight(Block block)
		{
			ChunkColumn chunk = _worldProvider.GenerateChunkColumn(new ChunkCoordinates(block.Coordinates.X >> 4, block.Coordinates.Z >> 4));
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
			SetBlock(air, broadcast, applyPhysics: true);
		}

		public BlockEntity GetBlockEntity(BlockCoordinates blockCoordinates)
		{
			var blockEntity = BlockEntities.FirstOrDefault(entity => entity.Coordinates == blockCoordinates);
			if (blockEntity != null)
			{
				return blockEntity;
			}

			ChunkColumn chunk = _worldProvider.GenerateChunkColumn(new ChunkCoordinates(blockCoordinates.X >> 4, blockCoordinates.Z >> 4));

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
			ChunkColumn chunk = _worldProvider.GenerateChunkColumn(new ChunkCoordinates(blockEntity.Coordinates.X >> 4, blockEntity.Coordinates.Z >> 4));
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
			ChunkColumn chunk = _worldProvider.GenerateChunkColumn(new ChunkCoordinates(blockCoordinates.X >> 4, blockCoordinates.Z >> 4));
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
			EventHandler<BlockPlaceEventArgs> handler = BlockPlace;
			if (handler != null) handler(this, e);

			return !e.Cancel;
		}

		public void Interact(Level world, Player player, short itemId, BlockCoordinates blockCoordinates, short metadata, BlockFace face, Vector3 faceCoords)
		{
			// Make sure we are holding the item we claim to be using

			Block target = GetBlock(blockCoordinates);
			if (target.Interact(world, player, blockCoordinates, face, faceCoords)) return; // Handled in block interaction

			Item itemInHand = player.Inventory.GetItemInHand();

			if (itemInHand.GetType() == typeof (Item))
			{
				Log.Warn($"Generic item in hand when placing block. Can not complete request. Expected item {itemId} and item in hand is {itemInHand?.Id}");
				return; // Cheat(?)
			}

			if (itemInHand == null || itemInHand.Id != itemId)
			{
				if (player.GameMode != GameMode.Creative) Log.Error($"Wrong item in hand when placing block. Expected item {itemId} but had item {itemInHand?.Id}");
				return; // Cheat(?)
			}

			if (itemInHand is ItemBlock)
			{
				Block block = GetBlock(blockCoordinates);
				if (!block.IsReplacible)
				{
					block = GetBlock(itemInHand.GetNewCoordinatesFromFace(blockCoordinates, face));
				}

				if (!AllowBuild || !OnBlockPlace(new BlockPlaceEventArgs(player, this, target, block)))
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

			itemInHand.UseItem(world, player, blockCoordinates, face, faceCoords);
		}

		public event EventHandler<BlockBreakEventArgs> BlockBreak;

		protected virtual bool OnBlockBreak(BlockBreakEventArgs e)
		{
			EventHandler<BlockBreakEventArgs> handler = BlockBreak;
			if (handler != null) handler(this, e);

			return !e.Cancel;
		}

		public void BreakBlock(Player player, BlockCoordinates blockCoordinates)
		{
			List<Item> drops = new List<Item>();

			Block block = GetBlock(blockCoordinates);
			BlockEntity blockEntity = GetBlockEntity(blockCoordinates);
			drops.AddRange(block.GetDrops());

			Item inHand = player.Inventory.GetItemInHand();
			bool canBreak = inHand.BreakBlock(this, player, block, blockEntity);

			if (!canBreak || !AllowBreak || !OnBlockBreak(new BlockBreakEventArgs(player, this, block, drops)))
			{
				// Revert

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
			else
			{
				block.BreakBlock(this);

				if (blockEntity != null)
				{
					RemoveBlockEntity(blockCoordinates);
					drops.AddRange(blockEntity.GetDrops());
				}

				if (player.GameMode != GameMode.Creative)
				{
					foreach (Item drop in drops)
					{
						DropItem(blockCoordinates, drop);
					}
				}

				player.HungerManager.IncreaseExhaustion(0.025f);
			}
		}

		public void DropItem(Vector3 coordinates, Item drop)
		{
			if (GameMode == GameMode.Creative) return;

			if (drop == null) return;
			if (drop.Id == 0) return;
			if (drop.Count == 0) return;

			Random random = new Random();
			var itemEntity = new ItemEntity(this, drop)
			{
				KnownPosition =
				{
					X = (float) coordinates.X + 0.5f,
					Y = (float) coordinates.Y + 0.5f,
					Z = (float) coordinates.Z + 0.5f
				},
				Velocity = new Vector3((float) (random.NextDouble()*0.3), (float) (random.NextDouble()*0.3), (float) (random.NextDouble()*0.3))
			};

			itemEntity.SpawnEntity();
		}

		public void SetData(int x, int y, int z, byte meta)
		{
			Block block = GetBlock(new BlockCoordinates(x, y, z));
			block.Metadata = meta;
			SetBlock(block, applyPhysics: false);
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
			var cacheProvider = _worldProvider as ICachingWorldProvider;
			if (cacheProvider != null)
			{
				return cacheProvider.GetCachedChunks();
			}

			return new ChunkColumn[0];
		}

		public void ClearLoadedChunks()
		{
			var cacheProvider = _worldProvider as ICachingWorldProvider;
			if (cacheProvider != null)
			{
				cacheProvider.ClearCachedChunks();
			}
		}

		public void StrikeLightning(Vector3 position)
		{
			Lightning lightning = new Lightning(this);
			lightning.SpawnEntity();
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