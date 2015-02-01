using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Craft.Net.Common;
using Craft.Net.TerrainGeneration;
using fNbt;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Entities;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;
using MetadataSlot = MiNET.Utils.MetadataSlot;

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
		public static readonly Coordinates3D Up = new Coordinates3D(0, 1, 0);
		public static readonly Coordinates3D Down = new Coordinates3D(0, -1, 0);
		public static readonly Coordinates3D East = new Coordinates3D(0, 0, -1);
		public static readonly Coordinates3D West = new Coordinates3D(0, 0, 1);
		public static readonly Coordinates3D North = new Coordinates3D(1, 0, 0);
		public static readonly Coordinates3D South = new Coordinates3D(-1, 0, 0);

		public IWorldProvider _worldProvider;
		private int _viewDistance = 256;
		private Timer _levelTicker;
		private int _worldTickTime = 50;
		private int _worldDayCycleTime = 24000;
		//private int _worldDayCycleTime = 14400;

		public Coordinates3D SpawnPoint { get; private set; }
		public List<Player> Players { get; private set; } //TODO: Need to protect this, not threadsafe
		public List<Entity> Entities { get; private set; } //TODO: Need to protect this, not threadsafe
		public string LevelId { get; private set; }

		public GameMode GameMode { get; private set; }
		public Difficulty Difficulty { get; private set; }
		public int CurrentWorldTime { get; private set; }
		public long StartTimeInTicks { get; private set; }
		public bool WorldTimeStarted { get; private set; }

		public EntityManager EntityManager { get; private set; }

		public Level(string levelId, IWorldProvider worldProvider = null)
		{
			EntityManager = new EntityManager();
			SpawnPoint = new Coordinates3D(50, 10, 50);
			Players = new List<Player>();
			Entities = new List<Entity>();
			LevelId = levelId;
			GameMode = ConfigParser.GetProperty("DefaultGamemode", GameMode.Creative);
			Difficulty = ConfigParser.GetProperty("Difficulty", Difficulty.Peaceful);
			if (ConfigParser.GetProperty("UsePCWorld", false))
			{
				_worldProvider = new CraftNetAnvilWorldProvider();
			}
			else
			{
				_worldProvider = worldProvider;
			}

			//McpeMovePlayer._pool.FillPool(100);
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
					foreach (var chunk in GenerateChunks(new Coordinates2D(SpawnPoint.X, SpawnPoint.Z), new Dictionary<Tuple<int, int>, ChunkColumn>()))
					{
						chunk.GetBytes();
					}
				});
			}

			StartTimeInTicks = DateTime.UtcNow.Ticks;

			_levelTicker = new Timer(LevelTickerTicked, null, 0, _worldTickTime); // MC worlds tick-time
		}

		public void AddPlayer(Player newPlayer)
		{
			lock (Players)
			{
				if (newPlayer.Username == null) return;

				EntityManager.AddEntity(null, newPlayer);

				Player[] targetPlayers = GetSpawnedPlayers();

				foreach (var targetPlayer in targetPlayers)
				{
					targetPlayer.SendAddForPlayer(newPlayer);
					newPlayer.SendAddForPlayer(targetPlayer);
				}

				BroadcastTextMessage(string.Format("Player {0} joined the game!", newPlayer.Username));

				if (!Players.Contains(newPlayer))
				{
					Players.Add(newPlayer);
				}
				else
				{
					throw new Exception("Player existed in the players list when it should not");
				}
			}
		}

		public void RemovePlayer(Player player)
		{
			lock (Players)
			{
				if (!Players.Remove(player)) throw new Exception("Expected player to exist on remove.");

				foreach (var targetPlayer in GetSpawnedPlayers())
				{
					targetPlayer.SendRemoveForPlayer(player);
					player.SendRemoveForPlayer(targetPlayer);
				}

				BroadcastTextMessage(string.Format("Player {0} left the game!", player.Username));

				EntityManager.RemoveEntity(null, player);
			}
		}

		public void AddEntity(Entity entity)
		{
			lock (Entities)
			{
				EntityManager.AddEntity(null, entity);

				RelayBroadcast(new McpeAddEntity
				{
					entityType = entity.EntityTypeId,
					entityId = entity.EntityId,
					x = entity.KnownPosition.X,
					y = entity.KnownPosition.Y,
					z = entity.KnownPosition.Z
				});

				if (!Entities.Contains(entity))
				{
					Entities.Add(entity);
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
				if (!Entities.Remove(entity)) throw new Exception("Expected entity to exist on remove.");

				RelayBroadcast(new McpeRemoveEntity()
				{
					entityId = entity.EntityId,
				});
			}
		}


		public void RemoveDuplicatePlayers(string username)
		{
			lock (Players)
			{
				var existingPlayers = Players.Where(player => player.Username == username);
				foreach (var existingPlayer in existingPlayers)
				{
					Debug.WriteLine(string.Format("Removing staled players on login {0}", username));
					RemovePlayer(existingPlayer);
				}
			}
		}

		public void BroadcastTextMessage(string text, Player sender = null)
		{
			var response = new McpeMessage
			{
				source = "",
				message = (sender == null ? "" : "<" + sender.Username + "> ") + text
			};

			foreach (var player in GetSpawnedPlayers())
			{
				// Should probaby encode first...
				player.SendPackage((Package) response.Clone());
			}
		}


		private object _tickSync = new object();
		private int tickTimeCount = 0;
		private Stopwatch _tickTimer = new Stopwatch();

		private void LevelTickerTicked(object sender)
		{
			if (!Monitor.TryEnter(_tickSync))
			{
				return;
			}
			else
			{
				_tickTimer.Restart();
				try
				{
					CurrentWorldTime += 1;
					if (CurrentWorldTime > _worldDayCycleTime) CurrentWorldTime = 0;

					Player[] players = GetSpawnedPlayers();

					if (CurrentWorldTime%10 == 0)
					{
						McpeSetTime message = McpeSetTime.CreateObject();
						message.time = CurrentWorldTime;
						message.started = (byte) (WorldTimeStarted ? 0x80 : 0x00);

						RelayBroadcast(players, message, false);
					}

					// broadcast events to all players

					// Movement
					Player[] updatedPlayers = GetUpdatedPlayers(players);
					BroadCastMovement(players, updatedPlayers);

					// Entity updates
				}
				finally
				{
					lastTickProcessingTime = _tickTimer.ElapsedMilliseconds;
					Monitor.Exit(_tickSync);
				}
			}
		}

		public long lastTickProcessingTime = 0;

		private Player[] GetSpawnedPlayers()
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

			foreach (var player in updatedPlayers)
			{
				Player updatedPlayer = player;
				var knownPosition = updatedPlayer.KnownPosition;

				int entityId = EntityManager.GetEntityId(null, player);
				if (entityId == 0) throw new Exception("Souldn't have 0 entity IDs here.");

				var task = new Task(delegate
				{
					McpeMovePlayer move = McpeMovePlayer.CreateObject();
					move.entityId = entityId;
					move.x = knownPosition.X;
					move.y = knownPosition.Y;
					move.z = knownPosition.Z;
					move.yaw = knownPosition.Yaw;
					move.pitch = knownPosition.Pitch;
					move.bodyYaw = knownPosition.BodyYaw;
					move.teleport = 0;
					var bytes = move.Encode(); // Optmized

					foreach (var p in players)
					{
						if (p == updatedPlayer) continue;

						move = move.AddReference(move);
						p.SendMovementForPlayer(updatedPlayer, move);
					}
					move.PutPool();
				});
				tasks.Add(task);
				task.Start();
			}

			Task.WaitAll(tasks.ToArray());
		}


		public IEnumerable<ChunkColumn> GenerateChunks(Coordinates2D chunkPosition, Dictionary<Tuple<int, int>, ChunkColumn> chunksUsed)
		{
			lock (chunksUsed)
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

					ChunkColumn chunk = _worldProvider.GenerateChunkColumn(new Coordinates2D(pair.Key.Item1, pair.Key.Item2));
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


		public void RelayBroadcast<T>(Player[] sendList, T message, bool clone = true) where T : Package<T>, new()
		{
			RelayBroadcast(null, sendList, message, clone);
		}

		public void RelayBroadcast<T>(T message, bool clone = true) where T : Package<T>, new()
		{
			RelayBroadcast(null, GetSpawnedPlayers(), message, clone);
		}

		public void RelayBroadcast<T>(Player source, T message, bool clone = true) where T : Package<T>, new()
		{
			RelayBroadcast(source, GetSpawnedPlayers(), message, clone);
		}

		public void RelayBroadcast<T>(Player source, Player[] sendList, T message, bool clone = true) where T : Package<T>, new()
		{
			if (message.IsPooled && clone) throw new Exception(string.Format("Can not clone a pooled message: {0}", message.GetType().Name));
			if (!message.IsPooled && !clone) throw new Exception(string.Format("Must clone a message not on the pool: {0}", message.GetType().Name));

			if (clone)
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

		public Block GetBlock(PlayerPosition3D blockCoordinates)
		{
			return GetBlock(new Coordinates3D((int) Math.Floor(blockCoordinates.X), (int) Math.Floor(blockCoordinates.Y), (int) Math.Floor(blockCoordinates.Z)));
		}

		public Block GetBlock(int x, int y, int z)
		{
			return GetBlock(new Coordinates3D(x, y, z));
		}

		public Block GetBlock(Coordinates3D blockCoordinates)
		{
			ChunkColumn chunk = _worldProvider.GenerateChunkColumn(new Coordinates2D(blockCoordinates.X/16, blockCoordinates.Z/16));
			byte bid = chunk.GetBlock(blockCoordinates.X & 0x0f, blockCoordinates.Y & 0x7f, blockCoordinates.Z & 0x0f);
			byte metadata = chunk.GetMetadata(blockCoordinates.X & 0x0f, blockCoordinates.Y & 0x7f, blockCoordinates.Z & 0x0f);

			Block block = BlockFactory.GetBlockById(bid);
			block.Coordinates = blockCoordinates;
			block.Metadata = metadata;

			return block;
		}

		public void SetBlock(Block block, bool broadcast = true)
		{
			ChunkColumn chunk = _worldProvider.GenerateChunkColumn(new Coordinates2D(block.Coordinates.X/16, block.Coordinates.Z/16));
			chunk.SetBlock(block.Coordinates.X & 0x0f, block.Coordinates.Y & 0x7f, block.Coordinates.Z & 0x0f, block.Id);
			chunk.SetMetadata(block.Coordinates.X & 0x0f, block.Coordinates.Y & 0x7f, block.Coordinates.Z & 0x0f, block.Metadata);

			if (!broadcast) return;

			var message = McpeUpdateBlock.CreateObject();
			message.x = block.Coordinates.X;
			message.y = (byte) block.Coordinates.Y;
			message.z = block.Coordinates.Z;
			message.block = block.Id;
			message.meta = block.Metadata;

			RelayBroadcast(message, false);
		}


		public BlockEntity GetBlockEntity(Coordinates3D blockCoordinates)
		{
			ChunkColumn chunk = _worldProvider.GenerateChunkColumn(new Coordinates2D(blockCoordinates.X/16, blockCoordinates.Z/16));
			var nbt = chunk.GetBlockEntity(blockCoordinates);
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
			ChunkColumn chunk = _worldProvider.GenerateChunkColumn(new Coordinates2D(blockEntity.Coordinates.X/16, blockEntity.Coordinates.Z/16));
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

		public void Interact(Level world, Player player, short itemId, Coordinates3D blockCoordinates, short metadata, BlockFace face)
		{
			MetadataSlot itemSlot = player.ItemInHand;
			Item itemInHand = ItemFactory.GetItem(itemSlot.Value.Id);

			if (itemInHand == null || itemInHand.Id != itemId) return;

			Block target = GetBlock(blockCoordinates);
			if (target.Interact(world, player, blockCoordinates, face)) return;

			itemInHand.Metadata = metadata;
			itemInHand.UseItem(world, player, blockCoordinates, face);
		}

		public void BreakBlock(Level world, Player player, Coordinates3D blockCoordinates)
		{
			Block block = GetBlock(blockCoordinates);

			//MetadataSlot itemSlot = newPlayer.ItemInHand;
			//Item itemInHand = ItemFactory.GetItem(itemSlot.Value.Id);
			//if (itemInHand == null) return;

			//itemInHand.Metadata = metadata;
			//itemInHand.UseItem(world, newPlayer, blockCoordinates, face);

			block.BreakBlock(world);
		}
	}
}