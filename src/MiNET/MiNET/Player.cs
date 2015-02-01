using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using MetadataInt = MiNET.Utils.MetadataInt;
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
		private int _datagramSequenceNumber;
		private object _sequenceNumberSync = new object();

		private Queue<Package> _sendQueue = new Queue<Package>();
		private Timer _sendTicker;

		private Coordinates2D _currentChunkPosition;
		private Timer _playerTimer;

		public bool IsConnected { get; set; }
		public HealthManager HealthManager { get; private set; }
		public MetadataSlots Armor { get; private set; }
		public MetadataSlots Items { get; private set; }
		public MetadataInts ItemHotbar { get; private set; }
		public MetadataSlot ItemInHand { get; private set; }

		public bool IsBot { get; set; }

		public bool IsSpawned { get; private set; }
		public string Username { get; private set; }
		public PermissionManager Permissions { get; set; }

		internal Player()
			: base(-1, null)
		{
		}

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

			Armor = new MetadataSlots();
			Armor[0] = new MetadataSlot(new ItemStack(310, 3));
			Armor[1] = new MetadataSlot(new ItemStack(311, 3));
			Armor[2] = new MetadataSlot(new ItemStack(312, 3));
			Armor[3] = new MetadataSlot(new ItemStack(313, 3));

			Items = new MetadataSlots();
			for (byte i = 0; i < 35; i++)
			{
				Items[i] = new MetadataSlot(new ItemStack(i, 1));
			}
			Items[0] = new MetadataSlot(new ItemStack(267, 1));
			Items[1] = new MetadataSlot(new ItemStack(46, 1));
			Items[2] = new MetadataSlot(new ItemStack(259, 1));
			Items[3] = new MetadataSlot(new ItemStack(305, 3));

			ItemHotbar = new MetadataInts();
			for (byte i = 0; i < 6; i++)
			{
				ItemHotbar[i] = new MetadataInt(-1);
			}

			// Hotbar starts at max size = 9 to set the slots. Don't understand why.
			ItemHotbar[0] = new MetadataInt(9);
			ItemHotbar[1] = new MetadataInt(10);
			ItemHotbar[2] = new MetadataInt(11);
			ItemHotbar[3] = new MetadataInt(12);
			ItemHotbar[4] = new MetadataInt(13);
			ItemHotbar[5] = new MetadataInt(14);

			ItemInHand = new MetadataSlot(new ItemStack(-1));
			IsConnected = true;
		}

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

		private void HandleAnimate(McpeAnimate message)
		{
			Level.RelayBroadcast(this, message, false);
		}

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

		private void HandlePing(InternalPing message)
		{
			SendPackage(message);
		}

		private void HandleEntityData(McpeEntityData message)
		{
			Debug.WriteLine("x:" + message.x);
			Debug.WriteLine("y:" + message.y);
			Debug.WriteLine("z:" + message.z);
			Debug.WriteLine("NBT " + message.namedtag.NbtFile);

			var blockEntity = Level.GetBlockEntity(new Coordinates3D(message.x, message.y, message.z));

			if (blockEntity == null) return;

			blockEntity.SetCompound(message.namedtag.NbtFile.RootTag);
			Level.SetBlockEntity(blockEntity);
		}

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
				slotData = Items,
				hotbarData = ItemHotbar
			});

			SendPackage(new McpeContainerSetContent
			{
				windowId = 0x78, // Armor windows ID
				slotData = Armor,
				hotbarData = null
			});

			// Broadcast spawn to all
			Level.AddPlayer(this);

			BroadcastSetEntityData();
		}

		private void HandleDisconnectionNotification()
		{
			IsConnected = false;
			Level.RemovePlayer(this);
		}

		private void HandleConnectionRequest(ConnectionRequest message)
		{
			var response = new ConnectionRequestAcceptedManual((short) _endpoint.Port, message.timestamp);
			//response.Encode();

			SendPackage(response);
		}

		private void HandleConnectedPing(ConnectedPing message)
		{
			SendPackage(new ConnectedPong
			{
				sendpingtime = message.sendpingtime,
				sendpongtime = DateTimeOffset.UtcNow.Ticks/TimeSpan.TicksPerMillisecond
			});
		}

		private void HandleLogin(McpeLogin message)
		{
			Username = message.username;

			if (Username == null) throw new Exception("No username on login");

			// Check if the user already exist, that case bumpt the old one
			Level.RemoveDuplicatePlayers(Username);

			if (Username.StartsWith("Player")) IsBot = true;

			if (Username == null) throw new Exception("No username on login");
			SendPackage(new McpeLoginStatus {status = 0});

			// Start game
			SendStartGame();
			SendSetTime();
			SendSetSpawnPosition();
			SendSetHealth();
			SendPackage(new McpeSetDifficulty {difficulty = (int) Level.Difficulty});
			SendChunksForKnownPosition();
			LastUpdatedTime = DateTime.UtcNow;
		}

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

		private void HandleMovePlayer(McpeMovePlayer message)
		{
			if (HealthManager.IsDead) return;

			KnownPosition = new PlayerPosition3D(message.x, message.y, message.z) {Pitch = message.pitch, Yaw = message.yaw, BodyYaw = message.bodyYaw};
			LastUpdatedTime = DateTime.UtcNow;

			if (IsBot) return;

			SendChunksForKnownPosition();
		}

		private void HandleRemoveBlock(McpeRemoveBlock message)
		{
			Level.BreakBlock(Level, this, new Coordinates3D(message.x, message.y, message.z));
		}

		private void HandlePlayerArmorEquipment(McpePlayerArmorEquipment message)
		{
			if (HealthManager.IsDead) return;

			message.entityId = EntityId;

			Level.RelayBroadcast(this, message, false);
		}

		private void HandlePlayerEquipment(McpePlayerEquipment message)
		{
			if (HealthManager.IsDead) return;

			ItemInHand.Value.Id = message.item;
			ItemInHand.Value.Metadata = message.meta;

			message.entityId = EntityId;

			Level.RelayBroadcast(this, message, false);
		}

		private void HandleContainerSetSlot(McpeContainerSetSlot message)
		{
			if (HealthManager.IsDead) return;

			switch (message.windowId)
			{
				case 0:
					Items[(byte) message.slot] = new MetadataSlot(new ItemStack(message.itemId, (sbyte) message.itemCount, message.itemDamage));
					break;
				case 0x78:
					Armor[(byte) message.slot] = new MetadataSlot(new ItemStack(message.itemId, (sbyte) message.itemCount, message.itemDamage));
					break;
			}
			Level.RelayBroadcast(this, new McpePlayerArmorEquipment()
			{
				entityId = EntityId,
				helmet = (byte) (((MetadataSlot) Armor[0]).Value.Id - 256),
				chestplate = (byte) (((MetadataSlot) Armor[1]).Value.Id - 256),
				leggings = (byte) (((MetadataSlot) Armor[2]).Value.Id - 256),
				boots = (byte) (((MetadataSlot) Armor[3]).Value.Id - 256)
			});

			Level.RelayBroadcast(this, new McpePlayerEquipment()
			{
				entityId = EntityId,
				item = ItemInHand.Value.Id,
				meta = ItemInHand.Value.Metadata,
				slot = 0
			});
		}

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
		}

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

		public byte GetDirection()
		{
			return DirectionByRotationFlat(KnownPosition.Yaw);
		}

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

		private void SendSetSpawnPosition()
		{
			SendPackage(new McpeSetSpawnPosition
			{
				x = Level.SpawnPoint.X,
				y = (byte) Level.SpawnPoint.Y,
				z = Level.SpawnPoint.Z
			});
		}

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
				slotData = Items,
				hotbarData = ItemHotbar
			});

			SendPackage(new McpeContainerSetContent
			{
				windowId = 0x78, // Armor windows ID
				slotData = Armor,
				hotbarData = null
			});

			_playerTimer = new Timer(OnPlayerTick, null, 50, 50);

			IsSpawned = true;
			Level.AddPlayer(this);

			BroadcastSetEntityData();
		}

		private void OnPlayerTick(object state)
		{
			HealthManager.OnTick();
		}

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

		private void SendEquipmentForPlayer(Player player)
		{
			SendPackage(new McpePlayerEquipment()
			{
				entityId = player.EntityId,
				item = player.ItemInHand.Value.Id,
				meta = player.ItemInHand.Value.Metadata,
				slot = 0
			});
		}

		private void SendArmorForPlayer(Player player)
		{
			SendPackage(new McpePlayerArmorEquipment()
			{
				entityId = player.EntityId,
				helmet = (byte) (((MetadataSlot) player.Armor[0]).Value.Id - 256),
				chestplate = (byte) (((MetadataSlot) player.Armor[1]).Value.Id - 256),
				leggings = (byte) (((MetadataSlot) player.Armor[2]).Value.Id - 256),
				boots = (byte) (((MetadataSlot) player.Armor[3]).Value.Id - 256)
			});
		}

		public void SendRemoveForPlayer(Player player)
		{
			if (player == this) return;

			SendPackage(new McpeRemovePlayer
			{
				clientId = 0,
				entityId = player.EntityId
			});
		}

		public void BroadcastEntityEvent()
		{
			Level.RelayBroadcast(this, new McpeEntityEvent()
			{
				entityId = 0,
				eventId = (byte) (HealthManager.Health <= 0 ? 3 : 2)
			});

			if (HealthManager.IsDead)
			{
				Level.BroadcastTextMessage("Player " + this.Username + " " + HealthManager.LastDamageCause.ToString().Replace('_', ' '));
			}
		}

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

			if (IsSpawned /* && package is McpeMovePlayer*/)
			{
				lock (_sendQueue)
				{
					_sendQueue.Enqueue(package);

					if (_sendTicker == null)
					{
						_sendTicker = new Timer(SendQueue, null, 10, 10); // RakNet send tick-time
					}
				}
			}
			else
			{
				lock (_sequenceNumberSync)
				{
					_server.SendPackage(_endpoint, new List<Package>(new[] {package}), _mtuSize, ref _datagramSequenceNumber, ref _reliableMessageNumber);
				}
			}
		}

		private void SendQueue(object sender)
		{
			if (!IsConnected) return;

			List<Package> messages = new List<Package>();
			lock (_sendQueue)
			{
				while (_sendQueue.Count > 0)
				{
					Package package = _sendQueue.Dequeue();
					//if (package.Timer.ElapsedMilliseconds > 100) continue;
					messages.Add(package);
				}
			}

			if (messages.Count == 0) return;

			lock (_sequenceNumberSync)
			{
				_server.SendPackage(_endpoint, messages, _mtuSize, ref _datagramSequenceNumber, ref _reliableMessageNumber);
			}
		}

		public int GetEntityId(Player player)
		{
			return Level.EntityManager.GetEntityId(this, player);
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
	}
}