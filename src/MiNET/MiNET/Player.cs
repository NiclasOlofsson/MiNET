﻿using System;
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
using MiNET.Crafting;
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

		public override string ToString()
		{
			return $"Name: {Name}, MinValue: {MinValue}, MaxValue: {MaxValue}, Value: {Value}";
		}
	}

	public class Player : Entity
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Player));

		public MiNetServer Server { get; private set; }
		public IPEndPoint EndPoint { get; private set; }
		public PlayerNetworkSession NetworkSession { get; set; }

		private int _mtuSize;
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
		public string Username { get; private set; }
		public int ClientId { get; set; }
		public long ClientGuid { get; set; }
		public string ClientSecret { get; set; }
		public UUID ClientUuid { get; set; }

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

			Inventory = new PlayerInventory(this);

			_chunksUsed = new Dictionary<Tuple<int, int>, McpeBatch>();

			IsSpawned = false;
			IsConnected = endPoint != null; // Can't connect if there is no endpoint

			if (IsConnected) _sendTicker = new Timer(SendQueue, null, 10, 10); // RakNet send tick-time
		}

		public DateTime LastNetworkActivity { get; set; }

		public void HandlePackage(Package message)
		{
			//if (!IsConnected) return;

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

		protected virtual void HandleNewIncomingConnection(NewIncomingConnection message)
		{
			NetworkSession.State = ConnectionState.Connected;
			Log.DebugFormat("New incoming connection from {0} {1}", EndPoint.Address, message.port);
		}

		/// <summary>
		///     Handles an animate packet.
		/// </summary>
		/// <param name="message">The message.</param>
		protected virtual void HandleAnimate(McpeAnimate message)
		{
			if (Level == null) return;

			//Log.DebugFormat("Action: {0}", message.actionId);

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
			//Log.DebugFormat("Player action: {0}", message.actionId);
			//Log.DebugFormat("Entity ID: {0}", message.entityId);
			//Log.DebugFormat("Action ID:  {0}", message.actionId);
			//Log.DebugFormat("x:  {0}", message.x);
			//Log.DebugFormat("y:  {0}", message.y);
			//Log.DebugFormat("z:  {0}", message.z);
			//Log.DebugFormat("Face:  {0}", message.face);

			switch ((PlayerAction) message.actionId)
			{
				case PlayerAction.StartBreak:
					break;
				case PlayerAction.AbortBreak:
					break;
				case PlayerAction.StopBreak:
					break;
				case PlayerAction.ReleaseItem:
					if (_itemUseTimer == null) return;

					Item itemInHand = Inventory.GetItemInHand().Item;

					if (itemInHand == null) return; // Cheat(?)

					_itemUseTimer.Stop();
					itemInHand.Release(Level, this, new BlockCoordinates(message.x, message.y, message.z), _itemUseTimer.ElapsedMilliseconds);

					_itemUseTimer = null;

					break;
				case PlayerAction.StopSleeping:
					break;
				case PlayerAction.Respawn:
					ThreadPool.QueueUserWorkItem(delegate(object state) { HandleRespawn(null); });
					break;
				case PlayerAction.Jump:
					break;
				case PlayerAction.StartSprint:
					break;
				case PlayerAction.StopSprint:
					break;
				case PlayerAction.StartSneak:
					break;
				case PlayerAction.StopSneak:
					break;
				case PlayerAction.DimensionChange:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			// Player out of action
			MetadataDictionary metadata = new MetadataDictionary();
			metadata[0] = new MetadataByte(0);

			var setEntityData = McpeSetEntityData.CreateObject();
			setEntityData.entityId = EntityId;
			setEntityData.metadata = metadata;
			Level?.RelayBroadcast(this, setEntityData);
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

			if (message.protocol < 38)
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

			if (message.username == null || message.username.Trim().Length == 0 || !Regex.IsMatch(message.username, "^[A-Za-z0-9_-]{3,16}$"))
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
			ClientId = (int) message.clientId;
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
				KnownPosition = new PlayerLocation
				{
					X = SpawnPosition.X, Y = SpawnPosition.Y, Z = SpawnPosition.Z, Yaw = SpawnPosition.Yaw, Pitch = SpawnPosition.Pitch, HeadYaw = SpawnPosition.HeadYaw,
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

				Level.AddPlayer(this, false);

				{
					var craftingData = new McpeCraftingData();
					craftingData.recipes = RecipeManager.Recipes;
					SendPackage(craftingData);
				}
			}
			finally
			{
				Interlocked.Decrement(ref serverInfo.ConnectionsInConnectPhase);
			}

			LastUpdatedTime = DateTime.UtcNow;

			SendPlayerInventory();

			//Level.SpawnToAll(this);

			ThreadPool.QueueUserWorkItem(delegate(object state) { SendChunksForKnownPosition(); });

			LastUpdatedTime = DateTime.UtcNow;
			Log.InfoFormat("Login complete by: {0} from {2} in {1}ms", Username, watch.ElapsedMilliseconds, EndPoint);
		}

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
			_haveJoined = true;
		}

		protected virtual void HandleRespawn(McpeRespawn msg)
		{
			HealthManager.ResetHealth();
			SendSetHealth();

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
				X = KnownPosition.X, Y = 4000, Z = KnownPosition.Z, Yaw = 91, Pitch = 28, HeadYaw = 91,
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
			HealthManager.ResetHealth();
			SendSetHealth();

			SendSetSpawnPosition();

			SendAdventureSettings();

			SendPlayerInventory();

			CleanCache();

			ForcedSendChunk(spawnPoint);

			// send teleport to spawn
			SetPosition(spawnPoint);

			SetNoAi(oldNoAi);

			Level.AddPlayer(this, true);

			Log.InfoFormat("Respawn player {0} on level {1}", Username, Level.LevelId);

			SendSetTime();

			ThreadPool.QueueUserWorkItem(delegate(object state) { ForcedSendChunks(); });
		}


		public void SendSetEntityData()
		{
			McpeSetEntityData mcpeSetEntityData = McpeSetEntityData.CreateObject();
			mcpeSetEntityData.entityId = 0;
			mcpeSetEntityData.metadata = GetMetadata();
			mcpeSetEntityData.Encode();
			SendPackage(mcpeSetEntityData);

			BroadcastSetEntityData();
		}

		public void SendSetDificulty()
		{
			McpeSetDifficulty mcpeSetDifficulty = McpeSetDifficulty.CreateObject();
			mcpeSetDifficulty.difficulty = (int) Level.Difficulty;
			SendPackage(mcpeSetDifficulty);
		}

		public void SendPlayerInventory()
		{
			MetadataSlots creativeInv = new MetadataSlots();

			if (GameMode == GameMode.Creative)
			{
				creativeInv = InventoryUtils.GetCreativeMetadataSlots();
			}

			McpeContainerSetContent creativeContent = McpeContainerSetContent.CreateObject();
			creativeContent.windowId = (byte) 0x79;
			creativeContent.slotData = creativeInv;
			creativeContent.hotbarData = Inventory.GetHotbar();
			SendPackage(creativeContent);

			McpeContainerSetContent inventoryContent = McpeContainerSetContent.CreateObject();
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

			SendPlayerInventory();

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
			lock(_disconnectSync)
			{
				if (IsConnected)
				{
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

				if (Level != null && IsSpawned)
				{
					Level.RemovePlayer(this);
				}

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
				PlayerNetworkSession session;
				if (Server.ServerInfo.PlayerSessions.TryRemove(EndPoint, out session))
				{
					session.State = ConnectionState.Unconnected;
					session.Evicted = true;

					NetworkSession = null;
					session.Player = null;

					session.Clean();
				}

				SendQueue(null);

				CleanCache();

				Server.GreylistManager.Greylist(EndPoint.Address, 10000);
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

			KnownPosition = new PlayerLocation
			{
				X = message.x, Y = message.y - 1.62f, Z = message.z, Pitch = message.pitch, Yaw = message.yaw, HeadYaw = message.headYaw
			};

			LastUpdatedTime = DateTime.UtcNow;

			ThreadPool.QueueUserWorkItem(delegate(object state) { SendChunksForKnownPosition(); });
		}

		private int _isKnownCheater = 0;
		private int _cheatLimit = 5;

		protected virtual bool AcceptPlayerMove(McpeMovePlayer message)
		{
			if (GameMode != GameMode.Creative && _isKnownCheater <= _cheatLimit)
			{
				long td = DateTime.UtcNow.Ticks - LastUpdatedTime.Ticks;
				if (GameMode == GameMode.Survival && HealthManager.CooldownTick == 0 && td > 49*TimeSpan.TicksPerMillisecond && td < 500*TimeSpan.TicksPerMillisecond && Level.SpawnPoint.DistanceTo(KnownPosition) > 2.0)
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
						double speedLimit = (message.y - 1.62) - KnownPosition.Y < 0 ? -70d : 12d; //6d;
						double distanceTo = (message.y - 1.62) - KnownPosition.Y;
						verticalSpeed = distanceTo/td*TimeSpan.TicksPerSecond;
						if (!(horizSpeed > 0) && Math.Abs(verticalSpeed) > Math.Abs(speedLimit))
						{
							if (_isKnownCheater == _cheatLimit)
							{
								Level.BroadcastMessage(string.Format("{0} is detected as flying {3:##.##}m/s {1:##.##}m {2}ms", Username, distanceTo, (int) ((double) td/TimeSpan.TicksPerMillisecond), verticalSpeed), type: MessageType.Raw);
								Log.WarnFormat("{0} is fly cheating {3:##.##}m/s {1:##.##}m {2}ms", Username, distanceTo, (int) ((double) td/TimeSpan.TicksPerMillisecond), verticalSpeed);
							}
							//AddPopup(new Popup
							//{
							//	MessageType = MessageType.Tip,
							//	Message = string.Format("{3:##.##}m/s {1:##.##}m {2}ms", Username, distanceTo, (int)((double)td / TimeSpan.TicksPerMillisecond), verticalSpeed),
							//	Duration = 1
							//});

							LastUpdatedTime = DateTime.UtcNow;
							//HealthManager.TakeHit(this, 1, DamageCause.Suicide);
							//SendMovePlayer();
							_isKnownCheater++;
							return false;
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

			return true;
		}

		protected virtual void HandleRemoveBlock(McpeRemoveBlock message)
		{
			Level.BreakBlock(this, new BlockCoordinates(message.x, message.y, message.z));
		}

		protected virtual void HandlePlayerArmorEquipment(McpePlayerArmorEquipment message)
		{
		}

		protected virtual void HandlePlayerDropItem(McpeDropItem message)
		{
			lock (Inventory)
			{
				var stack = message.item.Value;
				Log.Debug($"Player {Username} drops item with inv slot: {message.itemtype} and Item ID: {stack.Id} with count item count: {stack.Count}");

				if (_transaction == null && !Inventory.HasItem(message.item) || (_transaction != null && !_transaction.Equals(stack)))
				{
					Log.Error($"Player {Username} failed to drops item with inv slot: {message.itemtype}) and Item ID: {stack.Id} with count item count: {stack.Count}. Did not match current transaction state.");
					return;
				}

				// Clear current inventory slot.
				if (_transaction != null)
				{
					Log.Info("Closed inventory transaction via drop item");
					_transaction = null;

					var itemEntity = new ItemEntity(Level, stack.Item)
					{
						Velocity = KnownPosition.GetDirection()*0.7,
						KnownPosition =
						{
							X = KnownPosition.X,
							Y = KnownPosition.Y + 1.62f,
							Z = KnownPosition.Z
						},
						Count = stack.Count
					};

					itemEntity.SpawnEntity();
				}
				else
				{
					Log.Info("Begin drop transaction on quick drop item");
					_dropTransaction = stack;
				}
			}
		}

		protected virtual void HandlePlayerEquipment(McpePlayerEquipment message)
		{
			if (HealthManager.IsDead) return;

			//Log.Info($"Player {Username} called set equiptment with inv slot: {message.slot}) and hotbar slot {message.selectedSlot} and Item ID: {message.item.Value.Id} with count item count: {message.item.Value.Count}");

			byte selectedHotbarSlot = message.selectedSlot;
			int selectedInventorySlot = (byte) (message.slot - PlayerInventory.HotbarSize);

			{
				// 255 indicates empty hmmm
				if (selectedInventorySlot < 0 || (message.slot != 255 && selectedInventorySlot >= Inventory.Slots.Count))
				{
					if (GameMode != GameMode.Creative)
					{
						Log.Error($"Player {Username} set equiptment fails with inv slot: {selectedInventorySlot}({message.slot}) and hotbar slot {selectedHotbarSlot} for inventory size: {Inventory.Slots.Count} and Item ID: {message.item?.Value?.Id}");
					}
					return;
				}

				Log.Info($"Player {Username} set equiptment with inv slot: {selectedInventorySlot}({message.slot}) and hotbar slot {selectedHotbarSlot}");

				//ItemStack itemStack = message.slot != 255 ? Inventory.Slots[selectedInventorySlot] : message.item.Value;
				//if (itemStack != null)
				//{
				//	var existingItemId = itemStack.Id;
				//	var incomingItemId = message.item.Value.Id;

				//	if (existingItemId != incomingItemId)
				//	{
				//		if (GameMode != GameMode.Creative)
				//		{
				//			Log.Error($"Player {Username} set equiptment fails because incoming item ID {incomingItemId} didn't match existing inventory item ID {existingItemId}");
				//		}
				//		//return;
				//	}
				//	else
				//	{
				//		//Log.InfoFormat("Player {2} set equiptment SUCCESS because incoming item ID {1} matched existing inventory item ID {0}", existingItemId, incomingItemId, Username);
				//	}
				//}
				//else
				//{
				//	Log.ErrorFormat("Player {0} set equiptment fails, probably hacker", Username);
				//}

				for (int i = 0; i < Inventory.ItemHotbar.Length; i++)
				{
					if (Inventory.ItemHotbar[i] == selectedInventorySlot)
					{
						Inventory.ItemHotbar[i] = Inventory.ItemHotbar[selectedHotbarSlot];
						break;
					}
				}

				Inventory.ItemHotbar[selectedHotbarSlot] = selectedInventorySlot;
				Inventory.SetHeldItemSlotNoSend(selectedHotbarSlot);
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

				_transaction = null;

				// get inventory from coordinates
				// - get blockentity
				// - get inventory from block entity

				Inventory inventory = Level.InventoryManager.GetInventory(inventoryCoord);

				if (inventory == null) return;

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

		private void OnInventoryChange(Player player, Inventory inventory, byte slot, ItemStack itemStack)
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
				containerSetSlot.item = new MetadataSlot(itemStack);
				SendPackage(containerSetSlot);
			}
		}


		private ItemStack _transaction = null;
		private ItemStack _dropTransaction = null;
		private int _isInArmorWorkaround = -1;
		private int _lastChestOrderingIndex;

		/// <summary>
		///     Handles the container set slot.
		/// </summary>
		/// <param name="message">The message.</param>
		protected virtual void HandleContainerSetSlot(McpeContainerSetSlot message)
		{
			lock (Inventory)
			{
				bool isLagging = _lastChestOrderingIndex >= message.OrderingIndex;
				if (_lastChestOrderingIndex == message.OrderingIndex) return; // Resend :-(
                _lastChestOrderingIndex = message.OrderingIndex;

				if (HealthManager.IsDead) return;

				ItemStack itemStack = message.item.Value;
				Log.Info($"Player {Username} set inventory item on window 0x{message.windowId:X2} with slot: {message.slot} HOTBAR: {message.unknown} Item ID: {itemStack.Id} Item Count: {itemStack.Count} Meta: {itemStack.Metadata}: DatagramSequenceNumber: {message.DatagramSequenceNumber}, ReliableMessageNumber: {message.ReliableMessageNumber}, OrderingIndex: {message.OrderingIndex}");

				// on all set container content, check if we have active inventory
				// and update that inventory.
				// Inventory manager makes sure other players with the same inventory open will 
				// also get the update.


				if (_openInventory != null)
				{
					if (_openInventory.WindowsId == message.windowId)
					{
						// block inventories of various kinds (chests, furnace, etc)
						_openInventory.SetSlot(this, (byte) message.slot, itemStack);
						return;
					}

					if (message.windowId == 0)
					{
						Inventory.Slots[message.slot] = itemStack;
						return;
					}
				}
				else if (GameMode == GameMode.Creative)
				{
					Inventory.Slots[message.slot] = itemStack;
					return;
				}


				switch (message.windowId)
				{
					case 0:
						if (message.slot >= Inventory.Slots.Count) return;

						if (_dropTransaction != null)
						{
							bool doDrop = false;

							if (itemStack.Id == _dropTransaction.Id)
							{
								var stack = Inventory.Slots[message.slot];
								if (stack.Count - (itemStack.Count + _dropTransaction.Count) == 0)
								{
									Inventory.Slots[message.slot] = itemStack;
									doDrop = true;
								}
							}
							else if (itemStack.Id == 0)
							{
								Inventory.Slots[message.slot] = new ItemStack();
								doDrop = true;
							}

							if (doDrop)
							{
								var itemEntity = new ItemEntity(Level, _dropTransaction.Item)
								{
									Velocity = KnownPosition.GetDirection()*0.7,
									KnownPosition =
									{
										X = KnownPosition.X,
										Y = KnownPosition.Y + 1.62f,
										Z = KnownPosition.Z
									},
									Count = _dropTransaction.Count
								};

								itemEntity.SpawnEntity();

								Log.Info("Close drop transaction.");
								_transaction = null;
								_dropTransaction = null;
							}
							else
							{
								Log.Error($"Lagging: {isLagging}. Abort inventory drop transaction. Failed item match for {Username} . Incoming item id: {itemStack.Id}, meta: {itemStack.Metadata}, count: {itemStack.Count}");
								_transaction = null;
								_dropTransaction = null;
								SendPlayerInventory();
							}


							return;
						}

						if (_transaction == null)
						{
							_isInArmorWorkaround = -1;

							Log.Info("Start inventory transaction");

							_transaction = Inventory.Slots[message.slot];

							if (itemStack.Equals(_transaction))
							{
								_transaction.Item.Metadata = itemStack.Metadata;
								_transaction = null;
								Log.Info("Closed inventory confirm only transaction");

								return;
							}

							if (itemStack.Id == _transaction.Id && itemStack.Count == _transaction.Count && itemStack.Metadata != _transaction.Metadata)
							{
								_transaction.Item.Metadata = itemStack.Metadata;
								_transaction = null;
								Log.Info("Closed inventory metadata only transaction");

								return;
							}

							if (itemStack.Id == _transaction.Id && itemStack.Count != _transaction.Count && itemStack.Metadata == _transaction.Metadata)
							{
								Log.Info("Partial stack begin transaction");

								byte delta = (byte) (_transaction.Count - itemStack.Count);
								if (delta > 0)
								{
									Inventory.Slots[message.slot] = new ItemStack(_transaction.Id, itemStack.Count, _transaction.Metadata);
									_transaction.Count = delta;
								}

								return;
							}

							if (itemStack.Id == 0)
							{
								Log.Info($"Full stack begin transaction. Existing id: {_transaction.Id}, meta: {_transaction.Metadata}, count: {_transaction.Count}");
								Inventory.Slots[message.slot] = new ItemStack();
							}
							else
							{
								if (itemStack.Count == 1 && Inventory.Slots[message.slot].Id == 0)
								{
									Inventory.Slots[message.slot] = itemStack;
									_isInArmorWorkaround = message.slot;
								}
								else
								{
									Log.Error($"Lagging: {isLagging}. Abort (hack?) inventory transaction for {Username} . Incoming item id: {itemStack.Id}, meta: {itemStack.Metadata}, count: {itemStack.Count}");
									_transaction = null;
									SendPlayerInventory();
								}
							}

							return;
						}
						else
						{
							if (_isInArmorWorkaround >= 0)
							{
								Log.Error($"Lagging: {isLagging}. Abort (hack?) inventory/armor transaction for {Username} . Incoming item id: {itemStack.Id}, meta: {itemStack.Metadata}, count: {itemStack.Count}");
								Inventory.Slots[_isInArmorWorkaround] = new ItemStack();
								_transaction = null;
								_isInArmorWorkaround = -1;
								SendPlayerInventory();
								return;
							}

							if (itemStack.Id == _transaction.Id && itemStack.Metadata == _transaction.Metadata)
							{
								if (Inventory.Slots[message.slot].Id != 0
								    && (itemStack.Id != Inventory.Slots[message.slot].Id || itemStack.Metadata != Inventory.Slots[message.slot].Metadata))
								{
									Log.Info("Partial inventory transaction, change transaction item");
									_transaction = Inventory.Slots[message.slot];
									Inventory.Slots[message.slot] = itemStack;
								}
								else
								{
									byte existing = Inventory.Slots[message.slot].Count;
									byte delta = (byte) (_transaction.Count - (itemStack.Count - existing));
									if (delta > 0)
									{
										Inventory.Slots[message.slot] = new ItemStack(_transaction.Id, itemStack.Count, _transaction.Metadata);
										_transaction.Count -= (byte) (itemStack.Count - existing);
										Log.Info("Partial inventory transaction. Left in transaction: " + _transaction.Count);
									}
									else
									{
										_transaction.Count += existing;
										Inventory.Slots[message.slot] = _transaction;
										_transaction = null;
										Log.Info("Close inventory transaction");
									}
								}
							}
							else
							{
								Log.Info("Partial inventory transaction, clear slot");
								Inventory.Slots[message.slot] = new ItemStack();
							}

							return;
						}

						break;
					case 0x79:
						Inventory.Slots[(byte) message.slot] = itemStack;
						break;
					case 0x78:

						if (_isInArmorWorkaround >= 0 || itemStack.Id == 0 && _transaction == null || _transaction != null && _transaction.Id == itemStack.Id)
						{
							var armorItem = itemStack.Item;
							Item currentArmorItem = null;
							switch (message.slot)
							{
								case 0:
									currentArmorItem = Inventory.Helmet;
									Inventory.Helmet = armorItem;
									break;
								case 1:
									currentArmorItem = Inventory.Chest;
									Inventory.Chest = armorItem;
									break;
								case 2:
									currentArmorItem = Inventory.Leggings;
									Inventory.Leggings = armorItem;
									break;
								case 3:
									currentArmorItem = Inventory.Boots;
									Inventory.Boots = armorItem;
									break;
							}

							if (_isInArmorWorkaround >= 0 || currentArmorItem == null || currentArmorItem.Id == 0)
							{
								_isInArmorWorkaround = -1;
								_transaction = null;
								Log.Info("Close armor transaction");
							}
							else
							{
								_transaction = new ItemStack(currentArmorItem, 1);
								Log.Info("Partial armor transaction");
							}


							McpePlayerArmorEquipment armorEquipment = McpePlayerArmorEquipment.CreateObject();
							armorEquipment.entityId = EntityId;
							armorEquipment.helmet = new MetadataSlot(new ItemStack(Inventory.Helmet, 1));
							armorEquipment.chestplate = new MetadataSlot(new ItemStack(Inventory.Chest, 1));
							armorEquipment.leggings = new MetadataSlot(new ItemStack(Inventory.Leggings, 1));
							armorEquipment.boots = new MetadataSlot(new ItemStack(Inventory.Boots, 1));
							Level.RelayBroadcast(this, armorEquipment);
						}
						else if (itemStack.Id != 0 && _transaction == null)
						{
							bool isVerifyOnly = false;

							var armorItem = itemStack.Item;
							switch (message.slot)
							{
								case 0:
									isVerifyOnly = Inventory.Helmet.Equals(armorItem);
									break;
								case 1:
									isVerifyOnly = Inventory.Chest.Equals(armorItem);
									break;
								case 2:
									isVerifyOnly = Inventory.Leggings.Equals(armorItem);
									break;
								case 3:
									isVerifyOnly = Inventory.Boots.Equals(armorItem);
									break;
							}

							if (!isVerifyOnly)
							{
								Log.Error($"Lagging: {isLagging}. Armor transaction failed (hack?) for {Username} . Incoming item id: {itemStack.Id}, meta: {itemStack.Metadata}, count: {itemStack.Count}");
								_transaction = null;
								SendPlayerInventory();
							}
						}
						else
						{
							Log.Error($"Lagging: {isLagging}. Armor transaction failed (unknown reason?) for {Username} . Incoming item id: {itemStack.Id}, meta: {itemStack.Metadata}, count: {itemStack.Count}");
							_transaction = null;
							SendPlayerInventory();
						}

						break;
				}
			}
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

				//SendPlayerInventory();

				// active inventory set to null
			}
		}

		/// <summary>
		///     Handles the interact.
		/// </summary>
		/// <param name="message">The message.</param>
		protected virtual void HandleInteract(McpeInteract message)
		{
			Entity target = Level.GetEntity(message.targetEntityId);

			//Log.DebugFormat("Interact Action ID: {0}", message.actionId);
			//Log.DebugFormat("Interact Target Entity ID: {0}", message.targetEntityId);

			if (target == null) return;

			Player player = target as Player;
			if (player != null)
			{
				int damage = Inventory.GetItemInHand().Item.GetDamage(); //Item Damage.
				player.HealthManager.TakeHit(this, CalculatePlayerDamage(player, damage), DamageCause.EntityAttack);
			}
			else
			{
				target.HealthManager.TakeHit(this, CalculateDamage(target), DamageCause.EntityAttack);
			}
		}

		public int CalculatePlayerDamage(Player target, int damage)
		{
			double armorValue = 0;

			{
				{
					var armorPiece = target.Inventory.Helmet;
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
					var armorPiece = target.Inventory.Chest;
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
					var armorPiece = target.Inventory.Leggings;
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
					var armorPiece = target.Inventory.Boots;
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

			return (int) Math.Floor(damage*(1.0 - armorValue));
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

					if (GameMode == GameMode.Survival)
					{
						FoodItem foodItem = Inventory.GetItemInHand().Item as FoodItem;
						if (foodItem != null)
						{
							foodItem.Consume(this);
							Inventory.GetItemInHand().Count--;
							SendPlayerInventory();
						}
					}
					break;
			}
		}

		private Stopwatch _itemUseTimer;

		protected virtual void HandleUseItem(McpeUseItem message)
		{
			//Log.DebugFormat("Use item: {0}", message.item.Value.Id);
			//Log.DebugFormat("item meta: {0}", message.item.Value.Metadata);
			//Log.DebugFormat("x:  {0}", message.x);
			//Log.DebugFormat("y:  {0}", message.y);
			//Log.DebugFormat("z:  {0}", message.z);
			//Log.DebugFormat("face:  {0}", message.face);
			//Log.DebugFormat("fx:  {0}", message.fx);
			//Log.DebugFormat("fy:  {0}", message.fy);
			//Log.DebugFormat("fz:  {0}", message.fz);
			//Log.DebugFormat("px:  {0}", message.positionX);
			//Log.DebugFormat("py:  {0}", message.positionY);
			//Log.DebugFormat("pz:  {0}", message.positionZ);

			if (message.face <= 5)
			{
				// Right click

				var mcpeAnimate = McpeAnimate.CreateObject();
				mcpeAnimate.actionId = 1;
				mcpeAnimate.entityId = EntityId;
				Level.RelayBroadcast(this, mcpeAnimate);

				Vector3 faceCoords = new Vector3(message.fx, message.fy, message.fz);
				//if (Inventory.GetItemInHand().Id != message.item.Value.Id)
				//{
				//	//Disconnect("Inventory hacks not allowed.");
				//	return;
				//}

				Level.Interact(Level, this, message.item.Value.Id, new BlockCoordinates(message.x, message.y, message.z), message.item.Value.Metadata, (BlockFace) message.face, faceCoords);
			}
			else
			{
				// Snowballs and shit

				_itemUseTimer = new Stopwatch();
				_itemUseTimer.Start();

				Level.Interact(Level, this, message.item.Value.Id, new BlockCoordinates(message.x, message.y, message.z), message.item.Value.Metadata);

				// Player in action
				MetadataDictionary metadata = new MetadataDictionary();
				metadata[0] = new MetadataByte(16);

				var setEntityData = McpeSetEntityData.CreateObject();
				setEntityData.entityId = EntityId;
				setEntityData.metadata = metadata;
				Level.RelayBroadcast(this, setEntityData);
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

		private void ForcedSendChunks()
		{
			if (!Monitor.TryEnter(_sendChunkSync)) return;
			try
			{
				var chunkPosition = new ChunkCoordinates(KnownPosition);

				_currentChunkPosition = chunkPosition;

				if (Level == null) return;

				foreach (McpeBatch chunk in Level.GenerateChunks(_currentChunkPosition, _chunksUsed))
				{
					if (chunk == null) continue;

					SendPackage(chunk, true);
				}
			}
			finally
			{
				Monitor.Exit(_sendChunkSync);
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
					return;
				}

				_currentChunkPosition = chunkPosition;

				int packetCount = 0;

				if (Level == null) return;

				foreach (McpeBatch chunk in Level.GenerateChunks(_currentChunkPosition, _chunksUsed))
				{
					if (chunk != null) SendPackage(chunk);

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

			var attributes = new PlayerAttributes();
			attributes["generic.health"] = new PlayerAttribute
			{
				Name = "generic.health", MinValue = 0, MaxValue = 20, Value = HealthManager.Hearts
			};
			attributes["player.hunger"] = new PlayerAttribute
			{
				Name = "player.hunger", MinValue = 0, MaxValue = 20, Value = 15
			};
			attributes["player.level"] = new PlayerAttribute
			{
				Name = "player.level", MinValue = 0, MaxValue = 24791, Value = 0
			};
			attributes["player.experience"] = new PlayerAttribute
			{
				Name = "player.experience", MinValue = 0, MaxValue = 1, Value = 0
			};

			McpeUpdateAttributes attributesPackate = McpeUpdateAttributes.CreateObject();
			attributesPackate.entityId = 0;
			attributesPackate.attributes = attributes;
			SendPackage(attributesPackate);
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
			metadata[0] = new MetadataByte((byte) (HealthManager.IsOnFire ? 1 : 0));
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
			Server.SendPackage(this, package, _mtuSize);
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
						Server.SendPackage(this, package, _mtuSize);
					}
					else if (!IsSpawned)
					{
						SendBuffered(messageCount);
						messageCount = 0;
						Server.SendPackage(this, package, _mtuSize);
					}
					else if (package.NoBatch)
					{
						SendBuffered(messageCount);
						messageCount = 0;
						Server.SendPackage(this, package, _mtuSize);
					}
					else if (package is McpeBatch)
					{
						SendBuffered(messageCount);
						messageCount = 0;
						Server.SendPackage(this, package, _mtuSize);
						Thread.Sleep(1); // Really important to slow down speed a bit
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
			batch.Encode();

			memStream.Position = 0;
			memStream.SetLength(0);

			Server.SendPackage(this, batch, _mtuSize);
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
				//Server.SendPackage(this, batch, _mtuSize, ref _reliableMessageNumber);
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

			foreach (var stack in slots.ToArray())
			{
				Level.DropItem(KnownPosition.GetCoordinates3D(), stack);
			}

			if (Inventory.Helmet.Id != 0)
			{
				Level.DropItem(KnownPosition.GetCoordinates3D(), new ItemStack(Inventory.Helmet, 1));
				Inventory.Helmet = new Item(0, 0);
			}
			if (Inventory.Chest.Id != 0)
			{
				Level.DropItem(KnownPosition.GetCoordinates3D(), new ItemStack(Inventory.Chest, 1));
				Inventory.Chest = new Item(0, 0);
			}
			if (Inventory.Leggings.Id != 0)
			{
				Level.DropItem(KnownPosition.GetCoordinates3D(), new ItemStack(Inventory.Leggings, 1));
				Inventory.Leggings = new Item(0, 0);
			}
			if (Inventory.Boots.Id != 0)
			{
				Level.DropItem(KnownPosition.GetCoordinates3D(), new ItemStack(Inventory.Boots, 1));
				Inventory.Boots = new Item(0, 0);
			}

			Inventory.Clear();
		}
	}
}