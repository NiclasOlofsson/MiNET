using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using Craft.Net.Common;
using log4net;
using MiNET.Entities;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;
using ItemStack = MiNET.Utils.ItemStack;
using MetadataByte = MiNET.Utils.MetadataByte;
using MetadataDictionary = MiNET.Utils.MetadataDictionary;
using MetadataShort = MiNET.Utils.MetadataShort;
using MetadataSlot = MiNET.Utils.MetadataSlot;

namespace MiNET
{
	public enum BlockFace
	{
		NegativeY = 0,
		PositiveY = 1,
		NegativeZ = 2,
		PositiveZ = 3,
		NegativeX = 4,
		PositiveX = 5
	}

	public class Player : Entity
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Player));

		private readonly MiNetServer _server;
		private readonly IPEndPoint _endpoint;
		private Dictionary<Tuple<int, int>, ChunkColumn> _chunksUsed;

		private short _mtuSize;
		private int _reliableMessageNumber;
		private int _datagramSequenceNumber = -1; // Very important to start with -1 since Interlock.Increament doesn't offer another solution
		private object _sequenceNumberSync = new object();

		private ConcurrentQueue<Package> _sendQueue = new ConcurrentQueue<Package>();
		private Timer _sendTicker;

		private Coordinates2D _currentChunkPosition;
		private Timer _playerTimer;

		public bool IsConnected { get; set; }
		public HealthManager HealthManager { get; private set; }
		public InventoryManager _InventoryManager { get; private set; }

		public bool IsBot { get; set; }

		public bool IsSpawned { get; private set; }
		public string Username { get; private set; }
		public PermissionManager Permissions { get; set; }

		/// <summary>
		///     Initializes a new instance of the <see cref="Player" /> class.
		/// </summary>
		internal Player()
			: base(-1, null)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Player" /> class.
		/// </summary>
		/// <param name="server">The server.</param>
		/// <param name="endpoint">The endpoint.</param>
		/// <param name="level">The level.</param>
		/// <param name="mtuSize">Size of the mtu.</param>
		public Player(MiNetServer server, IPEndPoint endpoint, Level level, short mtuSize)
			: base(-1, level)
		{
			_server = server;
			_endpoint = endpoint;
			Level = level;
			_mtuSize = mtuSize;
			HealthManager = new HealthManager(this);
			Permissions = new PermissionManager(UserGroup.User);
			_chunksUsed = new Dictionary<Tuple<int, int>, ChunkColumn>();
			EntityId = -1;
			IsSpawned = false;
			KnownPosition = new PlayerPosition3D
			{
				X = Level.SpawnPoint.X,
				Y = Level.SpawnPoint.Y,
				Z = Level.SpawnPoint.Z,
				Yaw = 91,
				Pitch = 28,
				BodyYaw = 91
			};

			_InventoryManager = new InventoryManager(this);
			IsConnected = true;

			_sendTicker = new Timer(SendQueue, null, 10, 10); // RakNet send tick-time
		}

		/// <summary>
		///     Handles the package.
		/// </summary>
		/// <param name="message">The message.</param>
		public void HandlePackage(Package message)
		{
			if (typeof (McpeContainerSetSlot) == message.GetType())
			{
				HandleContainerSetSlot((McpeContainerSetSlot) message);
			}

			else if (typeof (McpePlayerEquipment) == message.GetType())
			{
				HandlePlayerEquipment((McpePlayerEquipment) message);
			}

			else if (typeof (McpePlayerArmorEquipment) == message.GetType())
			{
				HandlePlayerArmorEquipment((McpePlayerArmorEquipment) message);
			}

			else if (typeof (McpeUpdateBlock) == message.GetType())
			{
				// DO NOT USE. Will dissapear from MCPE any release. 
				// It is a bug that it leaks these messages.
				var block = ((McpeUpdateBlock) message);
				Debug.WriteLine("block:" + block.block);
				Debug.WriteLine("meta:" + block.meta);
				Debug.WriteLine("x:" + block.x);
				Debug.WriteLine("y:" + block.y);
				Debug.WriteLine("z:" + block.z);
			}

			else if (typeof (McpeRemoveBlock) == message.GetType())
			{
				HandleRemoveBlock((McpeRemoveBlock) message);
			}

			else if (typeof (McpeAnimate) == message.GetType())
			{
				HandleAnimate((McpeAnimate) message);
			}

			else if (typeof (McpeUseItem) == message.GetType())
			{
				HandleUseItem((McpeUseItem) message);
			}

			else if (typeof (ConnectedPing) == message.GetType())
			{
				HandleConnectedPing((ConnectedPing) message);
			}

			else if (typeof (ConnectionRequest) == message.GetType())
			{
				HandleConnectionRequest((ConnectionRequest) message);
			}

			else if (typeof (DisconnectionNotification) == message.GetType())
			{
				HandleDisconnectionNotification();
			}

			else if (typeof (McpeMessage) == message.GetType())
			{
				HandleMessage((McpeMessage) message);
			}

			else if (typeof (McpeRemovePlayer) == message.GetType())
			{
				// Do nothing right now, but should clear out the entities and stuff
				// from this players internal structure.
			}

			else if (typeof (McpeLogin) == message.GetType())
			{
				HandleLogin((McpeLogin) message);
			}

			else if (typeof (McpeMovePlayer) == message.GetType())
			{
				HandleMovePlayer((McpeMovePlayer) message);
			}

			else if (typeof (McpeInteract) == message.GetType())
			{
				HandleInteract((McpeInteract) message);
			}

			else if (typeof (McpeRespawn) == message.GetType())
			{
				HandleRespawn((McpeRespawn) message);
			}

			else if (typeof (McpeEntityData) == message.GetType())
			{
				HandleEntityData((McpeEntityData) message);
			}

			else if (typeof (InternalPing) == message.GetType())
			{
				HandlePing((InternalPing) message);
			}

			else if (typeof (McpePlayerAction) == message.GetType())
			{
				HandlePlayerAction((McpePlayerAction) message);
			}

			long elapsedMilliseconds = message.Timer.ElapsedMilliseconds;
			if (elapsedMilliseconds > 100)
			{
				Log.WarnFormat("Package ({1}) handling too long {0}ms", elapsedMilliseconds, message.Id);
			}
		}

		/// <summary>
		///     Handles an animate packet.
		/// </summary>
		/// <param name="message">The message.</param>
		private void HandleAnimate(McpeAnimate message)
		{
			message.entityId = EntityId;

			Level.RelayBroadcast(this, message, false);
		}

		/// <summary>
		///     Handles the player action.
		/// </summary>
		/// <param name="message">The message.</param>
		private void HandlePlayerAction(McpePlayerAction message)
		{
			if (message.entityId != EntityId) return;

			switch (message.actionId)
			{
				case 5: // Shoot arrow
				{
					//new McpeEntityEvent()

					break;
				}
				default:
					return;
			}
		}

		/// <summary>
		///     Handles the ping.
		/// </summary>
		/// <param name="message">The message.</param>
		private void HandlePing(InternalPing message)
		{
			SendPackage(message);
		}

		/// <summary>
		///     Handles the entity data.
		/// </summary>
		/// <param name="message">The message.</param>
		private void HandleEntityData(McpeEntityData message)
		{
			Log.DebugFormat("x:  {0}", message.x);
			Log.DebugFormat("y:  {0}", message.y);
			Log.DebugFormat("z:  {0}", message.z);
			Log.DebugFormat("NBT {0}", message.namedtag.NbtFile);

			var blockEntity = Level.GetBlockEntity(new Coordinates3D(message.x, message.y, message.z));

			if (blockEntity == null) return;

			blockEntity.SetCompound(message.namedtag.NbtFile.RootTag);
			Level.SetBlockEntity(blockEntity);
		}

		/// <summary>
		///     Handles the respawn.
		/// </summary>
		/// <param name="msg">The MSG.</param>
		private void HandleRespawn(McpeRespawn msg)
		{
			// reset all health states
			HealthManager.ResetHealth();

			// send teleport to spawn
			KnownPosition = new PlayerPosition3D
			{
				X = Level.SpawnPoint.X,
				Y = Level.SpawnPoint.Y,
				Z = Level.SpawnPoint.Z,
				Yaw = 91,
				Pitch = 28,
				BodyYaw = 91
			};

			SendMovePlayer();

			SendSetHealth();

			SendPackage(new McpeAdventureSettings {flags = 0x20});

			SendPackage(new McpeContainerSetContent
			{
				windowId = 0,
				slotData = _InventoryManager.Slots,
				hotbarData = _InventoryManager.ItemHotbar
			});

			SendPackage(new McpeContainerSetContent
			{
				windowId = 0x78, // Armor windows ID
				slotData = _InventoryManager.Armor,
				hotbarData = null
			});

			// Broadcast spawn to all
			Level.AddPlayer(this);

			BroadcastSetEntityData();
		}

		/// <summary>
		///     Handles the disconnection notification.
		/// </summary>
		public void HandleDisconnectionNotification()
		{
			SavePlayerData();
			IsConnected = false;
			IsSpawned = true;
			Level.RemovePlayer(this);
		}

		/// <summary>
		///     Handles the connection request.
		/// </summary>
		/// <param name="message">The message.</param>
		private void HandleConnectionRequest(ConnectionRequest message)
		{
			var response = new ConnectionRequestAcceptedManual((short) _endpoint.Port, message.timestamp);
			//response.Encode();

			SendPackage(response);
		}

		/// <summary>
		///     Handles the connected ping.
		/// </summary>
		/// <param name="message">The message.</param>
		private void HandleConnectedPing(ConnectedPing message)
		{
			SendPackage(new ConnectedPong
			{
				sendpingtime = message.sendpingtime,
				sendpongtime = DateTimeOffset.UtcNow.Ticks/TimeSpan.TicksPerMillisecond
			});
		}

		/// <summary>
		///     Handles the login.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <exception cref="System.Exception">
		///     No username on login
		///     or
		///     No username on login
		/// </exception>
		private void HandleLogin(McpeLogin message)
		{
			if (Username != null) return; // Already doing login

			Username = message.username;

			if (Username == null) throw new Exception("No username on login");

			// Check if the user already exist, that case bumpt the old one
			Level.RemoveDuplicatePlayers(Username);

			if (Username.StartsWith("Player")) IsBot = true;

			SendPackage(new McpeLoginStatus {status = 0});

			LoadFromFile();

			// Start game
			SendStartGame();
			SendSetTime();
			SendSetSpawnPosition();
			SendSetHealth();
			SendPackage(new McpeSetDifficulty {difficulty = (int) Level.Difficulty});
			SendChunksForKnownPosition();
			LastUpdatedTime = DateTime.UtcNow;
		}

		/// <summary>
		///     Handles the message.
		/// </summary>
		/// <param name="message">The message.</param>
		private void HandleMessage(McpeMessage message)
		{
			string text = message.message;
			if (text.StartsWith("/"))
			{
				new CommandHandler.CommandHandler().HandleCommand(text, this);
			}
			else
			{
				Level.BroadcastTextMessage(text, this);
			}
		}

		/// <summary>
		///     Handles the move player.
		/// </summary>
		/// <param name="message">The message.</param>
		private void HandleMovePlayer(McpeMovePlayer message)
		{
			if (HealthManager.IsDead) return;

			KnownPosition = new PlayerPosition3D(message.x, message.y, message.z) {Pitch = message.pitch, Yaw = message.yaw, BodyYaw = message.bodyYaw};
			LastUpdatedTime = DateTime.UtcNow;

			if (IsBot) return;

			SendChunksForKnownPosition();
		}

		/// <summary>
		///     Handles the remove block.
		/// </summary>
		/// <param name="message">The message.</param>
		private void HandleRemoveBlock(McpeRemoveBlock message)
		{
			Level.BreakBlock(Level, this, new Coordinates3D(message.x, message.y, message.z));
		}

		/// <summary>
		///     Handles the player armor equipment.
		/// </summary>
		/// <param name="message">The message.</param>
		private void HandlePlayerArmorEquipment(McpePlayerArmorEquipment message)
		{
			if (HealthManager.IsDead) return;

			message.entityId = EntityId;

			Level.RelayBroadcast(this, message, false);
		}

		/// <summary>
		///     Handles the player equipment.
		/// </summary>
		/// <param name="message">The message.</param>
		private void HandlePlayerEquipment(McpePlayerEquipment message)
		{
			if (HealthManager.IsDead) return;

			_InventoryManager.ItemInHand.Value.Id = message.item;
			_InventoryManager.ItemInHand.Value.Metadata = message.meta;

			message.entityId = EntityId;

			Level.RelayBroadcast(this, message, false);
		}

		/// <summary>
		///     Handles the container set slot.
		/// </summary>
		/// <param name="message">The message.</param>
		private void HandleContainerSetSlot(McpeContainerSetSlot message)
		{
			if (HealthManager.IsDead) return;

			switch (message.windowId)
			{
				case 0:
					_InventoryManager.Slots[(byte) message.slot] = new MetadataSlot(new ItemStack(message.itemId, (sbyte) message.itemCount, message.itemDamage));
					break;
				case 0x78:
					_InventoryManager.Armor[(byte) message.slot] = new MetadataSlot(new ItemStack(message.itemId, (sbyte) message.itemCount, message.itemDamage));
					break;
			}
			Level.RelayBroadcast(this, new McpePlayerArmorEquipment()
			{
				entityId = EntityId,
				helmet = (byte) (((MetadataSlot) _InventoryManager.Armor[0]).Value.Id - 256),
				chestplate = (byte) (((MetadataSlot) _InventoryManager.Armor[1]).Value.Id - 256),
				leggings = (byte) (((MetadataSlot) _InventoryManager.Armor[2]).Value.Id - 256),
				boots = (byte) (((MetadataSlot) _InventoryManager.Armor[3]).Value.Id - 256)
			});

			Level.RelayBroadcast(this, new McpePlayerEquipment()
			{
				entityId = EntityId,
				item = _InventoryManager.ItemInHand.Value.Id,
				meta = _InventoryManager.ItemInHand.Value.Metadata,
				slot = 0
			});
		}

		/// <summary>
		///     Handles the interact.
		/// </summary>
		/// <param name="message">The message.</param>
		private void HandleInteract(McpeInteract message)
		{
			Player target = (Player) Level.EntityManager.GetEntity(message.targetEntityId);

			if (target == null) return;

			target.HealthManager.TakeHit(this);

			Level.RelayBroadcast(target, new McpeEntityEvent()
			{
				entityId = target.EntityId,
				eventId = (byte) (target.HealthManager.Health <= 0 ? 3 : 2)
			});

			target.SendPackage(new McpeEntityEvent()
			{
				entityId = 0,
				eventId = (byte) (target.HealthManager.Health <= 0 ? 3 : 2)
			});
		}

		/// <summary>
		///     Handles the use item.
		/// </summary>
		/// <param name="message">The message.</param>
		private void HandleUseItem(McpeUseItem message)
		{
			if (message.face <= 5)
			{
				Level.RelayBroadcast(this, new McpeAnimate()
				{
					actionId = 1,
					entityId = EntityId
				});

				Log.DebugFormat("Use item: {0}", message.item);
				Level.Interact(Level, this, message.item, new Coordinates3D(message.x, message.y, message.z), message.meta, (BlockFace) message.face);
			}
			else
			{
				Log.DebugFormat("No face - Use item: {0}", message.item);
				// Probably break block
			}
		}

		/// <summary>
		///     Gets the direction.
		/// </summary>
		/// <returns></returns>
		public byte GetDirection()
		{
			return DirectionByRotationFlat(KnownPosition.Yaw);
		}

		/// <summary>
		/// </summary>
		/// <param name="yaw">The yaw.</param>
		/// <returns></returns>
		public static byte DirectionByRotationFlat(float yaw)
		{
			byte direction = (byte) ((int) Math.Floor((yaw*4F)/360F + 0.5D) & 0x03);
			switch (direction)
			{
				case 0:
					return 1; // West
				case 1:
					return 2; // North
				case 2:
					return 3; // East
				case 3:
					return 0; // South 
			}
			return 0;
		}

		/// <summary>
		///     Sends the start game packet.
		/// </summary>
		private void SendStartGame()
		{
			SendPackage(new McpeStartGame
			{
				seed = 1406827239,
				generator = 1,
				gamemode = (int) Level.GameMode,
				entityId = 0,
				spawnX = (int) KnownPosition.X,
				spawnY = (int) KnownPosition.Y,
				spawnZ = (int) KnownPosition.Z,
				x = KnownPosition.X,
				y = KnownPosition.Y,
				z = KnownPosition.Z
			});
		}

		/// <summary>
		///     Sends the set spawn position packet.
		/// </summary>
		private void SendSetSpawnPosition()
		{
			SendPackage(new McpeSetSpawnPosition
			{
				x = Level.SpawnPoint.X,
				y = (byte) Level.SpawnPoint.Y,
				z = Level.SpawnPoint.Z
			});
		}

		/// <summary>
		///     Sends the chunks for known position.
		/// </summary>
		public void SendChunksForKnownPosition()
		{
			var chunkPosition = new Coordinates2D((int) KnownPosition.X/16, (int) KnownPosition.Z/16);
			if (IsSpawned && _currentChunkPosition == chunkPosition) return;

			_currentChunkPosition = chunkPosition;

			ThreadPool.QueueUserWorkItem(delegate(object state)
			{
				int count = 0;
				foreach (var chunk in Level.GenerateChunks(_currentChunkPosition, _chunksUsed))
				{
					McpeFullChunkData fullChunkData = McpeFullChunkData.CreateObject();
					fullChunkData.chunkData = chunk.GetBytes();

					SendPackage(fullChunkData);

					if (count == 56 && !IsSpawned)
					{
						InitializePlayer();
					}

					count++;
				}
			});
		}

		/// <summary>
		///     Sends the set health packet.
		/// </summary>
		internal void SendSetHealth()
		{
			SendPackage(new McpeSetHealth {health = (byte) HealthManager.Health});
		}

		/// <summary>
		///     Sends the set time packet.
		/// </summary>
		public void SendSetTime()
		{
			// started == true ? 0x80 : 0x00);
			McpeSetTime message = McpeSetTime.CreateObject();
			message.time = Level.CurrentWorldTime;
			message.started = (byte) (Level.WorldTimeStarted ? 0x80 : 0x00);
			SendPackage(message);
		}

		/// <summary>
		///     Sends the move player packet.
		/// </summary>
		public void SendMovePlayer()
		{
			var package = McpeMovePlayer.CreateObject();
			package.entityId = 0;
			package.x = KnownPosition.X;
			package.y = KnownPosition.Y;
			package.z = KnownPosition.Z;
			package.yaw = KnownPosition.Yaw;
			package.pitch = KnownPosition.Pitch;
			package.bodyYaw = KnownPosition.BodyYaw;
			package.teleport = 0x80;

			SendPackage(package);
		}

		/// <summary>
		///     Initializes the player.
		/// </summary>
		private void InitializePlayer()
		{
			//send time again
			SendSetTime();

			// Teleport user (MovePlayerPacket) teleport=1
			SendMovePlayer();

			SendPackage(new McpeAdventureSettings {flags = 0x20});

			SendPackage(new McpeContainerSetContent
			{
				windowId = 0,
				slotData = _InventoryManager.Slots,
				hotbarData = _InventoryManager.ItemHotbar
			});

			SendPackage(new McpeContainerSetContent
			{
				windowId = 0x78, // Armor windows ID
				slotData = _InventoryManager.Armor,
				hotbarData = null
			});

			_playerTimer = new Timer(OnPlayerTick, null, 50, 50);

			IsSpawned = true;
			Level.AddPlayer(this);

			BroadcastSetEntityData();
		}

		/// <summary>
		///     Called when [player tick].
		/// </summary>
		/// <param name="state">The state.</param>
		private void OnPlayerTick(object state)
		{
			HealthManager.OnTick();
		}

		/// <summary>
		///     Sends the add for player.
		/// </summary>
		/// <param name="player">The player.</param>
		public void SendAddForPlayer(Player player)
		{
			if (player == this) return;

			SendPackage(new McpeAddPlayer
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

			SendEquipmentForPlayer(player);

			SendArmorForPlayer(player);
		}

		/// <summary>
		///     Sends the equipment for player.
		/// </summary>
		/// <param name="player">The player.</param>
		private void SendEquipmentForPlayer(Player player)
		{
			SendPackage(new McpePlayerEquipment()
			{
				entityId = player.EntityId,
				item = player._InventoryManager.ItemInHand.Value.Id,
				meta = player._InventoryManager.ItemInHand.Value.Metadata,
				slot = 0
			});
		}

		/// <summary>
		///     Sends the armor for player.
		/// </summary>
		/// <param name="player">The player.</param>
		private void SendArmorForPlayer(Player player)
		{
			SendPackage(new McpePlayerArmorEquipment()
			{
				entityId = player.EntityId,
				helmet = (byte) (((MetadataSlot) player._InventoryManager.Armor[0]).Value.Id - 256),
				chestplate = (byte) (((MetadataSlot) player._InventoryManager.Armor[1]).Value.Id - 256),
				leggings = (byte) (((MetadataSlot) player._InventoryManager.Armor[2]).Value.Id - 256),
				boots = (byte) (((MetadataSlot) player._InventoryManager.Armor[3]).Value.Id - 256)
			});
		}

		/// <summary>
		///     Sends the remove for player.
		/// </summary>
		/// <param name="player">The player.</param>
		public void SendRemoveForPlayer(Player player)
		{
			if (player == this) return;

			SendPackage(new McpeRemovePlayer
			{
				clientId = 0,
				entityId = player.EntityId
			});
		}

		/// <summary>
		///     Broadcasts the entity event.
		/// </summary>
		public void BroadcastEntityEvent()
		{
			Level.RelayBroadcast(this, new McpeEntityEvent()
			{
				entityId = EntityId,
				eventId = (byte) (HealthManager.Health <= 0 ? 3 : 2)
			});

			if (HealthManager.IsDead)
			{
				Level.BroadcastTextMessage("Player " + this.Username + " " + HealthManager.LastDamageCause.ToString().Replace('_', ' '));
			}
		}

		/// <summary>
		///     Broadcasts the set entity data.
		/// </summary>
		public void BroadcastSetEntityData()
		{
			MetadataDictionary metadata = new MetadataDictionary();
			metadata[0] = new MetadataByte((byte) (HealthManager.IsOnFire ? 1 : 0));
			metadata[1] = new MetadataShort(HealthManager.Air);
			metadata[16] = new MetadataByte(0);

			Level.RelayBroadcast(this, new McpeSetEntityData()
			{
				entityId = EntityId,
				namedtag = metadata.GetBytes()
			});
		}

		public void SendMovementForPlayer(Player player, McpeMovePlayer move)
		{
			if (HealthManager.IsDead) return;
			if (player == this) return;

			SendPackage(move);
		}

		/// <summary>
		///     Very important litle method. This does all the sending of packages for
		///     the player class. Treat with respect!
		/// </summary>
		/// <param name="package"></param>
		public void SendPackage(Package package)
		{
			if (!IsConnected) return;

			if (IsSpawned)
			{
				_sendQueue.Enqueue(package);
			}
			else
			{
				//lock (_sequenceNumberSync)
				{
					_server.SendPackage(_endpoint, new List<Package>(new[] {package}), _mtuSize, ref _datagramSequenceNumber, ref _reliableMessageNumber);
				}
			}
		}

		private void SendQueue(object sender)
		{
			if (!IsConnected) return;

			List<Package> messages = new List<Package>();

			int lenght = _sendQueue.Count;
			for (int i = 0; i < lenght; i++)
			{
				Package package;
				if (_sendQueue.TryDequeue(out package)) messages.Add(package);
			}

			if (messages.Count == 0) return;

			//lock (_sequenceNumberSync)
			{
				_server.SendPackage(_endpoint, messages, _mtuSize, ref _datagramSequenceNumber, ref _reliableMessageNumber);
			}
		}

		public void Kill()
		{
			Level.RemovePlayer(this);
		}

		public void SendMessage(string message, Player sender = null)
		{
			var response = new McpeMessage
			{
				source = "",
				message = (sender == null ? "" : "<" + sender.Username + "> ") + message
			};
			SendPackage((Package) response.Clone());
		}

		public void SavePlayerData()
		{
			if (!Directory.Exists("Players")) Directory.CreateDirectory("Players");

			byte[] buffer;
			using (MemoryStream stream = new MemoryStream())
			{
				NbtBinaryWriter writer = new NbtBinaryWriter(stream, false);

				writer.Write(_InventoryManager.Export().Length);
				writer.Write(_InventoryManager.Export());

				writer.Write(HealthManager.Export().Length);
				writer.Write(HealthManager.Export());

				writer.Write(KnownPosition.Export().Length);
				writer.Write(KnownPosition.Export());

				writer.Flush();

				buffer = stream.GetBuffer();
			}
			File.WriteAllBytes("Players/" + Username + ".data", Compression.Compress(buffer));
		}

		public void LoadFromFile()
		{
			try
			{
				if (File.Exists("Players/" + Username + ".data"))
				{
					using (MemoryStream stream = new MemoryStream(Compression.Decompress(File.ReadAllBytes("Players/" + Username + ".data"))))
					{
						NbtBinaryReader reader = new NbtBinaryReader(stream, false);

						int invLength = reader.ReadInt32();
						_InventoryManager.Import(reader.ReadBytes(invLength));

						int healthLength = reader.ReadInt32();
						HealthManager.Import(reader.ReadBytes(healthLength));

						int knownLength = reader.ReadInt32();
						KnownPosition.Import(reader.ReadBytes(knownLength));
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}
	}
}