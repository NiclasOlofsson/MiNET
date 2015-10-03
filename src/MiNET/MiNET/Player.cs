using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using log4net;
using Microsoft.AspNet.Identity;
using MiNET.Effects;
using MiNET.Entities;
using MiNET.Items;
using MiNET.Net;
using MiNET.Security;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET
{
	public class PlayerAttributes : Dictionary<string, PlayerAttribute>
	{
	}

	public class PlayerAttribute
	{
		public string Name { get; set; }
		public float MinValue { get; set; }
		public float MaxValue { get; set; }
		public float Value { get; set; }
	}

	public class Player : Entity
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Player));

		public MiNetServer Server { get; private set; }
		public IPEndPoint EndPoint { get; private set; }
		public PlayerNetworkSession NetworkSession { get; set; }

		private int _mtuSize;
		private int _reliableMessageNumber;
		private int _datagramSequenceNumber = -1; // Very important to start with -1 since Interlock.Increament doesn't offer another solution
		private Queue<Package> _sendQueueNotConcurrent = new Queue<Package>();
		private object _queueSync = new object();
		// ReSharper disable once NotAccessedField.Local
		private Timer _sendTicker;

		private Dictionary<Tuple<int, int>, McpeBatch> _chunksUsed;
		private ChunkCoordinates _currentChunkPosition;

		private Inventory _openInventory;
		public PlayerInventory Inventory { get; private set; }

		public PlayerLocation SpawnPosition { get; set; }

		public GameMode GameMode { get; set; }
		public bool IsConnected { get; set; }
		public bool IsSpawned { get; set; }
		public string Username { get; private set; }
		public int ClientId { get; set; }
		public long ClientGuid { get; set; }
		public PermissionManager Permissions { get; set; }
		public Skin Skin { get; set; }
		public bool Silent { get; set; }
		public bool HideNameTag { get; set; }
		public bool NoAi { get; set; }

		public Dictionary<EffectType, Effect> Effects { get; set; }

		public long Rtt { get; set; }
		public long RttVar { get; set; }
		public long Rto { get; set; }

		public List<Popup> Popups { get; set; }

		public User User { get; set; }
		public Session Session { get; set; }

		// HACK
		public int Kills { get; set; }
		public int Deaths { get; set; }

		public Player(MiNetServer server, IPEndPoint endPoint, int mtuSize) : base(-1, null)
		{
			Rtt = 300;
			Width = 0.6;
			Length = 0.6;
			Height = 1.80;

			Popups = new List<Popup>();
			Effects = new Dictionary<EffectType, Effect>();

			Server = server;
			EndPoint = endPoint;
			_mtuSize = mtuSize;

			Permissions = new PermissionManager(UserGroup.User);
			Permissions.AddPermission("*"); //All users can use all commands. (For debugging purposes)

			Inventory = new PlayerInventory(this);

			_chunksUsed = new Dictionary<Tuple<int, int>, McpeBatch>();

			IsSpawned = false;
			IsConnected = endPoint != null; // Can't connect if there is no endpoint

			if (IsConnected) _sendTicker = new Timer(SendQueue, null, 10, 10); // RakNet send tick-time
		}

		public DateTime LastNetworkActivity { get; set; }

		public void HandlePackage(Package message)
		{
			LastNetworkActivity = DateTime.UtcNow;

			var result = Server.PluginManager.PluginPacketHandler(message, true, this);
			//if (result != message) message.PutPool();
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

			else if (typeof (McpeEntityEvent) == message.GetType())
			{
				HandleEntityEvent((McpeEntityEvent) message);
			}

			else if (typeof (ConnectedPing) == message.GetType())
			{
				HandleConnectedPing((ConnectedPing) message);
			}

			else if (typeof (ConnectedPong) == message.GetType())
			{
				HandleConnectedPong((ConnectedPong) message);
			}

			else if (typeof (ConnectionRequest) == message.GetType())
			{
				HandleConnectionRequest((ConnectionRequest) message);
			}

			else if (typeof (NewIncomingConnection) == message.GetType())
			{
				HandleNewIncomingConnection((NewIncomingConnection) message);
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
				if (elapsedMilliseconds > 500)
				{
					Log.DebugFormat("Package (0x{1:x2}) handling too long {0}ms for {2}", elapsedMilliseconds, message.Id, Username);
				}
			}
			else
			{
				Log.WarnFormat("Package (0x{0:x2}) timer not started for {1}.", message.Id, Username);
			}
		}

		protected virtual void HandleNewIncomingConnection(NewIncomingConnection message)
		{
			NetworkSession.State = ConnectionState.Connected;
			Log.DebugFormat("New incoming connection from {0} {1}", EndPoint.Address, message.port);
		}

		protected virtual void HandlePlayerDropItem(McpeDropItem message)
		{
			if (!Inventory.HasItem(message.item)) return;

			ItemStack itemStack = message.item.Value;

			Item item = ItemFactory.GetItem(itemStack.Id, itemStack.Metadata);

			var itemEntity = new ItemEntity(Level, item)
			{
				Velocity = KnownPosition.GetDirection()*0.7,
				KnownPosition =
				{
					X = KnownPosition.X,
					Y = (float) (KnownPosition.Y + Height),
					Z = KnownPosition.Z
				},
				Count = itemStack.Count
			};

			itemEntity.SpawnEntity();
		}

		/// <summary>
		///     Handles an animate packet.
		/// </summary>
		/// <param name="message">The message.</param>
		protected virtual void HandleAnimate(McpeAnimate message)
		{
			if (Level == null) return;

			Log.DebugFormat("Action: {0}", message.actionId);

			McpeAnimate msg = McpeAnimate.CreateObject();
			msg.entityId = message.entityId;
			msg.actionId = message.actionId;

			Level.RelayBroadcast(this, msg);
		}

		/// <summary>
		///     Handles the player action.
		/// </summary>
		/// <param name="message">The message.</param>
		protected virtual void HandlePlayerAction(McpePlayerAction message)
		{
			Log.DebugFormat("Player action: {0}", message.actionId);
			Log.DebugFormat("Entity ID: {0}", message.entityId);
			Log.DebugFormat("Action ID:  {0}", message.actionId);
			Log.DebugFormat("x:  {0}", message.x);
			Log.DebugFormat("y:  {0}", message.y);
			Log.DebugFormat("z:  {0}", message.z);
			Log.DebugFormat("Face:  {0}", message.face);

			switch (message.actionId)
			{
				case 5: // Shoot arrow
				{
					if (_itemUseTimer == null) return;

					Item itemInHand = Inventory.GetItemInHand().Item;

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
				case 7: // Respawn
					ThreadPool.QueueUserWorkItem(delegate(object state) { HandleRespawn(null); });
					break;
				default:
					return;
			}
		}

		/// <summary>
		///     Handles the ping.
		/// </summary>
		/// <param name="message">The message.</param>
		protected virtual void HandlePing(InternalPing message)
		{
			SendPackage(message, sendDirect: true);
		}

		/// <summary>
		///     Handles the entity data.
		/// </summary>
		/// <param name="message">The message.</param>
		protected virtual void HandleEntityData(McpeTileEntityData message)
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


		private void SendAdventureSettings()
		{
			McpeAdventureSettings mcpeAdventureSettings = McpeAdventureSettings.CreateObject();
			mcpeAdventureSettings.flags = Level.IsSurvival ? 0x20 : 0x80;

			if (IsAdventure)
			{
				mcpeAdventureSettings.flags |= 0x01;
			}

			if (IsAutoJump)
			{
				mcpeAdventureSettings.flags |= 0x40;
			}

			if (IsSpectator)
			{
				mcpeAdventureSettings.flags |= 0x100;
			}

			SendPackage(mcpeAdventureSettings);
		}

		public bool IsAdventure { get; set; }

		[Wired]
		public void SetAdventure(bool isAdventure)
		{
			IsAdventure = isAdventure;
			SendAdventureSettings();
		}

		public bool IsSpectator { get; set; }

		[Wired]
		public void SetSpecator(bool isSpectator)
		{
			IsSpectator = isSpectator;
			SendAdventureSettings();
		}

		public bool IsAutoJump { get; set; }

		[Wired]
		public void SetAutoJump(bool isAutoJump)
		{
			IsAutoJump = isAutoJump;
			SendAdventureSettings();
		}

		/// <summary>
		///     Handles the disconnection notification.
		/// </summary>
		public virtual void HandleDisconnectionNotification()
		{
			Disconnect("Client requested disconnected", false);
		}

		/// <summary>
		///     Handles the connection request.
		/// </summary>
		/// <param name="message">The message.</param>
		protected virtual void HandleConnectionRequest(ConnectionRequest message)
		{
			Log.DebugFormat("Connection request from: {0}", EndPoint.Address);

			ClientGuid = message.clientGuid;

			var response = ConnectionRequestAccepted.CreateObject();
			response.systemAddress = new IPEndPoint(IPAddress.Loopback, 19132);
			response.systemAddresses = new IPEndPoint[10];
			response.systemAddresses[0] = new IPEndPoint(IPAddress.Loopback, 19132);
			response.incomingTimestamp = message.timestamp;
			response.serverTimestamp = DateTime.UtcNow.Ticks/TimeSpan.TicksPerMillisecond;

			for (int i = 1; i < 10; i++)
			{
				response.systemAddresses[i] = new IPEndPoint(IPAddress.Any, 19132);
			}

			SendPackage(response);
		}

		/// <summary>
		///     Handles the connected ping.
		/// </summary>
		/// <param name="message">The message.</param>
		protected virtual void HandleConnectedPing(ConnectedPing message)
		{
			ConnectedPong package = ConnectedPong.CreateObject();
			package.sendpingtime = message.sendpingtime;
			package.sendpongtime = DateTimeOffset.UtcNow.Ticks/TimeSpan.TicksPerMillisecond;
			SendPackage(package);
		}

		protected virtual void HandleConnectedPong(ConnectedPong message)
		{
		}

		private object _loginSyncLock = new object();

		protected virtual void HandleLogin(McpeLogin message)
		{
			//Disconnect("Este servidor ya no existe. Por favor, conecta a " + ChatColors.Aqua + "play.bladestorm.net" + ChatColors.White + " para seguir jugando.");
			////Disconnect("This server is closed. Please connect to " + ChatColors.Aqua + "play.bladestorm.net" + ChatColors.White + " to continue playing.");
			//return;

			// Only one login!
			lock (_loginSyncLock)
			{
				if (Username != null)
				{
					Log.WarnFormat("Player {0} doing multiple logins", Username);
					return; // Already doing login
				}

				Username = message.username;
			}

			if (message.protocol != 34)
			{
				Disconnect(string.Format("Wrong version {0} of Minecraft Pocket Edition, please upgrade.", message.protocol));
				return;
			}

			//if (!message.username.Equals("gurun") && !message.username.Equals("TruDan") && !message.username.Equals("Morehs"))
			//{
			//	if (serverInfo.NumberOfPlayers > serverInfo.MaxNumberOfPlayers)
			//	{
			//		Disconnect("Too many players (" + serverInfo.NumberOfPlayers + ") at this time, please try again.");
			//		return;
			//	}

			//	// Use for loadbalance only right now.
			//	if (serverInfo.ConnectionsInConnectPhase > serverInfo.MaxNumberOfConcurrentConnects)
			//	{
			//		Disconnect("Too many concurrent logins (" + serverInfo.ConnectionsInConnectPhase + "), please try again.");
			//		return;
			//	}
			//}

			if (message.username == null || message.username.Trim().Length == 0 || !Regex.IsMatch(message.username, "^[A-Za-z0-9_-]{3,16}$"))
			{
				Disconnect("Invalid username.");
				return;
			}

			if (message.skin.Texture == null)
			{
				Disconnect("Invalid skin.");
				return;
			}

			SendPlayerStatus(0); // Hmm, login success?

			Username = message.username;
			ClientId = (int) message.clientId;
			ClientUuid = message.clientUuid;
			Skin = message.skin;

			// THIS counter exist to protect the level from being swamped with player list add
			// attempts during startup (normally).
			var serverInfo = Server.ServerInfo;
			Interlocked.Increment(ref serverInfo.ConnectionsInConnectPhase);

			new Thread(Start).Start();
		}

		private void Start(object o)
		{
			Stopwatch watch = new Stopwatch();
			watch.Restart();

			var serverInfo = Server.ServerInfo;

			try
			{
				Session = Server.SessionManager.CreateSession(this);
				if (Server.IsSecurityEnabled)
				{
					User = Server.UserManager.FindByName(Username);
				}

				Level = Server.LevelManager.GetLevel(this, "Default");

				SpawnPosition = SpawnPosition ?? Level.SpawnPoint;
				KnownPosition = new PlayerLocation
				{
					X = SpawnPosition.X,
					Y = SpawnPosition.Y,
					Z = SpawnPosition.Z,
					Yaw = SpawnPosition.Yaw,
					Pitch = SpawnPosition.Pitch,
					HeadYaw = SpawnPosition.HeadYaw,
				};


				// Check if the user already exist, that case bumpt the old one
				Level.RemoveDuplicatePlayers(Username, ClientId);

				Level.EntityManager.AddEntity(null, this);

				GameMode = Level.GameMode;

				// Start game

				SendStartGame();

				SendSetSpawnPosition();

				SendSetTime();

				SendSetDificulty();

				SendAdventureSettings();

				SendSetHealth();

				SendSetEntityData();

				Level.AddPlayer(this, string.Format("{0} joined the game!", Username), true);
			}
			finally
			{
				Interlocked.Decrement(ref serverInfo.ConnectionsInConnectPhase);
			}

			LastUpdatedTime = DateTime.UtcNow;

			//if (GameMode == GameMode.Creative)
			//{
			//	Inventory.Slots.Clear();

			//	Inventory.Slots.AddRange(InventoryUtils.CreativeInventoryItems);
			//}

			SendPlayerInventory();

			//Level.SpawnToAll(this);

			ThreadPool.QueueUserWorkItem(delegate(object state) { SendChunksForKnownPosition(); });

			LastUpdatedTime = DateTime.UtcNow;
			Log.InfoFormat("Login complete by: {0} from {2} in {1}ms", Username, watch.ElapsedMilliseconds, EndPoint);
		}

		public UUID ClientUuid { get; set; }

		public virtual void InitializePlayer()
		{
			SendPlayerStatus(3);

			McpeRespawn mcpeRespawn = McpeRespawn.CreateObject();
			mcpeRespawn.x = SpawnPosition.X;
			mcpeRespawn.y = SpawnPosition.Y;
			mcpeRespawn.z = SpawnPosition.Z;
			SendPackage(mcpeRespawn);

			//send time again
			SendSetTime();
			IsSpawned = true;
			LastUpdatedTime = DateTime.UtcNow;
		}

		protected virtual void HandleRespawn(McpeRespawn msg)
		{
			ServerInfo serverInfo = Server.ServerInfo;
			try
			{
				//Interlocked.Increment(ref serverInfo.ConnectionsInConnectPhase);

				// reset all health states
				HealthManager.ResetHealth();

				// send teleport to spawn
				KnownPosition = new PlayerLocation
				{
					X = SpawnPosition.X,
					Y = SpawnPosition.Y,
					Z = SpawnPosition.Z,
					Yaw = SpawnPosition.Yaw,
					Pitch = SpawnPosition.Pitch,
					HeadYaw = SpawnPosition.HeadYaw,
				};

				SendSetHealth();

				SendAdventureSettings();

				SendPlayerInventory();

				BroadcastSetEntityData();

				Level.SpawnToAll(this);

				ThreadPool.QueueUserWorkItem(delegate(object state) { SendChunksForKnownPosition(); });

				IsSpawned = true;

				SendMovePlayer();

				Log.InfoFormat("Respawn player {0} on level {1}", Username, Level.LevelId);
			}
			finally
			{
				//Interlocked.Decrement(ref serverInfo.ConnectionsInConnectPhase);
			}
		}

		[Wired]
		public void SetPosition(PlayerLocation position, bool teleport = true)
		{
			KnownPosition = position;

			var package = McpeMovePlayer.CreateObject();
			package.entityId = 0;
			package.x = position.X;
			package.y = position.Y + 1.62f;
			package.z = position.Z;
			package.yaw = position.Yaw;
			package.headYaw = position.HeadYaw;
			package.pitch = position.Pitch;
			package.mode = (byte) (teleport ? 1 : 0);

			SendPackage(package);
		}

		public void SpawnLevel(Level toLevel)
		{
			SpawnLevel(toLevel, toLevel.SpawnPoint);
		}

		public virtual void SpawnLevel(Level toLevel, PlayerLocation spawnPoint)
		{
			bool oldNoAi = NoAi;
			SetNoAi(true);

			// send teleport straight up, no chunk loading
			SetPosition(new PlayerLocation
			{
				X = KnownPosition.X,
				Y = 4000,
				Z = KnownPosition.Z,
				Yaw = 91,
				Pitch = 28,
				HeadYaw = 91,
			});

			Level.RemovePlayer(this, true);
			Level.EntityManager.RemoveEntity(null, this);

			Level = toLevel; // Change level
			SpawnPosition = spawnPoint;
			//Level.AddPlayer(this, "", false);
			// reset all health states
			HealthManager.ResetHealth();
			SendSetHealth();

			SendSetSpawnPosition();

			SendAdventureSettings();

			SendPlayerInventory();

			lock (_chunksUsed)
			{
				_chunksUsed.Clear();
			}

			ForcedSendChunksForKnownPosition(spawnPoint);

			// send teleport to spawn
			SetPosition(spawnPoint);

			SetNoAi(oldNoAi);

			Level.AddPlayer(this, "", true);
			//Level.SpawnToAll(this);
			IsSpawned = true;

			Log.InfoFormat("Respawn player {0} on level {1}", Username, Level.LevelId);

			SendSetTime();
		}


		public void SendSetEntityData()
		{
			McpeSetEntityData mcpeSetEntityData = McpeSetEntityData.CreateObject();
			mcpeSetEntityData.entityId = 0;
			mcpeSetEntityData.metadata = GetMetadata();
			mcpeSetEntityData.Encode();
			SendPackage(mcpeSetEntityData);
		}

		public void SendSetDificulty()
		{
			McpeSetDifficulty mcpeSetDifficulty = McpeSetDifficulty.CreateObject();
			mcpeSetDifficulty.difficulty = (int) Level.Difficulty;
			SendPackage(mcpeSetDifficulty);
		}

		public void SendPlayerInventory()
		{
			//if (GameMode == GameMode.Creative)
			{
				McpeContainerSetContent creativeContent = McpeContainerSetContent.CreateObject();
				creativeContent.windowId = (byte) 0x79;
				creativeContent.slotData = Inventory.GetSlots();
				creativeContent.hotbarData = Inventory.GetHotbar();
				SendPackage(creativeContent);
			}

			{
				McpeContainerSetContent inventoryContent = McpeContainerSetContent.CreateObject();
				inventoryContent.windowId = (byte) 0x00;
				inventoryContent.slotData = Inventory.GetSlots();
				inventoryContent.hotbarData = Inventory.GetHotbar();
				SendPackage(inventoryContent);
			}

			McpeContainerSetContent armorContent = McpeContainerSetContent.CreateObject();
			armorContent.windowId = 0x78;
			armorContent.slotData = Inventory.GetArmor();
			armorContent.hotbarData = null;
			SendPackage(armorContent);
		}

		public void SendPlayerStatus(int status)
		{
			McpePlayerStatus mcpePlayerStatus = McpePlayerStatus.CreateObject();
			mcpePlayerStatus.status = status;
			SendPackage(mcpePlayerStatus);
		}

		[Wired]
		public void SetGameMode(GameMode gameMode)
		{
			GameMode = gameMode;
			SendStartGame();
		}

		private object _disconnectSync = new object();

		public virtual void Disconnect(string reason, bool sendDisconnect = true)
		{
			if (!Monitor.TryEnter(_disconnectSync)) return;
			try
			{
				if (_sendTicker != null)
				{
					_sendTicker.Dispose();
					_sendTicker = null;
				}

				if (Level != null)
				{
					Level.RemovePlayer(this);
				}

				if (IsConnected)
				{
					if (sendDisconnect)
					{
						McpeDisconnect disconnect = McpeDisconnect.CreateObject();
						disconnect.message = reason;
						SendPackage(disconnect, true);
					}

					//McpeTransfer transfer = new McpeTransfer();
					//transfer.endpoint = Server.Endpoint;
					////transfer.endpoint = new IPEndPoint(IPAddress.Parse("86.8.24.26"), 19132);
					//SendPackage(transfer, true);

					IsConnected = false;
				}

				//HACK: But needed
				PlayerNetworkSession session;
				if (Server.ServerInfo.PlayerSessions.TryRemove(EndPoint, out session))
				{
					NetworkSession = null;
					session.Player = null;
					session.State = ConnectionState.Unconnected;
					session.Evicted = true;
					session.Clean();
				}

				//while (_sendQueueNotConcurrent.Count > 0)
				{
					//lock (_queueSync)
					{
						SendQueue(null);
					}
				}
				string levelId = Level == null ? "" : Level.LevelId;
				Log.InfoFormat("Disconnected player {0} from level {3} {1}, reason: {2}", Username, EndPoint.Address, reason, levelId);
			}
			finally
			{
				Monitor.Exit(_disconnectSync);
			}
		}

		protected virtual void HandleMessage(McpeText message)
		{
			string text = message.message;
			if (text.StartsWith("/") || text.StartsWith("."))
			{
				Server.PluginManager.HandleCommand(Server.UserManager, text, this);
			}
			else
			{
				text = TextUtils.Strip(text);
				Level.BroadcastMessage(text, sender: this);
			}
		}

		private int _lastPlayerMoveSequenceNUmber;
		private int _lastOrderingIndex;
		private object _moveSyncLock = new object();
		private int _isKnownCheater = 0;
		private int _cheatLimit = 5;

		protected virtual void HandleMovePlayer(McpeMovePlayer message)
		{
			//if (_openInventory != null)
			//{
			//	// Hack for testing. Chests won't open again.
			//	Log.ErrorFormat("Force closing chest because player {0} moved. Probably a missing packet.", Username);
			//	ThreadPool.QueueUserWorkItem(delegate(object state)
			//	{
			//		try
			//		{
			//			HandleMcpeContainerClose(null);
			//		}
			//		finally
			//		{
			//		}
			//	});
			//}

			if (!IsSpawned || HealthManager.IsDead) return;

			lock (_moveSyncLock)
			{
				if (_lastPlayerMoveSequenceNUmber > message.DatagramSequenceNumber)
				{
					if (Log.IsDebugEnabled)
						Log.DebugFormat("Skipping move datagram {1}/{2} for player {0}", Username, _lastPlayerMoveSequenceNUmber, message.DatagramSequenceNumber);
					return;
				}
				_lastPlayerMoveSequenceNUmber = message.DatagramSequenceNumber;

				if (_lastOrderingIndex > message.OrderingIndex)
				{
					if (Log.IsDebugEnabled)
						Log.DebugFormat("Skipping move ordering {1}/{2} for player {0}", Username, _lastOrderingIndex, message.OrderingIndex);
					return;
				}
				_lastOrderingIndex = message.OrderingIndex;


				long td = DateTime.UtcNow.Ticks - LastUpdatedTime.Ticks;
				Vector3 origin = new Vector3(KnownPosition.X, 0, KnownPosition.Z);
				double distanceTo = origin.DistanceTo(new Vector3(message.x, 0, message.z));
				if (distanceTo/td*TimeSpan.TicksPerSecond > 25.0d)
				{
					//SendMovePlayer();
					return;
				}
			}

			bool useAntiCheat = true;
			if (GameMode != GameMode.Creative && _isKnownCheater <= _cheatLimit && useAntiCheat)
			{
				long td = DateTime.UtcNow.Ticks - LastUpdatedTime.Ticks;
				if (GameMode == GameMode.Survival
				    && HealthManager.CooldownTick == 0
				    && td > 49*TimeSpan.TicksPerMillisecond
				    && td < 500*TimeSpan.TicksPerMillisecond
				    && Level.SpawnPoint.DistanceTo(KnownPosition) > 2.0
					)
				{
					double horizSpeed;
					{
						// Speed in the xz plane

						Vector3 origin = new Vector3(KnownPosition.X, 0, KnownPosition.Z);
						double distanceTo = origin.DistanceTo(new Vector3(message.x, 0, message.z));
						horizSpeed = distanceTo/td*TimeSpan.TicksPerSecond;
						//if (horizSpeed > 11.0d)
						//{
						//	_isKnownCheater = true;
						//	Level.BroadcastMessage(string.Format("{0} is spead cheating {3:##.##}m/s {1:##.##}m {2}ms", Username, distanceTo, (int) ((double) td/TimeSpan.TicksPerMillisecond), horizSpeed), type: MessageType.Raw);
						//	AddPopup(new Popup
						//	{
						//		MessageType = MessageType.Tip,
						//		Message = string.Format("{0} sprinting {3:##.##}m/s {1:##.##}m {2}ms", Username, distanceTo, (int)((double)td / TimeSpan.TicksPerMillisecond), horizSpeed),
						//		Duration = 1
						//	});

						//	LastUpdatedTime = DateTime.UtcNow;
						//	//HealthManager.TakeHit(this, 1, DamageCause.Suicide);
						//	//SendMovePlayer();
						//	return;
						//}
					}

					double verticalSpeed;
					{
						// Speed in 3d
						double speedLimit = (message.y - 1.62) - KnownPosition.Y < 0 ? -70d : 6d;
						double distanceTo = (message.y - 1.62) - KnownPosition.Y;
						verticalSpeed = distanceTo/td*TimeSpan.TicksPerSecond;
						if (!(horizSpeed > 0) && Math.Abs(verticalSpeed) > Math.Abs(speedLimit))
						{
							if (_isKnownCheater == _cheatLimit)
							{
								Level.BroadcastMessage(string.Format("{0} is fly cheating {3:##.##}m/s {1:##.##}m {2}ms", Username, distanceTo, (int) ((double) td/TimeSpan.TicksPerMillisecond), verticalSpeed), type: MessageType.Raw);
								Log.WarnFormat("{0} is fly cheating {3:##.##}m/s {1:##.##}m {2}ms", Username, distanceTo, (int) ((double) td/TimeSpan.TicksPerMillisecond), verticalSpeed);
							} //AddPopup(new Popup
							//{
							//	MessageType = MessageType.Tip,
							//	Message = string.Format("{3:##.##}m/s {1:##.##}m {2}ms", Username, distanceTo, (int)((double)td / TimeSpan.TicksPerMillisecond), verticalSpeed),
							//	Duration = 1
							//});

							LastUpdatedTime = DateTime.UtcNow;
							//HealthManager.TakeHit(this, 1, DamageCause.Suicide);
							//SendMovePlayer();
							_isKnownCheater++;
							return;
						}
					}

					//AddPopup(new Popup
					//{
					//	MessageType = MessageType.Tip,
					//	Message = string.Format("Horiz: {0:##.##}m/s Vert: {1:##.##}m/s", horizSpeed, verticalSpeed),
					//	Duration = 1
					//});
				}
			}

			KnownPosition = new PlayerLocation
			{
				X = message.x,
				Y = message.y - 1.62f,
				Z = message.z,
				Pitch = message.pitch,
				Yaw = message.yaw,
				HeadYaw = message.headYaw
			};

			LastUpdatedTime = DateTime.UtcNow;

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

			ThreadPool.QueueUserWorkItem(delegate(object state) { SendChunksForKnownPosition(); });
		}

		/// <summary>
		///     Handles the remove block.
		/// </summary>
		/// <param name="message">The message.</param>
		protected virtual void HandleRemoveBlock(McpeRemoveBlock message)
		{
			Level.BreakBlock(this, new BlockCoordinates(message.x, message.y, message.z));
		}

		/// <summary>
		///     Handles the player armor equipment.
		/// </summary>
		/// <param name="message">The message.</param>
		protected virtual void HandlePlayerArmorEquipment(McpePlayerArmorEquipment message)
		{
			if (HealthManager.IsDead) return;

			McpePlayerArmorEquipment msg = McpePlayerArmorEquipment.CreateObject();
			msg.entityId = EntityId;
			msg.helmet = message.helmet;
			msg.chestplate = message.chestplate;
			msg.leggings = message.leggings;
			msg.boots = message.boots;

			Level.RelayBroadcast(this, msg);
		}

		/// <summary>
		///     Handles the player equipment.
		/// </summary>
		/// <param name="message">The message.</param>
		protected virtual void HandlePlayerEquipment(McpePlayerEquipment message)
		{
			if (HealthManager.IsDead) return;

			byte selectedHotbarSlot = message.selectedSlot;
			int selectedInventorySlot = (byte) (message.slot - PlayerInventory.HotbarSize);

			//if(GameMode == GameMode.Survival)
			{
				if (selectedInventorySlot < 0 || selectedInventorySlot >= Inventory.Slots.Count)
				{
					Log.InfoFormat("Player {2} set equiptment fails with inv slot: {0}, {1}", selectedInventorySlot, message.slot, Username);
					return;
				}

				if (GameMode != GameMode.Creative)
				{
					var itemStack = Inventory.Slots[selectedInventorySlot];
					if (itemStack != null)
					{
						var existingItemId = itemStack.Id;
						var incomingItemId = message.item.Value.Id;

						if (existingItemId != incomingItemId)
						{
							//Log.ErrorFormat("Player {2} set equiptment fails because incoming item ID {1} didn't match existing inventory item ID {0}", existingItemId, incomingItemId, Username);
							return;
						}
						else
						{
							//Log.ErrorFormat("Player {2} set equiptment SUCCESS because incoming item ID {1} matched existing inventory item ID {0}", existingItemId, incomingItemId, Username);
						}
					}
					else
					{
						Log.ErrorFormat("Player {0} set equiptment fails, probably hacker", Username);
					}
				}

				for (int i = 0; i < Inventory.ItemHotbar.Length; i++)
				{
					if (Inventory.ItemHotbar[i] == selectedInventorySlot)
					{
						Inventory.ItemHotbar[i] = Inventory.ItemHotbar[selectedHotbarSlot];
						break;
					}
				}

				Inventory.ItemHotbar[selectedHotbarSlot] = selectedInventorySlot;
				Inventory.InHandSlot = selectedHotbarSlot;
			}

			McpePlayerEquipment msg = McpePlayerEquipment.CreateObject();
			msg.entityId = EntityId;
			msg.item = message.item;
			msg.slot = (byte) selectedInventorySlot;
			msg.selectedSlot = selectedHotbarSlot;
			Level.RelayBroadcast(this, msg);
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
				item = new MetadataSlot(itemStack)
			});
		}

		/// <summary>
		///     Handles the container set slot.
		/// </summary>
		/// <param name="message">The message.</param>
		protected virtual void HandleContainerSetSlot(McpeContainerSetSlot message)
		{
			if (HealthManager.IsDead) return;

			// on all set container content, check if we have active inventory
			// and update that inventory.
			// Inventory manager makes sure other players with the same inventory open will 
			// also get the update.

			var itemStack = message.item.Value;

			Inventory inventory = Level.InventoryManager.GetInventory(message.windowId);
			if (inventory != null)
			{
				// block inventories of various kinds (chests, furnace, etc)
				inventory.SetSlot((byte) message.slot, itemStack);
				return;
			}

			switch (message.windowId)
			{
				case 0:
					//if (GameMode != GameMode.Creative)
					//{
					//	if (Inventory.Slots[(byte) message.slot].Id != itemStack.Id)
					//	{
					//		Disconnect("Inventory hacking not allowed!");
					//	}
					//	else if (GameMode == GameMode.Creative)
					//	{
					//		try
					//		{
					//			Inventory.Slots[(byte) message.slot] = itemStack;
					//		}
					//		catch (Exception e)
					//		{
					//			Disconnect("Inventory hacking not allowed!");
					//		}
					//	}
					//}
					//else if (GameMode == GameMode.Creative)
				{
					try
					{
						Inventory.Slots[(byte) message.slot] = itemStack;
					}
					catch (Exception e)
					{
						//Disconnect("Inventory hacking not allowed!");
					}
				}

					break;
				case 0x79:
					Inventory.Slots[(byte) message.slot] = itemStack;
					break;
				case 0x78:
					int itemId = message.item.Value.Id;
					short itemMetadata = message.item.Value.Metadata;
					switch ((byte) message.slot)
					{
						case 0:
							Inventory.Helmet = ItemFactory.GetItem(itemId, itemMetadata);
							break;
						case 1:
							Inventory.Chest = ItemFactory.GetItem(itemId, itemMetadata);
							break;
						case 2:
							Inventory.Leggings = ItemFactory.GetItem(itemId, itemMetadata);
							break;
						case 3:
							Inventory.Boots = ItemFactory.GetItem(itemId, itemMetadata);
							break;
					}
					break;
			}

			McpePlayerArmorEquipment armorEquipment = McpePlayerArmorEquipment.CreateObject();
			armorEquipment.entityId = EntityId;
			armorEquipment.helmet = new MetadataSlot(new ItemStack(Inventory.Helmet, 1));
			armorEquipment.chestplate = new MetadataSlot(new ItemStack(Inventory.Chest, 1));
			armorEquipment.leggings = new MetadataSlot(new ItemStack(Inventory.Leggings, 1));
			armorEquipment.boots = new MetadataSlot(new ItemStack(Inventory.Boots, 1));
			Level.RelayBroadcast(this, armorEquipment);

			var playerEquipment = McpePlayerEquipment.CreateObject();
			playerEquipment.entityId = EntityId;
			playerEquipment.item = new MetadataSlot(Inventory.GetItemInHand());
			playerEquipment.slot = 0;
			Level.RelayBroadcast(this, playerEquipment);
		}

		protected virtual void HandleMcpeContainerClose(McpeContainerClose message)
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
		protected virtual void HandleInteract(McpeInteract message)
		{
			Entity target = Level.GetEntity(message.targetEntityId);

			Log.DebugFormat("Interact Action ID: {0}", message.actionId);
			Log.DebugFormat("Interact Target Entity ID: {0}", message.targetEntityId);

			if (target == null) return;

			target.HealthManager.TakeHit(this, CalculateDamage(target), DamageCause.EntityAttack);
		}

		private int CalculateDamage(Player target)
		{
			double armorValue = 0;

			{
				{
					var armorPiece = Inventory.Helmet;
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

				{
					var armorPiece = Inventory.Chest;
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

				{
					var armorPiece = Inventory.Leggings;
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

				{
					var armorPiece = Inventory.Boots;
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

			int damage = Inventory.GetItemInHand().Item.GetDamage(); //Item Damage.

			damage = (int) Math.Floor(damage*(1.0 - armorValue));

			return damage;
		}

		private int CalculateDamage(Entity target)
		{
			int damage = Inventory.GetItemInHand().Item.GetDamage(); //Item Damage.

			damage = (int) Math.Floor(damage*(1.0));

			return damage;
		}


		private void HandleEntityEvent(McpeEntityEvent message)
		{
			Log.Debug("Entity Id:" + message.entityId);
			Log.Debug("Entity Event:" + message.eventId);

			//if (message.eventId != 0) return; // Should probably broadcast?!

			switch (message.eventId)
			{
				case 9:
					// Eat food

					FoodItem foodItem = Inventory.GetItemInHand().Item as FoodItem;
					if (foodItem != null)
					{
						foodItem.Consume(this);
						Inventory.GetItemInHand().Count--;
						SendPlayerInventory();
					}

					break;
			}
		}

		private Stopwatch _itemUseTimer;

		protected virtual void HandleUseItem(McpeUseItem message)
		{
			Log.DebugFormat("Use item: {0}", message.item.Value.Id);
			Log.DebugFormat("item meta: {0}", message.item.Value.Metadata);
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
				// Right click

				var mcpeAnimate = McpeAnimate.CreateObject();
				mcpeAnimate.actionId = 1;
				mcpeAnimate.entityId = EntityId;
				Level.RelayBroadcast(this, mcpeAnimate);

				Vector3 faceCoords = new Vector3(message.fx, message.fy, message.fz);
				if (Inventory.GetItemInHand().Id != message.item.Value.Id)
				{
					//Disconnect("Inventory hacks not allowed.");
					return;
				}

				Level.Interact(Level, this, message.item.Value.Id, new BlockCoordinates(message.x, message.y, message.z), message.item.Value.Metadata, (BlockFace) message.face, faceCoords);
			}
			else
			{
				// Snowballs and shit

				_itemUseTimer = new Stopwatch();
				_itemUseTimer.Start();

				Level.Interact(Level, this, message.item.Value.Id, new BlockCoordinates(message.x, message.y, message.z), message.item.Value.Metadata);

				MetadataDictionary metadata = new MetadataDictionary();
				metadata[0] = new MetadataByte(16);
				Level.RelayBroadcast(this, new McpeSetEntityData
				{
					entityId = EntityId,
					metadata = metadata,
				});
			}
		}

		public void SendStartGame()
		{
			McpeStartGame mcpeStartGame = McpeStartGame.CreateObject();
			mcpeStartGame.seed = -1;
			mcpeStartGame.generator = 1;
			mcpeStartGame.gamemode = (int) GameMode;
			mcpeStartGame.entityId = 0;
			mcpeStartGame.spawnX = (int) SpawnPosition.X;
			mcpeStartGame.spawnY = (int) SpawnPosition.Y;
			mcpeStartGame.spawnZ = (int) SpawnPosition.Z;
			mcpeStartGame.x = KnownPosition.X;
			mcpeStartGame.y = (float) (KnownPosition.Y + Height);
			mcpeStartGame.z = KnownPosition.Z;
			SendPackage(mcpeStartGame);
		}

		/// <summary>
		///     Sends the set spawn position packet.
		/// </summary>
		public void SendSetSpawnPosition()
		{
			McpeSetSpawnPosition mcpeSetSpawnPosition = McpeSetSpawnPosition.CreateObject();
			mcpeSetSpawnPosition.x = (int) SpawnPosition.X;
			mcpeSetSpawnPosition.y = (int) SpawnPosition.Y;
			mcpeSetSpawnPosition.z = (int) SpawnPosition.Z;
			SendPackage(mcpeSetSpawnPosition);
		}

		private object _sendChunkSync = new object();

		private void ForcedSendChunksForKnownPosition(PlayerLocation position)
		{
			var chunkPosition = new ChunkCoordinates(position);
			_currentChunkPosition = chunkPosition;

			foreach (McpeBatch chunk in Level.GenerateChunks(_currentChunkPosition, _chunksUsed))
			{
				SendPackage(chunk, true);
			}
		}

		private void SendChunksForKnownPosition()
		{
			if (!Monitor.TryEnter(_sendChunkSync)) return;

			try
			{
				var chunkPosition = new ChunkCoordinates(KnownPosition);
				if (IsSpawned && _currentChunkPosition == chunkPosition) return;

				if (IsSpawned && _currentChunkPosition.DistanceTo(chunkPosition) < 5)
				{
					if (Log.IsDebugEnabled)
						Log.DebugFormat("Denied chunk, too little distance.");
					return;
				}

				_currentChunkPosition = chunkPosition;

				int packetCount = 0;

				foreach (McpeBatch chunk in Level.GenerateChunks(_currentChunkPosition, _chunksUsed))
				{
					SendPackage(chunk, sendDirect: true);
					if (!IsSpawned)
					{
						if (packetCount++ == 56)
						{
							InitializePlayer();
						}
					}
				}
			}
			finally
			{
				Monitor.Exit(_sendChunkSync);
			}
		}

		public static byte[] CompressBytes(byte[] input, CompressionLevel compressionLevel, bool writeLen = false)
		{
			MemoryStream stream = new MemoryStream();
			stream.WriteByte(0x78);
			stream.WriteByte(0x01);
			int checksum;
			using (var compressStream = new ZLibStream(stream, compressionLevel, true))
			{
				byte[] lenBytes = BitConverter.GetBytes(input.Length);
				Array.Reverse(lenBytes);
				if (writeLen) compressStream.Write(lenBytes, 0, lenBytes.Length); // ??
				compressStream.Write(input, 0, input.Length);
				checksum = compressStream.Checksum;
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


		public virtual void SendSetHealth()
		{
			//McpeSetHealth mcpeSetHealth = McpeSetHealth.CreateObject();
			//mcpeSetHealth.health = HealthManager.Hearts;
			//SendPackage(mcpeSetHealth);

			var attributes = new PlayerAttributes
			{
				["generic.health"] =
					new PlayerAttribute
					{
						Name = "generic.health",
						MinValue = 0,
						MaxValue = 20,
						Value = HealthManager.Hearts
					},
				["player.hunger"] =
					new PlayerAttribute
					{
						Name = "player.hunger",
						MinValue = 0,
						MaxValue = 20,
						Value = 20
					},
				["player.level"] =
					new PlayerAttribute
					{
						Name = "player.level",
						MinValue = 0,
						MaxValue = 24791,
						Value = 0
					},
				["player.experience"] =
					new PlayerAttribute
					{
						Name = "player.experience",
						MinValue = 0,
						MaxValue = 1,
						Value = 0
					}
			};

			McpeUpdateAttributes attributesPackate = McpeUpdateAttributes.CreateObject();
			attributesPackate.entityId = 0;
			attributesPackate.attributes = attributes;
			SendPackage(attributesPackate);
		}

		public virtual void SendSetTime()
		{
			McpeSetTime message = McpeSetTime.CreateObject();
			message.time = (int) Level.CurrentWorldTime;
			message.started = (byte) (Level.IsWorldTimeStarted ? 1 : 0);
			SendPackage(message);
		}

		public virtual void SendMovePlayer(bool teleport = false)
		{
			var package = McpeMovePlayer.CreateObject();
			package.entityId = 0;
			package.x = KnownPosition.X;
			package.y = KnownPosition.Y + 1.62f;
			package.z = KnownPosition.Z;
			package.yaw = KnownPosition.Yaw;
			package.headYaw = KnownPosition.HeadYaw;
			package.pitch = KnownPosition.Pitch;
			package.mode = (byte) (teleport ? 1 : 0);

			SendPackage(package);
		}


		public override void OnTick()
		{
			base.OnTick();

			//if (Level.TickTime%30 == 0)
			//{
			//	if (Username.Equals("gurun"))
			//	{
			//		Popup popup = new Popup
			//		{
			//			Duration = 1,
			//			MessageType = MessageType.Tip,
			//			Message = string.Format("TT: {0}ms AvgTT: {1}ms", Level.LastTickProcessingTime, Level.AvarageTickProcessingTime)
			//		};

			//		AddPopup(popup);
			//	}
			//}

			bool hasDisplayedPopup = false;
			bool hasDisplayedTip = false;
			lock (Popups)
			{
				// Code below is just pure magic and mystery. In short, it takes care of sorting a list of popups
				// based on priority, ticks and delays. And then makes sure that the most applicable popup and tip
				// is presented.
				// In the end it adjusts for the display times for tip (20ticks) and popup (10ticks) and sends it at
				// regular intervalls to make sure there is no blinking.
				foreach (var popup in Popups.OrderByDescending(p => p.Priority).ThenByDescending(p => p.CurrentTick))
				{
					if (popup.CurrentTick >= popup.Duration + popup.DisplayDelay)
					{
						Popups.Remove(popup);
						continue;
					}

					if (popup.CurrentTick >= popup.DisplayDelay)
					{
						if (popup.MessageType == MessageType.Popup && !hasDisplayedPopup)
						{
							if (popup.CurrentTick%10 == 0 || popup.CurrentTick == popup.Duration - 20) SendMessage(popup.Message, type: popup.MessageType);
							hasDisplayedPopup = true;
						}
						if (popup.MessageType == MessageType.Tip && !hasDisplayedTip)
						{
							if (popup.CurrentTick%10 == 0 || popup.CurrentTick == popup.Duration - 10) SendMessage(popup.Message, type: popup.MessageType);
							hasDisplayedTip = true;
						}
					}

					popup.CurrentTick++;
				}
			}
		}

		public void AddPopup(Popup popup)
		{
			lock (Popups)
			{
				if (popup.Id == 0) popup.Id = popup.Message.GetHashCode();
				var exist = Popups.FirstOrDefault(pop => pop.Id == popup.Id);
				if (exist != null) Popups.Remove(exist);

				Popups.Add(popup);
			}
		}

		public void ClearPopups()
		{
			lock (Popups) Popups.Clear();
		}

		public override void Knockback(Vector3 velocity)
		{
			{
				McpeSetEntityMotion motions = McpeSetEntityMotion.CreateObject();
				motions.entities = new EntityMotions {{0, velocity}};
				SendPackage(motions, true);
			}
			ThreadPool.QueueUserWorkItem(delegate(object state)
			{
				Thread.Sleep(500);

				McpeSetEntityMotion motions = McpeSetEntityMotion.CreateObject();
				motions.entities = new EntityMotions {{0, Vector3.Zero}};
				SendPackage(motions, true);
			});
		}

		public override MetadataDictionary GetMetadata()
		{
			MetadataDictionary metadata = new MetadataDictionary();
			metadata[0] = new MetadataByte((byte) (HealthManager.IsOnFire ? 1 : 0));
			metadata[1] = new MetadataShort(HealthManager.Air);
			metadata[2] = new MetadataString(NameTag ?? Username);
			metadata[3] = new MetadataByte(!HideNameTag);
			metadata[4] = new MetadataByte(Silent);
			metadata[7] = new MetadataInt(0); // Potion Color
			metadata[8] = new MetadataByte(0); // Potion Ambient
			metadata[15] = new MetadataByte(NoAi);
			metadata[16] = new MetadataByte(0); // Player flags
			metadata[17] = new MetadataLong(0);

			return metadata;
		}

		[Wired]
		public void SetNoAi(bool noAi)
		{
			NoAi = noAi;

			SendSetEntityData();
		}

		[Wired]
		public void SetHideNameTag(bool hideNameTag)
		{
			HideNameTag = hideNameTag;

			SendSetEntityData();
		}

		[Wired]
		public void SetNameTag(string nameTag)
		{
			NameTag = nameTag;

			SendSetEntityData();
		}

		[Wired]
		public void SetEffect(Effect effect)
		{
			if (Effects.ContainsKey(effect.EffectId))
			{
				effect.SendUpdate(this);
			}
			else
			{
				effect.SendAdd(this);
			}

			Effects[effect.EffectId] = effect;
		}

		[Wired]
		public void RemoveEffect(Effect effect)
		{
			if (Effects.ContainsKey(effect.EffectId))
			{
				effect.SendRemove(this);
				Effects.Remove(effect.EffectId);
			}
		}

		[Wired]
		public void RemoveAllEffects()
		{
			foreach (var effect	 in Effects.Values.ToArray())
			{
				RemoveEffect(effect);
			}
		}

		public override void DespawnEntity()
		{
			IsSpawned = false;
			Level.DespawnFromAll(this);
		}

		public virtual void SendMessage(string text, MessageType type = MessageType.Chat, Player sender = null)
		{
			foreach (var line in text.Split(new string[] {"\n", Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries))
			{
				McpeText message = McpeText.CreateObject();
				message.type = (byte) type;
				message.source = sender == null ? "" : sender.Username;
				message.message = line;

				SendPackage(message);
			}
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
				BroadcastDeathMessage(player, HealthManager.LastDamageCause);
			}
		}

		public virtual void BroadcastDeathMessage(Player player, DamageCause lastDamageCause)
		{
			string deathMessage = string.Format(HealthManager.GetDescription(lastDamageCause), Username, player == null ? "" : player.Username);
			Level.BroadcastMessage(deathMessage, type: McpeText.TypeRaw);
			Log.Debug(deathMessage);
		}

		public void DetectLostConnection()
		{
			DetectLostConnections ping = DetectLostConnections.CreateObject();
			SendPackage(ping);
		}

		/// <summary>
		///     Very important litle method. This does all the sending of packages for
		///     the player class. Treat with respect!
		/// </summary>
		public void SendPackage(Package package, bool sendDirect = false)
		{
			if (package == null) return;

			if (!IsConnected)
			{
				package.PutPool();
				return;
			}

			bool isBatch = package is McpeBatch;

			if (!isBatch)
			{
				var result = Server.PluginManager.PluginPacketHandler(package, false, this);
				if (result != package) package.PutPool();
				package = result;
			}

			if (package == null) return;

			if (!IsSpawned || sendDirect || isBatch)
			{
				Server.SendPackage(this, package, _mtuSize, ref _reliableMessageNumber);
			}
			else
			{
				lock (_queueSync)
				{
					_sendQueueNotConcurrent.Enqueue(package);
				}
			}
		}

		private void SendQueueSingleThreaded(object state)
		{
			while (IsConnected)
			{
				//Task.Delay(10).Wait();

				Thread.Sleep(10);

				Queue<Package> queue = _sendQueueNotConcurrent;

				int messageCount = 0;

				int lenght = queue.Count;
				MemoryStream memStream = new MemoryStream();
				for (int i = 0; i < lenght; i++)
				{
					Package package = null;
					lock (_queueSync)
					{
						if (queue.Count == 0) break;
						try
						{
							package = queue.Dequeue();
						}
						catch (Exception e)
						{
						}
					}

					if (package == null) continue;

					byte[] bytes = package.Encode();
					if (bytes != null)
					{
						messageCount++;
						memStream.Write(BitConverter.GetBytes(Endian.SwapInt32(bytes.Length)), 0, 4);
						memStream.Write(bytes, 0, bytes.Length);
					}
					package.PutPool();
				}

				if (messageCount == 0) continue;
				if (!IsConnected) continue;

				ThreadPool.QueueUserWorkItem(delegate(object o)
				{
					McpeBatch batch = McpeBatch.CreateObject();
					byte[] buffer = CompressBytes(memStream.ToArray(), CompressionLevel.Optimal);
					batch.payloadSize = buffer.Length;
					batch.payload = buffer;
					batch.Encode();

					Server.SendPackage(this, batch, _mtuSize, ref _reliableMessageNumber);
				});
			}
		}


		private object _syncHack = new object();
		private MemoryStream memStream = new MemoryStream();

		private void SendQueue(object state)
		{
			if (!Monitor.TryEnter(_syncHack)) return;

			try
			{
				memStream.Position = 0;
				memStream.SetLength(0);
				Queue<Package> queue = _sendQueueNotConcurrent;

				int messageCount = 0;

				int lenght = queue.Count;
				for (int i = 0; i < lenght; i++)
				{
					Package package = null;
					lock (_queueSync)
					{
						if (queue.Count == 0) break;
						try
						{
							package = queue.Dequeue();
						}
						catch (Exception e)
						{
						}
					}

					if (package == null) continue;

					byte[] bytes = package.Encode();
					if (bytes != null)
					{
						messageCount++;
						memStream.Write(BitConverter.GetBytes(Endian.SwapInt32(bytes.Length)), 0, 4);
						memStream.Write(bytes, 0, bytes.Length);
					}
					package.PutPool();
				}

				if (messageCount == 0) return;
				if (!IsConnected) return;

				McpeBatch batch = McpeBatch.CreateObject();
				byte[] buffer = CompressBytes(memStream.ToArray(), CompressionLevel.Optimal);
				batch.payloadSize = buffer.Length;
				batch.payload = buffer;
				batch.Encode();

				Server.SendPackage(this, batch, _mtuSize, ref _reliableMessageNumber);
			}
			finally
			{
				Monitor.Exit(_syncHack);
			}
		}

		private object _sendMoveListSync = new object();
		private DateTime _lastMoveListSendTime = DateTime.UtcNow;

		public void SendMoveList(McpeBatch batch, DateTime sendTime)
		{
			if (sendTime < _lastMoveListSendTime || !Monitor.TryEnter(_sendMoveListSync))
			{
				batch.PutPool();
				return;
			}

			_lastMoveListSendTime = sendTime;

			try
			{
				Server.SendPackage(this, batch, _mtuSize, ref _reliableMessageNumber);
			}
			finally
			{
				Monitor.Exit(_sendMoveListSync);
			}
		}

		public void CleanCache()
		{
			lock (_chunksUsed)
			{
				_chunksUsed.Clear();
			}
		}
	}
}