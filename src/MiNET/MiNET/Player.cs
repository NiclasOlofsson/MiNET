using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using fNbt;
using Jose;
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
using Newtonsoft.Json.Linq;

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
		public bool IsXboxLiveVerified { get; private set; }

		public Skin Skin { get; set; }
		public bool Silent { get; set; }
		public bool HideNameTag { get; set; }
		public bool NoAi { get; set; }

		public float ExperienceLevel { get; set; } = 0f;
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

			else if (typeof (McpeClientMagic) == message.GetType())
			{
				// Start encrypotion
				HandleMcpeClientMagic((McpeClientMagic) message);
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

			else if (typeof (McpeRemoveEntity) == message.GetType())
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

			else if (typeof (McpeBlockEntityData) == message.GetType())
			{
				HandleEntityData((McpeBlockEntityData) message);
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

			else if (typeof (McpeMobEquipment) == message.GetType())
			{
				HandleMobEquipment((McpeMobEquipment) message);
			}

			else if (typeof (McpeMobArmorEquipment) == message.GetType())
			{
				HandlePlayerArmorEquipment((McpeMobArmorEquipment) message);
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

			else if (typeof (McpeItemFramDropItem) == message.GetType())
			{
				HandleMcpePlayerInput((McpePlayerInput) message);
			}

			else
			{
				Log.Error($"Unhandled package: {message.GetType().Name} 0x{message.Id:X2} for user: {Username}, IP {EndPoint.Address}");
				return;
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

		private void HandleMcpeClientMagic(McpeClientMagic message)
		{
			SendPlayerStatus(0);

			new Thread(Start).Start();
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
		protected virtual void HandleEntityData(McpeBlockEntityData message)
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

			//mcpeAdventureSettings.flags |= 0x01; // Immutable World (Remove hit markers client-side).

			mcpeAdventureSettings.flags |= 0x02; // No PvP (Remove hit markers client-side).
			mcpeAdventureSettings.flags |= 0x04; // No PvM (Remove hit markers client-side).
			mcpeAdventureSettings.flags |= 0x08; // No PvE (Remove hit markers client-side).

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

			if (!AllowFly)
			{
				SendStartGame();
			}
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
			package.NoBatch = true;
			package.ForceClear = true;
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
					Log.InfoFormat("Player {0} doing multiple logins", Username);
					return; // Already doing login
				}

				Username = string.Empty;
			}

			if (message.protocolVersion != 81)
			{
				Server.GreylistManager.Greylist(EndPoint.Address, 30000);
				Disconnect(string.Format("Wrong version ({0}) of Minecraft Pocket Edition, please upgrade.", message.protocolVersion));
				return;
			}

			// THIS counter exist to protect the level from being swamped with player list add
			// attempts during startup (normally).

			var serverInfo = Server.ServerInfo;
			Interlocked.Increment(ref serverInfo.ConnectionsInConnectPhase);

			DecodeCert(message);

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

			//if (message.username == null || message.username.Trim().Length == 0 || !Regex.IsMatch(message.username, "^[A-Za-z0-9_-]{3,56}$"))
			//{
			//	Disconnect("Invalid username.");
			//	return;
			//}

			//if (string.IsNullOrEmpty(message.skin.SkinType) || message.skin.Texture == null)
			//{
			//	Disconnect("Invalid skin. Please upgrade your version of Minecraft Pocket Edition");
			//	return;
			//}

			//SendPlayerStatus(0); // Hmm, login success?

			//Username = message.username;
			//ClientId = message.clientId;
			//ClientUuid = message.clientUuid;
			//ClientSecret = message.clientSecret;
			//Skin = message.skin;

			////string fileName = Path.GetTempPath() + "Skin_" + Skin.SkinType + ".png";
			////Log.Info($"Writing skin to filename: {fileName}");
			////Skin.SaveTextureToFile(fileName, Skin.Texture);

			//if (ClientSecret != null)
			//{
			//	var count = serverInfo.PlayerSessions.Values.Count(session => session.Player != null && ClientSecret.Equals(session.Player.ClientSecret));
			//	if (count != 1)
			//	{
			//		Disconnect($"Invalid skin {count}.");
			//		return;
			//	}
			//}

			//new Thread(Start).Start();
		}

		protected virtual void DecodeCert(McpeLogin message)
		{
			// Get bytes
			byte[] buffer = message.payload;

			if (message.payloadLenght != buffer.Length) throw new Exception($"Wrong lenght {message.payloadLenght} != {message.payload.Length}");
			// Decompress bytes

			Log.Debug("Lenght" + message.payloadLenght + "Message: " + Convert.ToBase64String(buffer));

			MemoryStream stream = new MemoryStream(buffer);
			if (stream.ReadByte() != 0x78)
			{
				throw new InvalidDataException("Incorrect ZLib header. Expected 0x78 0x9C");
			}
			stream.ReadByte();

			string certificateChain;
			string skinData;

			using (var defStream2 = new DeflateStream(stream, CompressionMode.Decompress, false))
			{
				// Get actual package out of bytes
				MemoryStream destination = MiNetServer.MemoryStreamManager.GetStream();
				defStream2.CopyTo(destination);
				destination.Position = 0;
				NbtBinaryReader reader = new NbtBinaryReader(destination, true);

				certificateChain = Encoding.UTF8.GetString(reader.ReadBytes(Endian.SwapInt32(reader.ReadInt32())));
				skinData = Encoding.UTF8.GetString(reader.ReadBytes(Endian.SwapInt32(reader.ReadInt32())));

				//if (Log.IsDebugEnabled)
				//	Log.Debug($"\n{Package.HexDump(destination.ToArray())}");
			}


			try
			{
				bool isXboxLogin = false;

				{
					Log.Debug("Input JSON string: " + certificateChain);

					dynamic json = JObject.Parse(certificateChain);

					//Log.Debug($"Raw: {certificateChain}");
					Log.Debug($"JSON:\n{json}");

					bool haveValidRealmsToken = false;
					string validKey = null;
					if (json.chain.Count > 1)
					{
						// Xbox Login
						validKey = "MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAE8ELkixyLcwlZryUQcu1TvPOmI2B7vX83ndnWRUaXm74wFfa5f/lwQNTfrLVHa2PmenpGI6JhIMUJaWZrjmMj90NoKNFSNBuKdm8rYiXsfaz3K36x/1U26HpG0ZxK/V1V";
						isXboxLogin = true;
					}

					foreach (dynamic o in json.chain)
					{
						Log.Debug("Raw chain element:\n" + o.ToString());
						IDictionary<string, dynamic> headers = JWT.Headers(o.ToString());
						Log.Debug($"JWT Header: {string.Join(";", headers)}");

						dynamic jsonPayload = JObject.Parse(JWT.Payload(o.ToString()));
						Log.Debug($"JWT Payload:\n{jsonPayload}");

						// x5u cert (string): MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAE8ELkixyLcwlZryUQcu1TvPOmI2B7vX83ndnWRUaXm74wFfa5f/lwQNTfrLVHa2PmenpGI6JhIMUJaWZrjmMj90NoKNFSNBuKdm8rYiXsfaz3K36x/1U26HpG0ZxK/V1V

						//
						// XBOX login
						//

						//{
						//	"nbf": 1465304604,
						//	"randomNonce": 2876920962471578546,
						//	"iss": "RealmsAuthorization",
						//	"exp": 1465391064,
						//	"iat": 1465304664,
						//	"certificateAuthority": true,
						//	"identityPublicKey": "MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAEr935ZYD18b9p1mgmwoMTWmBhJ/eTmqX9CmcZb1wsVZg20za1JRGro9kcHxJo5VW11HbJev3T+a0/WxpoLKxN9dwDl+USHuzlzWcMdzHdJLymiLQScJJ522DykllRM4Pe"
						//}
						//{
						//	"nbf": 1466694143,
						//	"extraData": {
						//		"identity": "6cdfec82-45b6-3322-9111-084cd74e32f0",
						//		"displayName": "gurunx",
						//		"XUID": "2535410512372218"
						//	},
						//	"randomNonce": -453593381138004104,
						//	"iss": "RealmsAuthorization",
						//	"exp": 1466780603,
						//	"iat": 1466694203,
						//	"identityPublicKey": "MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAECj+h2Z1+bnF1vnfkRJ9GFJhZrORvImXo7j4YozPjIIKuVXPlKsvAB5JXSzYpVG3gCXVprEw02a2SumqqGPTwJLce2YSVmuyQsD65jjXFIJUGlKYcb/kLRlpwO1uw5/t6"
						//}


						//
						// No XBOX login
						//

						//{
						//	"exp": 1464983845,
						//	"extraData": {
						//		"displayName": "gurunx",
						//		"identity": "af6f7c5e-fcea-3e43-bf3a-e005e400e578"
						//	},
						//	"identityPublicKey": "MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAE7nnZpCfxmCrSwDdBv7eBXXMtKhroxOriEr3hmMOJAuw/ZpQXj1K5GGtHS4CpFNttd1JYAKYoJxYgaykpie0EyAv3qiK6utIH2qnOAt3VNrQYXfIZJS/VRe3Il8Pgu9CB",
						//	"nbf": 1464983844
						//}


						if (headers.ContainsKey("x5u"))
						{
							string certString = headers["x5u"];
							Log.Debug($"x5u cert (string): {certString}");

							if (validKey == null) validKey = certString;

							if (validKey.Equals(certString, StringComparison.InvariantCultureIgnoreCase))
							{
								ECDiffieHellmanPublicKey publicKey = CryptoUtils.CreateEcDiffieHellmanPublicKey(certString);
								Log.Debug($"Cert:\n{publicKey.ToXmlString()}");

								// Validate
								var newKey = CryptoUtils.ImportECDsaCngKeyFromString(certString);
								string decoded = JWT.Decode(o.ToString(), newKey);
								if (decoded != null)
								{
									Log.Info("Decoded token success");
									dynamic content = JObject.Parse(decoded);
									validKey = content.identityPublicKey;
								}
								else
								{
									Log.Error("Not a valid Identity Public Key for decoding");
								}
							}
						}
					}

					if (isXboxLogin) IsXboxLiveVerified = true;
					if (Log.IsDebugEnabled) Log.Warn("Is XBOX: " + IsXboxLiveVerified);

					foreach (dynamic o in json.chain)
					{
						Log.Debug("Raw chain element:\n" + o.ToString());
						IDictionary<string, dynamic> headers = JWT.Headers(o.ToString());
						Log.Debug($"JWT Header: {string.Join(";", headers)}");

						dynamic jsonPayload = JObject.Parse(JWT.Payload(o.ToString()));
						Log.Debug($"JWT Payload:\n{jsonPayload}");

						string ident = jsonPayload.identityPublicKey;

						if (!validKey.Equals(ident, StringComparison.InvariantCultureIgnoreCase))
						{
							Log.Warn("Found no valid key");
							continue;
						}

						Username = jsonPayload.extraData.displayName;
						string identity = jsonPayload.extraData.identity;
						Log.Debug($"Connecting user {Username} with identity={identity}");
						ClientUuid = new UUID(new Guid(identity));

						if (Username == "gurun")
						{
							Username = "TheGrey" + new Random().Next();
							ClientUuid = new UUID(Guid.NewGuid());
						}

						{
							string certString = validKey;

							ECDiffieHellmanPublicKey publicKey = CryptoUtils.CreateEcDiffieHellmanPublicKey(certString);
							Log.Debug($"Cert:\n{publicKey.ToXmlString()}");

							// Create shared shared secret
							ECDiffieHellmanCng ecKey = new ECDiffieHellmanCng(384);
							ecKey.HashAlgorithm = CngAlgorithm.Sha256;
							ecKey.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
							ecKey.SecretPrepend = Encoding.UTF8.GetBytes("RANDOM SECRET"); // Server token

							byte[] secret = ecKey.DeriveKeyMaterial(publicKey);

							Log.Debug($"SECRET KEY (b64):\n{Convert.ToBase64String(secret)}");

							{
								RijndaelManaged rijAlg = new RijndaelManaged
								{
									BlockSize = 128,
									Padding = PaddingMode.None,
									Mode = CipherMode.CFB,
									FeedbackSize = 8,
									Key = secret,
									IV = secret.Take(16).ToArray(),
								};

								// Create a decrytor to perform the stream transform.
								ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
								MemoryStream inputStream = new MemoryStream();
								CryptoStream cryptoStreamIn = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read);

								ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
								MemoryStream outputStream = new MemoryStream();
								CryptoStream cryptoStreamOut = new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write);

								NetworkSession.CryptoContext = new CryptoContext
								{
									Algorithm = rijAlg,
									Decryptor = decryptor,
									Encryptor = encryptor,
									InputStream = inputStream,
									OutputStream = outputStream,
									CryptoStreamIn = cryptoStreamIn,
									CryptoStreamOut = cryptoStreamOut,
								};
							}

							var response = McpeServerExchange.CreateObject();
							response.ForceClear = true;
							response.serverPublicKey = Convert.ToBase64String(ecKey.PublicKey.GetDerEncoded());
							response.randomKeyToken = Encoding.UTF8.GetString(ecKey.SecretPrepend);

							if (Config.GetProperty("UseEncryption", true))
							{
								SendPackage(response);
							}
						}
					}
				}

				{
					Log.Debug("Input SKIN string: " + skinData);

					IDictionary<string, dynamic> headers = JWT.Headers(skinData);
					dynamic payload = JObject.Parse(JWT.Payload(skinData));

					Log.Debug($"Skin JWT Header: {string.Join(";", headers)}");
					Log.Debug($"Skin JWT Payload:\n{payload.ToString()}");

					// Skin JWT Payload: 
					//{
					// "ClientRandomId": -1256727416,
					// "ServerAddress": "yodamine.com:19132",
					// "SkinData": "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Af/3/wH/9/8B//f/Af/3/wAAAP/yrmj/8q5o//KuaP/yrmj/8q5o//KuaP/yrmj/8q5o/wMCAv8B//f/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD//wAA/wAAAP8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAgD/AQEB/wEBAf8BAQH//fn2//359v/9+fb//fn2/wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABQUF/wkICP8JCQn/EhIS//KuaP/yrmj/AAAA/wkJCf8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA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",
					// "SkinId": "Standard_Custom"
					//}

					ClientId = payload.ClientRandomId;

					Skin = new Skin()
					{
						SkinType = payload.SkinId,
						Texture = Convert.FromBase64String((string) payload.SkinData),
					};
				}

				if (!Config.GetProperty("UseEncryption", true))
				{
					SendPlayerStatus(0);

					new Thread(Start).Start();
				}
			}
			catch (Exception e)
			{
				Log.Error("Decrypt", e);
			}
		}

		private bool HaveProperty(dynamic obj, string property)
		{
			Type type = obj.GetType();
			return type.GetMember(property, BindingFlags.Public | BindingFlags.Instance).Length != 0;
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

		public virtual void Teleport(PlayerLocation newPosition)
		{
			bool oldNoAi = NoAi;
			SetNoAi(true);

			if (!IsChunkInCache(newPosition))
			{
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

				ForcedSendChunk(newPosition);
			}

			// send teleport to spawn
			SetPosition(newPosition);

			SetNoAi(oldNoAi);

			ThreadPool.QueueUserWorkItem(delegate { SendChunksForKnownPosition(); });
		}

		private bool IsChunkInCache(PlayerLocation position)
		{
			var chunkPosition = new ChunkCoordinates(position);

			var key = new Tuple<int, int>(chunkPosition.X, chunkPosition.Z);
			return _chunksUsed.ContainsKey(key);
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

			if (text == null) return;

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
			HungerManager.Move(Vector3.Distance(new Vector3(KnownPosition.X, 0, KnownPosition.Z), new Vector3(message.x, 0, message.z)));

			Vector3 origin = KnownPosition.ToVector3();
			double distanceTo = Vector3.Distance(origin, new Vector3(message.x, message.y, message.z));
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
			if (_currentChunkPosition != chunkPosition && _currentChunkPosition.DistanceTo(chunkPosition) >= Config.GetProperty("MoveRenderDistance", 1))
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

		protected virtual void HandlePlayerArmorEquipment(McpeMobArmorEquipment message)
		{
		}

		protected virtual void HandleMcpeItemFramDropItem(McpeItemFramDropItem message)
		{
			Item droppedItem = message.item;
			Log.Warn($"Player {Username} drops item frame {droppedItem} at {message.x}, {message.y}, {message.z}");
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
					Velocity = KnownPosition.GetDirection()*0.7f,
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

		protected virtual void HandleMobEquipment(McpeMobEquipment message)
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
					var tileEvent = McpeBlockEvent.CreateObject();
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

						McpeMobArmorEquipment armorEquipment = McpeMobArmorEquipment.CreateObject();
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
					var tileEvent = McpeBlockEvent.CreateObject();
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

		protected virtual double CalculateDamageIncreaseFromEnchantments(Item tool)
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


		public virtual double CalculatePlayerDamage(Player target, double damage)
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

		protected virtual double CalculateDamageReducationFromEnchantments(Item armor)
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

		protected virtual int CalculateDamage(Entity target)
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
			mcpeStartGame.isLoadedInCreative = true /*GameMode == GameMode.Creative*/;
			mcpeStartGame.dayCycleStopTime = 1;
			mcpeStartGame.eduMode = false;
			mcpeStartGame.unknownstr = "iX8AANxLbgA=";
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
					SendPackage(chunk);
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
					if (chunk != null) SendPackage(chunk);

					//if (packetCount > 16) Thread.Sleep(12);

					packetCount++;
				}
			}
			finally
			{
				Monitor.Exit(_sendChunkSync);
			}

			if (postAction != null)
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

				if (IsSpawned && _currentChunkPosition.DistanceTo(chunkPosition) < Config.GetProperty("MoveRenderDistance", 1))
				{
					return;
				}

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
				Name = "player.level", MinValue = 0, MaxValue = 24791, Value = ExperienceLevel
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
			metadata[23] = new MetadataLong(-1); // Leads EID (target or holder?)
			metadata[24] = new MetadataByte(0); // Leads on/off

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
			Level.BroadcastMessage(text, type, sender, new[] {this});
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
		public void SendPackage(Package package)
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
			batch.Encode();

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
			McpeMobEquipment mcpePlayerEquipment = McpeMobEquipment.CreateObject();
			mcpePlayerEquipment.entityId = EntityId;
			mcpePlayerEquipment.item = Inventory.GetItemInHand();
			mcpePlayerEquipment.slot = 0;
			Level.RelayBroadcast(this, receivers, mcpePlayerEquipment);
		}

		public virtual void SendArmorForPlayer(Player[] receivers)
		{
			McpeMobArmorEquipment mcpePlayerArmorEquipment = McpeMobArmorEquipment.CreateObject();
			mcpePlayerArmorEquipment.entityId = EntityId;
			mcpePlayerArmorEquipment.helmet = Inventory.Helmet;
			mcpePlayerArmorEquipment.chestplate = Inventory.Chest;
			mcpePlayerArmorEquipment.leggings = Inventory.Leggings;
			mcpePlayerArmorEquipment.boots = Inventory.Boots;
			Level.RelayBroadcast(this, receivers, mcpePlayerArmorEquipment);
		}

		public override void DespawnFromPlayers(Player[] players)
		{
			McpeRemoveEntity mcpeRemovePlayer = McpeRemoveEntity.CreateObject();
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