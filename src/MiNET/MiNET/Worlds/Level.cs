using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Craft.Net.TerrainGeneration;
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
	public enum GameMode
	{
		/// <summary>
		///     Players fight against the enviornment, mobs, and players
		///     with limited resources.
		/// </summary>
		Survival = 0,

		/// <summary>
		///     Players are given unlimited resources, flying, and
		///     invulnerability.
		/// </summary>
		Creative = 1,

		/// <summary>
		///     Similar to survival, with the exception that players may
		///     not place or remove blocks.
		/// </summary>
		Adventure = 2,

		/// <summary>
		///     Similar to creative, with the exception that players may
		///     not place or remove blocks.
		/// </summary>
		Spectator = 3
	}

	public enum Difficulty
	{
		Peaceful,
		Easy,
		Normal,
		Hard
	}


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
		private int _viewDistance = 256;
		// ReSharper disable once NotAccessedField.Local
		private Timer _levelTicker;
		private int _worldTickTime = 50;
		private int _worldDayCycleTime = 24000;
		//private int _worldDayCycleTime = 14400;

		public BlockCoordinates SpawnPoint { get; set; }
		public List<Player> Players { get; private set; } //TODO: Need to protect this, not threadsafe
		public List<Entity> Entities { get; private set; } //TODO: Need to protect this, not threadsafe
		public string LevelId { get; private set; }

		public GameMode GameMode { get; private set; }
		public Difficulty Difficulty { get; private set; }
		public int CurrentWorldTime { get; set; }
		public long StartTimeInTicks { get; private set; }
		public bool WorldTimeStarted { get; private set; }

		public EntityManager EntityManager { get; private set; }
		public InventoryManager InventoryManager { get; private set; }

		public Level(string levelId, IWorldProvider worldProvider = null)
		{
			EntityManager = new EntityManager();
			InventoryManager = new InventoryManager(this);
			SpawnPoint = new BlockCoordinates(50, 10, 50);
			Players = new List<Player>();
			Entities = new List<Entity>();
			LevelId = levelId;
			GameMode = ConfigParser.GetProperty("Gamemode", GameMode.Survival);
			Difficulty = ConfigParser.GetProperty("Difficulty", Difficulty.Peaceful);
			if (ConfigParser.GetProperty("UsePCWorld", false))
			{
				_worldProvider = new CraftNetAnvilWorldProvider();
			}
			else
			{
				_worldProvider = worldProvider;
			}
		}

		public void Initialize()
		{
			CurrentWorldTime = 6000;
			WorldTimeStarted = true;

			var loadIt = new FlatlandGenerator(); // Don't remove

			if (_worldProvider == null) _worldProvider = new FlatlandWorldProvider();
			//if (_worldProvider == null) _worldProvider = new CraftNetAnvilWorldProvider();
			_worldProvider.Initialize();

			SpawnPoint = _worldProvider.GetSpawnPoint();

			if (_worldProvider.IsCaching)
			{
				ThreadPool.QueueUserWorkItem(delegate(object state)
				{
					// Pre-cache chunks for spawn coordinates
					foreach (var chunk in GenerateChunks(new ChunkCoordinates(SpawnPoint.X, SpawnPoint.Z), new Dictionary<Tuple<int, int>, ChunkColumn>(), -1))
					{
						chunk.GetBytes();
					}
				});
			}

			StartTimeInTicks = DateTime.UtcNow.Ticks;

			_levelTicker = new Timer(WorldTick, null, 0, _worldTickTime); // MC worlds tick-time
		}

		public Player GetPlayer(IPEndPoint endPoint)
		{
			foreach (var player in Players)
			{
				if (Equals(player.EndPoint, endPoint))
				{
					return player;
				}
			}
			throw new Exception("Player not found!");
		}

		public void AddPlayer(Player newPlayer)
		{
			lock (Players)
			{
				if (Players.Contains(newPlayer)) return;

				if (newPlayer.Username == null) return;

				EntityManager.AddEntity(null, newPlayer);

				foreach (var targetPlayer in GetSpawnedPlayers())
				{
					SendAddForPlayer(targetPlayer, newPlayer);
					SendAddForPlayer(newPlayer, targetPlayer);
				}

				if (!Players.Contains(newPlayer)) Players.Add(newPlayer);

				BroadcastTextMessage(string.Format("{0} joined the game!", newPlayer.Username));

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
					yaw = (byte) player.KnownPosition.Yaw,
					pitch = (byte) player.KnownPosition.Pitch,
					metadata = new byte[0]
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


		public void RemovePlayer(Player player)
		{
			lock (Players)
			{
				if (!Players.Remove(player)) return;

				foreach (var targetPlayer in GetSpawnedPlayers())
				{
					SendRemoveForPlayer(targetPlayer, player);
					SendRemoveForPlayer(player, targetPlayer);
				}

				EntityManager.RemoveEntity(null, player);

				BroadcastTextMessage(string.Format("{0} left the game!", player.Username));
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
						z = itemEntity.KnownPosition.Z + zr
					});
				}
				else
				{
					RelayBroadcast(new McpeAddEntity
					{
						entityType = entity.EntityTypeId,
						entityId = entity.EntityId,
						x = entity.KnownPosition.X,
						y = entity.KnownPosition.Y,
						z = entity.KnownPosition.Z
					});
				}
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
			var response = new McpeMessage
			{
				source = sender == null ? "MiNET" : sender.Username,
				message = "₽" + text
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

				if (CurrentWorldTime%(50*20*5) == 0)
				{
					McpeSetTime message = McpeSetTime.CreateObject();
					message.time = CurrentWorldTime;
					message.started = (byte) (WorldTimeStarted ? 0x80 : 0x00);

					RelayBroadcast(players, message);
				}

				// Block updates

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

		private void BroadCastMovement(Player[] players, Player[] updatedPlayers)
		{
			List<Task> tasks = new List<Task>();

			Parallel.ForEach(updatedPlayers, delegate(Player player) { });


			foreach (var player in updatedPlayers)
			{
				Player updatedPlayer = player;
				var knownPosition = updatedPlayer.KnownPosition;

				var task = new Task(delegate
				{
					McpeMovePlayer move = McpeMovePlayer.CreateObject(players.Length);
					move.entityId = updatedPlayer.EntityId;
					move.x = knownPosition.X;
					move.y = knownPosition.Y;
					move.z = knownPosition.Z;
					move.yaw = knownPosition.Yaw;
					move.pitch = knownPosition.Pitch;
					move.bodyYaw = knownPosition.BodyYaw;
					move.teleport = 0;
					move.Encode(); // Optmized

					foreach (var p in players)
					{
						if (p == updatedPlayer)
						{
							move.PutPool();
							continue;
						}

						p.SendPackage(move);
					}
				});
				tasks.Add(task);
				task.Start();
			}

			Task.WaitAll(tasks.ToArray());
		}


		public IEnumerable<ChunkColumn> GenerateChunks(ChunkCoordinates chunkPosition, Dictionary<Tuple<int, int>, ChunkColumn> chunksUsed, int timeout = 0)
		{
			if (!Monitor.TryEnter(chunksUsed, timeout)) yield break;

			//lock (chunksUsed)
			{
				Dictionary<Tuple<int, int>, double> newOrders = new Dictionary<Tuple<int, int>, double>();
				double radiusSquared = _viewDistance/Math.PI;
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
						Tuple<int, int> index = GetChunkHash(chunkX, chunkZ);
						newOrders[index] = distance;
					}
				}

				if (newOrders.Count > _viewDistance)
				{
					foreach (var pair in newOrders.OrderByDescending(pair => pair.Value))
					{
						if (newOrders.Count <= _viewDistance) break;
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

				if (chunksUsed.Count > _viewDistance) Debug.WriteLine("Too many chunks used: {0}", chunksUsed.Count);
			}
		}

		private Tuple<int, int> GetChunkHash(int chunkX, int chunkZ)
		{
			return new Tuple<int, int>(chunkX, chunkZ);
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

		public Block GetBlock(PlayerLocation blockCoordinates)
		{
			return GetBlock(new BlockCoordinates((int) Math.Floor(blockCoordinates.X), (int) Math.Floor(blockCoordinates.Y), (int) Math.Floor(blockCoordinates.Z)));
		}

		public Block GetBlock(int x, int y, int z)
		{
			return GetBlock(new BlockCoordinates(x, y, z));
		}

		public Block GetBlock(BlockCoordinates blockCoordinates)
		{
			ChunkColumn chunk = _worldProvider.GenerateChunkColumn(new ChunkCoordinates(blockCoordinates.X/16, blockCoordinates.Z/16));
			byte bid = chunk.GetBlock(blockCoordinates.X & 0x0f, blockCoordinates.Y & 0x7f, blockCoordinates.Z & 0x0f);
			byte metadata = chunk.GetMetadata(blockCoordinates.X & 0x0f, blockCoordinates.Y & 0x7f, blockCoordinates.Z & 0x0f);

			Block block = BlockFactory.GetBlockById(bid);
			block.Coordinates = blockCoordinates;
			block.Metadata = metadata;

			return block;
		}

		public void SetBlock(Block block, bool broadcast = true)
		{
			ChunkColumn chunk = _worldProvider.GenerateChunkColumn(new ChunkCoordinates(block.Coordinates.X/16, block.Coordinates.Z/16));
			chunk.SetBlock(block.Coordinates.X & 0x0f, block.Coordinates.Y & 0x7f, block.Coordinates.Z & 0x0f, block.Id);
			chunk.SetMetadata(block.Coordinates.X & 0x0f, block.Coordinates.Y & 0x7f, block.Coordinates.Z & 0x0f, block.Metadata);

			if (!broadcast) return;

			var message = McpeUpdateBlock.CreateObject();
			message.x = block.Coordinates.X;
			message.y = (byte) block.Coordinates.Y;
			message.z = block.Coordinates.Z;
			message.block = block.Id;
			message.meta = block.Metadata;

			RelayBroadcast(message);
		}


		public BlockEntity GetBlockEntity(BlockCoordinates blockCoordinates)
		{
			ChunkColumn chunk = _worldProvider.GenerateChunkColumn(new ChunkCoordinates(blockCoordinates.X/16, blockCoordinates.Z/16));
			NbtCompound nbt = chunk.GetBlockEntity(blockCoordinates);
			if (nbt == null) return null;

			string id = null;
			var idTag = nbt.Get("id");
			if (idTag != null)
			{
				id = idTag.StringValue;
			}

			if (string.IsNullOrEmpty(id)) return null;

			BlockEntity blockEntity = BlockEntityFactory.GetBlockEntityById(id);
			blockEntity.Coordinates = blockCoordinates;
			blockEntity.SetCompound(nbt);

			return blockEntity;
		}

		public void SetBlockEntity(BlockEntity blockEntity, bool broadcast = true)
		{
			ChunkColumn chunk = _worldProvider.GenerateChunkColumn(new ChunkCoordinates(blockEntity.Coordinates.X/16, blockEntity.Coordinates.Z/16));
			chunk.SetBlockEntity(blockEntity.Coordinates, blockEntity.GetCompound());

			if (!broadcast) return;

			Nbt nbt = new Nbt
			{
				NbtFile = new NbtFile
				{
					BigEndian = false,
					RootTag = blockEntity.GetCompound()
				}
			};

			var entityData = new McpeEntityData
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
			ChunkColumn chunk = _worldProvider.GenerateChunkColumn(new ChunkCoordinates(blockCoordinates.X/16, blockCoordinates.Z/16));
			var nbt = chunk.GetBlockEntity(blockCoordinates);

			if (nbt == null) return;

			chunk.RemoveBlockEntity(blockCoordinates);
		}

		public void Interact(Level world, Player player, short itemId, BlockCoordinates blockCoordinates, short metadata, BlockFace face)
		{
			// Make sure we are holding the item we claim to be using
			MetadataSlot itemSlot = player.Inventory.ItemInHand;
			Item itemInHand = ItemFactory.GetItem(itemSlot.Value.Id);

			if (itemInHand == null || itemInHand.Id != itemId) return; // Cheat(?)

			Block target = GetBlock(blockCoordinates);
			if (target.Interact(world, player, blockCoordinates, face)) return; // Handled in block interaction

			itemInHand.Metadata = metadata;
			itemInHand.UseItem(world, player, blockCoordinates, face);
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

			Item item = ItemFactory.GetItem(drop.Id);
			item.Metadata = drop.Metadata;

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
	}
}