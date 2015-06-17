using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
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
		private int _worldDayCycleTime = 24000;
		//private int _worldDayCycleTime = 14400;

		public BlockCoordinates SpawnPoint { get; set; }
		public List<Player> Players { get; private set; } //TODO: Need to protect this, not threadsafe
		public List<Entity> Entities { get; private set; } //TODO: Need to protect this, not threadsafe
		public List<BlockEntity> BlockEntities { get; private set; } //TODO: Need to protect this, not threadsafe
		public ConcurrentDictionary<BlockCoordinates, int> BlockWithTicks { get; private set; } //TODO: Need to protect this, not threadsafe
		public string LevelId { get; private set; }

		public GameMode GameMode { get; private set; }
		public Difficulty Difficulty { get; private set; }
		public int CurrentWorldTime { get; set; }
		public long StartTimeInTicks { get; private set; }
		public bool WorldTimeStarted { get; private set; }

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
			Players = new List<Player>();
			Entities = new List<Entity>();
			BlockEntities = new List<BlockEntity>();
			BlockWithTicks = new ConcurrentDictionary<BlockCoordinates, int>();
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

		public void Initialize()
		{
			CurrentWorldTime = 6000;
			WorldTimeStarted = false;
			_worldProvider.Initialize();

			SpawnPoint = _worldProvider.GetSpawnPoint();

			if (_worldProvider.IsCaching)
			{
				ThreadPool.QueueUserWorkItem(delegate(object state)
				{
					Stopwatch chunkLoading = new Stopwatch();
					chunkLoading.Start();
					// Pre-cache chunks for spawn coordinates
					int i = 0;
					foreach (var chunk in GenerateChunks(new ChunkCoordinates(SpawnPoint.X >> 4, SpawnPoint.Z >> 4), new Dictionary<Tuple<int, int>, ChunkColumn>()))
					{
						chunk.GetBytes();
						i++;
					}
					Log.InfoFormat("World pre-cache {0} chunks completed in {1}ms", i, chunkLoading.ElapsedMilliseconds);
				});
			}

			StartTimeInTicks = DateTime.UtcNow.Ticks;

			_levelTicker = new Timer(WorldTick, null, 0, _worldTickTime); // MC worlds tick-time
		}

		public virtual void AddPlayer(Player newPlayer, string broadcastText = null, bool spawn = true)
		{
			lock (Players)
			{
				if (Players.Contains(newPlayer)) return;

				if (newPlayer.Username == null) return;

				EntityManager.AddEntity(null, newPlayer);

				foreach (var targetPlayer in GetSpawnedPlayers())
				{
					if (spawn)
					{
						SendAddForPlayer(targetPlayer, newPlayer);
						SendAddForPlayer(newPlayer, targetPlayer);
					}
				}

				foreach (Entity entity in Entities.ToArray())
				{
					SendAddEntityToPlayer(entity, newPlayer);
				}

				if (!Players.Contains(newPlayer)) Players.Add(newPlayer);

				if (!string.IsNullOrEmpty(broadcastText))
				{
					BroadcastTextMessage(broadcastText);
				}

				BroadCastMovement(new[] {newPlayer}, GetSpawnedPlayers());
			}
		}

		public void SendAddForPlayer(Player receiver, Player player)
		{
			if (player == receiver) return;

			receiver.SendPackage(
				new McpeAddPlayer
				{
					clientId = 0,
					username = player.Username,
					entityId = player.EntityId,
					x = player.KnownPosition.X,
					y = player.KnownPosition.Y,
					z = player.KnownPosition.Z,
					yaw = player.KnownPosition.Yaw,
					headYaw = player.KnownPosition.Yaw,
					pitch = player.KnownPosition.Pitch,
					slim = 0,
					skin = player.Skin,
					metadata = player.GetMetadata().GetBytes()
				});

			SendEquipmentForPlayer(receiver, player);

			SendArmorForPlayer(receiver, player);
		}

		public void SendEquipmentForPlayer(Player receiver, Player player)
		{
			receiver.SendPackage(new McpePlayerEquipment()
			{
				entityId = player.EntityId,
				item = player.Inventory.ItemInHand.Value.Id,
				meta = player.Inventory.ItemInHand.Value.Metadata,
				slot = 0
			});
		}

		public void SendArmorForPlayer(Player receiver, Player player)
		{
			receiver.SendPackage(new McpePlayerArmorEquipment()
			{
				entityId = player.EntityId,
				helmet = (byte) (((MetadataSlot) player.Inventory.Armor[0]).Value.Id - 256),
				chestplate = (byte) (((MetadataSlot) player.Inventory.Armor[1]).Value.Id - 256),
				leggings = (byte) (((MetadataSlot) player.Inventory.Armor[2]).Value.Id - 256),
				boots = (byte) (((MetadataSlot) player.Inventory.Armor[3]).Value.Id - 256)
			});
		}


		public virtual void RemovePlayer(Player player, bool despawn = true)
		{
			lock (Players)
			{
				if (!Players.Remove(player)) return;

				if (despawn)
				{
					foreach (var targetPlayer in GetSpawnedPlayers())
					{
						SendRemoveForPlayer(targetPlayer, player);
						SendRemoveForPlayer(player, targetPlayer);
					}
				}
				EntityManager.RemoveEntity(null, player);

				BroadcastTextMessage(string.Format("{0} left the game!", player.Username));
			}
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

			receiver.SendPackage(new McpeRemovePlayer
			{
				clientId = 0,
				entityId = player.EntityId
			});
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

					RelayBroadcast(new McpeAddItemEntity()
					{
						entityId = itemEntity.EntityId,
						item = itemEntity.GetMetadataSlot(),
						x = itemEntity.KnownPosition.X + xr,
						y = itemEntity.KnownPosition.Y + yr,
						z = itemEntity.KnownPosition.Z + zr,
					});
				}
				else
				{
					var addEntity = new McpeAddEntity
					{
						entityType = entity.EntityTypeId,
						entityId = entity.EntityId,
						x = entity.KnownPosition.X,
						y = entity.KnownPosition.Y,
						z = entity.KnownPosition.Z,
						//yaw = entity.KnownPosition.Yaw,
						//pitch = entity.KnownPosition.Pitch,
						metadata = entity.GetMetadata()
					};
					{
						double dx = entity.Velocity.X;
						double dy = entity.Velocity.Y;
						double dz = entity.Velocity.Z;
						double maxVelocity = 3.92d;

						if (dx < -maxVelocity)
						{
							dx = -maxVelocity;
						}

						if (dy < -maxVelocity)
						{
							dy = -maxVelocity;
						}

						if (dz < -maxVelocity)
						{
							dz = -maxVelocity;
						}

						if (dx > maxVelocity)
						{
							dx = maxVelocity;
						}

						if (dy > maxVelocity)
						{
							dy = maxVelocity;
						}

						if (dz > maxVelocity)
						{
							dz = maxVelocity;
						}

						//addEntity.speedX = (short) (dx*8000.0d);
						//addEntity.speedY = (short) (dy*8000.0d);
						//addEntity.speedZ = (short) (dz*8000.0d);
						addEntity.speedX = (float) (dx);
						addEntity.speedY = (float) (dy);
						addEntity.speedZ = (float) (dz);
					}

					RelayBroadcast(addEntity);

					Log.DebugFormat("Metadata: {0}", entity.GetMetadata());

					RelayBroadcast(new McpeSetEntityData
					{
						entityId = entity.EntityId,
						metadata = entity.GetMetadata(),
					});
				}
			}
		}

		public void SendAddEntityToPlayer(Entity entity, Player player)
		{
			if (entity is ItemEntity)
			{
				ItemEntity itemEntity = (ItemEntity) entity;
				player.SendPackage(new McpeAddItemEntity()
				{
					entityId = itemEntity.EntityId,
					item = itemEntity.GetMetadataSlot(),
					x = itemEntity.KnownPosition.X,
					y = itemEntity.KnownPosition.Y,
					z = itemEntity.KnownPosition.Z,
				});
			}
			else
			{
				player.SendPackage(new McpeAddEntity
				{
					entityType = entity.EntityTypeId,
					entityId = entity.EntityId,
					x = entity.KnownPosition.X,
					y = entity.KnownPosition.Y,
					z = entity.KnownPosition.Z,
					metadata = entity.GetMetadata(),
					speedX = (float) entity.Velocity.X,
					speedY = (float) entity.Velocity.Y,
					speedZ = (float) entity.Velocity.Z,
				});
			}
		}

		public void RemoveEntity(Entity entity)
		{
			lock (Entities)
			{
				if (!Entities.Remove(entity)) throw new Exception("Expected entity to exist on remove.");

				RelayBroadcast(new McpeRemoveEntity()
				{
					entityId = entity.EntityId,
				});
			}
		}


		public void RemoveDuplicatePlayers(string username)
		{
			Player[] existingPlayers;
			lock (Players)
			{
				existingPlayers = Players.Where(player => player.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase)).ToArray();
			}

			foreach (var existingPlayer in existingPlayers)
			{
				Log.InfoFormat("Removing staled players on login {0}", username);
				existingPlayer.HandleDisconnectionNotification();
			}
		}

		public void BroadcastTextMessage(string text, Player sender = null)
		{
			var response = new McpeText
			{
				type = McpeText.TypeChat,
				source = sender == null ? "MiNET" : sender.Username,
				message = text
			};

			foreach (var player in GetSpawnedPlayers())
			{
				// Should probaby encode first...
				player.SendPackage((Package) response.Clone());
			}
		}


		private object _tickSync = new object();
		private Stopwatch _tickTimer = new Stopwatch();

		private void WorldTick(object sender)
		{
			if (!Monitor.TryEnter(_tickSync)) return;

			_tickTimer.Restart();
			try
			{
				CurrentWorldTime += 1;
				if (CurrentWorldTime > _worldDayCycleTime) CurrentWorldTime = 0;

				Player[] players = GetSpawnedPlayers();

				//if (CurrentWorldTime%(50*20*5) == 0)
				//{
				//	McpeSetTime message = McpeSetTime.CreateObject();
				//	message.time = CurrentWorldTime;
				//	message.started = (byte) (WorldTimeStarted ? 0x80 : 0x00);

				//	RelayBroadcast(players, message);
				//}

				// Block updates
				//if (CurrentWorldTime%5 == 0)
				{
					foreach (KeyValuePair<BlockCoordinates, int> blockEvent in BlockWithTicks.ToArray())
					{
						if (blockEvent.Value <= CurrentWorldTime)
						{
							GetBlock(blockEvent.Key).OnTick(this);
							int value;
							BlockWithTicks.TryRemove(blockEvent.Key, out value);
						}
					}
				}
				foreach (BlockEntity blockEntity in BlockEntities.ToArray())
				{
					blockEntity.OnTick(this);
				}

				// Entity updates
				foreach (Entity entity in Entities.ToArray())
				{
					entity.OnTick();
				}

				foreach (Player player in players)
				{
					player.OnTick();
				}

				// Player movements
				Player[] updatedPlayers = GetUpdatedPlayers(players);
				BroadCastMovement(players, updatedPlayers);
			}
			finally
			{
				LastTickProcessingTime = _tickTimer.ElapsedMilliseconds;
				Monitor.Exit(_tickSync);
			}
		}

		public long LastTickProcessingTime = 0;

		public Player[] GetSpawnedPlayers()
		{
			lock (Players)
			{
				return Players.Where(player => player.IsSpawned).ToArray();
			}
		}

		private Player[] GetUpdatedPlayers(Player[] players)
		{
			long tickTime = _worldTickTime*TimeSpan.TicksPerMillisecond;
			long now = DateTime.UtcNow.Ticks;
			return players.Where(player => ((now - player.LastUpdatedTime.Ticks) <= tickTime)).ToArray();
		}

		protected virtual void BroadCastMovement(Player[] players, Player[] updatedPlayers)
		{
			if (updatedPlayers.Length == 0) return;

			List<McpeMovePlayer> moves = new List<McpeMovePlayer>();
			foreach (var player in updatedPlayers)
			{
				Player updatedPlayer = player;
				var knownPosition = updatedPlayer.KnownPosition;

				{
					McpeMovePlayer move = McpeMovePlayer.CreateObject(players.Length);
					move.entityId = updatedPlayer.EntityId;
					move.x = knownPosition.X;
					move.y = knownPosition.Y;
					move.z = knownPosition.Z;
					move.yaw = knownPosition.Yaw;
					move.pitch = knownPosition.Pitch;
					move.headYaw = knownPosition.HeadYaw;
					move.teleport = 0;
					move.Encode(); // Optmized

					moves.Add(move);
				}
			}

			foreach (var p in players)
			{
				Player player = p;
				Task sendTask = new Task(delegate { player.SendMoveList(moves); });
				sendTask.Start();
			}
		}


		public IEnumerable<ChunkColumn> GenerateChunks(ChunkCoordinates chunkPosition, Dictionary<Tuple<int, int>, ChunkColumn> chunksUsed)
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

					ChunkColumn chunk = _worldProvider.GenerateChunkColumn(new ChunkCoordinates(pair.Key.Item1, pair.Key.Item2));
					chunksUsed.Add(pair.Key, chunk);

					yield return chunk;
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
			BlockWithTicks[block.Coordinates] = CurrentWorldTime + tickRate;
		}
	}
}