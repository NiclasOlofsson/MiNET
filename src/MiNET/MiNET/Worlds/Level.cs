using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using fNbt;
using log4net;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Entities;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;

namespace MiNET.Worlds
{
	public class Level
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Level));

		public static readonly BlockCoordinates Up = new BlockCoordinates(0, 1, 0);
		public static readonly BlockCoordinates Down = new BlockCoordinates(0, -1, 0);
		public static readonly BlockCoordinates East = new BlockCoordinates(0, 0, -1);
		public static readonly BlockCoordinates West = new BlockCoordinates(0, 0, 1);
		public static readonly BlockCoordinates North = new BlockCoordinates(1, 0, 0);
		public static readonly BlockCoordinates South = new BlockCoordinates(-1, 0, 0);

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
		public Difficulty Difficulty { get; private set; }
		public double CurrentWorldTime { get; set; }
		public long TickTime { get; set; }
		public long StartTimeInTicks { get; private set; }
		public bool IsWorldTimeStarted { get; set; }

		public EntityManager EntityManager { get; private set; }
		public InventoryManager InventoryManager { get; private set; }

		public int ViewDistance { get; set; }

		public Random Random { get; private set; }

		public Level(string levelId, IWorldProvider worldProvider = null)
		{
			Random = new Random();

			EntityManager = new EntityManager();
			InventoryManager = new InventoryManager(this);
			SpawnPoint = new PlayerLocation(50, 4000, 50);
			Players = new ConcurrentDictionary<long, Player>();
			Entities = new ConcurrentDictionary<long, Entity>();
			BlockEntities = new List<BlockEntity>();
			BlockWithTicks = new ConcurrentDictionary<BlockCoordinates, long>();
			LevelId = levelId;
			GameMode = Config.GetProperty("GameMode", GameMode.Survival);
			Difficulty = Config.GetProperty("Difficulty", Difficulty.Peaceful);
			ViewDistance = Config.GetProperty("ViewDistance", 250);
			_worldProvider = worldProvider;

			if (_worldProvider == null)
			{
				switch (Config.GetProperty("WorldProvider", "flat").ToLower().Trim())
				{
					case "flat":
					case "flatland":
						_worldProvider = new FlatlandWorldProvider();
						break;
					case "cool":
						_worldProvider = new CoolWorldProvider();
						break;
					case "experimental":
						_worldProvider = new ExperimentalWorldProvider();
						break;
					case "anvil":
						_worldProvider = new AnvilWorldProvider();
						break;
					default:
						_worldProvider = new FlatlandWorldProvider();
						break;
				}
			}
		}

		public bool IsSurvival
		{
			get { return GameMode == GameMode.Survival; }
		}

		public void Initialize()
		{
			IsWorldTimeStarted = true;
			_worldProvider.Initialize();

			SpawnPoint = new PlayerLocation(_worldProvider.GetSpawnPoint());
			CurrentWorldTime = _worldProvider.GetTime();

			if (_worldProvider.IsCaching)
			{
				Stopwatch chunkLoading = new Stopwatch();
				chunkLoading.Start();
				// Pre-cache chunks for spawn coordinates
				int i = 0;
				foreach (var chunk in GenerateChunks(new ChunkCoordinates(SpawnPoint), new Dictionary<Tuple<int, int>, McpeBatch>()))
				{
					i++;
				}
				Log.InfoFormat("World pre-cache {0} chunks completed in {1}ms", i, chunkLoading.ElapsedMilliseconds);
			}

			StartTimeInTicks = DateTime.UtcNow.Ticks;

			_levelTicker = new Timer(WorldTick, null, 0, _worldTickTime); // MC worlds tick-time
		}

		public void Close()
		{
			_levelTicker.Change(Timeout.Infinite, Timeout.Infinite);
			WaitHandle waitHandle = new AutoResetEvent(false);
			_levelTicker.Dispose(waitHandle);
			WaitHandle.WaitAll(new[] {waitHandle}, TimeSpan.FromMinutes(2));
			_levelTicker = null;

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
				AnvilWorldProvider anvil = provider;
				anvil._chunkCache.Clear();
			}

			_worldProvider = null;

			Log.Info("Closed level: " + LevelId);
		}

		private object _playerWriteLock = new object();

		public virtual void AddPlayer(Player newPlayer, bool spawn)
		{
			if (newPlayer.Username == null) throw new ArgumentNullException("newPlayer");

			EntityManager.AddEntity(null, newPlayer);

			lock (_playerWriteLock)
			{
				if (Players.TryAdd(newPlayer.EntityId, newPlayer))
				{
					SpawnToAll(newPlayer);

					foreach (Entity entity in Entities.Values.ToArray())
					{
						SendAddEntityToPlayer(entity, newPlayer);
					}
				}

				newPlayer.IsSpawned = spawn;
			}
		}

		public void SpawnToAll(Player newPlayer)
		{
			lock (_playerWriteLock)
			{
				List<Player> spawnedPlayers = GetSpawnedPlayers().ToList();
				spawnedPlayers.Add(newPlayer);

				Player[] sendList = spawnedPlayers.ToArray();

				McpePlayerList playerListMessage = McpePlayerList.CreateObject();
				playerListMessage.records = new PlayerAddRecords(spawnedPlayers);
				var bytes = playerListMessage.Encode();
				playerListMessage.records = null;

				MemoryStream memStream = new MemoryStream();
				memStream.Write(BitConverter.GetBytes(Endian.SwapInt32(bytes.Length)), 0, 4);
				memStream.Write(bytes, 0, bytes.Length);

				McpeBatch batch = McpeBatch.CreateObject();
				byte[] buffer = Player.CompressBytes(memStream.ToArray(), CompressionLevel.Optimal);
				batch.payloadSize = buffer.Length;
				batch.payload = buffer;
				batch.Encode();

				newPlayer.SendPackage(batch);

				McpePlayerList playerList = McpePlayerList.CreateObject();
				playerList.records = new PlayerAddRecords {newPlayer};
				playerList.Encode();
				playerList.records = null;
				RelayBroadcast(newPlayer, sendList, playerList);

				McpeAddPlayer mcpeAddPlayer = McpeAddPlayer.CreateObject();
				mcpeAddPlayer.uuid = newPlayer.ClientUuid;
				mcpeAddPlayer.username = newPlayer.Username;
				mcpeAddPlayer.entityId = newPlayer.EntityId;
				mcpeAddPlayer.x = newPlayer.KnownPosition.X;
				mcpeAddPlayer.y = newPlayer.KnownPosition.Y;
				mcpeAddPlayer.z = newPlayer.KnownPosition.Z;
				mcpeAddPlayer.yaw = newPlayer.KnownPosition.Yaw;
				mcpeAddPlayer.headYaw = newPlayer.KnownPosition.HeadYaw;
				mcpeAddPlayer.pitch = newPlayer.KnownPosition.Pitch;
				mcpeAddPlayer.metadata = newPlayer.GetMetadata();
				RelayBroadcast(newPlayer, sendList, mcpeAddPlayer);

				McpePlayerEquipment mcpePlayerEquipment = McpePlayerEquipment.CreateObject();
				mcpePlayerEquipment.entityId = newPlayer.EntityId;
				mcpePlayerEquipment.item = new MetadataSlot(newPlayer.Inventory.GetItemInHand());
				mcpePlayerEquipment.slot = 0;
				RelayBroadcast(newPlayer, sendList, mcpePlayerEquipment);

				McpePlayerArmorEquipment mcpePlayerArmorEquipment = McpePlayerArmorEquipment.CreateObject();
				mcpePlayerArmorEquipment.entityId = newPlayer.EntityId;
				mcpePlayerArmorEquipment.helmet = new MetadataSlot(new ItemStack(newPlayer.Inventory.Helmet, 0));
				mcpePlayerArmorEquipment.chestplate = new MetadataSlot(new ItemStack(newPlayer.Inventory.Chest, 0));
				mcpePlayerArmorEquipment.leggings = new MetadataSlot(new ItemStack(newPlayer.Inventory.Leggings, 0));
				mcpePlayerArmorEquipment.boots = new MetadataSlot(new ItemStack(newPlayer.Inventory.Boots, 0));
				RelayBroadcast(newPlayer, sendList, mcpePlayerArmorEquipment);

				foreach (Player spawnedPlayer in spawnedPlayers)
				{
					SendAddForPlayer(newPlayer, spawnedPlayer, false);
				}
			}
		}

		public void SendAddForPlayer(Player receiver, Player addedPlayer, bool sendPlayerListAdd = true)
		{
			if (addedPlayer == receiver) return;

			McpeAddPlayer mcpeAddPlayer = McpeAddPlayer.CreateObject();
			mcpeAddPlayer.uuid = addedPlayer.ClientUuid;
			mcpeAddPlayer.username = addedPlayer.Username;
			mcpeAddPlayer.entityId = addedPlayer.EntityId;
			mcpeAddPlayer.x = addedPlayer.KnownPosition.X;
			mcpeAddPlayer.y = addedPlayer.KnownPosition.Y;
			mcpeAddPlayer.z = addedPlayer.KnownPosition.Z;
			mcpeAddPlayer.yaw = addedPlayer.KnownPosition.Yaw;
			mcpeAddPlayer.headYaw = addedPlayer.KnownPosition.HeadYaw;
			mcpeAddPlayer.pitch = addedPlayer.KnownPosition.Pitch;
			mcpeAddPlayer.metadata = addedPlayer.GetMetadata();
			receiver.SendPackage(mcpeAddPlayer);

			SendEquipmentForPlayer(receiver, addedPlayer);

			SendArmorForPlayer(receiver, addedPlayer);
		}

		public void SendEquipmentForPlayer(Player receiver, Player player)
		{
			McpePlayerEquipment mcpePlayerEquipment = McpePlayerEquipment.CreateObject();
			mcpePlayerEquipment.entityId = player.EntityId;
			mcpePlayerEquipment.item = new MetadataSlot(player.Inventory.GetItemInHand());
			mcpePlayerEquipment.slot = 0;
			receiver.SendPackage(mcpePlayerEquipment);
		}

		public void SendArmorForPlayer(Player receiver, Player player)
		{
			McpePlayerArmorEquipment mcpePlayerArmorEquipment = McpePlayerArmorEquipment.CreateObject();
			mcpePlayerArmorEquipment.entityId = player.EntityId;
			mcpePlayerArmorEquipment.helmet = new MetadataSlot(new ItemStack(player.Inventory.Helmet, 0));
			mcpePlayerArmorEquipment.chestplate = new MetadataSlot(new ItemStack(player.Inventory.Chest, 0));
			mcpePlayerArmorEquipment.leggings = new MetadataSlot(new ItemStack(player.Inventory.Leggings, 0));
			mcpePlayerArmorEquipment.boots = new MetadataSlot(new ItemStack(player.Inventory.Boots, 0));
			receiver.SendPackage(mcpePlayerArmorEquipment);
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
						entity.DespawnFromPlayer(removed);
					}
				}
			}
		}


		public void DespawnFromAll(Player player)
		{
			lock (_playerWriteLock)
			{
				List<Player> spawnedPlayers = GetSpawnedPlayers().ToList();

				McpePlayerList playerListMessage = McpePlayerList.CreateObject();
				playerListMessage.records = new PlayerRemoveRecords(spawnedPlayers);
				var bytes = playerListMessage.Encode();
				playerListMessage.records = null;

				MemoryStream memStream = new MemoryStream();
				memStream.Write(BitConverter.GetBytes(Endian.SwapInt32(bytes.Length)), 0, 4);
				memStream.Write(bytes, 0, bytes.Length);

				McpeBatch batch = McpeBatch.CreateObject();
				byte[] buffer = Player.CompressBytes(memStream.ToArray(), CompressionLevel.Optimal);
				batch.payloadSize = buffer.Length;
				batch.payload = buffer;
				batch.Encode();
				player.SendPackage(batch);

				foreach (Player spawnedPlayer in spawnedPlayers)
				{
					SendRemoveForPlayer(player, spawnedPlayer, false);
				}

				McpePlayerList playerList = McpePlayerList.CreateObject();
				playerList.records = new PlayerRemoveRecords {player};
				playerList.Encode();
				playerList.records = null;
				RelayBroadcast(player, playerList);

				McpeRemovePlayer removePlayerMessage = McpeRemovePlayer.CreateObject();
				removePlayerMessage.clientUuid = player.ClientUuid;
				removePlayerMessage.entityId = player.EntityId;
				RelayBroadcast(player, removePlayerMessage);
			}
		}

		public void SendRemoveForPlayer(Player receiver, Player player, bool sendRemovePlayerList = true)
		{
			if (player == receiver) return;

			McpeRemovePlayer mcpeRemovePlayer = McpeRemovePlayer.CreateObject();
			mcpeRemovePlayer.clientUuid = player.ClientUuid;
			mcpeRemovePlayer.entityId = player.EntityId;
			receiver.SendPackage(mcpeRemovePlayer);
		}

		public void AddEntity(Entity entity)
		{
			lock (Entities)
			{
				EntityManager.AddEntity(null, entity);

				if (Entities.TryAdd(entity.EntityId, entity))
				{
				}
				else
				{
					throw new Exception("Entity existed in the players list when it should not");
				}
			}
		}

		public void SendAddEntityToPlayer(Entity entity, Player player)
		{
			entity.SpawnToPlayer(player);
		}

		public void RemoveEntity(Entity entity)
		{
			lock (Entities)
			{
				if (!Entities.TryRemove(entity.EntityId, out entity)) return; // It's ok. Holograms destroy this play..

				List<Player> spawnedPlayers = GetSpawnedPlayers().ToList();

				foreach (Player player in spawnedPlayers)
				{
					entity.DespawnFromPlayer(player);
				}
			}
		}


		public void RemoveDuplicatePlayers(string username, int clientId)
		{
			var existingPlayers = Players.Where(player => player.Value.ClientId == clientId && player.Value.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));

			foreach (var existingPlayer in existingPlayers)
			{
				Log.InfoFormat("Removing staled players on login {0}", username);
				existingPlayer.Value.Disconnect("Duplicate player. Crashed.", false);
			}
		}

		public virtual void BroadcastMessage(string text, MessageType type = MessageType.Chat, Player sender = null)
		{
			foreach (var line in text.Split('\n'))
			{
				McpeText message = McpeText.CreateObject();
				message.type = (byte) type;
				message.source = sender == null ? "" : sender.Username;
				message.message = line;

				RelayBroadcast(message);
			}
		}


		private object _tickSync = new object();
		private Stopwatch _tickTimer = new Stopwatch();
		public long LastTickProcessingTime = 0;
		public long AvarageTickProcessingTime = 50;
		public int PlayerCount { get; private set; }

		private void WorldTick(object sender)
		{
			if (!Monitor.TryEnter(_tickSync)) return;

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

				// Block updates
				foreach (KeyValuePair<BlockCoordinates, long> blockEvent in BlockWithTicks)
				{
					if (blockEvent.Value <= TickTime)
					{
						GetBlock(blockEvent.Key).OnTick(this);
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

				Player[] players = GetSpawnedPlayers();
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
			}
			finally
			{
				LastTickProcessingTime = _tickTimer.ElapsedMilliseconds;
				AvarageTickProcessingTime = ((AvarageTickProcessingTime*9) + _tickTimer.ElapsedMilliseconds)/10L;

				Monitor.Exit(_tickSync);
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

		protected virtual void BroadCastMovement(Player[] players, Entity[] entities)
		{
			if (players.Length == 0) return;

			DateTime tickTime = _lastSendTime;
			_lastSendTime = DateTime.UtcNow;
			DateTime now = DateTime.UtcNow;

			MemoryStream stream = new MemoryStream();

			int count = 0;
			McpeMovePlayer move = McpeMovePlayer.CreateObject();
			foreach (var player in players)
			{
				if (((now - player.LastUpdatedTime) <= now - tickTime))
				{
					PlayerLocation knownPosition = player.KnownPosition;

					move.entityId = player.EntityId;
					move.x = knownPosition.X;
					move.y = knownPosition.Y + 1.62f;
					move.z = knownPosition.Z;
					move.yaw = knownPosition.Yaw;
					move.pitch = knownPosition.Pitch;
					move.headYaw = knownPosition.HeadYaw;
					move.mode = 0;
					byte[] bytes = move.Encode();
					stream.Write(BitConverter.GetBytes(Endian.SwapInt32(bytes.Length)), 0, 4);
					stream.Write(bytes, 0, bytes.Length);
					move.Reset();
					count++;
				}
			}
			move.PutPool();

			McpeMoveEntity moveEntity = McpeMoveEntity.CreateObject();
			moveEntity.entities = new EntityLocations();

			McpeSetEntityMotion entityMotion = McpeSetEntityMotion.CreateObject();
			entityMotion.entities = new EntityMotions();

			foreach (var entity in entities)
			{
				if (((now - entity.LastUpdatedTime) <= now - tickTime))
				{
					moveEntity.entities.Add(entity.EntityId, entity.KnownPosition);
					entityMotion.entities.Add(entity.EntityId, entity.Velocity);
					count++;
				}
			}

			{
				byte[] bytes = moveEntity.Encode();
				stream.Write(BitConverter.GetBytes(Endian.SwapInt32(bytes.Length)), 0, 4);
				stream.Write(bytes, 0, bytes.Length);
				moveEntity.PutPool();
			}
			{
				byte[] bytes = entityMotion.Encode();
				stream.Write(BitConverter.GetBytes(Endian.SwapInt32(bytes.Length)), 0, 4);
				stream.Write(bytes, 0, bytes.Length);
				entityMotion.PutPool();
			}

			if (count == 0) return;

			McpeBatch batch = McpeBatch.CreateObject(players.Length);
			byte[] buffer = Player.CompressBytes(stream.ToArray(), CompressionLevel.Optimal);
			batch.payloadSize = buffer.Length;
			batch.payload = buffer;
			batch.Encode();

			foreach (var player in players)
			{
				Task sendTask = new Task(obj => ((Player) obj).SendMoveList(batch, now), player);
				sendTask.Start();
			}
		}

		public void RelayBroadcast<T>(T message, bool sendDirect = false) where T : Package<T>, new()
		{
			RelayBroadcast(null, GetSpawnedPlayers(), message, sendDirect);
		}

		public void RelayBroadcast<T>(Entity source, T message, bool sendDirect = false) where T : Package<T>, new()
		{
			RelayBroadcast(source, GetSpawnedPlayers(), message, sendDirect);
		}

		public void RelayBroadcast<T>(Entity source, Player[] sendList, T message, bool sendDirect = false) where T : Package<T>, new()
		{
			if (message == null) return;

			if (!message.IsPooled)
			{
				message.MakePoolable();
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

			if (message.IsPooled) message.Encode(); // In case forgotten during create

			foreach (var player in sendList)
			{
				if (source != null && player == source)
				{
					message.PutPool();
					continue;
				}

				Task sendTask = new Task(obj => ((Player) obj).SendPackage(message, sendDirect), player);
				sendTask.Start();
			}
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

		public IEnumerable<McpeBatch> GenerateChunks(ChunkCoordinates chunkPosition, Dictionary<Tuple<int, int>, McpeBatch> chunksUsed)
		{
			lock (chunksUsed)
			{
				Dictionary<Tuple<int, int>, double> newOrders = new Dictionary<Tuple<int, int>, double>();
				// ViewDistance is actually ViewArea
				// A = pi r^2
				// sqrt(A/pi) = r
				double radiusSquared = ViewDistance/Math.PI;
				double radius = Math.Ceiling(Math.Sqrt(radiusSquared));
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

				if (newOrders.Count > ViewDistance)
				{
					foreach (var pair in newOrders.OrderByDescending(pair => pair.Value))
					{
						if (newOrders.Count <= ViewDistance) break;
						newOrders.Remove(pair.Key);
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

				if (chunksUsed.Count > ViewDistance) Debug.WriteLine("Too many chunks used: {0}", chunksUsed.Count);
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
			if (chunk == null) return new Air();

			byte bid = chunk.GetBlock(blockCoordinates.X & 0x0f, blockCoordinates.Y & 0x7f, blockCoordinates.Z & 0x0f);
			byte metadata = chunk.GetMetadata(blockCoordinates.X & 0x0f, blockCoordinates.Y & 0x7f, blockCoordinates.Z & 0x0f);

			Block block = BlockFactory.GetBlockById(bid);
			block.Coordinates = blockCoordinates;
			block.Metadata = metadata;

			return block;
		}

		public void SetBlock(Block block, bool broadcast = true, bool applyPhysics = true)
		{
			ChunkColumn chunk = _worldProvider.GenerateChunkColumn(new ChunkCoordinates(block.Coordinates.X >> 4, block.Coordinates.Z >> 4));
			chunk.SetBlock(block.Coordinates.X & 0x0f, block.Coordinates.Y & 0x7f, block.Coordinates.Z & 0x0f, block.Id);
			chunk.SetMetadata(block.Coordinates.X & 0x0f, block.Coordinates.Y & 0x7f, block.Coordinates.Z & 0x0f, block.Metadata);

			if (applyPhysics) ApplyPhysics(block.Coordinates.X, block.Coordinates.Y, block.Coordinates.Z);

			if (!broadcast) return;

			Block sendBlock = new Block(block.Id)
			{
				Coordinates = block.Coordinates,
				Metadata = (byte) (0xb << 4 | (block.Metadata & 0xf))
			};
			var message = McpeUpdateBlock.CreateObject();
			message.blocks = new BlockRecords {sendBlock};
			RelayBroadcast(message);
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
			NbtCompound nbt = chunk.GetBlockEntity(blockCoordinates);
			if (nbt == null) return null;

			string id = null;
			var idTag = nbt.Get("id");
			if (idTag != null)
			{
				id = idTag.StringValue;
			}

			if (string.IsNullOrEmpty(id)) return null;

			blockEntity = BlockEntityFactory.GetBlockEntityById(id);
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
					RootTag = blockEntity.GetCompound()
				}
			};

			var entityData = new McpeTileEntityData
			{
				namedtag = nbt,
				x = blockEntity.Coordinates.X,
				y = (byte) blockEntity.Coordinates.Y,
				z = blockEntity.Coordinates.Z
			};

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

		public void Interact(Level world, Player player, short itemId, BlockCoordinates blockCoordinates, short metadata)
		{
			// Make sure we are holding the item we claim to be using
			Item itemInHand = player.Inventory.GetItemInHand().Item;

			if (itemInHand == null || itemInHand.Id != itemId) return; // Cheat(?)

			itemInHand.UseItem(world, player, blockCoordinates);
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
			if (target.Interact(world, player, blockCoordinates, face)) return; // Handled in block interaction

			ItemStack itemStackInHand = player.Inventory.GetItemInHand();
			Item itemInHand = itemStackInHand.Item;

			if (itemInHand == null || itemInHand.Id != itemId) return; // Cheat(?)

			if (itemInHand is ItemBlock)
			{
				if (!OnBlockPlace(new BlockPlaceEventArgs(player, this, target)))
				{
					Block block = GetBlock(itemInHand.GetNewCoordinatesFromFace(blockCoordinates, face));

					Block sendBlock = new Block(block.Id)
					{
						Coordinates = block.Coordinates,
						Metadata = (byte) (0xb << 4 | (block.Metadata & 0xf))
					};

					var message = McpeUpdateBlock.CreateObject();
					message.blocks = new BlockRecords {sendBlock};
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
			List<ItemStack> drops = new List<ItemStack>();

			Block block = GetBlock(blockCoordinates);
			drops.Add(block.GetDrops());
			if (OnBlockBreak(new BlockBreakEventArgs(player, this, block, drops)))
			{
				block.BreakBlock(this);

				BlockEntity blockEnity = GetBlockEntity(blockCoordinates);
				if (blockEnity != null)
				{
					RemoveBlockEntity(blockCoordinates);
					drops.AddRange(blockEnity.GetDrops());
				}

				if (player.GameMode != GameMode.Creative)
				{
					foreach (ItemStack drop in drops)
					{
						DropItem(blockCoordinates, drop);
					}
				}
			}
			else
			{
				var message = McpeUpdateBlock.CreateObject();
				message.blocks = new BlockRecords {block};
				player.SendPackage(message);
			}
		}

		public void DropItem(BlockCoordinates coordinates, ItemStack drop)
		{
			if (GameMode == GameMode.Creative) return;

			if (drop == null) return;
			if (drop.Id == 0) return;
			if (drop.Count == 0) return;

			Item item = ItemFactory.GetItem(drop.Id, drop.Metadata);

			var itemEntity = new ItemEntity(this, item)
			{
				Count = drop.Count,
				KnownPosition =
				{
					X = coordinates.X,
					Y = coordinates.Y,
					Z = coordinates.Z
				},
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
			var provider = _worldProvider as AnvilWorldProvider;
			if (provider != null)
			{
				return provider.GetCachedChunks();
			}

			return new ChunkColumn[0];
		}

		public void StrikeLightning(Vector3 position)
		{
			Lightning lightning = new Lightning(this);
			lightning.SpawnEntity();
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
		public Block Block { get; private set; }

		public BlockPlaceEventArgs(Player player, Level level, Block block) : base(player, level)
		{
			Block = block;
		}
	}


	public class BlockBreakEventArgs : LevelEventArgs
	{
		public Block Block { get; private set; }
		public List<ItemStack> Drops { get; private set; }

		public BlockBreakEventArgs(Player player, Level level, Block block, List<ItemStack> drops) : base(player, level)
		{
			Block = block;
			Drops = drops;
		}
	}
}