using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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
		public int ClientId { get; set; }
		public long ClientGuid { get; set; }
		public PermissionManager Permissions { get; set; }

		public string Skin { get; set; }

		public Player(MiNetServer server, IPEndPoint endPoint, Level level, PluginManager pluginManager, short mtuSize) : base(-1, level)
		{
			Width = 0.6;
			Length = 0.6;
			Height = 1.80;

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
				HeadYaw = 91
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

			else if (typeof (McpeText) == message.GetType())
			{
				HandleMessage((McpeText) message);
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

			else if (typeof (McpeTileEntityData) == message.GetType())
			{
				HandleEntityData((McpeTileEntityData) message);
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

			if (message.Timer.IsRunning)
			{
				long elapsedMilliseconds = message.Timer.ElapsedMilliseconds;
				if (elapsedMilliseconds > 100)
				{
					Log.WarnFormat("Package ({1}) handling too long {0}ms", elapsedMilliseconds, message.Id);
				}
			}
			else
			{
				Log.DebugFormat("Package ({0}) timer not started.", message.Id);
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

			//Level.RelayBroadcast(this, message);
		}

		/// <summary>
		///     Handles the player action.
		/// </summary>
		/// <param name="message">The message.</param>
		private void HandlePlayerAction(McpePlayerAction message)
		{
			Log.DebugFormat("Player action: {0}", message.actionId);
			Log.DebugFormat("Entity ID: {0}", message.entityId);
			Log.DebugFormat("Action ID:  {0}", message.actionId);
			Log.DebugFormat("x:  {0}", message.x);
			Log.DebugFormat("y:  {0}", message.y);
			Log.DebugFormat("z:  {0}", message.z);
			Log.DebugFormat("face:  {0}", message.face);

			if (message.entityId != EntityId) return;

			switch (message.actionId)
			{
				case 5: // Shoot arrow
				{
					if (_itemUseTimer == null) return;

					MetadataSlot itemSlot = Inventory.ItemInHand;
					Item itemInHand = ItemFactory.GetItem(itemSlot.Value.Id);

					if (itemInHand == null) return; // Cheat(?)

					_itemUseTimer.Stop();
					itemInHand.Release(Level, this, new BlockCoordinates(message.x, message.y, message.z), _itemUseTimer.ElapsedMilliseconds);

					_itemUseTimer = null;

					MetadataDictionary metadata = new MetadataDictionary();
					metadata[0] = new MetadataByte(0);
					Level.RelayBroadcast(this, new McpeSetEntityData
					{
						entityId = EntityId,
						metadata = metadata,
					});

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
		private void HandleEntityData(McpeTileEntityData message)
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
			SendSetSpawnPosition();
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
				HeadYaw = 91
			};

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

			BroadcastSetEntityData();

			// Broadcast spawn to all
			Level.AddPlayer(this);

			SendMovePlayer();
		}

		/// <summary>
		///     Handles the disconnection notification.
		/// </summary>
		public void HandleDisconnectionNotification()
		{
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
			ClientGuid = message.clientGuid;
			EntityId = ClientGuid;

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
			ClientId = message.clientId;
			Skin = message.skin;

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

			//const LOGIN_SUCCESS = 0;
			//const LOGIN_FAILED_CLIENT = 1;
			//const LOGIN_FAILED_SERVER = 2;
			//const PLAYER_SPAWN = 3;

			SendPackage(new McpePlayerStatus {status = 0});
			SendStartGame();
			SendSetSpawnPosition();
			SendSetTime();
			SendPackage(new McpeSetDifficulty {difficulty = (int) Level.Difficulty});
			SendPackage(new McpeAdventureSettings {flags = 0x80});
			SendSetHealth();

			SendPackage(new McpeSetEntityData
			{
				entityId = EntityId,
				metadata = GetMetadata()
			});

			Level.AddPlayer(this, string.Format("{0} joined the game!", Username));

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

			SendPackage(new McpeRespawn
			{
				x = KnownPosition.X,
				y = KnownPosition.Y,
				z = KnownPosition.Z
			});

			SendPackage(new McpePlayerStatus {status = 3});

			SendChunksForKnownPosition();

			IsSpawned = true;

			LastUpdatedTime = DateTime.UtcNow;
		}

		/// <summary>
		///     Initializes the player.
		/// </summary>
		private void InitializePlayer()
		{
			//TODO: Send MobEffects here

			//send time again
			SendSetTime();


			// Teleport user (MovePlayerPacket) teleport=1
			//SendMovePlayer();

			//BroadcastSetEntityData();
		}


		/// <summary>
		///     Handles the message.
		/// </summary>
		/// <param name="message">The message.</param>
		private void HandleMessage(McpeText message)
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
				HeadYaw = message.headYaw
			};

			LastUpdatedTime = DateTime.UtcNow;

			if (_isBot) return;

			//if (Level.Random.Next(0, 5) == 0)
			//{
			//	int data = 0;
			//	//data = (int) uint.Parse("FFFF0000", NumberStyles.HexNumber);
			//	data = Level.Random.Next((int) uint.Parse("FFFF0000", NumberStyles.HexNumber), (int) uint.Parse("FFFFFFFF", NumberStyles.HexNumber));

			//	Level.RelayBroadcast(new McpeLevelEvent
			//	{
			//		eventId = 0x4000 | 22,
			//		x = KnownPosition.X,
			//		//y = KnownPosition.Y - 1.62f,
			//		y = KnownPosition.Y - 1f,
			//		z = KnownPosition.Z,
			//		data = data
			//	});
			//}

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
			Level.SetBlockEntity(inventory.BlockEntity, false);

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

			Log.DebugFormat("Interact Action ID: {0}", message.actionId);
			Log.DebugFormat("Interact Target Entity ID: {0}", message.targetEntityId);

			if (target == null) return;

			target.HealthManager.TakeHit(this, CalculateDamage(target), DamageCause.EntityAttack);
		}

		private int CalculateDamage(Player target)
		{
			double armorValue = 0;

			for (byte i = 0; i < target.Inventory.Armor.Count; i++)
			{
				MetadataSlot id = (MetadataSlot) (target.Inventory.Armor[i]);
				var armorPiece = ItemFactory.GetItem(id.Value.Id);
				if (armorPiece.ItemType == ItemType.Helmet)
				{
					switch (armorPiece.ItemMaterial)
					{
						case ItemMaterial.Leather:
							armorValue += 1;
							break;
						case ItemMaterial.Gold:
							armorValue += 2;
							break;
						case ItemMaterial.Chain:
							armorValue += 2;
							break;
						case ItemMaterial.Iron:
							armorValue += 2;
							break;
						case ItemMaterial.Diamond:
							armorValue += 3;
							break;
					}
				}

				if (armorPiece.ItemType == ItemType.Chestplate)
				{
					switch (armorPiece.ItemMaterial)
					{
						case ItemMaterial.Leather:
							armorValue += 3;
							break;
						case ItemMaterial.Gold:
							armorValue += 5;
							break;
						case ItemMaterial.Chain:
							armorValue += 5;
							break;
						case ItemMaterial.Iron:
							armorValue += 6;
							break;
						case ItemMaterial.Diamond:
							armorValue += 8;
							break;
					}
				}

				if (armorPiece.ItemType == ItemType.Leggings)
				{
					switch (armorPiece.ItemMaterial)
					{
						case ItemMaterial.Leather:
							armorValue += 2;
							break;
						case ItemMaterial.Gold:
							armorValue += 3;
							break;
						case ItemMaterial.Chain:
							armorValue += 4;
							break;
						case ItemMaterial.Iron:
							armorValue += 5;
							break;
						case ItemMaterial.Diamond:
							armorValue += 6;
							break;
					}
				}

				if (armorPiece.ItemType == ItemType.Boots)
				{
					switch (armorPiece.ItemMaterial)
					{
						case ItemMaterial.Leather:
							armorValue += 1;
							break;
						case ItemMaterial.Gold:
							armorValue += 1;
							break;
						case ItemMaterial.Chain:
							armorValue += 1;
							break;
						case ItemMaterial.Iron:
							armorValue += 2;
							break;
						case ItemMaterial.Diamond:
							armorValue += 3;
							break;
					}
				}
			}

			armorValue *= 0.04; // Each armor point represent 4% reduction

			int damage = ItemFactory.GetItem(Inventory.ItemInHand.Value.Id).GetDamage(); //Item Damage.

			damage = (int) Math.Floor(damage*(1.0 - armorValue));

			return damage;
		}


		private Stopwatch _itemUseTimer;

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
				_itemUseTimer = new Stopwatch();
				_itemUseTimer.Start();
				// Snowballs and shit
				Level.Interact(Level, this, message.item, new BlockCoordinates(message.x, message.y, message.z), message.meta);

				MetadataDictionary metadata = new MetadataDictionary();
				metadata[0] = new MetadataByte(16);
				Level.RelayBroadcast(this, new McpeSetEntityData
				{
					entityId = EntityId,
					metadata = metadata,
				});
			}
		}

		private void SendStartGame()
		{
			SendPackage(new McpeStartGame
			{
				seed = -1,
				generator = 1,
				gamemode = (int) Level.GameMode,
				entityId = EntityId,
				spawnX = Level.SpawnPoint.X,
				spawnY = Level.SpawnPoint.Y,
				spawnZ = Level.SpawnPoint.Z,
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
			var chunkPosition = new ChunkCoordinates(KnownPosition);
			if (IsSpawned && _currentChunkPosition == chunkPosition) return;

			_currentChunkPosition = chunkPosition;

			ThreadPool.QueueUserWorkItem(delegate(object state)
			{
				foreach (var chunk in Level.GenerateChunks(_currentChunkPosition, _chunksUsed))
				{
					McpeFullChunkData fullChunkData = McpeFullChunkData.CreateObject();
					fullChunkData.chunkX = chunk.x;
					fullChunkData.chunkZ = chunk.z;
					fullChunkData.chunkData = chunk.GetBytes();
					fullChunkData.chunkDataLength = fullChunkData.chunkData.Length;

					McpeBatch batch = new McpeBatch();
					byte[] buffer = CompressBytes(fullChunkData.Encode());

					batch.payloadSize = buffer.Length;
					batch.payload = buffer;

					// This is to slow down chunk-sending not to overrun old devices.
					// The timeout should be configurable and enable/disable.
					//Thread.Sleep(12);

					SendPackage(batch);
				}
			});
		}

		public byte[] CompressBytes(byte[] input)
		{
			MemoryStream stream = new MemoryStream();
			stream.WriteByte(0x78);
			stream.WriteByte(0x01);
			int checksum;
			using (var compressStream = new ZLibStream(stream, CompressionLevel.Optimal, true))
			{
				NbtBinaryWriter writer = new NbtBinaryWriter(compressStream, true);
				writer.Write(input);

				writer.Flush();

				checksum = compressStream.Checksum;
				writer.Close();
			}

			byte[] checksumBytes = BitConverter.GetBytes(checksum);
			if (BitConverter.IsLittleEndian)
			{
				// Adler32 checksum is big-endian
				Array.Reverse(checksumBytes);
			}
			stream.Write(checksumBytes, 0, checksumBytes.Length);

			var bytes = stream.ToArray();
			stream.Close();

			return bytes;
		}


		internal void SendSetHealth()
		{
			SendPackage(new McpeSetHealth {health = HealthManager.Hearts});
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
			package.headYaw = KnownPosition.HeadYaw;
			package.pitch = KnownPosition.Pitch;
			package.teleport = 0x80;

			SendPackage(package);
		}

		public override void OnTick()
		{
			base.OnTick();
		}


		public override MetadataDictionary GetMetadata()
		{
			//[0] byte 0 0, 
			//[1] short 1 300, 
			//[2] string 4 Client12, 
			//[3] byte 0 1, 
			//[4] byte 0 0, 
			//[7] int 2 0, 
			//[8] byte 0 0, 
			//[15] byte 0 0, 
			//[16] byte 0 0, 
			//[17] long 6 0

			MetadataDictionary metadata = new MetadataDictionary();
			metadata[0] = new MetadataByte((byte) (HealthManager.IsOnFire ? 1 : 0));
			metadata[1] = new MetadataShort(HealthManager.Air);
			metadata[2] = new MetadataString(Username);
			metadata[3] = new MetadataByte(1);
			metadata[4] = new MetadataByte(0);
			metadata[7] = new MetadataInt(0);
			metadata[8] = new MetadataByte(0);
			metadata[15] = new MetadataByte(0);
			metadata[16] = new MetadataByte(0);
			metadata[17] = new MetadataLong(0);

			return metadata;
		}


		public override void DespawnEntity()
		{
			//IsSpawned = false;
			Level.RemovePlayer(this);
		}

		public void SendMessage(string text, Player sender = null)
		{
			//const TYPE_RAW = 0;
			//const TYPE_CHAT = 1;
			//const TYPE_TRANSLATION = 2;
			//const TYPE_POPUP = 3;

			var response = new McpeText()
			{
				type = McpeText.TypeChat,
				source = sender == null ? "MiNET" : sender.Username,
				message = text
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

		public void DetectLostConnection()
		{
			DetectLostConnections ping = new DetectLostConnections();
			SendPackage(ping);
		}

		private Queue<Package> _sendQueueNotConcurrent = new Queue<Package>();
		private object _queueSync = new object();

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
				lock (_queueSync)
				{
					_sendQueueNotConcurrent.Enqueue(package);
				}
			}
			else
			{
				Server.SendPackage(EndPoint, new List<Package>(new[] {package}), _mtuSize, ref _datagramSequenceNumber, ref _reliableMessageNumber);
			}
		}

		private void SendQueue(object sender)
		{
			if (!IsConnected) return;

			Queue<Package> queue = _sendQueueNotConcurrent;

			int lenght = queue.Count;

			List<Package> messages = new List<Package>(lenght);

			for (int i = 0; i < lenght; i++)
			{
				Package package;
				lock (_queueSync)
				{
					if (queue.Count == 0) break;
					package = queue.Dequeue();
				}

				messages.Add(package);
			}
			if (messages.Count == 0) return;
			Server.SendPackage(EndPoint, messages, _mtuSize, ref _datagramSequenceNumber, ref _reliableMessageNumber);
		}

		public void SendMoveList(List<McpeMovePlayer> movePlayerPackages)
		{
			if (!IsConnected) return;

			List<Package> messages = new List<Package>();
			foreach (var movePlayer in movePlayerPackages)
			{
				if (movePlayer.entityId == EntityId) continue;

				Package package = movePlayer;

				var result = _pluginManager.PluginPacketHandler(package, false, this);
				if (result != package) package.PutPool();
				package = result;

				messages.Add(package);
			}

			if (messages.Count == 0) return;
			Server.SendPackage(EndPoint, messages, _mtuSize, ref _datagramSequenceNumber, ref _reliableMessageNumber);
		}
	}
}