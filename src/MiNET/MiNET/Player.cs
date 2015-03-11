using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using log4net;
using MiNET.Entities;
using MiNET.Items;
using MiNET.Net;
using MiNET.Plugins;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET
{
	public class Player : Entity
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Player));

		public MiNetServer Server { get; private set; }
		public IPEndPoint EndPoint { get; private set; }
		private Dictionary<Tuple<int, int>, ChunkColumn> _chunksUsed;

		private short _mtuSize;
		private int _reliableMessageNumber;
		private int _datagramSequenceNumber = -1; // Very important to start with -1 since Interlock.Increament doesn't offer another solution
		private ConcurrentQueue<Package> _sendQueue = new ConcurrentQueue<Package>();
		// ReSharper disable once NotAccessedField.Local
		private Timer _sendTicker;

		private ChunkCoordinates _currentChunkPosition;
		private bool _isBot;
		private Inventory _openInventory;
		private PluginManager _pluginManager;

		public bool IsConnected { get; set; }

		public PlayerInventory Inventory { get; private set; }

		public bool Console { get; set; }

		public bool IsSpawned { get; private set; }
		public string Username { get; private set; }
		public PermissionManager Permissions { get; set; }

		public Player(MiNetServer server, IPEndPoint endPoint, Level level, PluginManager pluginManager, short mtuSize) : base(-1, level)
		{
			Server = server;
			EndPoint = endPoint;
			_mtuSize = mtuSize;
			Level = level;
			_pluginManager = pluginManager;

			Permissions = new PermissionManager(UserGroup.User);
			Permissions.AddPermission("*"); //All users can use all commands. (For debugging purposes)

			Inventory = new PlayerInventory(this);

			_chunksUsed = new Dictionary<Tuple<int, int>, ChunkColumn>();

			IsSpawned = false;
			IsConnected = true;

			KnownPosition = new PlayerLocation
			{
				X = Level.SpawnPoint.X,
				Y = Level.SpawnPoint.Y,
				Z = Level.SpawnPoint.Z,
				Yaw = 91,
				Pitch = 28,
				BodyYaw = 91
			};

			_sendTicker = new Timer(SendQueue, null, 10, 10); // RakNet send tick-time
		}

		/// <summary>
		///     Handles the package.
		/// </summary>
		/// <param name="message">The message.</param>
		public void HandlePackage(Package message)
		{
			var result = _pluginManager.PluginPacketHandler(message, true, this);
			if (result != message) message.PutPool();
			message = result;

			if (message == null) return;

			if (typeof (McpeContainerSetSlot) == message.GetType())
			{
				HandleContainerSetSlot((McpeContainerSetSlot) message);
			}

			else if (typeof (McpeContainerClose) == message.GetType())
			{
				HandleMcpeContainerClose((McpeContainerClose) message);
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
				//var block = ((McpeUpdateBlock) message);
				//Debug.WriteLine("block:" + block.block);
				//Debug.WriteLine("meta:" + block.meta);
				//Debug.WriteLine("x:" + block.x);
				//Debug.WriteLine("y:" + block.y);
				//Debug.WriteLine("z:" + block.z);
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

			else if (typeof (McpeDropItem) == message.GetType())
			{
				HandlePlayerDropItem((McpeDropItem) message);
			}

			long elapsedMilliseconds = message.Timer.ElapsedMilliseconds;
			if (elapsedMilliseconds > 100)
			{
				Log.WarnFormat("Package ({1}) handling too long {0}ms", elapsedMilliseconds, message.Id);
			}
		}

		private void HandlePlayerDropItem(McpeDropItem message)
		{
			if (!Inventory.HasItem(message.item)) return;

			ItemStack itemStack = message.item.Value;

			Item item = ItemFactory.GetItem(itemStack.Id, itemStack.Metadata);

			var itemEntity = new ItemEntity(Level, item)
			{
				KnownPosition =
				{
					X = KnownPosition.X,
					Y = KnownPosition.Y,
					Z = KnownPosition.Z
				},
				Count = itemStack.Count
			};

			Level.AddEntity(itemEntity);
		}

		/// <summary>
		///     Handles an animate packet.
		/// </summary>
		/// <param name="message">The message.</param>
		private void HandleAnimate(McpeAnimate message)
		{
			message.entityId = EntityId;
			Log.DebugFormat("Action: {0}", message.actionId);

			Level.RelayBroadcast(this, message);
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

			var blockEntity = Level.GetBlockEntity(new BlockCoordinates(message.x, message.y, message.z));

			if (blockEntity == null) return;

			blockEntity.SetCompound(message.namedtag.NbtFile.RootTag);
			Level.SetBlockEntity(blockEntity);
		}

		public void SpawnLevel(Level level)
		{
			DespawnEntity();
			_chunksUsed.Clear();
			Level = level;
			HandleRespawn(null);
			SendChunksForKnownPosition();
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
			KnownPosition = new PlayerLocation
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
				slotData = Inventory.Slots,
				hotbarData = Inventory.ItemHotbar
			});

			SendPackage(new McpeContainerSetContent
			{
				windowId = 0x78, // Armor windows ID
				slotData = Inventory.Armor,
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
			var response = new ConnectionRequestAcceptedManual((short) EndPoint.Port, message.timestamp);

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

			//Success = 0;
			//FailedClientIsOld = 1;
			//FailedServerIsOld, FailedClientIsNew = 2;
			//FailedPlayerAuthentication = 3;

			//if (Server.UserManager != null)
			//{
			//	if (Username == null || Server.UserManager.FindByName(Username) == null)
			//	{
			//		//TODO: Must implement disconnect properly. This is not enough for the client to "get it".
			//		SendPackage(new McpeLoginStatus {status = 3});
			//		return;
			//	}
			//}

			// Check if the user already exist, that case bumpt the old one
			Level.RemoveDuplicatePlayers(Username);

			if (Username.StartsWith("Player")) _isBot = true;

			//LoadFromFile();

			// Start game

			Level.EntityManager.AddEntity(null, this);

			SendPackage(new McpeLoginStatus {status = 0});
			SendStartGame();
			SendSetTime();
			SendSetSpawnPosition();
			SendSetHealth();
			SendPackage(new McpeSetDifficulty {difficulty = (int) Level.Difficulty});
			SendChunksForKnownPosition();
			LastUpdatedTime = DateTime.UtcNow;
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
				slotData = Inventory.Slots,
				hotbarData = Inventory.ItemHotbar
			});

			SendPackage(new McpeContainerSetContent
			{
				windowId = 0x78, // Armor windows ID
				slotData = Inventory.Armor,
				hotbarData = null
			});

			IsSpawned = true;
			Level.AddPlayer(this);

			BroadcastSetEntityData();
		}


		/// <summary>
		///     Handles the message.
		/// </summary>
		/// <param name="message">The message.</param>
		private void HandleMessage(McpeMessage message)
		{
			string text = message.message;
			if (text.StartsWith("/") || text.StartsWith("."))
			{
				_pluginManager.HandleCommand(Server.UserManager, text, this);
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

			KnownPosition = new PlayerLocation
			{
				X = message.x,
				Y = message.y,
				Z = message.z,
				Pitch = message.pitch,
				Yaw = message.yaw,
				BodyYaw = message.bodyYaw
			};

			LastUpdatedTime = DateTime.UtcNow;

			if (_isBot) return;

			SendChunksForKnownPosition();
		}

		/// <summary>
		///     Handles the remove block.
		/// </summary>
		/// <param name="message">The message.</param>
		private void HandleRemoveBlock(McpeRemoveBlock message)
		{
			Level.BreakBlock(new BlockCoordinates(message.x, message.y, message.z));
		}

		/// <summary>
		///     Handles the player armor equipment.
		/// </summary>
		/// <param name="message">The message.</param>
		private void HandlePlayerArmorEquipment(McpePlayerArmorEquipment message)
		{
			if (HealthManager.IsDead) return;

			message.entityId = EntityId;

			Level.RelayBroadcast(this, message);
		}

		/// <summary>
		///     Handles the player equipment.
		/// </summary>
		/// <param name="message">The message.</param>
		private void HandlePlayerEquipment(McpePlayerEquipment message)
		{
			if (HealthManager.IsDead) return;

			Inventory.ItemInHand.Value.Id = message.item;
			Inventory.ItemInHand.Value.Metadata = message.meta;

			message.entityId = EntityId;

			Level.RelayBroadcast(this, message);
		}


		public void OpenInventory(BlockCoordinates inventoryCoord)
		{
			if (_openInventory != null) return;

			// get inventory from coordinates
			// - get blockentity
			// - get inventory from block entity

			Inventory inventory = Level.InventoryManager.GetInventory(inventoryCoord);

			if (inventory == null) return;

			// get inventory # from inventory manager
			// set inventory as active on player

			_openInventory = inventory;

			// subscribe to inventory changes
			inventory.InventoryChange += OnInventoryChange;

			// open inventory

			SendPackage(
				new McpeContainerOpen()
				{
					windowId = inventory.Id,
					type = inventory.Type,
					slotCount = inventory.Size,
					x = inventoryCoord.X,
					y = inventoryCoord.Y,
					z = inventoryCoord.Z,
				});

			SendPackage(
				new McpeContainerSetContent()
				{
					windowId = inventory.Id,
					slotData = inventory.Slots,
				});

			if (inventory.Type == 0) // Chest open animation
			{
				SendPackage(
					new McpeTileEvent()
					{
						x = inventoryCoord.X,
						y = inventoryCoord.Y,
						z = inventoryCoord.Z,
						case1 = 1,
						case2 = 2,
					});
			}
		}

		private void OnInventoryChange(Inventory inventory, byte slot, ItemStack itemStack)
		{
			SendPackage(new McpeContainerSetSlot()
			{
				windowId = inventory.Id,
				slot = slot,
				itemCount = itemStack.Count,
				itemId = itemStack.Id,
				itemDamage = itemStack.Metadata,
			});
		}

		/// <summary>
		///     Handles the container set slot.
		/// </summary>
		/// <param name="message">The message.</param>
		private void HandleContainerSetSlot(McpeContainerSetSlot message)
		{
			if (HealthManager.IsDead) return;

			// on all set container content, check if we have active inventory
			// and update that inventory.
			// Inventory manager makes sure other players with the same inventory open will 
			// also get the update.

			var itemStack = new ItemStack(message.itemId, message.itemCount, message.itemDamage);
			var metadataSlot = new MetadataSlot(itemStack);

			Inventory inventory = Level.InventoryManager.GetInventory(message.windowId);
			if (inventory != null)
			{
				inventory.SetSlot((byte) message.slot, itemStack);
				return;
			}

			switch (message.windowId)
			{
				case 0:
					Inventory.Slots[(byte) message.slot] = metadataSlot;
					break;
				case 0x78:
					Inventory.Armor[(byte) message.slot] = metadataSlot;
					break;
			}

			Level.RelayBroadcast(this, new McpePlayerArmorEquipment()
			{
				entityId = EntityId,
				helmet = (byte) (((MetadataSlot) Inventory.Armor[0]).Value.Id - 256),
				chestplate = (byte) (((MetadataSlot) Inventory.Armor[1]).Value.Id - 256),
				leggings = (byte) (((MetadataSlot) Inventory.Armor[2]).Value.Id - 256),
				boots = (byte) (((MetadataSlot) Inventory.Armor[3]).Value.Id - 256)
			});

			Level.RelayBroadcast(this, new McpePlayerEquipment()
			{
				entityId = EntityId,
				item = Inventory.ItemInHand.Value.Id,
				meta = Inventory.ItemInHand.Value.Metadata,
				slot = 0
			});
		}

		private void HandleMcpeContainerClose(McpeContainerClose message)
		{
			if (_openInventory == null) return;

			// unsubscribe to inventory changes
			_openInventory.InventoryChange -= OnInventoryChange;

			// close container 
			if (_openInventory.Type == 0)
			{
				SendPackage(
					new McpeTileEvent()
					{
						x = _openInventory.Coordinates.X,
						y = _openInventory.Coordinates.Y,
						z = _openInventory.Coordinates.Z,
						case1 = 1,
						case2 = 0,
					});
			}

			// active inventory set to null
			_openInventory = null;
		}

		/// <summary>
		///     Handles the interact.
		/// </summary>
		/// <param name="message">The message.</param>
		private void HandleInteract(McpeInteract message)
		{
			Player target = Level.EntityManager.GetEntity(message.targetEntityId) as Player;

			if (target == null) return;

			target.HealthManager.TakeHit(this, ItemFactory.GetItem(this.Inventory.ItemInHand.Value.Id).GetDamage(), DamageCause.EntityAttack);

			target.BroadcastEntityEvent();
		}

		/// <summary>
		///     Handles the use item.
		/// </summary>
		/// <param name="message">The message.</param>
		private void HandleUseItem(McpeUseItem message)
		{
			Log.DebugFormat("Use item: {0}", message.item);
			Log.DebugFormat("Entity ID: {0}", message.entityId);
			Log.DebugFormat("meta:  {0}", message.meta);
			Log.DebugFormat("x:  {0}", message.x);
			Log.DebugFormat("y:  {0}", message.y);
			Log.DebugFormat("z:  {0}", message.z);
			Log.DebugFormat("face:  {0}", message.face);
			Log.DebugFormat("fx:  {0}", message.fx);
			Log.DebugFormat("fy:  {0}", message.fy);
			Log.DebugFormat("fz:  {0}", message.fz);
			Log.DebugFormat("px:  {0}", message.positionX);
			Log.DebugFormat("py:  {0}", message.positionY);
			Log.DebugFormat("pz:  {0}", message.positionZ);

			if (message.face <= 5)
			{
				Level.RelayBroadcast(this, new McpeAnimate()
				{
					actionId = 1,
					entityId = EntityId
				});

				Vector3 faceCoords = new Vector3(message.fx, message.fy, message.fz);

				Level.Interact(Level, this, message.item, new BlockCoordinates(message.x, message.y, message.z), message.meta, (BlockFace) message.face, faceCoords);
			}
			else
			{
				// Snowballs and shit
			}
		}

		private void SendStartGame()
		{
			SendPackage(new McpeStartGame
			{
				seed = 1406827239,
				generator = 1,
				gamemode = (int) Level.GameMode,
				entityId = EntityId,
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
		private void SendChunksForKnownPosition()
		{
			var chunkPosition = new ChunkCoordinates((int) KnownPosition.X/16, (int) KnownPosition.Z/16);
			if (IsSpawned && _currentChunkPosition == chunkPosition) return;

			_currentChunkPosition = chunkPosition;

			ThreadPool.QueueUserWorkItem(delegate(object state)
			{
				int count = 0;
				foreach (var chunk in Level.GenerateChunks(_currentChunkPosition, _chunksUsed, IsSpawned ? 0 : -1))
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

		internal void SendSetHealth()
		{
			SendPackage(new McpeSetHealth {health = (byte) HealthManager.Health});
		}

		public void SendSetTime()
		{
			// started == true ? 0x80 : 0x00);
			McpeSetTime message = McpeSetTime.CreateObject();
			message.time = Level.CurrentWorldTime;
			message.started = (byte) (Level.WorldTimeStarted ? 0x80 : 0x00);
			SendPackage(message);
		}

		public void SendMovePlayer()
		{
			var package = McpeMovePlayer.CreateObject();
			package.entityId = EntityId;
			package.x = KnownPosition.X;
			package.y = KnownPosition.Y;
			package.z = KnownPosition.Z;
			package.yaw = KnownPosition.Yaw;
			package.pitch = KnownPosition.Pitch;
			package.bodyYaw = KnownPosition.BodyYaw;
			package.teleport = 0x80;

			SendPackage(package);
		}

		public override void OnTick()
		{
			base.OnTick();
		}

		public override void DespawnEntity()
		{
			Level.RemovePlayer(this);
		}

		public void SendMessage(string text, Player sender = null)
		{
			var response = new McpeMessage
			{
				source = sender == null ? "MiNET" : sender.Username,
				message = "₽" + text
			};
			SendPackage((Package) response.Clone());
		}

		public void BroadcastEntityEvent()
		{
			Level.RelayBroadcast(new McpeEntityEvent()
			{
				entityId = EntityId,
				eventId = (byte) (HealthManager.Health <= 0 ? 3 : 2)
			});

			if (HealthManager.IsDead)
			{
				Player player = HealthManager.LastDamageSource as Player;
				string cause = string.Format(HealthManager.GetDescription(HealthManager.LastDamageCause), Username, player == null ? "" : player.Username);
				Level.BroadcastTextMessage(cause);
			}
		}

		/// <summary>
		///     Very important litle method. This does all the sending of packages for
		///     the player class. Treat with respect!
		/// </summary>
		/// <param name="package"></param>
		public void SendPackage(Package package)
		{
			if (!IsConnected) return;


			var result = _pluginManager.PluginPacketHandler(package, false, this);
			if (result != package) package.PutPool();
			package = result;

			if (package == null) return;

			if (IsSpawned)
			{
				_sendQueue.Enqueue(package);
			}
			else
			{
				Server.SendPackage(EndPoint, new List<Package>(new[] {package}), _mtuSize, ref _datagramSequenceNumber, ref _reliableMessageNumber);
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
			Server.SendPackage(EndPoint, messages, _mtuSize, ref _datagramSequenceNumber, ref _reliableMessageNumber);
		}

		public void SavePlayerData()
		{
			if (!ConfigParser.GetProperty("save_pe", true)) return;

			if (!Directory.Exists("Players")) Directory.CreateDirectory("Players");

			byte[] buffer;
			using (MemoryStream stream = new MemoryStream())
			{
				NbtBinaryWriter writer = new NbtBinaryWriter(stream, false);

				writer.Write(Inventory.Export().Length);
				writer.Write(Inventory.Export());

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
			if (!ConfigParser.GetProperty("load_pe", true)) return;

			try
			{
				if (File.Exists("Players/" + Username + ".data"))
				{
					using (MemoryStream stream = new MemoryStream(Compression.Decompress(File.ReadAllBytes("Players/" + Username + ".data"))))
					{
						NbtBinaryReader reader = new NbtBinaryReader(stream, false);

						int invLength = reader.ReadInt32();
						Inventory.Import(reader.ReadBytes(invLength));

						int healthLength = reader.ReadInt32();
						HealthManager.Import(reader.ReadBytes(healthLength));

						int knownLength = reader.ReadInt32();
						KnownPosition.Import(reader.ReadBytes(knownLength));
					}
				}
			}
			catch (Exception ex)
			{
				//Console.WriteLine(ex);
			}
		}
	}
}