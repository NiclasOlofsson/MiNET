using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Threading;
using log4net;
using Microsoft.AspNet.Identity;
using MiNET.Blocks;
using MiNET.Crafting;
using MiNET.Effects;
using MiNET.Entities;
using MiNET.Entities.World;
using MiNET.Items;
using MiNET.Net;
using MiNET.Particles;
using MiNET.Security;
using MiNET.Utils;
using MiNET.Worlds;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MiNET
{
	public class Player : Entity, IMcpeMessageHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Player));

		private MiNetServer Server { get; set; }
		public IPEndPoint EndPoint { get; private set; }
		public INetworkHandler NetworkHandler { get; set; }

		private Dictionary<Tuple<int, int>, McpeBatch> _chunksUsed = new Dictionary<Tuple<int, int>, McpeBatch>();
		private ChunkCoordinates _currentChunkPosition;

		private Inventory _openInventory;
		public PlayerInventory Inventory { get; private set; }

		public PlayerLocation SpawnPosition { get; set; }

		public int MaxViewDistance { get; set; } = 22;
		public int MoveRenderDistance { get; set; } = 1;

		public GameMode GameMode { get; set; }
		public bool UseCreativeInventory { get; set; } = true;
		public bool IsConnected { get; set; }
		public CertificateData CertificateData { get; set; }
		public string Username { get; set; }
		public string DisplayName { get; set; }
		public long ClientId { get; set; }
		public UUID ClientUuid { get; set; }
		public string ServerAddress { get; set; }

		public Skin Skin { get; set; }

		public float ExperienceLevel { get; set; } = 0f;
		public float Experience { get; set; } = 0f;
		public float MovementSpeed { get; set; } = 0.1f;
		public ConcurrentDictionary<EffectType, Effect> Effects { get; set; } = new ConcurrentDictionary<EffectType, Effect>();

		public HungerManager HungerManager { get; set; }

		public bool IsOnGround { get; set; }
		public bool IsFalling { get; set; }
		public bool IsFlyingHorizontally { get; set; }

		public List<Popup> Popups { get; set; } = new List<Popup>();

		public User User { get; set; }
		public Session Session { get; set; }

		public DamageCalculator DamageCalculator { get; set; } = new DamageCalculator();

		public Player(MiNetServer server, IPEndPoint endPoint) : base(-1, null)
		{
			Server = server;
			EndPoint = endPoint;

			Inventory = new PlayerInventory(this);
			HungerManager = new HungerManager(this);

			IsSpawned = false;
			IsConnected = endPoint != null; // Can't connect if there is no endpoint

			Width = 0.6f;
			Length = Width;
			Height = 1.80;

			HideNameTag = false;
			IsAlwaysShowName = true;
		}

		public void HandleMcpeClientMagic(McpeClientMagic message)
		{
			// Beware that message might be null here.

			var serverInfo = Server.ServerInfo;
			Interlocked.Increment(ref serverInfo.ConnectionsInConnectPhase);

			SendPlayerStatus(0);

			{
				SendResourcePacksInfo();
			}

			//MiNetServer.FastThreadPool.QueueUserWorkItem(() => { Start(null); });
		}

		public virtual void HandleMcpeResourcePackChunkRequest(McpeResourcePackChunkRequest message)
		{
			var jsonSerializerSettings = new JsonSerializerSettings
			{
				PreserveReferencesHandling = PreserveReferencesHandling.None,
				Formatting = Formatting.Indented,
			};

			string result = JsonConvert.SerializeObject(message, jsonSerializerSettings);
			Log.Debug($"{message.GetType().Name}\n{result}");

			var content = File.ReadAllBytes(@"D:\Temp\ResourcePackChunkData_8f760cf7-2ca4-44ab-ab60-9be2469b9777.zip");
			McpeResourcePackChunkData chunkData = McpeResourcePackChunkData.CreateObject();
			chunkData.packageId = "5abdb963-4f3f-4d97-8482-88e2049ab149";
			chunkData.unknown1 = 0; // Package index ?
			chunkData.unknown3 = 0; // Long, maybe timestamp?
			chunkData.length = (uint) content.Length;
			chunkData.payload = content;
			SendPackage(chunkData);
		}

		private bool _serverHaveResources = false;

		public virtual void HandleMcpeResourcePackClientResponse(McpeResourcePackClientResponse message)
		{
			if (Log.IsDebugEnabled) Log.Debug($"Handled package 0x{message.Id:X2}\n{Package.HexDump(message.Bytes)}");

			if (message.responseStatus == 2)
			{
				McpeResourcePackDataInfo dataInfo = McpeResourcePackDataInfo.CreateObject();
				dataInfo.packageId = "5abdb963-4f3f-4d97-8482-88e2049ab149";
				dataInfo.unknown1 = 1048576;
				dataInfo.unknown2 = 1;
				dataInfo.unknown3 = 359901; // Lenght of data
				dataInfo.unknown4 = "9&\r2'ëX•;\u001bð—Ð‹\u0006´6\u0007TÞ/[Üx…x*\u0005h\u0002à\u0012"; //TODO: Fix encoding for this. Right now, must be Default :-(
				SendPackage(dataInfo);
				return;
			}
			else if (message.responseStatus == 3)
			{
				//if (_serverHaveResources)
				{
					SendResourcePackStack();
				}
				//else
				//{
				//	MiNetServer.FastThreadPool.QueueUserWorkItem(() => { Start(null); });
				//}
				return;
			}
			else if (message.responseStatus == 4)
			{
				//if (_serverHaveResources)
				{
					MiNetServer.FastThreadPool.QueueUserWorkItem(() => { Start(null); });
				}
				return;
			}
		}

		private void SendResourcePacksInfo()
		{
			McpeResourcePacksInfo packInfo = McpeResourcePacksInfo.CreateObject();
			if (_serverHaveResources)
			{
				packInfo.mustAccept = false;
				packInfo.resourcepackinfos = new ResourcePackInfos
				{
					new ResourcePackInfo() {PackIdVersion = new PackIdVersion() {Id = "5abdb963-4f3f-4d97-8482-88e2049ab149", Version = "0.0.1"}, Unknown = 359901},
				};
			}
			SendPackage(packInfo);
		}

		private void SendResourcePackStack()
		{
			McpeResourcePackStack packStack = McpeResourcePackStack.CreateObject();
			if (_serverHaveResources)
			{
				packStack.mustAccept = false;
				packStack.resourcepackidversions = new ResourcePackIdVersions
				{
					new PackIdVersion() {Id = "5abdb963-4f3f-4d97-8482-88e2049ab149", Version = "0.0.1"},
				};
			}
			SendPackage(packStack);
		}

		public virtual void HandleMcpePlayerInput(McpePlayerInput message)
		{
			Log.Debug($"Player input: Motion X={message.motionX}, Motion Z={message.motionZ}, Flags=0x{message.motionX:X2}");
		}

		private object _mapInfoSync = new object();

		private Timer _mapSender;
		private ConcurrentQueue<McpeBatch> _mapBatches = new ConcurrentQueue<McpeBatch>();

		public virtual void HandleMcpeMapInfoRequest(McpeMapInfoRequest message)
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

		public virtual void SendMapInfo(MapInfo mapInfo)
		{
			McpeClientboundMapItemData packet = McpeClientboundMapItemData.CreateObject();
			packet.mapinfo = mapInfo;
			SendPackage(packet);
		}

		public int ChunkRadius { get; private set; } = -1;

		public virtual void HandleMcpeRequestChunkRadius(McpeRequestChunkRadius message)
		{
			Log.Debug($"Requested chunk radius of: {message.chunkRadius}");

			ChunkRadius = Math.Max(5, Math.Min(message.chunkRadius, MaxViewDistance));

			SendChunkRadiusUpdate();

			//if (_completedStartSequence)
			{
				MiNetServer.FastThreadPool.QueueUserWorkItem(SendChunksForKnownPosition);
			}
		}


		public virtual void HandleMcpePlayerFall(McpePlayerFall message)
		{
			double damage = message.fallDistance - 3;
			if (damage > 0)
			{
				HealthManager.TakeHit(null, (int) DamageCalculator.CalculatePlayerDamage(null, this, null, damage, DamageCause.Fall), DamageCause.Fall);
			}
		}

		/// <summary>
		///     Handles an animate packet.
		/// </summary>
		/// <param name="message">The message.</param>
		public virtual void HandleMcpeAnimate(McpeAnimate message)
		{
			if (Level == null) return;

			var itemInHand = Inventory.GetItemInHand();
			if (itemInHand != null)
			{
				bool isHandled = itemInHand.Animate(Level, this);
				if (isHandled) return; // Handled, return
			}

			McpeAnimate msg = McpeAnimate.CreateObject();
			msg.entityId = EntityId;
			msg.actionId = message.actionId;

			Level.RelayBroadcast(this, msg);
		}

		/// <summary>
		///     Handles the player action.
		/// </summary>
		/// <param name="message">The message.</param>
		public virtual void HandleMcpePlayerAction(McpePlayerAction message)
		{
			switch ((PlayerAction) message.actionId)
			{
				case PlayerAction.StartBreak:
				case PlayerAction.AbortBreak:
				case PlayerAction.StopBreak:
					break;
				case PlayerAction.ReleaseItem:
					if (_itemUseTimer <= 0) return;

					Item itemInHand = Inventory.GetItemInHand();

					if (itemInHand == null) return; // Cheat(?)

					itemInHand.Release(Level, this, new BlockCoordinates(message.coordinates.X, message.coordinates.Y, message.coordinates.Z), Level.TickTime - _itemUseTimer);

					_itemUseTimer = 0;

					break;
				case PlayerAction.StopSleeping:
					break;
				case PlayerAction.Respawn:
					MiNetServer.FastThreadPool.QueueUserWorkItem(HandleMcpeRespawn);
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
				case PlayerAction.WorldImmutable:
					break;
				case PlayerAction.StartGlide:
					IsGliding = true;
					Height = 0.6;

					var particle = new WhiteSmokeParticle(Level);
					particle.Position = KnownPosition.ToVector3();
					particle.Spawn();

					break;
				case PlayerAction.StopGlide:
					IsGliding = false;
					Height = 1.8;
					break;
				default:
					Log.Warn($"Unhandled action ID={message.actionId}");
					throw new ArgumentOutOfRangeException(nameof(message.actionId));
			}

			IsUsingItem = false;

			BroadcastSetEntityData();

			//MetadataDictionary metadata = new MetadataDictionary
			//{
			//	[0] = GetDataValue()
			//};

			//var setEntityData = McpeSetEntityData.CreateObject();
			//setEntityData.entityId = EntityId;
			//setEntityData.metadata = metadata;
			//Level?.RelayBroadcast(this, setEntityData);
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
				}

				SendUpdateAttributes();
			}
		}

		/// <summary>
		///     Handles the entity data.
		/// </summary>
		/// <param name="message">The message.</param>
		public virtual void HandleMcpeBlockEntityData(McpeBlockEntityData message)
		{
			Log.DebugFormat("x:  {0}", message.coordinates.X);
			Log.DebugFormat("y:  {0}", message.coordinates.Y);
			Log.DebugFormat("z:  {0}", message.coordinates.Z);
			Log.DebugFormat("NBT {0}", message.namedtag.NbtFile);

			var blockEntity = Level.GetBlockEntity(message.coordinates);

			if (blockEntity == null) return;

			blockEntity.SetCompound(message.namedtag.NbtFile.RootTag);
			Level.SetBlockEntity(blockEntity);
		}


		public bool IsWorldImmutable { get; set; }
		public bool IsNoPvp { get; set; }
		public bool IsNoPvm { get; set; }
		public bool IsNoMvp { get; set; }
		public bool IsNoClip { get; set; }

		public virtual void SendAdventureSettings()
		{
			McpeAdventureSettings mcpeAdventureSettings = McpeAdventureSettings.CreateObject();

			uint flags = 0;

			if (IsWorldImmutable || IsAdventure || GameMode == GameMode.Adventure) flags |= 0x01; // Immutable World (Remove hit markers client-side).
			if (IsNoPvp || IsSpectator || GameMode == GameMode.Spectator) flags |= 0x02; // No PvP (Remove hit markers client-side).
			if (IsNoPvm || IsSpectator || GameMode == GameMode.Spectator) flags |= 0x04; // No PvM (Remove hit markers client-side).
			if (IsNoMvp || IsSpectator || GameMode == GameMode.Spectator) flags |= 0x08;

			if (IsAutoJump) flags |= 0x20;

			if (AllowFly || GameMode == GameMode.Creative) flags |= 0x40;

			if (IsNoClip || IsSpectator || GameMode == GameMode.Spectator) flags |= 0x80; // No clip

			mcpeAdventureSettings.flags = flags;
			mcpeAdventureSettings.userPermission = (uint) PermissionLevel;

			SendPackage(mcpeAdventureSettings);
		}

		public UserPermission PermissionLevel { get; set; } = UserPermission.Any;

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
			SendAdventureSettings();
		}

		private object _loginSyncLock = new object();

		public virtual void HandleMcpeLogin(McpeLogin message)
		{
			// Do nothing
		}

		private bool _completedStartSequence = false;

		public void Start(object o)
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

				Level.EntityManager.AddEntity(this);

				GameMode = Level.GameMode;

				//
				// Start game - spawn sequence starts here
				//

				// Vanilla 1st player list here

				//Level.AddPlayer(this, false);

				SendSetTime();

				SendStartGame();

				if (ChunkRadius == -1) ChunkRadius = 5;

				SendChunkRadiusUpdate();

				//SendSetSpawnPosition();

				SendSetTime();

				SendSetDificulty();


				SendSetCommandsEnabled();

				SendAdventureSettings();

				// Send McpeGameRulesChanged

				//McpeContainerSetContent Window ID: 0x7b lenght 0

				// Vanilla 2nd player list here

				Level.AddPlayer(this, false);

				SendAvailableCommands();

				SendUpdateAttributes();

				SendPlayerInventory();

				SendCreativeInventory();

				{
					McpeCraftingData craftingData = McpeCraftingData.CreateObject();
					craftingData.recipes = RecipeManager.Recipes;
					SendPackage(craftingData);
				}

				// More Send Attributes here (have to check)

				BroadcastSetEntityData();
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
			finally
			{
				Interlocked.Decrement(ref serverInfo.ConnectionsInConnectPhase);
			}

			LastUpdatedTime = DateTime.UtcNow;

			_completedStartSequence = true;

			//ForcedSendChunk(KnownPosition);
			//SendChunksForKnownPosition();
			//MiNetServer.FastThreadPool.QueueUserWorkItem(SendChunksForKnownPosition);

			LastUpdatedTime = DateTime.UtcNow;
			Log.InfoFormat("Login complete by: {0} from {2} in {1}ms", Username, watch.ElapsedMilliseconds, EndPoint);
		}

		public bool EnableCommands { get; set; } = Config.GetProperty("EnableCommands", false);

		protected virtual void SendSetCommandsEnabled()
		{
			McpeSetCommandsEnabled enabled = McpeSetCommandsEnabled.CreateObject();
			enabled.enabled = EnableCommands;
			SendPackage(enabled);
		}

		protected virtual void SendAvailableCommands()
		{
			var settings = new JsonSerializerSettings();
			settings.NullValueHandling = NullValueHandling.Ignore;
			settings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
			settings.MissingMemberHandling = MissingMemberHandling.Error;
			settings.Formatting = Formatting.Indented;
			settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			var content = JsonConvert.SerializeObject(Server.PluginManager.Commands, settings);

			McpeAvailableCommands commands = McpeAvailableCommands.CreateObject();
			commands.commands = content;
			commands.unknown = "{}";
			SendPackage(commands);
		}

		public virtual void HandleMcpeCommandStep(McpeCommandStep message)
		{
			var jsonSerializerSettings = new JsonSerializerSettings
			{
				PreserveReferencesHandling = PreserveReferencesHandling.None,
				Formatting = Formatting.Indented,
			};

			var commandJson = JsonConvert.DeserializeObject<dynamic>(message.commandInputJson);
			Log.Debug($"CommandJson\n{JsonConvert.SerializeObject(commandJson, jsonSerializerSettings)}");
			object result = Server.PluginManager.HandleCommand(this, message.commandName, message.commandOverload, commandJson);
			if (result != null)
			{
				var settings = new JsonSerializerSettings();
				settings.NullValueHandling = NullValueHandling.Ignore;
				settings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
				settings.MissingMemberHandling = MissingMemberHandling.Error;
				settings.Formatting = Formatting.Indented;
				settings.StringEscapeHandling = StringEscapeHandling.EscapeNonAscii;
				settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

				var content = JsonConvert.SerializeObject(result, settings);
				McpeCommandStep commandResult = McpeCommandStep.CreateObject();
				commandResult.commandName = message.commandName;
				commandResult.commandOverload = message.commandOverload;
				commandResult.isOutput = true;
				commandResult.commandInputJson = "null\n";
				commandResult.commandOutputJson = content;
				commandResult.entityId = EntityId;
				SendPackage(commandResult);

				Log.Debug($"Command Respone Json\n{content}");
			}
		}

		public virtual void InitializePlayer()
		{
			// Send set health

			SendPlayerStatus(3);

			//send time again
			SendSetTime();
			IsSpawned = true;

			SetPosition(SpawnPosition);

			LastUpdatedTime = DateTime.UtcNow;
			_haveJoined = true;

			OnPlayerJoin(new PlayerEventArgs(this));
		}

		public virtual void HandleMcpeRespawn()
		{
			HandleMcpeRespawn(null);
		}

		public virtual void HandleMcpeRespawn(McpeRespawn message)
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

			MiNetServer.FastThreadPool.QueueUserWorkItem(() => ForcedSendChunks());

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

			MiNetServer.FastThreadPool.QueueUserWorkItem(SendChunksForKnownPosition);
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

		public virtual void SpawnLevel(Level toLevel, PlayerLocation spawnPoint, bool useLoadingScreen = false, Func<Level> levelFunc = null)
		{
			bool oldNoAi = NoAi;
			SetNoAi(true);

			if (toLevel == null && levelFunc != null)
			{
				toLevel = levelFunc();
			}

			if (useLoadingScreen)
			{
				{
					McpeChangeDimension dimension = McpeChangeDimension.CreateObject();
					dimension.dimension = 0;
					SendPackage(dimension);

					if (toLevel == null && levelFunc != null)
					{
						toLevel = levelFunc();
					}

					McpePlayerStatus status = McpePlayerStatus.CreateObject();
					status.status = 3;
					SendPackage(status);
				}
				{
					McpeChangeDimension dimension = McpeChangeDimension.CreateObject();
					dimension.dimension = 1;
					SendPackage(dimension);

					McpePlayerStatus status = McpePlayerStatus.CreateObject();
					status.status = 3;
					SendPackage(status);
				}
			}

			// send teleport straight up, no chunk loading
			// +1000000 on X/Z axis is to unload the previously loaded chunks (https://bugs.mojang.com/browse/MCPE-19818)
			SetPosition(new PlayerLocation
			{
				X = KnownPosition.X + 1000000,
				Y = 4000,
				Z = KnownPosition.Z + 1000000,
				Yaw = 91,
				Pitch = 28,
				HeadYaw = 91,
			});

			if (useLoadingScreen)
			{
				ForcedSendEmptyChunks();

				{
					McpeChangeDimension dimension = McpeChangeDimension.CreateObject();
					dimension.dimension = 1;
					SendPackage(dimension);

					McpePlayerStatus status = McpePlayerStatus.CreateObject();
					status.status = 3;
					SendPackage(status);
				}
				{
					McpeChangeDimension dimension = McpeChangeDimension.CreateObject();
					dimension.dimension = 0;
					SendPackage(dimension);

					McpePlayerStatus status = McpePlayerStatus.CreateObject();
					status.status = 3;
					SendPackage(status);
				}
			}

			Level.RemovePlayer(this, true);
			//Level.EntityManager.RemoveEntity(null, this);

			Level = toLevel; // Change level
			SpawnPosition = spawnPoint ?? Level?.SpawnPoint;
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

			ForcedSendChunk(SpawnPosition);

			// send teleport to spawn
			SetPosition(SpawnPosition);

			SetNoAi(oldNoAi);

			MiNetServer.FastThreadPool.QueueUserWorkItem(delegate
			{
				Level.AddPlayer(this, true);

				ForcedSendChunks(() =>
				{
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
			mcpeSetDifficulty.difficulty = (uint) Level.Difficulty;
			SendPackage(mcpeSetDifficulty);
		}

		public virtual void SendPlayerInventory()
		{
			McpeContainerSetContent strangeContent = McpeContainerSetContent.CreateObject();
			strangeContent.windowId = (byte) 0x7b;
			strangeContent.slotData = new ItemStacks();
			strangeContent.hotbarData = new MetadataInts();
			SendPackage(strangeContent);

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

			McpeMobEquipment mobEquipment = McpeMobEquipment.CreateObject();
			mobEquipment.entityId = 0;
			mobEquipment.item = Inventory.GetItemInHand();
			mobEquipment.slot = 0;
			SendPackage(mobEquipment);
		}

		public virtual void SendCreativeInventory()
		{
			if (!UseCreativeInventory) return;

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
						NetworkHandler.SendDirectPackage(disconnect);
					}

					NetworkHandler.Close();
					NetworkHandler = null;

					IsConnected = false;
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
				if (!_haveJoined)
				{
					Log.WarnFormat("Disconnected crashed player {0}/{1} from level <{3}>, reason: {2}", Username, EndPoint.Address, reason, levelId);
				}
				else
				{
					Log.Warn(string.Format("Disconnected player {0}/{1} from level <{3}>, reason: {2}", Username, EndPoint.Address, reason, levelId));
				}

				CleanCache();
			}
		}

		private string _prevText = null;

		public virtual void HandleMcpeText(McpeText message)
		{
			string text = message.message;

			if (string.IsNullOrEmpty(text)) return;

			Level.BroadcastMessage(text, sender: this);
		}

		private int _lastPlayerMoveSequenceNUmber;
		private int _lastOrderingIndex;
		private object _moveSyncLock = new object();

		public virtual void HandleMcpeMovePlayer(McpeMovePlayer message)
		{
			if (!IsSpawned || HealthManager.IsDead) return;

			if (Server.ServerRole != ServerRole.Node)
			{
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
			}


			Vector3 origin = KnownPosition.ToVector3();
			double distanceTo = Vector3.Distance(origin, new Vector3(message.x, message.y - 1.62f, message.z));

			CurrentSpeed = distanceTo/((double) (DateTime.UtcNow - LastUpdatedTime).Ticks/TimeSpan.TicksPerSecond);

			double verticalMove = message.y - 1.62 - KnownPosition.Y;

			bool isOnGround = IsOnGround;
			bool isFlyingHorizontally = false;
			if (Math.Abs(distanceTo) > 0.01)
			{
				isOnGround = CheckOnGround(message);
				isFlyingHorizontally = DetectSimpleFly(message, isOnGround);
			}

			if (!AcceptPlayerMove(message, isOnGround, isFlyingHorizontally)) return;

			IsFlyingHorizontally = isFlyingHorizontally;
			IsOnGround = isOnGround;

			// Hunger management
			if(!IsGliding) HungerManager.Move(Vector3.Distance(new Vector3(KnownPosition.X, 0, KnownPosition.Z), new Vector3(message.x, 0, message.z)));

			KnownPosition = new PlayerLocation
			{
				X = message.x, Y = message.y - 1.62f, Z = message.z, Pitch = message.pitch, Yaw = message.yaw, HeadYaw = message.headYaw
			};

			IsFalling = verticalMove < 0 && !IsOnGround;

			LastUpdatedTime = DateTime.UtcNow;

			var chunkPosition = new ChunkCoordinates(KnownPosition);
			if (_currentChunkPosition != chunkPosition && _currentChunkPosition.DistanceTo(chunkPosition) >= MoveRenderDistance)
			{
				MiNetServer.FastThreadPool.QueueUserWorkItem(SendChunksForKnownPosition);
			}
		}

		public double CurrentSpeed { get; private set; } = 0;

		protected virtual bool AcceptPlayerMove(McpeMovePlayer message, bool isOnGround, bool isFlyingHorizontally)
		{
			return true;
		}

		protected virtual bool DetectSimpleFly(McpeMovePlayer message, bool isOnGround)
		{
			double d = Math.Abs(KnownPosition.Y - (message.y - 1.62f));
			return !(AllowFly || IsOnGround || isOnGround || d > 0.001);
		}

		private static readonly int[] Layers = {-1, 0};
		private static readonly int[] Arounds = {0, 1, -1};

		public bool CheckOnGround(McpeMovePlayer message)
		{
			if (Level == null)
				return true;

			BlockCoordinates pos = new Vector3(message.x, message.y - 1.62f, message.z);

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


		public virtual void HandleMcpeRemoveBlock(McpeRemoveBlock message)
		{
			Level.BreakBlock(this, message.coordinates);
		}

		public void HandleMcpeLevelSoundEvent(McpeLevelSoundEvent message)
		{
			McpeLevelSoundEvent sound = McpeLevelSoundEvent.CreateObject();
			sound.soundId = message.soundId;
			sound.position = message.position;
			sound.volume = message.volume;
			sound.pitch = message.pitch;
			sound.unknown1 = message.unknown1;
			sound.unknown2 = message.unknown2;
			Level.RelayBroadcast(sound);
		}

		public virtual void HandleMcpeMobArmorEquipment(McpeMobArmorEquipment message)
		{
		}

		public virtual void HandleMcpeItemFramDropItem(McpeItemFramDropItem message)
		{
			Item droppedItem = message.item;
			Log.Warn($"Player {Username} drops item frame {droppedItem} at {message.coordinates}");
		}

		public virtual void HandleMcpeDropItem(McpeDropItem message)
		{
			lock (Inventory)
			{
				Item droppedItem = message.item;
				if (Log.IsDebugEnabled) Log.Debug($"Player {Username} drops item {droppedItem} with inv slot {message.itemtype}");

				if (droppedItem.Count == 0) return; // 0.15 bug

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

		public virtual void HandleMcpeMobEquipment(McpeMobEquipment message)
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

				if (Log.IsDebugEnabled) Log.Debug($"Player {Username} called set equiptment with inv slot: {selectedInventorySlot}({message.slot}) and hotbar slot {message.selectedSlot} with item {message.item}");

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
					tileEvent.coordinates = inventoryCoord;
					tileEvent.case1 = 1;
					tileEvent.case2 = 2;
					Level.RelayBroadcast(tileEvent);
				}

				// subscribe to inventory changes
				inventory.InventoryChange += OnInventoryChange;
				inventory.AddObserver(this);

				// open inventory

				var containerOpen = McpeContainerOpen.CreateObject();
				containerOpen.windowId = inventory.WindowsId;
				containerOpen.type = inventory.Type;
				containerOpen.slotCount = inventory.Size;
				containerOpen.coordinates = inventoryCoord;
				containerOpen.unknownEntityId = 1;
				SendPackage(containerOpen);

				var containerSetContent = McpeContainerSetContent.CreateObject();
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


		public virtual void HandleMcpeCraftingEvent(McpeCraftingEvent message)
		{
			Log.Debug($"Player {Username} crafted item on window 0x{message.windowId:X2} on type: {message.recipeType}");
		}

		/// <summary>
		///     Handles the container set slot.
		/// </summary>
		/// <param name="message">The message.</param>
		public virtual void HandleMcpeContainerSetSlot(McpeContainerSetSlot message)
		{
			Log.Debug($"Handle slot unknown={message.hotbarslot}, unknown2={message.unknown2}");
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
					Log.Debug($"Player {Username} set inventory item on window 0x{message.windowId:X2} with slot: {message.slot} HOTBAR: {message.hotbarslot} and item: {itemStack}");

				if (_openInventory != null)
				{
					if (_openInventory.WindowsId == message.windowId)
					{
						if (_openInventory.Type == 3)
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
						Inventory.UpdateInventorySlot(message.slot, itemStack);
						//Inventory.Slots[message.slot] = itemStack;
						break;
					case 0x79:
						Inventory.UpdateInventorySlot(message.slot, itemStack);
						//Inventory.Slots[message.slot] = itemStack;
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

		public virtual void HandleMcpeContainerClose(McpeContainerClose message)
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
					tileEvent.coordinates = inventory.Coordinates;
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
		public virtual void HandleMcpeInteract(McpeInteract message)
		{
			Entity target = Level.GetEntity(message.targetEntityId);

			if (message.actionId != 4)
			{
				Log.DebugFormat("Interact Action ID: {0}", message.actionId);
				Log.DebugFormat("Interact Target Entity ID: {0}", message.targetEntityId);
			}

			if (target == null) return;
			switch (message.actionId)
			{
				case 1:
				{
					// Button pressed

					//McpeAnimate animate = McpeAnimate.CreateObject();
					//animate.entityId = target.EntityId;
					//animate.actionId = 4;
					//Level.RelayBroadcast(animate);

					DoInteraction(message.actionId, this);
					target.DoInteraction(message.actionId, this);
					break;
				}
				case 4:
				{
					// Mouse over
					//McpeAnimate animate = McpeAnimate.CreateObject();
					//animate.entityId = target.EntityId;
					//animate.actionId = 4;
					//Level.RelayBroadcast(animate);

					DoMouseOverInteraction(message.actionId, this);
					target.DoMouseOverInteraction(message.actionId, this);
					break;
				}
			}


			// Old code...
			if (message.actionId != 2) return;

			Item itemInHand = Inventory.GetItemInHand();

			Player player = target as Player;
			if (player != null)
			{
				double damage = DamageCalculator.CalculateItemDamage(this, itemInHand, player);

				if (IsFalling)
				{
					damage += DamageCalculator.CalculateFallDamage(this, damage, player);
				}

				damage += DamageCalculator.CalculateEffectDamage(this, damage, player);

				if (damage < 0) damage = 0;

				damage += DamageCalculator.CalculateDamageIncreaseFromEnchantments(this, itemInHand, player);

				player.HealthManager.TakeHit(this, itemInHand, (int) DamageCalculator.CalculatePlayerDamage(this, player, itemInHand, damage, DamageCause.EntityAttack), DamageCause.EntityAttack);
				var fireAspectLevel = itemInHand.GetEnchantingLevel(EnchantingType.FireAspect);
				if (fireAspectLevel > 0)
				{
					player.HealthManager.Ignite(fireAspectLevel*80);
				}
			}
			else
			{
				// This is totally wrong. Need to merge with the above damage calculation
				target.HealthManager.TakeHit(this, itemInHand, CalculateDamage(target), DamageCause.EntityAttack);
			}

			HungerManager.IncreaseExhaustion(0.3f);
		}

		protected virtual int CalculateDamage(Entity target)
		{
			int damage = Inventory.GetItemInHand().GetDamage(); //Item Damage.

			damage = (int) Math.Floor(damage*(1.0));

			return damage;
		}


		public virtual void HandleMcpeEntityEvent(McpeEntityEvent message)
		{
			Log.Debug("Entity Id:" + message.entityId);
			Log.Debug("Entity Event:" + message.eventId);
			Log.Debug("Entity Event:" + message.unknown);

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
				case 34:
					RemoveExperienceLevels(message.unknown);
					break;
			}
		}

		private long _itemUseTimer;

		public virtual void HandleMcpeUseItem(McpeUseItem message)
		{
			if (message.item == null)
			{
				Log.Warn($"{Username} sent us a use item message with no item (null).");
				return;
			}

			if (GameMode != GameMode.Creative && !VerifyItemStack(message.item))
			{
				Log.Warn($"Kicked {Username} for use item hacking.");
				Disconnect("Error #324. Please report this error.");
				return;
			}

			// Make sure we are holding the item we claim to be using
			Item itemInHand = Inventory.GetItemInHand();
			if (itemInHand == null || itemInHand.Id != message.item.Id)
			{
				Log.Warn($"Use item detected difference between server and client. Expected item {message.item} but server had item {itemInHand}");
				return; // Cheat(?)
			}

			if (itemInHand.GetType() == typeof (Item))
			{
				Log.Warn($"Generic item in hand when placing block. Can not complete request. Expected item {message.item} and item in hand is {itemInHand}");
			}

			if (message.face >= 0 && message.face <= 5)
			{
				// Right click

				Level.Interact(this, itemInHand, message.blockcoordinates, (BlockFace) message.face, message.facecoordinates);
			}
			else
			{
				Log.Debug($"Begin non-block action with {itemInHand}");

				// Snowballs and shit

				_itemUseTimer = Level.TickTime;

				itemInHand.UseItem(Level, this, message.blockcoordinates);

				IsUsingItem = true;
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
			mcpeStartGame.entityId = EntityId;
			mcpeStartGame.runtimeEntityId = 0;
			mcpeStartGame.spawn = KnownPosition.ToVector3();
			mcpeStartGame.unknown1 = new Vector2(KnownPosition.HeadYaw, KnownPosition.Pitch);
			mcpeStartGame.seed = 12345;
			mcpeStartGame.dimension = 0;
			mcpeStartGame.generator = 1;
			mcpeStartGame.gamemode = (int) GameMode;
			mcpeStartGame.x = (int) SpawnPosition.X;
			mcpeStartGame.y = (int) (SpawnPosition.Y + Height);
			mcpeStartGame.z = (int) SpawnPosition.Z;
			//mcpeStartGame.hasAchievementsDisabled = GameMode == GameMode.Creative || EnableCommands;
			mcpeStartGame.hasAchievementsDisabled = true;
			mcpeStartGame.dayCycleStopTime = (int) Level.CurrentWorldTime;
			mcpeStartGame.eduMode = false;
			mcpeStartGame.rainLevel = 0;
			mcpeStartGame.lightnigLevel = 0;
			mcpeStartGame.enableCommands = EnableCommands;
			mcpeStartGame.isTexturepacksRequired = false;
			mcpeStartGame.secret = "1m0AAMIFIgA=";
			mcpeStartGame.worldName = Level.LevelName;

			SendPackage(mcpeStartGame);
		}

		/// <summary>
		///     Sends the set spawn position packet.
		/// </summary>
		public void SendSetSpawnPosition()
		{
			McpeSetSpawnPosition mcpeSetSpawnPosition = McpeSetSpawnPosition.CreateObject();
			mcpeSetSpawnPosition.unknown1 = 1;
			mcpeSetSpawnPosition.coordinates = (BlockCoordinates) SpawnPosition;
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

		private void ForcedSendEmptyChunks()
		{
			Monitor.Enter(_sendChunkSync);
			try
			{
				var chunkPosition = new ChunkCoordinates(KnownPosition);

				_currentChunkPosition = chunkPosition;

				if (Level == null) return;

				for (int x = -3; x < 3; x++)
				{
					for (int z = -3; z < 3; z++)
					{
						McpeFullChunkData chunk = new McpeFullChunkData();
						chunk.chunkX = chunkPosition.X + x;
						chunk.chunkZ = chunkPosition.Z + z;
						chunk.chunkData = new byte[0];
						SendPackage(chunk);
					}
				}
			}
			finally
			{
				Monitor.Exit(_sendChunkSync);
			}
		}

		public void ForcedSendChunks(Action postAction = null)
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

				if (IsSpawned && _currentChunkPosition.DistanceTo(chunkPosition) < MoveRenderDistance)
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

		public virtual void SendUpdateAttributes()
		{
			var attributes = new PlayerAttributes();
			attributes["minecraft:attack_damage"] = new PlayerAttribute
			{
				Name = "minecraft:attack_damage",
				MinValue = 1,
				MaxValue = 1,
				Value = 1,
				Default = 1,
			};
			attributes["minecraft:absorption"] = new PlayerAttribute
			{
				Name = "minecraft:absorption",
				MinValue = 0,
				MaxValue = float.MaxValue,
				Value = HealthManager.Absorption,
				Default = 0,
			};
			attributes["minecraft:health"] = new PlayerAttribute
			{
				Name = "minecraft:health",
				MinValue = 0,
				MaxValue = 20,
				Value = HealthManager.Hearts,
				Default = 20,
			};
			attributes["minecraft:movement"] = new PlayerAttribute
			{
				Name = "minecraft:movement",
				MinValue = 0,
				MaxValue = 0.5f,
				Value = MovementSpeed,
				Default = MovementSpeed,
			};
			attributes["minecraft:knockback_resistance"] = new PlayerAttribute
			{
				Name = "minecraft:knockback_resistance",
				MinValue = 0,
				MaxValue = 1,
				Value = 0,
				Default = 0,
			};
			attributes["minecraft:luck"] = new PlayerAttribute
			{
				Name = "minecraft:luck",
				MinValue = -1025,
				MaxValue = 1024,
				Value = 0,
				Default = 0,
			};
			attributes["minecraft:fall_damage"] = new PlayerAttribute
			{
				Name = "minecraft:fall_damage",
				MinValue = 0,
				MaxValue = float.MaxValue,
				Value = 1,
				Default = 1,
			};
			attributes["minecraft:follow_range"] = new PlayerAttribute
			{
				Name = "minecraft:follow_range",
				MinValue = 0,
				MaxValue = 2048,
				Value = 16,
				Default = 16,
			};
			attributes["minecraft:player.experience"] = new PlayerAttribute
			{
				Name = "minecraft:player.experience",
				MinValue = 0,
				MaxValue = 1,
				Value = CalculateXp(),
				Default = 0,
			};
			attributes["minecraft:player.level"] = new PlayerAttribute
			{
				Name = "minecraft:player.level",
				MinValue = 0,
				MaxValue = 24791,
				Value = ExperienceLevel,
				Default = 0,
			};

			// Workaround, bad design.
			attributes = HungerManager.AddHungerAttributes(attributes);

			McpeUpdateAttributes attributesPackate = McpeUpdateAttributes.CreateObject();
			attributesPackate.entityId = 0;
			attributesPackate.attributes = attributes;
			SendPackage(attributesPackate);
		}

		private float CalculateXp()
		{
			var xpToNextLevel = GetXpToNextLevel();

			return Experience/xpToNextLevel;
		}

		public void RemoveExperienceLevels(float levels)
		{
			var currentXp = CalculateXp();
			ExperienceLevel = Experience - Math.Abs(levels);
			var xpToNextLevel = GetXpToNextLevel();
			Experience = xpToNextLevel*currentXp;
		}

		public void AddExperience(float xp, bool send = true)
		{
			var xpToNextLevel = GetXpToNextLevel();

			if (xpToNextLevel - (xp + Experience) > 0)
			{
				Experience += xp;
			}
			else
			{
				ExperienceLevel++;
				AddExperience(Experience + xp - xpToNextLevel, false);
			}

			if (send) SendUpdateAttributes();
		}

		private float GetXpToNextLevel()
		{
			float xpToNextLevel = 0;
			if (ExperienceLevel >= 0 && ExperienceLevel <= 15)
			{
				xpToNextLevel = 2*ExperienceLevel + 7;
			}
			else if (ExperienceLevel > 15 && ExperienceLevel <= 30)
			{
				xpToNextLevel = 5*ExperienceLevel - 38;
			}
			else if (ExperienceLevel > 30)
			{
				xpToNextLevel = 9*ExperienceLevel - 158;
			}
			return xpToNextLevel;
		}

		public virtual void SendSetTime()
		{
			McpeSetTime message = McpeSetTime.CreateObject();
			message.time = (int) Level.CurrentWorldTime;
			message.started = Level.IsWorldTimeStarted;
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

			if (IsGliding && CurrentSpeed > 30)
			{
				var particle = new CriticalParticle(Level);
				particle.Position = KnownPosition.ToVector3();
				particle.Spawn();
			}

			if (Log.IsDebugEnabled && Level.TickTime%10 == 0)
			{
				AddPopup(new Popup()
				{
					Id = 10,
					MessageType = MessageType.Tip,
					Message = $"Speed: {CurrentSpeed:F2}m/s",
					Duration = 20*5,
				});
			}

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
						// Tip is ontop
						if (popup.MessageType == MessageType.Tip && !hasDisplayedTip)
						{
							if (popup.CurrentTick <= popup.Duration + popup.DisplayDelay - 30)
								if (popup.CurrentTick%20 == 0 || popup.CurrentTick == popup.Duration + popup.DisplayDelay - 30) SendMessage(popup.Message, type: popup.MessageType);
							hasDisplayedTip = true;
						}

						// Popup is below
						if (popup.MessageType == MessageType.Popup && !hasDisplayedPopup)
						{
							if (popup.CurrentTick <= popup.Duration + popup.DisplayDelay - 30)
								if (popup.CurrentTick%20 == 0 || popup.CurrentTick == popup.Duration + popup.DisplayDelay - 30) SendMessage(popup.Message, type: popup.MessageType);
							hasDisplayedPopup = true;
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
			McpeSetEntityMotion motions = McpeSetEntityMotion.CreateObject();
			motions.entityId = 0;
			motions.velocity = velocity;
			SendPackage(motions);
		}

		public string ButtonText { get; set; }

		public override MetadataDictionary GetMetadata()
		{
			var metadata = base.GetMetadata();
			metadata[4] = new MetadataString(NameTag ?? Username);
			metadata[40] = new MetadataString(ButtonText ?? string.Empty);

			//MetadataDictionary metadata = new MetadataDictionary();
			//metadata[0] = new MetadataLong(GetDataValue()); // 10000000000000011000000000000000
			//metadata[1] = new MetadataInt(1);
			//metadata[2] = new MetadataInt(0);
			//metadata[3] = new MetadataByte(0);
			//metadata[4] = new MetadataString(NameTag ?? Username);
			//metadata[5] = new MetadataLong(1);
			//metadata[7] = new MetadataShort(400);
			//metadata[8] = new MetadataInt(0);
			//metadata[9] = new MetadataByte(0);
			//metadata[27] = new MetadataByte(0);
			//metadata[28] = new MetadataInt(1);
			//metadata[29] = new MetadataIntCoordinates((int) SpawnPosition.X, (int) SpawnPosition.Y, (int) SpawnPosition.Z);
			//metadata[38] = new MetadataLong(0);
			//metadata[39] = new MetadataFloat(1f);
			//metadata[40] = new MetadataString(ButtonText ?? string.Empty);
			//metadata[41] = new MetadataLong(0);
			//metadata[44] = new MetadataShort(400);
			//metadata[45] = new MetadataInt(0);
			//metadata[46] = new MetadataByte(0);
			//metadata[47] = new MetadataInt(0);
			//metadata[53] = new MetadataFloat(0.8f);
			//metadata[54] = new MetadataFloat(1.8f);
			//metadata[56] = new MetadataVector3(10, 50, 10);
			//metadata[57] = new MetadataByte(0);
			//metadata[58] = new MetadataFloat(0f);
			//metadata[59] = new MetadataFloat(0f);

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

		public override void BroadcastEntityEvent()
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

		/// <summary>
		///     Very important litle method. This does all the sending of packages for
		///     the player class. Treat with respect!
		/// </summary>
		public void SendPackage(Package package)
		{
			if (NetworkHandler == null)
			{
				package.PutPool();
			}
			else
			{
				NetworkHandler?.SendPackage(package);
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
			mcpeAddPlayer.runtimeEntityId = EntityId;
			mcpeAddPlayer.x = KnownPosition.X;
			mcpeAddPlayer.y = KnownPosition.Y;
			mcpeAddPlayer.z = KnownPosition.Z;
			mcpeAddPlayer.speedX = Velocity.X;
			mcpeAddPlayer.speedY = Velocity.Y;
			mcpeAddPlayer.speedZ = Velocity.Z;
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

	public enum UserPermission
	{
		Any = 0,
		Op = 1,
		Host = 2,
		Automation = 3,
		Admin = 4,
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