﻿using System;
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

		public BlockCoordinates SpawnPoint { get; set; }
		public ConcurrentDictionary<long, Player> Players { get; private set; } //TODO: Need to protect this, not threadsafe
		public List<Entity> Entities { get; private set; } //TODO: Need to protect this, not threadsafe
		public List<BlockEntity> BlockEntities { get; private set; } //TODO: Need to protect this, not threadsafe
		public ConcurrentDictionary<BlockCoordinates, long> BlockWithTicks { get; private set; } //TODO: Need to protect this, not threadsafe
		public string LevelId { get; private set; }

		public GameMode GameMode { get; private set; }
		public Difficulty Difficulty { get; private set; }
		public double CurrentWorldTime { get; set; }
		public long TickTime { get; set; }
		public long StartTimeInTicks { get; private set; }
		public bool IsWorldTimeStarted { get; private set; }

		public EntityManager EntityManager { get; private set; }
		public InventoryManager InventoryManager { get; private set; }

		public int ViewDistance { get; set; }

		public Random Random { get; private set; }

		public Level(string levelId, IWorldProvider worldProvider = null)
		{
			Random = new Random();

			EntityManager = new EntityManager();
			InventoryManager = new InventoryManager(this);
			SpawnPoint = new BlockCoordinates(50, 10, 50);
			Players = new ConcurrentDictionary<long, Player>();
			Entities = new List<Entity>();
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

			SpawnPoint = _worldProvider.GetSpawnPoint();
			CurrentWorldTime = _worldProvider.GetTime();

			if (_worldProvider.IsCaching)
			{
				//ThreadPool.QueueUserWorkItem(delegate(object state)
				//{
				Stopwatch chunkLoading = new Stopwatch();
				chunkLoading.Start();
				// Pre-cache chunks for spawn coordinates
				int i = 0;
				foreach (var chunk in GenerateChunks(new ChunkCoordinates(SpawnPoint.X >> 4, SpawnPoint.Z >> 4), new Dictionary<Tuple<int, int>, McpeBatch>()))
				{
					chunk.PutPool();
					i++;
				}
				Log.InfoFormat("World pre-cache {0} chunks completed in {1}ms", i, chunkLoading.ElapsedMilliseconds);
				//});
			}

			StartTimeInTicks = DateTime.UtcNow.Ticks;

			_levelTicker = new Timer(WorldTick, null, 0, _worldTickTime); // MC worlds tick-time
		}

		private object _playerWriteLock = new object();

		public virtual void AddPlayer(Player newPlayer, string broadcastText = null, bool spawn = true)
		{
			if (newPlayer.Username == null) return;

			EntityManager.AddEntity(null, newPlayer);

			if (Players.TryAdd(newPlayer.EntityId, newPlayer))
			{
				if (spawn)
				{
					foreach (var pair in Players)
					{
						Player targetPlayer = pair.Value;
						if (!targetPlayer.IsSpawned) continue;
						SendAddForPlayer(newPlayer, targetPlayer);
						SendAddForPlayer(targetPlayer, newPlayer);
					}
				}
			}

			//foreach (var targetPlayer in allPlayers)
			//{
			//	if (!targetPlayer.IsSpawned) continue;
			//	SendAddForPlayer(newPlayer, targetPlayer);
			//	SendAddForPlayer(targetPlayer, newPlayer);
			//}

			foreach (Entity entity in Entities.ToArray())
			{
				SendAddEntityToPlayer(entity, newPlayer);
			}

			//ThreadPool.QueueUserWorkItem(delegate(object state)
			//{
			//	foreach (var targetPlayer in allPlayers)
			//	{
			//		if (!targetPlayer.IsSpawned) continue;

			//		if (spawn)
			//		{
			//			SendAddForPlayer(targetPlayer, newPlayer);
			//		}
			//	}
			//});

			//lock (Players)
			//{
			//	if (!Players.Contains(newPlayer)) Players.Add(newPlayer);

			//	if (!string.IsNullOrEmpty(broadcastText))
			//	{
			//		//BroadcastTextMessage(broadcastText);
			//	}

			//	//BroadCastMovement(new[] {newPlayer}, GetSpawnedPlayers());
			//}

			//newPlayer.IsSpawned = true;
		}

		public void SendAddForPlayer(Player receiver, Player player)
		{
			if (player == receiver) return;

			McpeAddPlayer mcpeAddPlayer = McpeAddPlayer.CreateObject();
			mcpeAddPlayer.clientId = player.EntityId;
			mcpeAddPlayer.username = player.Username;
			mcpeAddPlayer.entityId = player.EntityId;
			mcpeAddPlayer.x = player.KnownPosition.X;
			mcpeAddPlayer.y = player.KnownPosition.Y;
			mcpeAddPlayer.z = player.KnownPosition.Z;
			mcpeAddPlayer.yaw = player.KnownPosition.Yaw;
			mcpeAddPlayer.headYaw = player.KnownPosition.HeadYaw;
			mcpeAddPlayer.pitch = player.KnownPosition.Pitch;
			mcpeAddPlayer.skin = player.Skin;
			mcpeAddPlayer.metadata = player.GetMetadata().GetBytes();
			receiver.SendPackage(mcpeAddPlayer);

			//SendEquipmentForPlayer(receiver, player);

			//SendArmorForPlayer(receiver, player);
		}

		public void SendEquipmentForPlayer(Player receiver, Player player)
		{
			McpePlayerEquipment mcpePlayerEquipment = McpePlayerEquipment.CreateObject();
			mcpePlayerEquipment.entityId = player.EntityId;
			mcpePlayerEquipment.item = player.Inventory.ItemInHand.Value.Id;
			mcpePlayerEquipment.meta = player.Inventory.ItemInHand.Value.Metadata;
			mcpePlayerEquipment.slot = 0;
			receiver.SendPackage(mcpePlayerEquipment);
		}

		public void SendArmorForPlayer(Player receiver, Player player)
		{
			McpePlayerArmorEquipment mcpePlayerArmorEquipment = McpePlayerArmorEquipment.CreateObject();
			mcpePlayerArmorEquipment.entityId = player.EntityId;
			mcpePlayerArmorEquipment.helmet = (byte) (((MetadataSlot) player.Inventory.Armor[0]).Value.Id - 256);
			mcpePlayerArmorEquipment.chestplate = (byte) (((MetadataSlot) player.Inventory.Armor[1]).Value.Id - 256);
			mcpePlayerArmorEquipment.leggings = (byte) (((MetadataSlot) player.Inventory.Armor[2]).Value.Id - 256);
			mcpePlayerArmorEquipment.boots = (byte) (((MetadataSlot) player.Inventory.Armor[3]).Value.Id - 256);
			receiver.SendPackage(mcpePlayerArmorEquipment);
		}


		public virtual void RemovePlayer(Player player, bool despawn = true)
		{
			Player removed;
			if (Players.TryRemove(player.EntityId, out removed))
			{
				if (despawn)
				{
					foreach (var pair in Players)
					{
						Player targetPlayer = pair.Value;

						SendRemoveForPlayer(targetPlayer, player);
						SendRemoveForPlayer(player, targetPlayer);
					}
				}
			}

			//BroadcastTextMessage(string.Format("{0} left the game!", player.Username));
		}

		public void HidePlayer(Player player, bool hide)
		{
			foreach (var targetPlayer in GetSpawnedPlayers())
			{
				if (hide) SendRemoveForPlayer(targetPlayer, player);
				else SendAddForPlayer(targetPlayer, player);
			}
		}

		public void SendRemoveForPlayer(Player receiver, Player player)
		{
			if (player == receiver) return;

			McpeRemovePlayer mcpeRemovePlayer = McpeRemovePlayer.CreateObject();
			mcpeRemovePlayer.clientId = 0;
			mcpeRemovePlayer.entityId = player.EntityId;
			receiver.SendPackage(mcpeRemovePlayer);
		}


		public void AddEntity(Entity entity)
		{
			lock (Entities)
			{
				EntityManager.AddEntity(null, entity);

				if (!Entities.Contains(entity))
				{
					Entities.Add(entity);
				}
				else
				{
					throw new Exception("Entity existed in the players list when it should not");
				}

				if (entity is ItemEntity)
				{
					ItemEntity itemEntity = (ItemEntity) entity;

					Random random = new Random();

					float f = 0.7F;
					float xr = (float) (random.NextDouble()*f + (1.0F - f)*0.5D);
					float yr = (float) (random.NextDouble()*f + (1.0F - f)*0.5D);
					float zr = (float) (random.NextDouble()*f + (1.0F - f)*0.5D);

					McpeAddItemEntity mcpeAddItemEntity = McpeAddItemEntity.CreateObject();
					mcpeAddItemEntity.entityId = itemEntity.EntityId;
					mcpeAddItemEntity.item = itemEntity.GetMetadataSlot();
					mcpeAddItemEntity.x = itemEntity.KnownPosition.X + xr;
					mcpeAddItemEntity.y = itemEntity.KnownPosition.Y + yr;
					mcpeAddItemEntity.z = itemEntity.KnownPosition.Z + zr;
					RelayBroadcast(mcpeAddItemEntity);
				}
				else
				{
					var addEntity = McpeAddEntity.CreateObject();
					addEntity.entityType = entity.EntityTypeId;
					addEntity.entityId = entity.EntityId;
					addEntity.x = entity.KnownPosition.X;
					addEntity.y = entity.KnownPosition.Y;
					addEntity.z = entity.KnownPosition.Z;
					addEntity.yaw = entity.KnownPosition.Yaw;
					addEntity.pitch = entity.KnownPosition.Pitch;
					addEntity.metadata = entity.GetMetadata();
					{
						double dx = entity.Velocity.X;
						double dy = entity.Velocity.Y;
						double dz = entity.Velocity.Z;
						//double maxVelocity = 3.92d;

						//if (dx < -maxVelocity)
						//{
						//	dx = -maxVelocity;
						//}

						//if (dy < -maxVelocity)
						//{
						//	dy = -maxVelocity;
						//}

						//if (dz < -maxVelocity)
						//{
						//	dz = -maxVelocity;
						//}

						//if (dx > maxVelocity)
						//{
						//	dx = maxVelocity;
						//}

						//if (dy > maxVelocity)
						//{
						//	dy = maxVelocity;
						//}

						//if (dz > maxVelocity)
						//{
						//	dz = maxVelocity;
						//}

						//addEntity.speedX = (short)(dx * 8000.0d);
						//addEntity.speedY = (short)(dy * 8000.0d);
						//addEntity.speedZ = (short)(dz * 8000.0d);
						addEntity.speedX = (float) (dx);
						addEntity.speedY = (float) (dy);
						addEntity.speedZ = (float) (dz);
					}

					RelayBroadcast(addEntity);

					McpeSetEntityData mcpeSetEntityData = McpeSetEntityData.CreateObject();
					mcpeSetEntityData.entityId = entity.EntityId;
					mcpeSetEntityData.metadata = entity.GetMetadata();
					RelayBroadcast(mcpeSetEntityData);
				}

				entity.IsSpawned = true;
			}
		}

		public void SendAddEntityToPlayer(Entity entity, Player player)
		{
			if (entity is ItemEntity)
			{
				ItemEntity itemEntity = (ItemEntity) entity;
				McpeAddItemEntity mcpeAddItemEntity = McpeAddItemEntity.CreateObject();
				mcpeAddItemEntity.entityId = itemEntity.EntityId;
				mcpeAddItemEntity.item = itemEntity.GetMetadataSlot();
				mcpeAddItemEntity.x = itemEntity.KnownPosition.X;
				mcpeAddItemEntity.y = itemEntity.KnownPosition.Y;
				mcpeAddItemEntity.z = itemEntity.KnownPosition.Z;
				player.SendPackage(mcpeAddItemEntity);
			}
			else
			{
				McpeAddEntity mcpeAddEntity = McpeAddEntity.CreateObject();
				mcpeAddEntity.entityType = entity.EntityTypeId;
				mcpeAddEntity.entityId = entity.EntityId;
				mcpeAddEntity.x = entity.KnownPosition.X;
				mcpeAddEntity.y = entity.KnownPosition.Y;
				mcpeAddEntity.z = entity.KnownPosition.Z;
				mcpeAddEntity.yaw = entity.KnownPosition.Yaw;
				mcpeAddEntity.pitch = entity.KnownPosition.Pitch;
				mcpeAddEntity.metadata = entity.GetMetadata();
				mcpeAddEntity.speedX = (float) entity.Velocity.X;
				mcpeAddEntity.speedY = (float) entity.Velocity.Y;
				mcpeAddEntity.speedZ = (float) entity.Velocity.Z;
				player.SendPackage(mcpeAddEntity);
			}
		}

		public void RemoveEntity(Entity entity)
		{
			lock (Entities)
			{
				if (!Entities.Remove(entity)) throw new Exception("Expected entity to exist on remove.");

				McpeRemoveEntity mcpeRemoveEntity = McpeRemoveEntity.CreateObject();
				mcpeRemoveEntity.entityId = entity.EntityId;
				RelayBroadcast(mcpeRemoveEntity);
			}
		}


		public void RemoveDuplicatePlayers(string username, int clientId)
		{
			var existingPlayers = Players.Where(player => player.Value.ClientId == clientId && player.Value.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));

			foreach (var existingPlayer in existingPlayers)
			{
				Log.InfoFormat("Removing staled players on login {0}", username);
				existingPlayer.Value.Disconnect("Stale player.");
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

		private void WorldTick(object sender)
		{
			if (!Monitor.TryEnter(_tickSync)) return;

			_tickTimer.Restart();
			try
			{
				TickTime++;

				Player[] players = Players.Values.ToArray();

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
				foreach (KeyValuePair<BlockCoordinates, long> blockEvent in BlockWithTicks.ToArray())
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
				foreach (Entity entity in Entities.ToArray())
				{
					entity.OnTick();
				}

				// Player tick
				foreach (Player player in players)
				{
					if (player.IsSpawned) player.OnTick();
				}

				// Send player movements
				//if (TickTime % 2 == 0)
				{
					Player[] updatedPlayers = GetUpdatedPlayers(players);
					BroadCastMovement(players, updatedPlayers);
				}

				if (TickTime%100 == 0) // Every 5 seconds
				{
					Player[] staledPlayers = GetStaledPlayers(players);
					if (staledPlayers.Any())
					{
						foreach (var p in staledPlayers)
						{
							ThreadPool.QueueUserWorkItem(delegate(object state)
							{
								Player player = (Player) state;
								player.Disconnect("Too long inactivity.");
							}, p);
						}
					}
				}
			}
			finally
			{
				LastTickProcessingTime = _tickTimer.ElapsedMilliseconds;
				Monitor.Exit(_tickSync);
			}
		}

		public Player[] GetSpawnedPlayers()
		{
			return Players.Values.Where(player => player.IsSpawned).ToArray();
		}

		public Entity[] GetEntites()
		{
			lock (Entities)
			{
				return Entities.ToArray();
			}
		}

		private Player[] GetUpdatedPlayers(Player[] players)
		{
			long tickTime = _worldTickTime*TimeSpan.TicksPerMillisecond;
			long now = DateTime.UtcNow.Ticks;
			return players.Where(player => ((now - player.LastUpdatedTime.Ticks) <= tickTime)).ToArray();
		}

		private Player[] GetStaledPlayers(Player[] players)
		{
			DateTime now = DateTime.UtcNow;
			TimeSpan span = TimeSpan.FromSeconds(300);
			return players.Where(player =>
			{
				bool evict = ((now - player.LastUpdatedTime) > span);
				return evict;
			}).ToArray();
		}

		protected virtual void BroadCastMovement(Player[] players, Player[] updatedPlayers)
		{
			if (updatedPlayers.Length == 0) return;

			List<McpeMovePlayer> moves = new List<McpeMovePlayer>();
			foreach (var player in updatedPlayers)
			{
				PlayerLocation knownPosition = (PlayerLocation) player.KnownPosition.Clone();
				{
					//McpeMovePlayer move = McpeMovePlayer.CreateObject(players.Length);
					McpeMovePlayer move = McpeMovePlayer.CreateObject(players.Length);
					move.entityId = player.EntityId;
					move.x = knownPosition.X;
					move.y = knownPosition.Y + 1.62f;
					move.z = knownPosition.Z;
					move.yaw = knownPosition.Yaw;
					move.pitch = knownPosition.Pitch;
					move.headYaw = knownPosition.HeadYaw;
					move.teleport = 0;
					move.Encode(); // Optmized

					moves.Add(move);
				}
			}

			List<Task> tasks = new List<Task>();
			foreach (var p in players)
			{
				Task sendTask = new Task(obj => ((Player) obj).SendMoveList(moves), p);
				sendTask.Start();
				tasks.Add(sendTask);
			}

			Task.WaitAll(tasks.ToArray());
		}


		protected virtual void BroadCastMovement2(Player[] players, Player[] updatedPlayers)
		{
			if (updatedPlayers.Length == 0) return;

			MemoryStream stream = new MemoryStream();
			foreach (var player in updatedPlayers)
			{
				Player updatedPlayer = player;
				var knownPosition = updatedPlayer.KnownPosition;
				{
					McpeMovePlayer move = McpeMovePlayer.CreateObject();
					move.entityId = updatedPlayer.EntityId;
					move.x = knownPosition.X;
					move.y = knownPosition.Y + 1.62f;
					move.z = knownPosition.Z;
					move.yaw = knownPosition.Yaw;
					move.pitch = knownPosition.Pitch;
					move.headYaw = knownPosition.HeadYaw;
					move.teleport = 0;
					var bytes = move.Encode(); // Optmized
					stream.Write(bytes, 0, bytes.Length);
					move.PutPool();
				}
			}

			McpeBatch batch = McpeBatch.CreateObject(players.Length);
			byte[] buffer = Player.CompressBytes(stream.ToArray(), CompressionLevel.Fastest);
			batch.payloadSize = buffer.Length;
			batch.payload = buffer;
			batch.Encode();

			foreach (var p in players)
			{
				Player player = p;
				Task sendTask = new Task(delegate { player.SendPackage(batch, true); });
				sendTask.Start();
			}
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

					McpeBatch chunk = _worldProvider.GenerateFullBatch(new ChunkCoordinates(pair.Key.Item1, pair.Key.Item2));
					chunksUsed.Add(pair.Key, chunk);

					McpeBatch sendChunk = McpeBatch.CreateObject();
					sendChunk.Source = "GenerateChunks";
					sendChunk.SetEncodedMessage(chunk.Encode());

					yield return sendChunk;
				}

				if (chunksUsed.Count > ViewDistance) Debug.WriteLine("Too many chunks used: {0}", chunksUsed.Count);
			}
		}


		public void RelayBroadcast<T>(Player[] sendList, T message) where T : Package<T>, new()
		{
			RelayBroadcast(null, sendList, message);
		}

		public void RelayBroadcast<T>(T message) where T : Package<T>, new()
		{
			RelayBroadcast(null, GetSpawnedPlayers(), message);
		}

		public void RelayBroadcast<T>(Entity source, T message) where T : Package<T>, new()
		{
			RelayBroadcast(source, GetSpawnedPlayers(), message);
		}

		public void RelayBroadcast<T>(Entity source, Player[] sendList, T message) where T : Package<T>, new()
		{
			if (!message.IsPooled)
			{
				message.MakePoolable();
			}

			if (sendList.Length == 0)
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

				player.SendPackage(message);
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
			MetadataSlot itemSlot = player.Inventory.ItemInHand;
			Item itemInHand = ItemFactory.GetItem(itemSlot.Value.Id, metadata);

			if (itemInHand == null || itemInHand.Id != itemId) return; // Cheat(?)

			itemInHand.UseItem(world, player, blockCoordinates);
		}

		public void Interact(Level world, Player player, short itemId, BlockCoordinates blockCoordinates, short metadata, BlockFace face, Vector3 faceCoords)
		{
			// Make sure we are holding the item we claim to be using
			MetadataSlot itemSlot = player.Inventory.ItemInHand;
			Item itemInHand = ItemFactory.GetItem(itemSlot.Value.Id, metadata);

			if (itemInHand == null || itemInHand.Id != itemId) return; // Cheat(?)

			Block target = GetBlock(blockCoordinates);
			if (target.Interact(world, player, blockCoordinates, face)) return; // Handled in block interaction

			itemInHand.UseItem(world, player, blockCoordinates, face, faceCoords);
		}

		public void BreakBlock(BlockCoordinates blockCoordinates)
		{
			return;
			List<ItemStack> drops = new List<ItemStack>();

			Block block = GetBlock(blockCoordinates);
			block.BreakBlock(this);
			drops.Add(block.GetDrops());

			BlockEntity blockEnity = GetBlockEntity(blockCoordinates);
			if (blockEnity != null)
			{
				RemoveBlockEntity(blockCoordinates);
				drops.AddRange(blockEnity.GetDrops());
			}

			foreach (ItemStack drop in drops)
			{
				DropItem(blockCoordinates, drop);
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

			return entity ?? Entities.FirstOrDefault(e => e.EntityId == targetEntityId);
		}
	}
}