using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using fNbt;
using log4net;
using Microsoft.AspNet.Identity;
using MiNET.Blocks;
using MiNET.Crafting;
using MiNET.Effects;
using MiNET.Entities;
using MiNET.Entities.World;
using MiNET.Items;
using MiNET.Net;
using MiNET.Security;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET
{
	public class Player : Entity
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Player));

		public MiNetServer Server { get; private set; }
		public IPEndPoint EndPoint { get; private set; }
		public PlayerNetworkSession NetworkSession { get; set; }

		private Queue<Package> _sendQueueNotConcurrent = new Queue<Package>();
		private object _queueSync = new object();
		// ReSharper disable once NotAccessedField.Local
		private Timer _sendTicker;

		private Dictionary<Tuple<int, int>, McpeBatch> _chunksUsed = new Dictionary<Tuple<int, int>, McpeBatch>();
		private ChunkCoordinates _currentChunkPosition;

		private Inventory _openInventory;
		public PlayerInventory Inventory { get; private set; }

		public PlayerLocation SpawnPosition { get; set; }

		public int MaxViewDistance { get; set; } = 22;
		public GameMode GameMode { get; set; }
		public bool IsConnected { get; set; }
		public string Username { get; private set; }
		public string DisplayName { get; set; }
		public long ClientId { get; set; }
		public long ClientGuid { get; set; }
		public string ClientSecret { get; set; }
		public UUID ClientUuid { get; set; }

		public Skin Skin { get; set; }
		public bool Silent { get; set; }
		public bool HideNameTag { get; set; }
		public bool NoAi { get; set; }

		public float EnchantingLevel { get; set; } = 0f;
		public float Experience { get; set; } = 0f;
		public float MovementSpeed { get; set; } = 0.1f;
		public float Absorption { get; set; } = 0;
		public ConcurrentDictionary<EffectType, Effect> Effects { get; set; } = new ConcurrentDictionary<EffectType, Effect>();

		public HungerManager HungerManager { get; set; }

		public bool IsOnGround { get; set; }
		public bool IsFalling { get; set; }

		public long Rtt { get; set; } = 300;
		public long RttVar { get; set; }
		public long Rto { get; set; }

		public List<Popup> Popups { get; set; } = new List<Popup>();

		public User User { get; set; }
		public Session Session { get; set; }

		public DateTime LastNetworkActivity { get; set; }

		public Player(MiNetServer server, IPEndPoint endPoint) : base(-1, null)
		{
			Server = server;
			EndPoint = endPoint;

			Inventory = new PlayerInventory(this);
			HungerManager = new HungerManager(this);

			IsSpawned = false;
			IsConnected = endPoint != null; // Can't connect if there is no endpoint

			Height = 1.85;

			if (IsConnected) _sendTicker = new Timer(SendQueue, null, 10, 10); // RakNet send tick-time
		}

		public void HandlePackage(Package message)
		{
			//if (!IsConnected) return;

			LastNetworkActivity = DateTime.UtcNow;

			var result = Server.PluginManager.PluginPacketHandler(message, true, this);
			//if (result != message) message.PutPool();
			message = result;

			if (message == null)
			{
				return;
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

			else if (typeof (McpeContainerSetSlot) == message.GetType())
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

			else if (typeof (McpeCraftingEvent) == message.GetType())
			{
				HandleCraftingEvent((McpeCraftingEvent) message);
			}

			else if (typeof (McpeRequestChunkRadius) == message.GetType())
			{
				HandleMcpeRequestChunkRadius((McpeRequestChunkRadius) message);
			}

			else if (typeof (McpeMapInfoRequest) == message.GetType())
			{
				HandleMcpeMapInfoRequest((McpeMapInfoRequest) message);
			}

			else if (typeof (McpeItemFramDropItem) == message.GetType())
			{
				HandleMcpeItemFramDropItem((McpeItemFramDropItem) message);
			}

			else if (typeof(McpeItemFramDropItem) == message.GetType())
			{
				HandleMcpePlayerInput((McpePlayerInput)message);
			}

			else
			{
				Log.Error($"Unhandled package: {message.GetType().Name} for user: {Username}, IP {EndPoint.Address}");
				//Disconnect("Unhandled package", false);
			}

			if (message.Timer.IsRunning)
			{
				long elapsedMilliseconds = message.Timer.ElapsedMilliseconds;
				if (elapsedMilliseconds > 1000)
				{
					Log.WarnFormat("Package (0x{1:x2}) handling too long {0}ms for {2}", elapsedMilliseconds, message.Id, Username);
				}
			}
			else
			{
				Log.WarnFormat("Package (0x{0:x2}) timer not started for {1}.", message.Id, Username);
			}
		}

		protected virtual void HandleMcpePlayerInput(McpePlayerInput message)
		{
			Log.Debug($"Player input: Motion X={message.motionX}, Motion Z={message.motionZ}, Flags=0x{message.motionX:X2}");
		}

		private object _mapInfoSync = new object();

		private Timer _mapSender;
		private ConcurrentQueue<McpeBatch> _mapBatches = new ConcurrentQueue<McpeBatch>();

		protected virtual void HandleMcpeMapInfoRequest(McpeMapInfoRequest message)
		{
			lock (_mapInfoSync)
			{
				//if(_mapSender == null)
				//{
				//	_mapSender = new Timer(Callback);
				//}

				long mapId = message.mapId;

				//Log.Warn($"Requested map with ID: {mapId} 0x{mapId:X2}");

				if (mapId == 0)
				{
					// 2016-02-26 02:53:01,895 [17] INFO  MiNET.Player - Requested map with ID: 0xFFFFFFFFFFFFFFFF
					// Should not happen.
				}
				else
				{
					MapEntity mapEntity = Level.GetEntity(mapId) as MapEntity;
					//if (mapEntity == null)
					//{
					//	// Create new map entity
					//	// send map for that entity
					//	mapEntity = new MapEntity(Level, mapId);
					//	mapEntity.SpawnEntity();
					//}
					//else
					{
						mapEntity?.AddToMapListeners(this, mapId);
					}
				}
			}
		}

		private void Callback(object state)
		{
		}

		public virtual void SendMapInfo(MapInfo mapInfo)
		{
			McpeClientboundMapItemData packet = McpeClientboundMapItemData.CreateObject();
			packet.mapinfo = mapInfo;
			SendPackage(packet);
		}

		public int ChunkRadius { get; private set; } = -1;

		protected virtual void HandleMcpeRequestChunkRadius(McpeRequestChunkRadius message)
		{
			Log.Debug($"Requested chunk radius of: {message.chunkRadius}");

			ChunkRadius = Math.Max(5, Math.Min(message.chunkRadius, MaxViewDistance));

			SendChunkRadiusUpdate();

			if (_completedStartSequence)
			{
				ThreadPool.QueueUserWorkItem(delegate(object state) { SendChunksForKnownPosition(); });
			}
		}

		protected virtual void HandleNewIncomingConnection(NewIncomingConnection message)
		{
			NetworkSession.State = ConnectionState.Connected;
			Log.DebugFormat("New incoming connection from {0} {1}", EndPoint.Address, EndPoint.Port);
		}

		/// <summary>
		///     Handles an animate packet.
		/// </summary>
		/// <param name="message">The message.</param>
		protected virtual void HandleAnimate(McpeAnimate message)
		{
			if (Level == null) return;

			Log.DebugFormat("HandleAnimate Action: {0}", message.actionId);

			McpeAnimate msg = McpeAnimate.CreateObject();
			msg.entityId = EntityId;
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

			switch ((PlayerAction) message.actionId)
			{
				case PlayerAction.StartBreak:
					break;
				case PlayerAction.AbortBreak:
					break;
				case PlayerAction.StopBreak:
					break;
				case PlayerAction.ReleaseItem:
					if (_itemUseTimer <= 0) return;

					Item itemInHand = Inventory.GetItemInHand();

					if (itemInHand == null) return; // Cheat(?)

					itemInHand.Release(Level, this, new BlockCoordinates(message.x, message.y, message.z), Level.TickTime - _itemUseTimer);

					_itemUseTimer = 0;

					break;
				case PlayerAction.StopSleeping:
					break;
				case PlayerAction.Respawn:
					ThreadPool.QueueUserWorkItem(delegate(object state) { HandleRespawn(null); });
					break;
				case PlayerAction.Jump:
					HungerManager.IncreaseExhaustion(IsSprinting ? 0.8f : 0.2f);
					break;
				case PlayerAction.StartSprint:
					SetSprinting(true);
					break;
				case PlayerAction.StopSprint:
					SetSprinting(false);
					break;
				case PlayerAction.StartSneak:
					SetSprinting(false);
					IsSneaking = true;
					break;
				case PlayerAction.StopSneak:
					SetSprinting(false);
					IsSneaking = false;
					break;
				case PlayerAction.DimensionChange:
					break;
				case PlayerAction.AbortDimensionChange:
					break;
				default:
					Log.Warn($"Unhandled action ID={message.actionId}");
					throw new ArgumentOutOfRangeException(nameof(message.actionId));
			}

			IsInAction = false;
			MetadataDictionary metadata = new MetadataDictionary
			{
				[0] = GetDataValue()
			};

			var setEntityData = McpeSetEntityData.CreateObject();
			setEntityData.entityId = EntityId;
			setEntityData.metadata = metadata;
			Level?.RelayBroadcast(this, setEntityData);
		}

		private float _baseSpeed;
		private object _sprintLock = new object();

		public void SetSprinting(bool isSprinting)
		{
			lock (_sprintLock)
			{
				if (isSprinting == IsSprinting) return;

				if (isSprinting)
				{
					IsSprinting = true;
					_baseSpeed = MovementSpeed;
					MovementSpeed += MovementSpeed*0.3f;
				}
				else
				{
					IsSprinting = false;
					MovementSpeed = _baseSpeed;
					SendUpdateAttributes();
				}

				SendUpdateAttributes();
			}
		}

		/// <summary>
		///     Handles the ping.
		/// </summary>
		/// <param name="message">The message.</param>
		protected virtual void HandlePing(InternalPing message)
		{
			SendPackage(message);
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

			//if (IsAdventure)
			//{
			//	mcpeAdventureSettings.flags |= 0x01;
			//}

			if (IsAutoJump)
			{
				mcpeAdventureSettings.flags |= 0x40;
			}

			if (AllowFly || GameMode == GameMode.Creative)
			{
				mcpeAdventureSettings.flags |= 0x80;
			}
			//else
			//{
			//	mcpeAdventureSettings.flags |= 0x20;
			//}

			//if (IsSpectator)
			//{
			//	mcpeAdventureSettings.flags |= 0x100;
			//}

			mcpeAdventureSettings.userPermission = 0x2;
			mcpeAdventureSettings.globalPermission = 0x2;
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
		public void SetSpectator(bool isSpectator)
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

		public bool AllowFly { get; set; }

		[Wired]
		public void SetAllowFly(bool allowFly)
		{
			AllowFly = allowFly;
			if (!AllowFly)
			{
				SendAdventureSettings();
				SendStartGame();
			}
			else
			{
				SendAdventureSettings();
			}
		}

		/// <summary>
		///     Handles the disconnection notification.
		/// </summary>
		protected virtual void HandleDisconnectionNotification()
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
			SendPackage(package, true);
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
					Log.InfoFormat("Player {0} doing multiple logins", Username);
					return; // Already doing login
				}

				Username = message.username;
			}

			if (message.protocol < 60)
			{
				Server.GreylistManager.Greylist(EndPoint.Address, 30000);
				Disconnect(string.Format("Wrong version ({0}) of Minecraft Pocket Edition, please upgrade.", message.protocol));
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

			if (message.username == null || message.username.Trim().Length == 0 || !Regex.IsMatch(message.username, "^[A-Za-z0-9_-]{3,56}$"))
			{
				Disconnect("Invalid username.");
				return;
			}

			if (string.IsNullOrEmpty(message.skin.SkinType) || message.skin.Texture == null)
			{
				Disconnect("Invalid skin. Please upgrade your version of Minecraft Pocket Edition");
				return;
			}

			SendPlayerStatus(0); // Hmm, login success?

			Username = message.username;
			ClientId = message.clientId;
			ClientUuid = message.clientUuid;
			ClientSecret = message.clientSecret;
			Skin = message.skin;

			//string fileName = Path.GetTempPath() + "Skin_" + Skin.SkinType + ".png";
			//Log.Info($"Writing skin to filename: {fileName}");
			//Skin.SaveTextureToFile(fileName, Skin.Texture);

			var serverInfo = Server.ServerInfo;

			if (ClientSecret != null)
			{
				var count = serverInfo.PlayerSessions.Values.Count(session => session.Player != null && ClientSecret.Equals(session.Player.ClientSecret));
				if (count != 1)
				{
					Disconnect($"Invalid skin {count}.");
					return;
				}
			}

			// THIS counter exist to protect the level from being swamped with player list add
			// attempts during startup (normally).
			Interlocked.Increment(ref serverInfo.ConnectionsInConnectPhase);

			new Thread(Start).Start();
		}

		private bool _completedStartSequence = false;

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
					User = User ?? Server.UserManager.FindByName(Username);
				}

				lock (_disconnectSync)
				{
					if (!IsConnected) return;

					Level = Server.LevelManager.GetLevel(this, "Default");
				}
				if (Level == null)
				{
					Disconnect("No level assigned.");
					return;
				}

				SpawnPosition = SpawnPosition ?? Level.SpawnPoint;
				KnownPosition = SpawnPosition;

				// Check if the user already exist, that case bumpt the old one
				Level.RemoveDuplicatePlayers(Username, ClientId);

				Level.EntityManager.AddEntity(null, this);

				GameMode = Level.GameMode;

				// Start game

				SendStartGame();

				SendSetSpawnPosition();

				SetPosition(SpawnPosition);

				SendSetTime();

				SendSetDificulty();

				SendAdventureSettings();

				// Vanilla sends player list here...

				SendUpdateAttributes();

				SendRespawn();

				Level.AddPlayer(this, false);

				{
					McpeCraftingData craftingData = McpeCraftingData.CreateObject();
					craftingData.recipes = RecipeManager.Recipes;
					SendPackage(craftingData);
				}

				//SendPlayerStatus(3);

				BroadcastSetEntityData();
			}
			finally
			{
				Interlocked.Decrement(ref serverInfo.ConnectionsInConnectPhase);
			}

			LastUpdatedTime = DateTime.UtcNow;

			SendCreativeInventory();

			SendPlayerInventory();

			SendChunkRadiusUpdate();

			_completedStartSequence = true;

			ThreadPool.QueueUserWorkItem(delegate(object state) { SendChunksForKnownPosition(); });

			LastUpdatedTime = DateTime.UtcNow;
			Log.InfoFormat("Login complete by: {0} from {2} in {1}ms", Username, watch.ElapsedMilliseconds, EndPoint);
		}

		public virtual void InitializePlayer()
		{
			SendPlayerStatus(3);

			//send time again
			SendSetTime();
			IsSpawned = true;

			SetPosition(SpawnPosition);

			LastUpdatedTime = DateTime.UtcNow;
			_haveJoined = true;

			OnPlayerJoin(new PlayerEventArgs(this));
		}

		protected virtual void HandleRespawn(McpeRespawn msg)
		{
			HealthManager.ResetHealth();

			HungerManager.ResetHunger();

			BroadcastSetEntityData();

			SendUpdateAttributes();

			SendSetSpawnPosition();

			SendAdventureSettings();

			SendPlayerInventory();

			CleanCache();

			ForcedSendChunk(SpawnPosition);

			// send teleport to spawn
			SetPosition(SpawnPosition);

			Level.SpawnToAll(this);

			IsSpawned = true;

			Log.InfoFormat("Respawn player {0} on level {1}", Username, Level.LevelId);

			SendSetTime();

			ThreadPool.QueueUserWorkItem(delegate(object state) { ForcedSendChunks(); });

			//SendPlayerStatus(3);

			//McpeRespawn mcpeRespawn = McpeRespawn.CreateObject();
			//mcpeRespawn.x = SpawnPosition.X;
			//mcpeRespawn.y = SpawnPosition.Y;
			//mcpeRespawn.z = SpawnPosition.Z;
			//SendPackage(mcpeRespawn);

			////send time again
			//SendSetTime();
			//IsSpawned = true;
			//LastUpdatedTime = DateTime.UtcNow;
			//_haveJoined = true;
		}

		[Wired]
		public void SetPosition(PlayerLocation position, bool teleport = true)
		{
			KnownPosition = position;
			LastUpdatedTime = DateTime.UtcNow;

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

			//if (Level != null)
			{
				Level.RemovePlayer(this, true);
				Level.EntityManager.RemoveEntity(null, this);
			}

			Level = toLevel; // Change level
			SpawnPosition = spawnPoint;
			//Level.AddPlayer(this, "", false);
			// reset all health states

			HungerManager.ResetHunger();

			HealthManager.ResetHealth();

			BroadcastSetEntityData();

			SendUpdateAttributes();

			SendSetSpawnPosition();

			SendAdventureSettings();

			SendPlayerInventory();

			CleanCache();

			ForcedSendChunk(spawnPoint);

			// send teleport to spawn
			SetPosition(spawnPoint);

			SetNoAi(oldNoAi);

		    ThreadPool.QueueUserWorkItem(delegate
		    {
		        ForcedSendChunks(() =>
		        {
                    Level.AddPlayer(this, true);

                    Log.InfoFormat("Respawn player {0} on level {1}", Username, Level.LevelId);

                    SendSetTime();
                });
		    }); 
        }

        public override void BroadcastSetEntityData()
		{
			McpeSetEntityData mcpeSetEntityData = McpeSetEntityData.CreateObject();
			mcpeSetEntityData.entityId = 0;
			mcpeSetEntityData.metadata = GetMetadata();
			mcpeSetEntityData.Encode();
			SendPackage(mcpeSetEntityData);

			base.BroadcastSetEntityData();
		}

		public void SendSetDificulty()
		{
			McpeSetDifficulty mcpeSetDifficulty = McpeSetDifficulty.CreateObject();
			mcpeSetDifficulty.difficulty = (int) Level.Difficulty;
			SendPackage(mcpeSetDifficulty);
		}

		public virtual void SendPlayerInventory()
		{
			//Log.Error("Send player inventory");
			McpeContainerSetContent inventoryContent = McpeContainerSetContent.CreateObject();
			inventoryContent.NoBatch = true;
			inventoryContent.windowId = (byte) 0x00;
			inventoryContent.slotData = Inventory.GetSlots();
			inventoryContent.hotbarData = Inventory.GetHotbar();
			SendPackage(inventoryContent);

			McpeContainerSetContent armorContent = McpeContainerSetContent.CreateObject();
			armorContent.windowId = 0x78;
			armorContent.slotData = Inventory.GetArmor();
			armorContent.hotbarData = null;
			SendPackage(armorContent);
		}

		public virtual void SendCreativeInventory()
		{
			McpeContainerSetContent creativeContent = McpeContainerSetContent.CreateObject();
			creativeContent.windowId = (byte) 0x79;
			creativeContent.slotData = InventoryUtils.GetCreativeMetadataSlots();
			creativeContent.hotbarData = Inventory.GetHotbar();
			SendPackage(creativeContent);
		}

		private void SendChunkRadiusUpdate()
		{
			McpeChunkRadiusUpdate package = McpeChunkRadiusUpdate.CreateObject();
			package.chunkRadius = ChunkRadius;

			SendPackage(package);
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


		[Wired]
		public void StrikeLightning()
		{
			Lightning lightning = new Lightning(Level) {KnownPosition = KnownPosition};

			if (lightning.Level == null) return;

			lightning.SpawnEntity();
		}

		private object _disconnectSync = new object();

		private bool _haveJoined = false;

		public virtual void Disconnect(string reason, bool sendDisconnect = true)
		{
			lock (_disconnectSync)
			{
				if (IsConnected)
				{
					if (Level != null) OnPlayerLeave(new PlayerEventArgs(this));

					if (sendDisconnect)
					{
						McpeDisconnect disconnect = McpeDisconnect.CreateObject();
						disconnect.NoBatch = true;
						disconnect.message = reason;
						SendDirectPackage(disconnect);
					}
					IsConnected = false;
				}

				if (_sendTicker != null)
				{
					_sendTicker.Change(Timeout.Infinite, Timeout.Infinite);
					WaitHandle waitHandle = new AutoResetEvent(false);
					_sendTicker.Dispose(waitHandle);
					WaitHandle.WaitAll(new[] {waitHandle}, TimeSpan.FromMinutes(2));
					_sendTicker = null;
				}

				Level?.RemovePlayer(this);

				var playerSession = Session;
				Session = null;
				if (playerSession != null)
				{
					Server.SessionManager.RemoveSession(playerSession);
					playerSession.Player = null;
				}

				string levelId = Level == null ? "Unknown" : Level.LevelId;
				Log.InfoFormat("Disconnected player {0}/{1} from level <{3}>, reason: {2}", Username, EndPoint.Address, reason, levelId);
				if (!_haveJoined)
				{
					Log.WarnFormat("Disconnected crashed player {0}/{1} from level <{3}>, reason: {2}", Username, EndPoint.Address, reason, levelId);
				}
				else if (NetworkSession != null && NetworkSession.CreateTime.AddSeconds(10) > DateTime.UtcNow)
				{
					Log.WarnFormat("Early disconnect of player {0}/{1} from level <{3}> after less then 10s with reason: {2}", Username, EndPoint.Address, reason, levelId);
				}

				//HACK: But needed

				if (NetworkSession != null && NetworkSession.PlayerAckQueue.Count > 0)
				{
					Thread.Sleep(50);
				}

				PlayerNetworkSession session;
				if (Server.ServerInfo.PlayerSessions.TryRemove(EndPoint, out session))
				{
					session.Clean();

					session.State = ConnectionState.Unconnected;
					session.Evicted = true;

					NetworkSession = null;
					session.Player = null;
				}

				SendQueue(null);

				CleanCache();

				//Server.GreylistManager.Greylist(EndPoint.Address, 10000);
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
				Level.BroadcastMessage(text, sender: this);
			}
		}

		private int _lastPlayerMoveSequenceNUmber;
		private int _lastOrderingIndex;
		private object _moveSyncLock = new object();

		protected virtual void HandleMovePlayer(McpeMovePlayer message)
		{
			if (!IsSpawned || HealthManager.IsDead) return;

			lock (_moveSyncLock)
			{
				if (_lastPlayerMoveSequenceNUmber > message.DatagramSequenceNumber)
				{
					return;
				}
				_lastPlayerMoveSequenceNUmber = message.DatagramSequenceNumber;

				if (_lastOrderingIndex > message.OrderingIndex)
				{
					return;
				}
				_lastOrderingIndex = message.OrderingIndex;
			}

			if (!AcceptPlayerMove(message)) return;

			// Hunger management
			HungerManager.Move(new Vector3(KnownPosition.X, 0, KnownPosition.Z).DistanceTo(new Vector3(message.x, 0, message.z)));

			Vector3 origin = KnownPosition.ToVector3();
			double distanceTo = origin.DistanceTo(new Vector3(message.x, message.y, message.z));
			double verticalMove = message.y - 1.62 - KnownPosition.Y;

			KnownPosition = new PlayerLocation
			{
				X = message.x, Y = message.y - 1.62f, Z = message.z, Pitch = message.pitch, Yaw = message.yaw, HeadYaw = message.headYaw
			};

			if (Math.Abs(distanceTo) > 0.001)
			{
				IsOnGround = CheckOnGround();
			}

			IsFalling = verticalMove < 0 && !IsOnGround;

			LastUpdatedTime = DateTime.UtcNow;

			var chunkPosition = new ChunkCoordinates(KnownPosition);
			if (_currentChunkPosition != chunkPosition /*&& _currentChunkPosition.DistanceTo(chunkPosition) > 2*/)
			{
				ThreadPool.QueueUserWorkItem(delegate { SendChunksForKnownPosition(); });
			}
		}

		protected virtual bool AcceptPlayerMove(McpeMovePlayer message)
		{
			return true;
		}

		private static readonly int[] Layers = {-1, 0};
		private static readonly int[] Arounds = {0, 1, -1};

		public bool CheckOnGround()
		{
			if (Level == null)
				return true;

			BlockCoordinates pos = (BlockCoordinates) KnownPosition;

			foreach (int layer in Layers)
			{
				foreach (int x in Arounds)
				{
					foreach (int z in Arounds)
					{
						var offset = new BlockCoordinates(x, layer, z);
						Block block = Level.GetBlock(pos + offset);
						if (block.IsSolid)
						{
							//Level.SetBlock(new GoldBlock() {Coordinates = block.Coordinates});
							return true;
						}
					}
				}
			}

			return false;
		}


		protected virtual void HandleRemoveBlock(McpeRemoveBlock message)
		{
			Level.BreakBlock(this, new BlockCoordinates(message.x, message.y, message.z));
		}

		protected virtual void HandlePlayerArmorEquipment(McpePlayerArmorEquipment message)
		{
		}

		protected virtual void HandleMcpeItemFramDropItem(McpeItemFramDropItem message)
		{
			Item droppedItem = message.item;
			/*if (Log.IsDebugEnabled) */Log.Warn($"Player {Username} drops item frame {droppedItem} at {message.x}, {message.y}, {message.z}");
		}

		protected virtual void HandlePlayerDropItem(McpeDropItem message)
		{
			lock (Inventory)
			{
				Item droppedItem = message.item;
				if (Log.IsDebugEnabled) Log.Debug($"Player {Username} drops item {droppedItem} with inv slot {message.itemtype}");


				if (!VerifyItemStack(droppedItem)) return;

				// Clear current inventory slot.

				ItemEntity itemEntity = new ItemEntity(Level, droppedItem)
				{
					Velocity = KnownPosition.GetDirection()*0.7,
					KnownPosition =
					{
						X = KnownPosition.X,
						Y = KnownPosition.Y + 1.62f,
						Z = KnownPosition.Z
					},
				};

				itemEntity.SpawnEntity();
			}
		}

		protected virtual void HandlePlayerEquipment(McpePlayerEquipment message)
		{
			if (HealthManager.IsDead) return;

			lock (_inventorySync)
			{
				Item itemStack = message.item;
				if (GameMode != GameMode.Creative && itemStack != null && !VerifyItemStack(itemStack))
				{
					Log.Error($"Kicked {Username} for equipment hacking.");
					Disconnect("Error #376. Please report this error.");
				}

				byte selectedHotbarSlot = message.selectedSlot;
				int selectedInventorySlot = (byte) (message.slot - PlayerInventory.HotbarSize);

				if (Log.IsDebugEnabled) Log.Debug($"Player {Username} called set equiptment with inv slot: {selectedInventorySlot}({message.slot}) and hotbar slot {message.selectedSlot} and Item ID: {message.item.Id} with count item count: {message.item.Count}");

				// 255 indicates empty hmmm
				if (selectedInventorySlot < 0 || (message.slot != 255 && selectedInventorySlot >= Inventory.Slots.Count))
				{
					if (GameMode != GameMode.Creative)
					{
						Log.Error($"Player {Username} set equiptment fails with inv slot: {selectedInventorySlot}({message.slot}) and hotbar slot {selectedHotbarSlot} for inventory size: {Inventory.Slots.Count} and Item ID: {message.item?.Id}");
					}
					return;
				}

				if (message.slot == 255)
				{
					//Inventory.ItemHotbar[selectedHotbarSlot] = -1;
					//return;
					selectedInventorySlot = -1;
				}
				else
				{
					for (int i = 0; i < Inventory.ItemHotbar.Length; i++)
					{
						if (Inventory.ItemHotbar[i] == selectedInventorySlot)
						{
							Inventory.ItemHotbar[i] = Inventory.ItemHotbar[selectedHotbarSlot];
							break;
						}
					}
				}

				Inventory.ItemHotbar[selectedHotbarSlot] = selectedInventorySlot;
				Inventory.SetHeldItemSlot(selectedHotbarSlot, false);

				//if (selectedInventorySlot < Inventory.Slots.Count)
				//{
				//	Inventory.Slots[selectedInventorySlot] = message.item.Value;
				//}

				if (Log.IsDebugEnabled) Log.Debug($"Player {Username} set equiptment with inv slot: {selectedInventorySlot}({message.slot}) and hotbar slot {selectedHotbarSlot}");
			}
		}


		private object _inventorySync = new object();

		public void OpenInventory(BlockCoordinates inventoryCoord)
		{
			lock (_inventorySync)
			{
				if (_openInventory != null)
				{
					if (_openInventory.Coordinates.Equals(inventoryCoord)) return;
					HandleMcpeContainerClose(null);
				}

				// get inventory from coordinates
				// - get blockentity
				// - get inventory from block entity

				Inventory inventory = Level.InventoryManager.GetInventory(inventoryCoord);

				if (inventory == null)
				{
					Log.Warn($"No inventory found at {inventoryCoord}");
					return;
				}

				// get inventory # from inventory manager
				// set inventory as active on player

				_openInventory = inventory;

				if (inventory.Type == 0 && !inventory.IsOpen()) // Chest open animation
				{
					var tileEvent = McpeTileEvent.CreateObject();
					tileEvent.x = inventoryCoord.X;
					tileEvent.y = inventoryCoord.Y;
					tileEvent.z = inventoryCoord.Z;
					tileEvent.case1 = 1;
					tileEvent.case2 = 2;
					Level.RelayBroadcast(tileEvent);
				}

				// subscribe to inventory changes
				inventory.InventoryChange += OnInventoryChange;
				inventory.AddObserver(this);

				// open inventory

				var containerOpen = McpeContainerOpen.CreateObject();
				containerOpen.NoBatch = true;
				containerOpen.windowId = inventory.WindowsId;
				containerOpen.type = inventory.Type;
				containerOpen.slotCount = inventory.Size;
				containerOpen.x = inventoryCoord.X;
				containerOpen.y = inventoryCoord.Y;
				containerOpen.z = inventoryCoord.Z;
				SendPackage(containerOpen);

				var containerSetContent = McpeContainerSetContent.CreateObject();
				containerSetContent.NoBatch = true;
				containerSetContent.windowId = inventory.WindowsId;
				containerSetContent.slotData = inventory.Slots;
				SendPackage(containerSetContent);
			}
		}

		private void OnInventoryChange(Player player, Inventory inventory, byte slot, Item itemStack)
		{
			if (player == this)
			{
				//TODO: This needs to be synced to work properly under heavy load (SG).
				//Level.SetBlockEntity(inventory.BlockEntity, false);
			}
			else
			{
				var containerSetSlot = McpeContainerSetSlot.CreateObject();
				containerSetSlot.windowId = inventory.WindowsId;
				containerSetSlot.slot = slot;
				containerSetSlot.item = itemStack;
				SendPackage(containerSetSlot);
			}

			//if(inventory.BlockEntity != null)
			//{
			//	Level.SetBlockEntity(inventory.BlockEntity, false);
			//}
		}


		protected virtual void HandleCraftingEvent(McpeCraftingEvent message)
		{
			Log.Debug($"Player {Username} crafted item on window 0x{message.windowId:X2} on type: {message.recipeType} DatagramSequenceNumber: {message.DatagramSequenceNumber}, ReliableMessageNumber: {message.ReliableMessageNumber}, OrderingIndex: {message.OrderingIndex}");
		}

		/// <summary>
		///     Handles the container set slot.
		/// </summary>
		/// <param name="message">The message.</param>
		protected virtual void HandleContainerSetSlot(McpeContainerSetSlot message)
		{
			lock (Inventory)
			{
				if (HealthManager.IsDead) return;

				Item itemStack = message.item;

				if (GameMode != GameMode.Creative)
				{
					if (!VerifyItemStack(itemStack))
					{
						Log.Error($"Kicked {Username} for inventory hacking.");
						Disconnect("Error #324. Please report this error.");
						return;
					}
				}

				if (Log.IsDebugEnabled)
					Log.Debug($"Player {Username} set inventory item on window 0x{message.windowId:X2} with slot: {message.slot} HOTBAR: {message.unknown} and item: {itemStack}");

				if (_openInventory != null)
				{
					if (_openInventory.WindowsId == message.windowId)
					{
						if (_openInventory.Type == 4)
						{
							Recipes recipes = new Recipes();
							recipes.Add(new EnchantingRecipe());
							McpeCraftingData crafting = McpeCraftingData.CreateObject();
							crafting.recipes = recipes;
							SendPackage(crafting);
						}

						// block inventories of various kinds (chests, furnace, etc)
						_openInventory.SetSlot(this, (byte) message.slot, itemStack);
						return;
					}
				}

				switch (message.windowId)
				{
					case 0:
						Inventory.Slots[message.slot] = itemStack;
						break;
					case 0x79:
						Inventory.Slots[message.slot] = itemStack;
						break;
					case 0x78:

						var armorItem = itemStack;
						switch (message.slot)
						{
							case 0:
								Inventory.Helmet = armorItem;
								break;
							case 1:
								Inventory.Chest = armorItem;
								break;
							case 2:
								Inventory.Leggings = armorItem;
								break;
							case 3:
								Inventory.Boots = armorItem;
								break;
						}

						McpePlayerArmorEquipment armorEquipment = McpePlayerArmorEquipment.CreateObject();
						armorEquipment.entityId = EntityId;
						armorEquipment.helmet = Inventory.Helmet;
						armorEquipment.chestplate = Inventory.Chest;
						armorEquipment.leggings = Inventory.Leggings;
						armorEquipment.boots = Inventory.Boots;
						Level.RelayBroadcast(this, armorEquipment);

						break;
					case 0x7A:
						//Inventory.ItemHotbar[message.unknown] = message.slot/* + PlayerInventory.HotbarSize*/;
						break;
				}
			}
		}

		public virtual bool VerifyItemStack(Item itemStack)
		{
			if (ItemSigner.DefaultItemSigner == null) return true;

			return ItemSigner.DefaultItemSigner.VerifyItemStack(this, itemStack);
		}

		protected virtual void HandleMcpeContainerClose(McpeContainerClose message)
		{
			lock (_inventorySync)
			{
				var inventory = _openInventory;
				_openInventory = null;

				if (inventory == null) return;

				// unsubscribe to inventory changes
				inventory.InventoryChange -= OnInventoryChange;
				inventory.RemoveObserver(this);

				if (message != null && message.windowId != inventory.WindowsId) return;

				// close container 
				if (inventory.Type == 0 && !inventory.IsOpen())
				{
					var tileEvent = McpeTileEvent.CreateObject();
					tileEvent.x = inventory.Coordinates.X;
					tileEvent.y = inventory.Coordinates.Y;
					tileEvent.z = inventory.Coordinates.Z;
					tileEvent.case1 = 1;
					tileEvent.case2 = 0;
					Level.RelayBroadcast(tileEvent);
				}
			}
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
			if (message.actionId != 2) return;

			Player player = target as Player;
			if (player != null)
			{
				Item itemInHand = Inventory.GetItemInHand();
				double damage = itemInHand.GetDamage(); //Item Damage.
				if (IsFalling)
				{
					damage += Level.Random.Next((int) (damage/2 + 2));

					McpeAnimate animate = McpeAnimate.CreateObject();
					animate.entityId = target.EntityId;
					animate.actionId = 4;
					Level.RelayBroadcast(animate);
				}

				Effect effect;
				if (Effects.TryGetValue(EffectType.Weakness, out effect))
				{
					damage -= (effect.Level + 1)*4;
					if (damage < 0) damage = 0;
				}
				else if (Effects.TryGetValue(EffectType.Strength, out effect))
				{
					damage += (effect.Level + 1)*3;
				}

				damage += CalculateDamageIncreaseFromEnchantments(itemInHand);

				player.HealthManager.TakeHit(this, (int) CalculatePlayerDamage(player, damage), DamageCause.EntityAttack);
			}
			else
			{
				// This is totally wrong. Need to merge with the above damage calculation
				target.HealthManager.TakeHit(this, CalculateDamage(target), DamageCause.EntityAttack);
			}

			HungerManager.IncreaseExhaustion(0.3f);
		}

		public double CalculateDamageIncreaseFromEnchantments(Item tool)
		{
			if (tool == null) return 0;
			if (tool.ExtraData == null) return 0;

			NbtList enchantings;
			if (!tool.ExtraData.TryGet("ench", out enchantings)) return 0;

			double increase = 0;
			foreach (NbtCompound enchanting in enchantings)
			{
				short level = enchanting["lvl"].ShortValue;

				if (level == 0) continue;

				short id = enchanting["id"].ShortValue;
				if (id == 9)
				{
					increase += 1 + ((level - 1)*0.5);
				}
			}

			return increase;
		}


		public double CalculatePlayerDamage(Player target, double damage)
		{
			double originalDamage = damage;
			double armorValue = 0;
			double epfValue = 0;

			{
				{
					Item armorPiece = target.Inventory.Helmet;
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
					epfValue += CalculateDamageReducationFromEnchantments(armorPiece);
				}

				{
					Item armorPiece = target.Inventory.Chest;
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
					epfValue += CalculateDamageReducationFromEnchantments(armorPiece);
				}

				{
					Item armorPiece = target.Inventory.Leggings;
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
					epfValue += CalculateDamageReducationFromEnchantments(armorPiece);
				}

				{
					Item armorPiece = target.Inventory.Boots;
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
					epfValue += CalculateDamageReducationFromEnchantments(armorPiece);
				}
			}

			damage = damage*(1 - Math.Max(armorValue/5, armorValue - damage/2)/25);

			epfValue = Math.Min(20, epfValue);
			damage = damage*(1 - epfValue/25);


			Log.Debug($"Original Damage={originalDamage:F1} Redused Damage={damage:F1}, Armor Value={armorValue:F1}, EPF {epfValue:F1}");
			return (int) damage;

			//armorValue *= 0.04; // Each armor point represent 4% reduction
			//return (int) Math.Floor(damage*(1.0 - armorValue));
		}

		public double CalculateDamageReducationFromEnchantments(Item armor)
		{
			if (armor == null) return 0;
			if (armor.ExtraData == null) return 0;

			NbtList enchantings;
			if (!armor.ExtraData.TryGet("ench", out enchantings)) return 0;

			double reduction = 0;
			foreach (NbtCompound enchanting in enchantings)
			{
				short level = enchanting["lvl"].ShortValue;

				double typeModifier = 0;
				short id = enchanting["id"].ShortValue;
				if (id == 0) typeModifier = 1;
				else if (id == 1) typeModifier = 2;
				else if (id == 2) typeModifier = 2;
				else if (id == 3) typeModifier = 2;
				else if (id == 4) typeModifier = 3;

				reduction += level*typeModifier;
			}

			return reduction;
		}

		private int CalculateDamage(Entity target)
		{
			int damage = Inventory.GetItemInHand().GetDamage(); //Item Damage.

			damage = (int) Math.Floor(damage*(1.0));

			return damage;
		}


		protected virtual void HandleEntityEvent(McpeEntityEvent message)
		{
			Log.Debug("Entity Id:" + message.entityId);
			Log.Debug("Entity Event:" + message.eventId);

			//if (message.eventId != 0) return; // Should probably broadcast?!

			switch (message.eventId)
			{
				case 9:
					// Eat food

					if (GameMode == GameMode.Survival)
					{
						Item itemInHand = Inventory.GetItemInHand();

						if (itemInHand is FoodItem)
						{
							FoodItem foodItem = (FoodItem) Inventory.GetItemInHand();
							foodItem.Consume(this);
							foodItem.Count--;
							SendPlayerInventory();
						}
						else if (itemInHand is ItemPotion)
						{
							ItemPotion potion = (ItemPotion) Inventory.GetItemInHand();
							potion.Consume(this);
							potion.Count--;
							SendPlayerInventory();
						}
					}
					break;
			}
		}

		private long _itemUseTimer;

		protected virtual void HandleUseItem(McpeUseItem message)
		{
			Log.DebugFormat("Use item: {0}", message.item);
			Log.DebugFormat("BlockCoordinates:  {0}", message.blockcoordinates);
			Log.DebugFormat("face:  {0}", message.face);
			Log.DebugFormat("Facecoordinates:  {0}", message.facecoordinates);
			Log.DebugFormat("Player position:  {0}", message.playerposition);

			if (message.item == null)
			{
				Log.Warn($"{Username} sent us a use item message with no item (null).");
				return;
			}

			if (GameMode != GameMode.Creative && !VerifyItemStack(message.item))
			{
				Log.Error($"Kicked {Username} for use item hacking.");
				Disconnect("Error #324. Please report this error.");
				return;
			}

			// Make sure we are holding the item we claim to be using
			Item itemInHand = Inventory.GetItemInHand();
			if (itemInHand == null || itemInHand.Id != message.item.Id)
			{
				if (GameMode != GameMode.Creative) Log.Error($"Use item detected difference between server and client. Expected item {message.item.Id} but server had item {itemInHand?.Id}");
				return; // Cheat(?)
			}

			if (message.face <= 5)
			{
				// Right click

				Level.Interact(Level, this, message.item.Id, message.blockcoordinates, message.item.Metadata, (BlockFace) message.face, message.facecoordinates);
			}
			else
			{
				// Snowballs and shit

				_itemUseTimer = Level.TickTime;

				itemInHand.UseItem(Level, this, message.blockcoordinates);

				IsInAction = true;
				MetadataDictionary metadata = new MetadataDictionary
				{
					[0] = GetDataValue()
				};

				var setEntityData = McpeSetEntityData.CreateObject();
				setEntityData.entityId = EntityId;
				setEntityData.metadata = metadata;
				Level.RelayBroadcast(this, setEntityData);
			}
		}

		public void SendRespawn()
		{
			McpeRespawn mcpeRespawn = McpeRespawn.CreateObject();
			mcpeRespawn.x = SpawnPosition.X;
			mcpeRespawn.y = SpawnPosition.Y;
			mcpeRespawn.z = SpawnPosition.Z;
			SendPackage(mcpeRespawn);
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
			mcpeStartGame.b1 = true;
			mcpeStartGame.b2 = true;
			mcpeStartGame.b3 = false;
			mcpeStartGame.unknownstr = null;
			// unknownstr=iX8AANxLbgA=

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

		private void ForcedSendChunk(PlayerLocation position)
		{
			lock (_sendChunkSync)
			{
				var chunkPosition = new ChunkCoordinates(position);

				McpeBatch chunk = Level.GenerateChunk(chunkPosition);
				var key = new Tuple<int, int>(chunkPosition.X, chunkPosition.Z);
				if (!_chunksUsed.ContainsKey(key))
				{
					_chunksUsed.Add(key, chunk);
				}

				if (chunk != null)
				{
					SendPackage(chunk, true);
				}
			}
		}

		private void ForcedSendChunks(Action postAction = null)
		{
			Monitor.Enter(_sendChunkSync);
			try
			{
				var chunkPosition = new ChunkCoordinates(KnownPosition);

				_currentChunkPosition = chunkPosition;

				if (Level == null) return;

				int packetCount = 0;
				foreach (McpeBatch chunk in Level.GenerateChunks(_currentChunkPosition, _chunksUsed, ChunkRadius))
				{
					if (chunk != null) SendPackage(chunk, true);

					//if (packetCount > 16) Thread.Sleep(12);

					packetCount++;
				}
			}
			finally
			{
				Monitor.Exit(_sendChunkSync);
			}

            if(postAction != null)
            {
                postAction();
            }
		}

		private void SendChunksForKnownPosition()
		{
			if (!Monitor.TryEnter(_sendChunkSync)) return;

			try
			{
				if (ChunkRadius <= 0) return;


				var chunkPosition = new ChunkCoordinates(KnownPosition);
				if (IsSpawned && _currentChunkPosition == chunkPosition) return;

				//if (IsSpawned && _currentChunkPosition.DistanceTo(chunkPosition) < 2)
				//{
				//	return;
				//}

				_currentChunkPosition = chunkPosition;

				int packetCount = 0;

				if (Level == null) return;

				foreach (McpeBatch chunk in Level.GenerateChunks(_currentChunkPosition, _chunksUsed, ChunkRadius))
				{
					if (chunk != null) SendPackage(chunk);

					if (!IsSpawned)
					{
						if (packetCount++ == 56)
						{
							InitializePlayer();
						}
					}
					else
					{
						//if (packetCount++ > 56) Thread.Sleep(1);
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
			MemoryStream stream = MiNetServer.MemoryStreamManager.GetStream();
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


		public virtual void SendUpdateAttributes()
		{
			//Attribute[generic.absorption, Name: generic.absorption, MinValue: 0, MaxValue: 3, 402823E+38, Value: 0]
			//Attribute[player.saturation, Name: player.saturation, MinValue: 0, MaxValue: 20, Value: 5]
			//Attribute[player.exhaustion, Name: player.exhaustion, MinValue: 0, MaxValue: 5, Value: 0]
			//Attribute[generic.knockbackResistance, Name: generic.knockbackResistance, MinValue: 0, MaxValue: 1, Value: 0]
			//Attribute[generic.health, Name: generic.health, MinValue: 0, MaxValue: 20, Value: 20]
			//Attribute[generic.movementSpeed, Name: generic.movementSpeed, MinValue: 0, MaxValue: 3, 402823E+38, Value: 0, 1]
			//Attribute[generic.followRange, Name: generic.followRange, MinValue: 0, MaxValue: 2048, Value: 16]
			//Attribute[player.hunger, Name: player.hunger, MinValue: 0, MaxValue: 20, Value: 20]
			//Attribute[generic.attackDamage, Name: generic.attackDamage, MinValue: 0, MaxValue: 3, 402823E+38, Value: 1]
			//Attribute[player.level, Name: player.level, MinValue: 0, MaxValue: 24791, Value: 0]
			//Attribute[player.experience, Name: player.experience, MinValue: 0, MaxValue: 1, Value: 0]

			var attributes = new PlayerAttributes();
			attributes["generic.health"] = new PlayerAttribute
			{
				Name = "generic.health", MinValue = 0, MaxValue = 20, Value = HealthManager.Hearts
			};
			attributes["player.hunger"] = new PlayerAttribute
			{
				Name = "player.hunger", MinValue = HungerManager.MinHunger, MaxValue = HungerManager.MaxHunger, Value = HungerManager.Hunger
			};
			attributes["player.level"] = new PlayerAttribute
			{
				Name = "player.level", MinValue = 0, MaxValue = 24791, Value = EnchantingLevel
			};
			attributes["player.experience"] = new PlayerAttribute
			{
				Name = "player.experience", MinValue = 0, MaxValue = 1, Value = Experience
			};
			attributes["generic.movementSpeed"] = new PlayerAttribute
			{
				Name = "generic.movementSpeed", MinValue = 0, MaxValue = 24791, Value = MovementSpeed
			};
			attributes["generic.absorption"] = new PlayerAttribute
			{
				Name = "generic.absorption", MinValue = 0, MaxValue = float.MaxValue, Value = Absorption
			};

			McpeUpdateAttributes attributesPackate = McpeUpdateAttributes.CreateObject();
			attributesPackate.entityId = 0;
			attributesPackate.attributes = attributes;
			SendPackage(attributesPackate);

			// Workaround, bad design.
			HungerManager.SendHungerAttributes();
		}

		public virtual void SendSetTime()
		{
			McpeSetTime message = McpeSetTime.CreateObject();
			message.NoBatch = true;
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
			HungerManager.OnTick();

			base.OnTick();

			foreach (var effect in Effects)
			{
				effect.Value.OnTick(this);
			}

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
				SendPackage(motions);
			}

			//Task.Delay(500).ContinueWith(delegate(Task task)
			//{
			//	McpeSetEntityMotion motions = McpeSetEntityMotion.CreateObject();
			//	motions.entities = new EntityMotions {{0, Vector3.Zero}};
			//	SendPackage(motions);
			//}
			//);
		}

		public override MetadataDictionary GetMetadata()
		{
			MetadataDictionary metadata = new MetadataDictionary();
			metadata[0] = new MetadataByte(GetDataValue());
			metadata[1] = new MetadataShort(HealthManager.Air);
			metadata[2] = new MetadataString(NameTag ?? Username);
			metadata[3] = new MetadataByte(!HideNameTag);
			metadata[4] = new MetadataByte(Silent);
			metadata[7] = new MetadataInt(0); // Potion Color
			metadata[8] = new MetadataByte(0); // Potion Ambient
			metadata[15] = new MetadataByte(NoAi);
			metadata[16] = new MetadataByte(0); // Player flags
			metadata[17] = new MetadataIntCoordinates(0, 0, 0);

			return metadata;
		}

		[Wired]
		public void SetNoAi(bool noAi)
		{
			NoAi = noAi;

			BroadcastSetEntityData();
		}

		[Wired]
		public void SetHideNameTag(bool hideNameTag)
		{
			HideNameTag = hideNameTag;

			BroadcastSetEntityData();
		}

		[Wired]
		public void SetNameTag(string nameTag)
		{
			NameTag = nameTag;

			BroadcastSetEntityData();
		}

		[Wired]
		public void SetDisplayName(string displayName)
		{
			DisplayName = displayName;

			{
				McpePlayerList playerList = McpePlayerList.CreateObject();
				playerList.records = new PlayerRemoveRecords {this};
				Level.RelayBroadcast(playerList);
			}
			{
				McpePlayerList playerList = McpePlayerList.CreateObject();
				playerList.records = new PlayerAddRecords {this};
				Level.RelayBroadcast(playerList);
			}
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
				Effects.TryRemove(effect.EffectId, out effect);
			}
		}

		[Wired]
		public void RemoveAllEffects()
		{
			foreach (var effect	 in Effects)
			{
				RemoveEffect(effect.Value);
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

		public virtual void BroadcastEntityEvent()
		{
			{
				var entityEvent = McpeEntityEvent.CreateObject();
				entityEvent.entityId = 0;
				entityEvent.eventId = (byte) (HealthManager.Health <= 0 ? 3 : 2);
				SendPackage(entityEvent);
			}
			{
				var entityEvent = McpeEntityEvent.CreateObject();
				entityEvent.entityId = EntityId;
				entityEvent.eventId = (byte) (HealthManager.Health <= 0 ? 3 : 2);
				Level.RelayBroadcast(this, entityEvent);
			}

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

		public void SendDirectPackage(Package package)
		{
			Server.SendPackage(this, package);
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

			lock (_queueSync)
			{
				_sendQueueNotConcurrent.Enqueue(package);
			}
		}

		private object _syncHack = new object();
		private MemoryStream memStream = new MemoryStream();

		private void SendQueue(object state)
		{
			if (!Monitor.TryEnter(_syncHack)) return;

			try
			{
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

					if (!IsConnected)
					{
						package.PutPool();
						continue;
					}

					if (lenght == 1)
					{
						Server.SendPackage(this, package);
					}
					else if (package is McpeBatch)
					{
						SendBuffered(messageCount);
						messageCount = 0;
						Server.SendPackage(this, package);
						Thread.Sleep(1); // Really important to slow down speed a bit
					}
					else if (package.NoBatch)
					{
						SendBuffered(messageCount);
						messageCount = 0;
						Server.SendPackage(this, package);
					}
					else if (!IsSpawned)
					{
						SendBuffered(messageCount);
						messageCount = 0;
						Server.SendPackage(this, package);
					}
					else
					{
						if (messageCount == 0)
						{
							memStream.Position = 0;
							memStream.SetLength(0);
						}

						byte[] bytes = package.Encode();
						if (bytes != null)
						{
							messageCount++;
							memStream.Write(BitConverter.GetBytes(Endian.SwapInt32(bytes.Length)), 0, 4);
							memStream.Write(bytes, 0, bytes.Length);
						}

						package.PutPool();
					}
				}

				if (!IsConnected) return;

				SendBuffered(messageCount);
			}
			finally
			{
				Monitor.Exit(_syncHack);
			}
		}

		private void SendBuffered(int messageCount)
		{
			if (messageCount == 0) return;

			McpeBatch batch = McpeBatch.CreateObject();
			var array = memStream.ToArray();
			//byte[] bufferNoComp = CompressBytes(array, CompressionLevel.NoCompression);
			//byte[] bufferOptimal = CompressBytes(array, CompressionLevel.Optimal);
			byte[] bufferSpeed = CompressBytes(array, CompressionLevel.Fastest);

			//Log.Error($"No comp: {bufferNoComp.Length}, Optimal: {bufferOptimal.Length}, Fastest: {bufferSpeed.Length}");

			var buffer = bufferSpeed;

			batch.payloadSize = buffer.Length;
			batch.payload = buffer;
			batch.Encode(true);

			memStream.Position = 0;
			memStream.SetLength(0);

			Server.SendPackage(this, batch);
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
				SendPackage(batch);
			}
			finally
			{
				Monitor.Exit(_sendMoveListSync);
			}
		}

		public void CleanCache()
		{
			lock (_sendChunkSync)
			{
				_chunksUsed.Clear();
			}
		}

		public virtual void DropInventory()
		{
			var slots = Inventory.Slots;

			Vector3 coordinates = KnownPosition.ToVector3();
			coordinates.Y += 0.5f;

			foreach (var stack in slots.ToArray())
			{
				Level.DropItem(coordinates, stack);
			}

			if (Inventory.Helmet.Id != 0)
			{
				Level.DropItem(coordinates, Inventory.Helmet);
				Inventory.Helmet = new ItemAir();
			}
			if (Inventory.Chest.Id != 0)
			{
				Level.DropItem(coordinates, Inventory.Chest);
				Inventory.Chest = new ItemAir();
			}
			if (Inventory.Leggings.Id != 0)
			{
				Level.DropItem(coordinates, Inventory.Leggings);
				Inventory.Leggings = new ItemAir();
			}
			if (Inventory.Boots.Id != 0)
			{
				Level.DropItem(coordinates, Inventory.Boots);
				Inventory.Boots = new ItemAir();
			}

			Inventory.Clear();
		}

		public override void SpawnToPlayers(Player[] players)
		{
			McpeAddPlayer mcpeAddPlayer = McpeAddPlayer.CreateObject();
			mcpeAddPlayer.uuid = ClientUuid;
			mcpeAddPlayer.username = Username;
			mcpeAddPlayer.entityId = EntityId;
			mcpeAddPlayer.x = KnownPosition.X;
			mcpeAddPlayer.y = KnownPosition.Y;
			mcpeAddPlayer.z = KnownPosition.Z;
			mcpeAddPlayer.yaw = KnownPosition.Yaw;
			mcpeAddPlayer.headYaw = KnownPosition.HeadYaw;
			mcpeAddPlayer.pitch = KnownPosition.Pitch;
			mcpeAddPlayer.metadata = GetMetadata();
			Level.RelayBroadcast(this, players, mcpeAddPlayer);

			SendEquipmentForPlayer(players);

			SendArmorForPlayer(players);
		}

		public virtual void SendEquipmentForPlayer(Player[] receivers)
		{
			McpePlayerEquipment mcpePlayerEquipment = McpePlayerEquipment.CreateObject();
			mcpePlayerEquipment.entityId = EntityId;
			mcpePlayerEquipment.item = Inventory.GetItemInHand();
			mcpePlayerEquipment.slot = 0;
			Level.RelayBroadcast(this, receivers, mcpePlayerEquipment);
		}

		public virtual void SendArmorForPlayer(Player[] receivers)
		{
			McpePlayerArmorEquipment mcpePlayerArmorEquipment = McpePlayerArmorEquipment.CreateObject();
			mcpePlayerArmorEquipment.entityId = EntityId;
			mcpePlayerArmorEquipment.helmet = Inventory.Helmet;
			mcpePlayerArmorEquipment.chestplate = Inventory.Chest;
			mcpePlayerArmorEquipment.leggings = Inventory.Leggings;
			mcpePlayerArmorEquipment.boots = Inventory.Boots;
			Level.RelayBroadcast(this, receivers, mcpePlayerArmorEquipment);
		}

		public override void DespawnFromPlayers(Player[] players)
		{
			McpeRemovePlayer mcpeRemovePlayer = McpeRemovePlayer.CreateObject();
			mcpeRemovePlayer.clientUuid = ClientUuid;
			mcpeRemovePlayer.entityId = EntityId;
			Level.RelayBroadcast(this, players, mcpeRemovePlayer);
		}


		// Events

		public event EventHandler<PlayerEventArgs> PlayerJoin;

		protected virtual void OnPlayerJoin(PlayerEventArgs e)
		{
			PlayerJoin?.Invoke(this, e);
		}

		public event EventHandler<PlayerEventArgs> PlayerLeave;

		protected virtual void OnPlayerLeave(PlayerEventArgs e)
		{
			PlayerLeave?.Invoke(this, e);
		}
	}

	public class PlayerEventArgs : CancelEventArgs
	{
		public Player Player { get; }
		public Level Level { get; }

		public PlayerEventArgs(Player player)
		{
			Player = player;
			Level = player.Level;
		}
	}
}