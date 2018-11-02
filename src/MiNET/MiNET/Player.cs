﻿#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE. 
// The License is based on the Mozilla Public License Version 1.1, but Sections 14 
// and 15 have been added to cover use of software over a computer network and 
// provide for limited attribution for the Original Developer. In addition, Exhibit A has 
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Threading;
using log4net;
using MiNET.Blocks;
using MiNET.Crafting;
using MiNET.Effects;
using MiNET.Entities;
using MiNET.Entities.Passive;
using MiNET.Entities.World;
using MiNET.Items;
using MiNET.Net;
using MiNET.Particles;
using MiNET.UI;
using MiNET.Utils;
using MiNET.Utils.Skins;
using MiNET.Worlds;
using Newtonsoft.Json;

namespace MiNET
{
	public class Player : Entity, IMcpeMessageHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Player));

		private MiNetServer Server { get; set; }
		public IPEndPoint EndPoint { get; private set; }
		public INetworkHandler NetworkHandler { get; set; }

		private Dictionary<ChunkCoordinates, McpeWrapper> _chunksUsed = new Dictionary<ChunkCoordinates, McpeWrapper>();
		private ChunkCoordinates _currentChunkPosition;

		private IInventory _openInventory;
		public PlayerInventory Inventory { get; set; }

		public PlayerLocation SpawnPosition { get; set; }
		public bool IsSleeping { get; set; } = false;

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
		public PlayerInfo PlayerInfo { get; set; }

		public Skin Skin { get; set; }

		public float ExperienceLevel { get; set; } = 0f;
		public float Experience { get; set; } = 0f;
		public float MovementSpeed { get; set; } = 0.1f;
		public ConcurrentDictionary<EffectType, Effect> Effects { get; set; } = new ConcurrentDictionary<EffectType, Effect>();

		public HungerManager HungerManager { get; set; }

		public bool IsFalling { get; set; }
		public bool IsFlyingHorizontally { get; set; }

		public Entity LastAttackTarget { get; set; }

		public List<Popup> Popups { get; set; } = new List<Popup>();

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
			CanClimb = true;
			HasCollision = true;
			IsAffectedByGravity = true;
			NoAi = false;
		}

		public void HandleMcpeClientToServerHandshake(McpeClientToServerHandshake message)
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

		public void HandleMcpeCommandBlockUpdate(McpeCommandBlockUpdate message)
		{
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
			chunkData.chunkIndex = 0; // Package index ?
			chunkData.progress = 0; // Long, maybe timestamp?
			chunkData.length = (uint) content.Length;
			chunkData.payload = content;
			SendPacket(chunkData);
		}

		public void HandleMcpePurchaseReceipt(McpePurchaseReceipt message)
		{
		}

		public void HandleMcpePlayerSkin(McpePlayerSkin message)
		{
		}

		public virtual void HandleMcpePhotoTransfer(McpePhotoTransfer message)
		{
			// Handle photos from the camera. Override to provide your own implementaion because
			// no sensible default for MiNET.
		}

		protected Form CurrentForm = null;

		public void HandleMcpeModalFormResponse(McpeModalFormResponse message)
		{
			if (CurrentForm == null) Log.Warn("No current form set for player when processing response");

			var form = CurrentForm;
			if (form == null || form.Id != message.formId)
			{
				Log.Warn("Receive data for form not currently active");
				return;
			}
			CurrentForm = null;
			form?.FromJson(message.data, this);
		}

		public virtual Form GetServerSettingsForm()
		{
			CustomForm customForm = new CustomForm();
			customForm.Title = "A title";
			customForm.Content = new List<CustomElement>()
			{
				new Label {Text = "A label"},
				new Input {Text = "", Placeholder = "Placeholder", Value = ""},
				new Toggle {Text = "A toggler", Value = true},
				new Slider {Text = "A slider", Min = 0, Max = 10, Step = 2, Value = 3},
				new StepSlider {Text = "A step slider", Steps = new List<string>() {"Step 1", "Step 2", "Step 3"}, Value = 1},
				new Dropdown {Text = "A step slider", Options = new List<string>() {"Option 1", "Option 2", "Option 3"}, Value = 1},
			};
			
			return customForm;
		}

		public void HandleMcpeServerSettingsRequest(McpeServerSettingsRequest message)
		{
			var form = GetServerSettingsForm();
			if (form == null) return;

			CurrentForm = form;
			
			McpeServerSettingsResponse response = McpeServerSettingsResponse.CreateObject();
			response.formId = form.Id;
			response.data = form.ToJson();
			SendPacket(response);
		}

		public virtual void HandleMcpeSetPlayerGameType(McpeSetPlayerGameType message)
		{
			SetGameMode((GameMode) message.gamemode);
		}

		public virtual void HandleMcpeLabTable(McpeLabTable message)
		{
		}

		public virtual void HandleMcpeSetLocalPlayerAsInitializedPacket(McpeSetLocalPlayerAsInitializedPacket message)
		{
		}

		private bool _serverHaveResources = false;

		public virtual void HandleMcpeResourcePackClientResponse(McpeResourcePackClientResponse message)
		{
			if (Log.IsDebugEnabled) Log.Debug($"Handled packet 0x{message.Id:X2}\n{Packet.HexDump(message.Bytes)}");

			if (message.responseStatus == 2)
			{
				McpeResourcePackDataInfo dataInfo = McpeResourcePackDataInfo.CreateObject();
				dataInfo.packageId = "5abdb963-4f3f-4d97-8482-88e2049ab149";
				dataInfo.maxChunkSize = 1048576;
				dataInfo.chunkCount = 1;
				dataInfo.compressedPackageSize = 359901; // Lenght of data
				dataInfo.hash = "9&\r2'ëX•;\u001bð—Ð‹\u0006´6\u0007TÞ/[Üx…x*\u0005h\u0002à\u0012"; //TODO: Fix encoding for this. Right now, must be Default :-(
				SendPacket(dataInfo);
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

		public virtual void SendResourcePacksInfo()
		{
			McpeResourcePacksInfo packInfo = McpeResourcePacksInfo.CreateObject();
			if (_serverHaveResources)
			{
				packInfo.mustAccept = false;
				packInfo.resourcepackinfos = new ResourcePackInfos
				{
					new ResourcePackInfo() {PackIdVersion = new PackIdVersion() {Id = "5abdb963-4f3f-4d97-8482-88e2049ab149", Version = "0.0.1"}, Size = 359901},
				};
			}

			SendPacket(packInfo);
		}

		public virtual void SendResourcePackStack()
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

			SendPacket(packStack);
		}

		public virtual void HandleMcpePlayerInput(McpePlayerInput message)
		{
			Log.Debug($"Player input: x={message.motionX}, z={message.motionZ}, jumping={message.jumping}, sneaking={message.sneaking}");
		}

		public void HandleMcpeRiderJump(McpeRiderJump message)
		{
			if (IsRiding && Vehicle > 0)
			{
				if (Level.TryGetEntity(Vehicle, out Mob mob))
				{
					mob.IsRearing = true;
					mob.BroadcastSetEntityData();
				}
			}
		}

		public virtual void HandleMcpeSetEntityData(McpeSetEntityData message)
		{
			// Only used by EDU NPC so far.
			if (Level.TryGetEntity(message.runtimeEntityId, out Entity entity))
			{
				entity.SetEntityData(message.metadata);
			}
		}

		public void HandleMcpeNpcRequest(McpeNpcRequest message)
		{
			// Only used by EDU NPC.

			if (Level.TryGetEntity(message.runtimeEntityId, out Entity entity))
			{
				// 0 is edit
				// 0 is exec command
				// 2 is exec link

				if (message.unknown0 == 0)
				{
					MetadataDictionary metadata = new MetadataDictionary();
					metadata[42] = new MetadataString(message.unknown1);
					entity.SetEntityData(metadata);
				}
			}

		}

		private object _mapInfoSync = new object();

		private Timer _mapSender;
		private ConcurrentQueue<McpeWrapper> _mapBatches = new ConcurrentQueue<McpeWrapper>();

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
					if (!Level.TryGetEntity(mapId, out MapEntity mapEntity))
					{
						// Create new map entity
						// send map for that entity
						//mapEntity = new MapEntity(Level, mapId);
						//mapEntity.SpawnEntity();
					}
					else
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
			SendPacket(packet);
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


		public virtual void HandleMcpeEntityFall(McpeEntityFall message)
		{
			double damage = message.fallDistance - 3;
			if (damage > 0)
			{
				HealthManager.TakeHit(null, (int) DamageCalculator.CalculatePlayerDamage(null, this, null, damage, DamageCause.Fall), DamageCause.Fall);
			}
		}

		public void HandleMcpeSetEntityMotion(McpeSetEntityMotion message)
		{
			Level.RelayBroadcast((McpeSetEntityMotion) message.Clone());
		}

		public void HandleMcpeMoveEntity(McpeMoveEntity message)
		{
			Level.RelayBroadcast((McpeMoveEntity) message.Clone());
			if (Level.TryGetEntity(message.runtimeEntityId, out Entity entity))
			{
				entity.KnownPosition = message.position;
				entity.IsOnGround = (message.flags & 1) == 1;
				if (entity.IsOnGround) Log.Debug("Horse is on ground");
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
			msg.runtimeEntityId = EntityId;
			msg.actionId = message.actionId;
			msg.unknownFloat = message.unknownFloat;

			Level.RelayBroadcast(this, msg);
		}

		Action _dimensionFunc;

		/// <summary>
		///     Handles the player action.
		/// </summary>
		/// <param name="message">The message.</param>
		public virtual void HandleMcpePlayerAction(McpePlayerAction message)
		{
			switch ((PlayerAction) message.actionId)
			{
				case PlayerAction.StartBreak:
				{
					if (message.face == (int) BlockFace.Up)
					{
						Block block = Level.GetBlock(message.coordinates.BlockUp());
						if (block is Fire)
						{
							Level.BreakBlock(this, message.coordinates.BlockUp());
							break;
						}
					}


					if (GameMode == GameMode.Survival)
					{
						Block target = Level.GetBlock(message.coordinates);
						var drops = target.GetDrops(Inventory.GetItemInHand());
						float tooltypeFactor = drops == null || drops.Length == 0 ? 5f : 1.5f; // 1.5 if proper tool
						double breakTime = Math.Ceiling(target.Hardness*tooltypeFactor*20);

						McpeLevelEvent breakEvent = McpeLevelEvent.CreateObject();
						breakEvent.eventId = 3600;
						breakEvent.position = message.coordinates;
						breakEvent.data = (int) (65535/breakTime);
						Log.Debug("Break speed: " + breakEvent.data);
						Level.RelayBroadcast(breakEvent);
					}

					break;
				}
				case PlayerAction.Breaking:
				{
					Block target = Level.GetBlock(message.coordinates);
					int data = ((int) target.GetRuntimeId()) | ((byte) (message.face << 24));

					McpeLevelEvent breakEvent = McpeLevelEvent.CreateObject();
					breakEvent.eventId = 2014;
					breakEvent.position = message.coordinates;
					breakEvent.data = data;
					Level.RelayBroadcast(breakEvent);
					break;
				}
				case PlayerAction.AbortBreak:
				case PlayerAction.StopBreak:
				{
					McpeLevelEvent breakEvent = McpeLevelEvent.CreateObject();
					breakEvent.eventId = 3601;
					breakEvent.position = message.coordinates;
					Level.RelayBroadcast(breakEvent);
					break;
				}
				case PlayerAction.StartSleeping:
				{
					break;
				}
				case PlayerAction.StopSleeping:
				{
					IsSleeping = false;
					Bed bed = Level.GetBlock(SpawnPosition) as Bed;
					if (bed != null)
					{
						bed.SetOccupied(Level, false);
					}
					else
					{
						Log.Warn($"Did not find a bed at {SpawnPosition}");
					}

					break;
				}
				case PlayerAction.Respawn:
				{
					MiNetServer.FastThreadPool.QueueUserWorkItem(HandleMcpeRespawn);
					break;
				}
				case PlayerAction.Jump:
				{
					HungerManager.IncreaseExhaustion(IsSprinting ? 0.8f : 0.2f);
					break;
				}
				case PlayerAction.StartSprint:
				{
					SetSprinting(true);
					break;
				}
				case PlayerAction.StopSprint:
				{
					SetSprinting(false);
					break;
				}
				case PlayerAction.StartSneak:
				{
					SetSprinting(false);
					IsSneaking = true;
					break;
				}
				case PlayerAction.StopSneak:
				{
					SetSprinting(false);
					IsSneaking = false;
					break;
				}
				case PlayerAction.DimensionChange:
				{
					break;
				}
				case PlayerAction.DimensionChangeAck:
				{
					if (_dimensionFunc != null)
					{
						_dimensionFunc();
						_dimensionFunc = null;
					}

					break;
				}
				case PlayerAction.WorldImmutable:
				{
					break;
				}
				case PlayerAction.StartGlide:
				{
					IsGliding = true;
					Height = 0.6;

					var particle = new WhiteSmokeParticle(Level);
					particle.Position = KnownPosition.ToVector3();
					particle.Spawn();

					break;
				}
				case PlayerAction.StopGlide:
				{
					IsGliding = false;
					Height = 1.8;
					break;
				}
				case PlayerAction.SetEnchantmentSeed:
				{
					Log.Debug($"Got PlayerAction.SetEnchantmentSeed with data={message.face} at {message.coordinates}");
					break;
				}
				default:
				{
					Log.Warn($"Unhandled action ID={message.actionId}");
					throw new ArgumentOutOfRangeException(nameof(message.actionId));
				}
			}

			IsUsingItem = false;

			BroadcastSetEntityData();
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

		public virtual void HandleMcpeBlockEntityData(McpeBlockEntityData message)
		{
			if (Log.IsDebugEnabled)
			{
				Log.DebugFormat("x:  {0}", message.coordinates.X);
				Log.DebugFormat("y:  {0}", message.coordinates.Y);
				Log.DebugFormat("z:  {0}", message.coordinates.Z);
				Log.DebugFormat("NBT {0}", message.namedtag.NbtFile);
			}

			var blockEntity = Level.GetBlockEntity(message.coordinates);

			if (blockEntity == null) return;

			blockEntity.SetCompound(message.namedtag.NbtFile.RootTag);
			Level.SetBlockEntity(blockEntity);
		}


		public bool IsWorldImmutable { get; set; }
		public bool IsWorldBuilder { get; set; }
		public bool IsMuted { get; set; }
		public bool IsNoPvp { get; set; }
		public bool IsNoPvm { get; set; }
		public bool IsNoMvp { get; set; }
		public bool IsNoClip { get; set; }
		public bool IsFlying { get; set; }

		public virtual void HandleMcpeAdventureSettings(McpeAdventureSettings message)
		{
			var flags = message.flags;
			IsAutoJump = (flags & 0x20) == 0x20;
			IsFlying = (flags & 0x200) == 0x200;
		}

		public virtual void SendGameRules()
		{
			McpeGameRulesChanged gameRulesChanged = McpeGameRulesChanged.CreateObject();
			gameRulesChanged.rules = Level.GetGameRules();
			SendPacket(gameRulesChanged);
		}

		public virtual void SendAdventureSettings()
		{
			McpeAdventureSettings mcpeAdventureSettings = McpeAdventureSettings.CreateObject();

			var flags = GetAdventureFlags();

			mcpeAdventureSettings.flags = flags;
			mcpeAdventureSettings.commandPermission = (uint) CommandPermission;
			mcpeAdventureSettings.actionPermissions = (uint) ActionPermissions;
			mcpeAdventureSettings.permissionLevel = (uint) PermissionLevel;
			mcpeAdventureSettings.customStoredPermissions = (uint) 0;
			mcpeAdventureSettings.userId = Endian.SwapInt64(EntityId);

			SendPacket(mcpeAdventureSettings);
		}

		private uint GetAdventureFlags()
		{
			uint flags = 0;
			if (IsWorldImmutable || GameMode == GameMode.Adventure) flags |= 0x01; // Immutable World (Remove hit markers client-side).
			if (IsNoPvp || IsSpectator || GameMode == GameMode.Spectator) flags |= 0x02; // No PvP (Remove hit markers client-side).
			if (IsNoPvm || IsSpectator || GameMode == GameMode.Spectator) flags |= 0x04; // No PvM (Remove hit markers client-side).
			if (IsNoMvp || IsSpectator || GameMode == GameMode.Spectator) flags |= 0x08;

			if (IsAutoJump) flags |= 0x20;

			if (AllowFly || GameMode == GameMode.Creative) flags |= 0x40;

			if (IsNoClip || IsSpectator || GameMode == GameMode.Spectator) flags |= 0x80; // No clip

			if (IsWorldBuilder) flags |= 0x100; // Worldbuilder

			if (IsFlying) flags |= 0x200;
			if (IsMuted) flags |= 0x400; // Mute
			return flags;
		}

		public PermissionLevel PermissionLevel { get; set; } = PermissionLevel.Operator;

		public int CommandPermission { get; set; } = (int) Net.CommandPermission.Normal;

		public ActionPermissions ActionPermissions { get; set; } = ActionPermissions.Default;

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

		public void Start(object o)
		{
			Stopwatch watch = new Stopwatch();
			watch.Restart();

			var serverInfo = Server.ServerInfo;

			try
			{
				Session = Server.SessionManager.CreateSession(this);

				lock (_disconnectSync)
				{
					if (!IsConnected) return;

					if (Level != null) return; // Already called this method.

					Level = Server.LevelManager.GetLevel(this, Dimension.Overworld.ToString());
				}

				if (Level == null)
				{
					Disconnect("No level assigned.");
					return;
				}

				OnPlayerJoining(new PlayerEventArgs(this));

				SpawnPosition = (PlayerLocation) (SpawnPosition ?? Level.SpawnPoint).Clone();
				KnownPosition = (PlayerLocation) SpawnPosition.Clone();

				// Check if the user already exist, that case bumpt the old one
				Level.RemoveDuplicatePlayers(Username, ClientId);

				Level.EntityManager.AddEntity(this);

				GameMode = Config.GetProperty("Player.GameMode", Level.GameMode);

				//
				// Start game - spawn sequence starts here
				//

				// Vanilla 1st player list here

				//Level.AddPlayer(this, false);


				SendSetTime();

				SendStartGame();

				BroadcastSetEntityData();

				if (ChunkRadius == -1) ChunkRadius = 5;

				SendChunkRadiusUpdate();

				//SendSetSpawnPosition();

				SendSetTime();

				SendSetDificulty();

				SendSetCommandsEnabled();

				SendAdventureSettings();

				SendGameRules();

				// Vanilla 2nd player list here

				Level.AddPlayer(this, false);

				SendUpdateAttributes();

				SendPlayerInventory();

				SendCreativeInventory();

				SendCraftingRecipes();

				SendAvailableCommands(); // Don't send this before StartGame!
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
			Log.InfoFormat("Login complete by: {0} from {2} in {1}ms", Username, watch.ElapsedMilliseconds, EndPoint);
		}

		public bool EnableCommands { get; set; } = Config.GetProperty("EnableCommands", false);

		protected virtual void SendSetCommandsEnabled()
		{
			McpeSetCommandsEnabled enabled = McpeSetCommandsEnabled.CreateObject();
			enabled.enabled = EnableCommands;
			SendPacket(enabled);
		}

		protected virtual void SendAvailableCommands()
		{
			//var settings = new JsonSerializerSettings();
			//settings.NullValueHandling = NullValueHandling.Ignore;
			//settings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
			//settings.MissingMemberHandling = MissingMemberHandling.Error;
			//settings.Formatting = Formatting.Indented;
			//settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			//var content = JsonConvert.SerializeObject(Server.PluginManager.Commands, settings);

			McpeAvailableCommands commands = McpeAvailableCommands.CreateObject();
			commands.CommandSet = Server.PluginManager.Commands;
			//commands.commands = content;
			//commands.unknown = "{}";
			SendPacket(commands);
		}

		public virtual void HandleMcpeCommandRequest(McpeCommandRequest message)
		{
			Log.Debug($"UUID: {message.unknownUuid}");

			var result = Server.PluginManager.HandleCommand(this, message.command);
			if (result is string)
			{
				string sRes = result as string;
				SendMessage(sRes);
			}

			//var jsonSerializerSettings = new JsonSerializerSettings
			//{
			//	PreserveReferencesHandling = PreserveReferencesHandling.None,
			//	Formatting = Formatting.Indented,
			//};

			//var commandJson = JsonConvert.DeserializeObject<dynamic>(message.commandInputJson);
			//Log.Debug($"CommandJson\n{JsonConvert.SerializeObject(commandJson, jsonSerializerSettings)}");
			//object result = Server.PluginManager.HandleCommand(this, message.commandName, message.commandOverload, commandJson);
			//if (result != null)
			//{
			//	var settings = new JsonSerializerSettings();
			//	settings.NullValueHandling = NullValueHandling.Ignore;
			//	settings.DefaultValueHandling = DefaultValueHandling.Include;
			//	settings.MissingMemberHandling = MissingMemberHandling.Error;
			//	settings.Formatting = Formatting.Indented;
			//	settings.StringEscapeHandling = StringEscapeHandling.EscapeNonAscii;
			//	settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			//	var content = JsonConvert.SerializeObject(result, settings);
			//	McpeCommandRequest commandResult = McpeCommandRequest.CreateObject();
			//	commandResult.commandName = message.commandName;
			//	commandResult.commandOverload = message.commandOverload;
			//	commandResult.isOutput = true;
			//	commandResult.clientId = NetworkHandler.GetNetworkNetworkIdentifier();
			//	commandResult.commandInputJson = "null\n";
			//	commandResult.commandOutputJson = content;
			//	commandResult.entityIdSelf = EntityId;
			//	SendPackage(commandResult);

			//	if (Log.IsDebugEnabled) Log.Debug($"NetworkId={commandResult.clientId}, Command Respone\n{Package.ToJson(commandResult)}\nJSON:\n{content}");
			//}
		}

		public virtual void InitializePlayer()
		{
			// Send set health

			SendSetEntityData();

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

			var packet = McpeMovePlayer.CreateObject();
			packet.runtimeEntityId = EntityManager.EntityIdSelf;
			packet.x = position.X;
			packet.y = position.Y + 1.62f;
			packet.z = position.Z;
			packet.yaw = position.Yaw;
			packet.headYaw = position.HeadYaw;
			packet.pitch = position.Pitch;
			packet.mode = (byte) (teleport ? 1 : 0);

			SendPacket(packet);
		}

		private object _teleportSync = new object();

		public virtual void Teleport(PlayerLocation newPosition)
		{
			if (!Monitor.TryEnter(_teleportSync)) return;

			try
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
			}
			finally
			{
				Monitor.Exit(_teleportSync);
			}

			MiNetServer.FastThreadPool.QueueUserWorkItem(SendChunksForKnownPosition);
		}

		private bool IsChunkInCache(PlayerLocation position)
		{
			return _chunksUsed.ContainsKey(new ChunkCoordinates(position));
		}

		public virtual void ChangeDimension(Level toLevel, PlayerLocation spawnPoint, Dimension dimension, Func<Level> levelFunc = null)
		{
			switch (dimension)
			{
				case Dimension.Overworld:
					break;
				case Dimension.Nether:
					if (!Level.WorldProvider.HaveNether()) return;
					break;
				case Dimension.TheEnd:
					if (!Level.WorldProvider.HaveTheEnd()) return;
					break;
			}

			SendChangeDimension(dimension);

			Level.RemovePlayer(this);

			Dimension fromDimension = Level.Dimension;

			if (toLevel == null && levelFunc != null)
			{
				toLevel = levelFunc();
			}

			Level = toLevel; // Change level
			SpawnPosition = spawnPoint ?? Level?.SpawnPoint;

			BroadcastSetEntityData();

			SendUpdateAttributes();

			CleanCache();

			// Check if we need to generate a platform
			if (dimension == Dimension.TheEnd)
			{
				BlockCoordinates platformPosition = ((BlockCoordinates) SpawnPosition).BlockDown();
				if (!(Level.GetBlock(platformPosition) is Obsidian))
				{
					for (int x = 0; x < 5; x++)
					{
						for (int z = 0; z < 5; z++)
						{
							for (int y = 0; y < 5; y++)
							{
								var coordinates = new BlockCoordinates(x, y, z) + platformPosition + new BlockCoordinates(-2, 0, -2);
								if (y == 0)
								{
									Level.SetBlock(new Obsidian() {Coordinates = coordinates});
								}
								else
								{
									Level.SetAir(coordinates);
								}
							}
						}
					}
				}
			}
			else if (dimension == Dimension.Overworld && fromDimension == Dimension.TheEnd)
			{
				// Spawn on player home spawn
			}
			else if (dimension == Dimension.Nether)
			{
				// Find closes portal or spawn new
				// coordinate translation x/8

				BlockCoordinates start = (BlockCoordinates) KnownPosition;
				start /= new BlockCoordinates(8, 1, 8);

				PlayerLocation pos = FindNetherSpawn(Level, start);
				if (pos != null)
				{
					SpawnPosition = pos;
				}
				else
				{
					SpawnPosition = CreateNetherPortal(Level);
				}
			}
			else if (dimension == Dimension.Overworld && fromDimension == Dimension.Nether)
			{
				// Find closes portal or spawn new
				// coordinate translation x * 8

				BlockCoordinates start = (BlockCoordinates) KnownPosition;
				start *= new BlockCoordinates(8, 1, 8);

				PlayerLocation pos = FindNetherSpawn(Level, start);
				if (pos != null)
				{
					SpawnPosition = pos;
				}
				else
				{
					SpawnPosition = CreateNetherPortal(Level);
				}
			}

			Log.Debug($"Spawn point: {SpawnPosition}");

			SendChunkRadiusUpdate();

			ForcedSendChunk(SpawnPosition);

			// send teleport to spawn
			SetPosition(SpawnPosition);

			MiNetServer.FastThreadPool.QueueUserWorkItem(() =>
			{
				Level.AddPlayer(this, true);

				ForcedSendChunks(() =>
				{
					Log.WarnFormat("Respawn player {0} on level {1}", Username, Level.LevelId);

					SendSetTime();
				});
			});
		}

		private PlayerLocation FindNetherSpawn(Level level, BlockCoordinates start)
		{
			int width = 128;
			int height = Level.Dimension == Dimension.Overworld ? 256 : 128;


			int portalId = new Portal().Id;
			int obsidionId = new Obsidian().Id;

			Log.Debug($"Starting point: {start}");

			BlockCoordinates? closestPortal = null;
			int closestDistance = int.MaxValue;
			for (int x = start.X - width; x < start.X + width; x++)
			{
				for (int z = start.Z - width; z < start.Z + width; z++)
				{
					if (level.Dimension == Dimension.Overworld)
					{
						height = level.GetHeight(new BlockCoordinates(x, 0, z)) + 10;
					}

					for (int y = height - 1; y >= 0; y--)
					{
						var coord = new BlockCoordinates(x, y, z);
						if (coord.DistanceTo(start) > closestDistance) continue;

						bool b = level.IsBlock(coord, portalId);
						b &= level.IsBlock(coord.BlockDown(), obsidionId);
						if (b)
						{
							Portal portal = (Portal) level.GetBlock(coord);
							if (portal.Metadata >= 2)
							{
								b &= level.IsBlock(coord.BlockNorth(), portalId);
							}
							else
							{
								b &= level.IsBlock(coord.BlockEast(), portalId);
							}

							Log.Debug($"Found portal block at {coord}, direction={portal.Metadata}");
							if (b && coord.DistanceTo(start) < closestDistance)
							{
								Log.Debug($"Found a closer portal at {coord}");
								closestPortal = coord;
								closestDistance = (int) coord.DistanceTo(start);
							}
						}
					}
				}
			}

			return closestPortal;
		}

		private PlayerLocation CreateNetherPortal(Level level)
		{
			int width = 16;
			int height = Level.Dimension == Dimension.Overworld ? 256 : 128;


			BlockCoordinates start = (BlockCoordinates) KnownPosition;
			if (Level.Dimension == Dimension.Nether)
			{
				start /= new BlockCoordinates(8, 1, 8);
			}
			else
			{
				start *= new BlockCoordinates(8, 1, 8);
			}

			Log.Debug($"Starting point: {start}");

			PortalInfo closestPortal = null;
			int closestPortalDistance = int.MaxValue;
			for (int x = start.X - width; x < start.X + width; x++)
			{
				for (int z = start.Z - width; z < start.Z + width; z++)
				{
					if (level.Dimension == Dimension.Overworld)
					{
						height = level.GetHeight(new BlockCoordinates(x, 0, z)) + 10;
					}

					for (int y = height - 1; y >= 0; y--)
					{
						var coord = new BlockCoordinates(x, y, z);
						if (coord.DistanceTo(start) > closestPortalDistance) continue;

						if (!(!level.IsAir(coord) && level.IsAir(coord.BlockUp()))) continue;

						var bbox = new BoundingBox(coord, coord + new BlockCoordinates(3, 5, 4));
						if (!SpawnAreaClear(bbox))
						{
							bbox = new BoundingBox(coord, coord + new BlockCoordinates(4, 5, 3));
							if (!SpawnAreaClear(bbox))
							{
								bbox = new BoundingBox(coord, coord + new BlockCoordinates(1, 5, 4));
								if (!SpawnAreaClear(bbox))
								{
									bbox = new BoundingBox(coord, coord + new BlockCoordinates(4, 5, 1));
									if (!SpawnAreaClear(bbox))
									{
										continue;
									}
								}
							}
						}

						//coord += BlockCoordinates.Up;

						Log.Debug($"Found portal location at {coord}");
						if (coord.DistanceTo(start) < closestPortalDistance)
						{
							Log.Debug($"Found a closer portal location at {coord}");
							closestPortal = new PortalInfo() {Coordinates = coord, Size = bbox};
							closestPortalDistance = (int) coord.DistanceTo(start);
						}
					}
				}
			}

			if (closestPortal == null)
			{
				// Force create between Y=YMAX - (10 to 70)
				int y = (int) Math.Max(Height - 70, start.Y);
				y = (int) Math.Min(Height - 10, y);
				start.Y = y;

				Log.Debug($"Force portal location at {start}");

				closestPortal = new PortalInfo();
				closestPortal.HasPlatform = true;
				closestPortal.Coordinates = start;
				closestPortal.Size = new BoundingBox(start, start + new BlockCoordinates(4, 5, 3));
			}


			if (closestPortal != null)
			{
				BuildPortal(level, closestPortal);
			}


			return closestPortal?.Coordinates;
		}

		public static void BuildPortal(Level level, PortalInfo portalInfo)
		{
			var bbox = portalInfo.Size;

			Log.Debug($"Building portal from BBOX: {bbox}");

			int minX = (int) (bbox.Min.X);
			int minZ = (int) (bbox.Min.Z);
			int width = (int) (bbox.Width);
			int depth = (int) (bbox.Depth);
			int height = (int) (bbox.Height);

			int midPoint = depth > 2 ? depth/2 : 0;

			bool haveSetCoordinate = false;
			for (int x = 0; x < width; x++)
			{
				for (int z = 0; z < depth; z++)
				{
					for (int y = 0; y < height; y++)
					{
						var coordinates = new BlockCoordinates(x + minX, (int) (y + bbox.Min.Y), z + minZ);
						Log.Debug($"Place: {coordinates}");

						if (width > depth && z == midPoint)
						{
							if ((x == 0 || x == width - 1) || (y == 0 || y == height - 1))
							{
								level.SetBlock(new Obsidian {Coordinates = coordinates});
							}
							else
							{
								level.SetBlock(new Portal {Coordinates = coordinates});
								if (!haveSetCoordinate)
								{
									haveSetCoordinate = true;
									portalInfo.Coordinates = coordinates;
								}
							}
						}
						else if (width <= depth && x == midPoint)
						{
							if ((z == 0 || z == depth - 1) || (y == 0 || y == height - 1))
							{
								level.SetBlock(new Obsidian {Coordinates = coordinates});
							}
							else
							{
								level.SetBlock(new Portal {Coordinates = coordinates, Metadata = 2});
								if (!haveSetCoordinate)
								{
									haveSetCoordinate = true;
									portalInfo.Coordinates = coordinates;
								}
							}
						}

						if (portalInfo.HasPlatform && y == 0)
						{
							level.SetBlock(new Obsidian {Coordinates = coordinates});
						}
					}
				}
			}
		}


		private bool SpawnAreaClear(BoundingBox bbox)
		{
			BlockCoordinates min = bbox.Min;
			BlockCoordinates max = bbox.Max;
			for (int x = min.X; x < max.X; x++)
			{
				for (int z = min.Z; z < max.Z; z++)
				{
					for (int y = min.Y; y < max.Y; y++)
					{
						//if (z == min.Z) if (!Level.GetBlock(new BlockCoordinates(x, y, z)).IsBuildable) return false;
						if (y == min.Y)
						{
							if (!Level.GetBlock(new BlockCoordinates(x, y, z)).IsBuildable) return false;
						}
						else
						{
							if (!Level.IsAir(new BlockCoordinates(x, y, z))) return false;
						}
					}
				}
			}

			return true;
		}


		public virtual void SpawnLevel(Level toLevel, PlayerLocation spawnPoint, bool useLoadingScreen = false, Func<Level> levelFunc = null, Action postSpawnAction = null)
		{
			bool oldNoAi = NoAi;
			SetNoAi(true);

			if (useLoadingScreen)
			{
				SendChangeDimension(Dimension.Nether);
			}

			if (toLevel == null && levelFunc != null)
			{
				toLevel = levelFunc();
			}

			SetPosition(new PlayerLocation
			{
				X = KnownPosition.X,
				Y = 4000,
				Z = KnownPosition.Z,
				Yaw = 91,
				Pitch = 28,
				HeadYaw = 91,
			});

			Action transferFunc = delegate
			{
				if (useLoadingScreen)
				{
					SendChangeDimension(Dimension.Overworld);
				}

				Level.RemovePlayer(this, true);

				Level = toLevel; // Change level
				SpawnPosition = spawnPoint ?? Level?.SpawnPoint;

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

				MiNetServer.FastThreadPool.QueueUserWorkItem(() =>
				{
					Level.AddPlayer(this, true);

					SetNoAi(oldNoAi);

					ForcedSendChunks(() =>
					{
						Log.InfoFormat("Respawn player {0} on level {1}", Username, Level.LevelId);

						SendSetTime();

						postSpawnAction?.Invoke();
					});
				});
			};


			if (useLoadingScreen)
			{
				_dimensionFunc = transferFunc;
				ForcedSendEmptyChunks();
			}
			else
			{
				transferFunc();
			}
		}

		protected virtual void SendChangeDimension(Dimension dimension, bool respawn = false, Vector3 position = new Vector3())
		{
			McpeChangeDimension changeDimension = McpeChangeDimension.CreateObject();
			changeDimension.dimension = (int) dimension;
			changeDimension.position = position;
			changeDimension.respawn = respawn;
			changeDimension.NoBatch = true; // This is here because the client crashes otherwise.
			SendPacket(changeDimension);
		}

		public override void BroadcastSetEntityData(MetadataDictionary metadata)
		{
			McpeSetEntityData mcpeSetEntityData = McpeSetEntityData.CreateObject();
			mcpeSetEntityData.runtimeEntityId = EntityManager.EntityIdSelf;
			mcpeSetEntityData.metadata = metadata;
			mcpeSetEntityData.Encode();
			SendPacket(mcpeSetEntityData);

			base.BroadcastSetEntityData(metadata);
		}

		public void SendSetEntityData()
		{
			McpeSetEntityData mcpeSetEntityData = McpeSetEntityData.CreateObject();
			mcpeSetEntityData.runtimeEntityId = EntityManager.EntityIdSelf;
			mcpeSetEntityData.metadata = GetMetadata();
			mcpeSetEntityData.Encode();
			SendPacket(mcpeSetEntityData);
		}

		public void SendSetDificulty()
		{
			McpeSetDifficulty mcpeSetDifficulty = McpeSetDifficulty.CreateObject();
			mcpeSetDifficulty.difficulty = (uint) Level.Difficulty;
			SendPacket(mcpeSetDifficulty);
		}

		public virtual void SendPlayerInventory()
		{
			McpeInventoryContent strangeContent = McpeInventoryContent.CreateObject();
			strangeContent.inventoryId = (byte) 0x7b;
			strangeContent.input = new ItemStacks();
			SendPacket(strangeContent);

			McpeInventoryContent inventoryContent = McpeInventoryContent.CreateObject();
			inventoryContent.inventoryId = (byte) 0x00;
			inventoryContent.input = Inventory.GetSlots();
			SendPacket(inventoryContent);

			McpeInventoryContent armorContent = McpeInventoryContent.CreateObject();
			armorContent.inventoryId = 0x78;
			armorContent.input = Inventory.GetArmor();
			SendPacket(armorContent);

			McpeMobEquipment mobEquipment = McpeMobEquipment.CreateObject();
			mobEquipment.runtimeEntityId = EntityManager.EntityIdSelf;
			mobEquipment.item = Inventory.GetItemInHand();
			mobEquipment.slot = (byte) Inventory.InHandSlot;
			SendPacket(mobEquipment);
		}

		public virtual void SendCraftingRecipes()
		{
			McpeCraftingData craftingData = McpeCraftingData.CreateObject();
			craftingData.recipes = RecipeManager.Recipes;
			SendPacket(craftingData);
		}

		public virtual void SendCreativeInventory()
		{
			if (!UseCreativeInventory) return;

			McpeInventoryContent creativeContent = McpeInventoryContent.CreateObject();
			creativeContent.inventoryId = (byte) 0x79;
			creativeContent.input = InventoryUtils.GetCreativeMetadataSlots();
			SendPacket(creativeContent);
		}

		private void SendChunkRadiusUpdate()
		{
			McpeChunkRadiusUpdate packet = McpeChunkRadiusUpdate.CreateObject();
			packet.chunkRadius = ChunkRadius;

			SendPacket(packet);
		}

		public void SendPlayerStatus(int status)
		{
			McpePlayStatus mcpePlayerStatus = McpePlayStatus.CreateObject();
			mcpePlayerStatus.status = status;
			SendPacket(mcpePlayerStatus);
		}

		[Wired]
		public void SetGameMode(GameMode gameMode)
		{
			GameMode = gameMode;

			SendSetPlayerGameType();
		}


		public void SendSetPlayerGameType()
		{
			McpeSetPlayerGameType gametype = McpeSetPlayerGameType.CreateObject();
			gametype.gamemode = (int) GameMode;
			SendPacket(gametype);
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
			try
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
							NetworkHandler.SendDirectPacket(disconnect);
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
			catch (Exception e)
			{
				Log.Error("On disconnect player", e);
				throw;
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
			if (!IsGliding) HungerManager.Move(Vector3.Distance(new Vector3(KnownPosition.X, 0, KnownPosition.Z), new Vector3(message.x, 0, message.z)));

			KnownPosition = new PlayerLocation
			{
				X = message.x,
				Y = message.y - 1.62f,
				Z = message.z,
				Pitch = message.pitch,
				Yaw = message.yaw,
				HeadYaw = message.headYaw
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

		public virtual void HandleMcpeLevelSoundEvent(McpeLevelSoundEvent message)
		{
			McpeLevelSoundEvent sound = McpeLevelSoundEvent.CreateObject();
			sound.soundId = message.soundId;
			sound.position = message.position;
			sound.blockId = message.blockId;
			sound.entityType = message.entityType;
			sound.isBabyMob = message.isBabyMob;
			sound.isGlobal = message.isGlobal;
			Level.RelayBroadcast(sound);
		}

		public virtual void HandleMcpeMobArmorEquipment(McpeMobArmorEquipment message)
		{
		}

		public virtual void HandleMcpeItemFrameDropItem(McpeItemFrameDropItem message)
		{
			Log.Debug($"Drops item frame at {message.coordinates}");
			if (Level.GetBlock(message.coordinates) is Frame frame)
			{
				Log.Debug($"Drops from frame {frame}");
				frame.ClearItem(Level);
			}
		}

		public virtual void HandleMcpeMobEquipment(McpeMobEquipment message)
		{
			if (HealthManager.IsDead) return;

			byte selectedHotbarSlot = message.selectedSlot;

			if (selectedHotbarSlot > 8)
			{
				Log.Error($"Player {Username} called set equipment with held hotbar slot {message.selectedSlot} with item {message.item}");
				return;
			}

			if (Log.IsDebugEnabled) Log.Debug($"Player {Username} called set equipment with held hotbar slot {message.selectedSlot} with item {message.item}");

			Inventory.SetHeldItemSlot(selectedHotbarSlot, false);
		}

		private object _inventorySync = new object();

		public virtual void SetOpenInventory(IInventory inventory)
		{
			_openInventory = inventory;
		}

		public void OpenInventory(BlockCoordinates inventoryCoord)
		{
			lock (_inventorySync)
			{
				if (_openInventory is Inventory openInventory)
				{
					if (openInventory.Coordinates.Equals(inventoryCoord)) return;
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
				containerOpen.coordinates = inventoryCoord;
				containerOpen.runtimeEntityId = -1;
				SendPacket(containerOpen);

				McpeInventoryContent containerSetContent = McpeInventoryContent.CreateObject();
				containerSetContent.inventoryId = inventory.WindowsId;
				containerSetContent.input = inventory.Slots;
				SendPacket(containerSetContent);
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
				McpeInventorySlot sendSlot = McpeInventorySlot.CreateObject();
				sendSlot.inventoryId = inventory.WindowsId;
				sendSlot.slot = slot;
				sendSlot.item = itemStack;
				SendPacket(sendSlot);
			}

			//if(inventory.BlockEntity != null)
			//{
			//	Level.SetBlockEntity(inventory.BlockEntity, false);
			//}
		}


		public void HandleMcpeInventorySlot(McpeInventorySlot message)
		{
		}

		public virtual void HandleMcpeCraftingEvent(McpeCraftingEvent message)
		{
			Log.Debug($"Player {Username} crafted item on window 0x{message.windowId:X2} on type: {message.recipeType}");
		}

		public virtual void HandleMcpeInventoryTransaction(McpeInventoryTransaction message)
		{
			switch (message.transaction.TransactionType)
			{
				case McpeInventoryTransaction.TransactionType.Normal:
					HandleNormalTransactions(message.transaction);
					break;
				case McpeInventoryTransaction.TransactionType.InventoryMismatch:
					HandleInventoryMismatchTransactions(message.transaction);
					break;
				case McpeInventoryTransaction.TransactionType.ItemUse:
					HandleItemUseTransactions(message.transaction);
					break;
				case McpeInventoryTransaction.TransactionType.ItemUseOnEntity:
					HandleItemUseOnEntityTransactions(message.transaction);
					break;
				case McpeInventoryTransaction.TransactionType.ItemRelease:
					HandleItemReleaseTransactions(message.transaction);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		protected virtual void HandleItemUseOnEntityTransactions(Transaction transaction)
		{
			switch ((McpeInventoryTransaction.ItemUseOnEntityAction) transaction.ActionType)
			{
				case McpeInventoryTransaction.ItemUseOnEntityAction.Interact: // Right click
					EntityInteract(transaction);
					break;
				case McpeInventoryTransaction.ItemUseOnEntityAction.Attack: // Left click
					EntityAttack(transaction);
					break;
				case McpeInventoryTransaction.ItemUseOnEntityAction.ItemInteract:
					Log.Warn($"Got Entity ItemInteract. Was't sure it existed, but obviously it does :-o");
					EntityItemInteract(transaction);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void EntityItemInteract(Transaction transaction)
		{
			Item itemInHand = Inventory.GetItemInHand();
			if (itemInHand.Id != transaction.Item.Id || itemInHand.Metadata != transaction.Item.Metadata)
			{
				Log.Warn($"Attack item mismatch. Expected {itemInHand}, but client reported {transaction.Item}");
			}

			if (!Level.TryGetEntity(transaction.EntityId, out Entity target)) return;
			target.DoItemInteraction(this, itemInHand);
		}

		protected virtual void EntityInteract(Transaction transaction)
		{
			DoInteraction(transaction.ActionType, this);

			if (!Level.TryGetEntity(transaction.EntityId, out Entity target)) return;
			target.DoInteraction(transaction.ActionType, this);
		}

		protected virtual void EntityAttack(Transaction transaction)
		{
			Item itemInHand = Inventory.GetItemInHand();
			if (itemInHand.Id != transaction.Item.Id || itemInHand.Metadata != transaction.Item.Metadata)
			{
				Log.Warn($"Attack item mismatch. Expected {itemInHand}, but client reported {transaction.Item}");
			}

			if (!Level.TryGetEntity(transaction.EntityId, out Entity target)) return;


			LastAttackTarget = target;

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
				var reducedDamage = (int) DamageCalculator.CalculatePlayerDamage(this, player, itemInHand, damage, DamageCause.EntityAttack);
				player.HealthManager.TakeHit(this, itemInHand, reducedDamage, DamageCause.EntityAttack);
				if(reducedDamage < damage)
				{
					player.Inventory.DamageArmor();
				}
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

			Inventory.DamageItemInHand(ItemDamageReason.EntityAttack, target, null);
			HungerManager.IncreaseExhaustion(0.3f);
		}

		protected virtual void HandleInventoryMismatchTransactions(Transaction transaction)
		{
			Log.Warn($"Transaction mismatch");
		}

		private long _itemUseTimer;

		protected virtual void HandleItemReleaseTransactions(Transaction transaction)
		{
			Item itemInHand = Inventory.GetItemInHand();

			switch ((McpeInventoryTransaction.ItemReleaseAction) transaction.ActionType)
			{
				case McpeInventoryTransaction.ItemReleaseAction.Release:
				{
					if (_itemUseTimer <= 0) return;

					itemInHand.Release(Level, this, transaction.FromPosition, Level.TickTime - _itemUseTimer);

					_itemUseTimer = 0;

					break;
				}
				case McpeInventoryTransaction.ItemReleaseAction.Use:
				{
					if (GameMode == GameMode.Survival)
					{
						if (itemInHand is FoodItem)
						{
							FoodItem foodItem = (FoodItem) Inventory.GetItemInHand();
							foodItem.Consume(this);
							foodItem.Count--;
						}
						else if (itemInHand is ItemPotion)
						{
							ItemPotion potion = (ItemPotion) Inventory.GetItemInHand();
							potion.Consume(this);
							potion.Count--;
						}
					}

					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
			}

			HandleNormalTransactions(transaction);
		}

		protected virtual void HandleItemUseTransactions(Transaction transaction)
		{
			var itemInHand = Inventory.GetItemInHand();

			switch ((McpeInventoryTransaction.ItemUseAction) transaction.ActionType)
			{
				case McpeInventoryTransaction.ItemUseAction.Place:
					Level.Interact(this, itemInHand, transaction.Position, (BlockFace) transaction.Face, transaction.ClickPosition);
					break;
				case McpeInventoryTransaction.ItemUseAction.Use:
					_itemUseTimer = Level.TickTime;
					itemInHand.UseItem(Level, this, transaction.Position);
					//Inventory.UpdateInventorySlot(transaction.Slot, transaction.Item);
					break;
				case McpeInventoryTransaction.ItemUseAction.Destroy:
					Level.BreakBlock(this, transaction.Position);
					break;
			}

			HandleNormalTransactions(transaction);
		}

		private List<Item> _craftingInput = new List<Item>(new Item[9]);
		public bool UsingCraftingTable { get; set; }

		protected virtual void HandleNormalTransactions(Transaction transaction)
		{
			foreach (var record in transaction.Transactions)
			{
				if (record is ContainerTransactionRecord)
				{
					var trans = (ContainerTransactionRecord) record;
					int invId = trans.InventoryId;
					int slot = trans.Slot;
					Item oldItem = trans.OldItem;
					Item newItem = trans.NewItem;

					if (oldItem is ItemAir) continue;

					Item oldItemSlots = null;
					switch (invId)
					{
						case 0:
							oldItemSlots = Inventory.Slots[trans.Slot];
							break;
						case 124:
							oldItemSlots = Inventory.Cursor;
							break;
						case 120:
							switch (slot)
							{
								case 0:
									oldItemSlots = Inventory.Helmet;
									break;
								case 1:
									oldItemSlots = Inventory.Chest;
									Inventory.Chest = newItem;
									break;
								case 2:
									oldItemSlots = Inventory.Leggings;
									break;
								case 3:
									oldItemSlots = Inventory.Boots;
									break;
								default:
									oldItemSlots = new ItemAir();
									break;
							}
							break;
						case 121:
							if (GameMode != GameMode.Creative && Log.IsDebugEnabled) Log.Warn($"Player {Username} made transaction with creative inventory without being in creative gamemode.");
							oldItemSlots = oldItem;
							break;
						default:
							if (_openInventory != null)
							{
								if (_openInventory is Inventory inventory && inventory.WindowsId == invId)
								{
									oldItemSlots = inventory.GetSlot((byte) slot);
								}
								else if (_openInventory is HorseInventory horseInventory)
								{
									oldItemSlots = horseInventory.GetSlot((byte) slot);
								}
							}
							else
							{
								oldItemSlots = new ItemAir();
							}
							break;
					}
					trans.OldItem = oldItemSlots;

				}
			}

			foreach (var record in transaction.Transactions)
			{
				Item oldItem = record.OldItem;
				Item newItem = null;
				if (record.NewItem is ItemAir)
				{
					newItem = new ItemAir();
				}
				else
					foreach (var r in transaction.Transactions)
					{
						if (r is ContainerTransactionRecord cont)
						{
							if (r.OldItem.Equals(record.NewItem))
							{
								if (r.OldItem.Count + (newItem?.Count ?? 0) >= record.NewItem.Count)
								{
									if (newItem == null)
									{
										newItem = r.OldItem;
									}


									r.OldItem = (Item) r.OldItem.Clone();
									r.OldItem.Count = (byte) (r.OldItem.Count + newItem.Count - record.NewItem.Count);
									newItem.Count = record.NewItem.Count;
									if (r.OldItem.Count <= 0)
									{
										r.OldItem = new ItemAir();
									}
									break;
								}
								else
								{
									if (newItem == null)
									{
										newItem = r.OldItem;
									}
									else
									{
										newItem.Count += r.OldItem.Count;
									}
									r.OldItem = new ItemAir();
								}
							}
						}
					}

				if (newItem == null)
				{
					newItem = record.NewItem;
					Log.Fatal($"Couldn't find item in slots for player {Username}, item {newItem}, old item {oldItem}");
				}


				if (record is ContainerTransactionRecord)
				{
					var trans = (ContainerTransactionRecord) record;
					int invId = trans.InventoryId;
					int slot = trans.Slot;

					if (invId == 0)
					{
						// Player inventory
						//if (!oldItem.Equals(Inventory.Slots[trans.Slot])) Log.Warn($"Inventory mismatch. Client reported old item as {oldItem} and it did not match existing the item {Inventory.Slots[trans.Slot]}");
						Inventory.Slots[trans.Slot] = newItem;
					}
					else if (invId == 120)
					{
						//if (!newItem.Equals(Inventory.Cursor)) Log.Warn($"Cursor mismatch. Client reported new item as {newItem} and it did not match existing the item {Inventory.Cursor}");
						switch (slot)
						{
							case 0:
								Inventory.Helmet = newItem;
								break;
							case 1:
								Inventory.Chest = newItem;
								break;
							case 2:
								Inventory.Leggings = newItem;
								break;
							case 3:
								Inventory.Boots = newItem;
								break;
						}

						SendArmorForPlayer();
					}
					else if (invId == 121)
					{
						if (GameMode != GameMode.Creative && Log.IsDebugEnabled) Log.Warn($"Player {Username} made transaction with creative inventory without being in creative gamemode.");
					}
					else if (invId == 124)
					{
						// Cursor
						//if (!oldItem.Equals(Inventory.Cursor)) Log.Warn($"Cursor mismatch. Client reported old item as {oldItem} and it did not match existing the item {Inventory.Cursor}");
						Inventory.Cursor = newItem;
					}
					else if (_openInventory != null)
					{
						if (_openInventory is Inventory inventory && inventory.WindowsId == invId)
						{
							//if (!oldItem.Equals(inventory.GetSlot((byte)slot))) Log.Warn($"Cursor mismatch. Client reported old item as {oldItem} and it did not match existing the item {inventory.GetSlot((byte)slot)}");

							// block inventories of various kinds (chests, furnace, etc)
							inventory.SetSlot(this, (byte) slot, newItem);
						}
						else if (_openInventory is HorseInventory horseInventory)
						{
							//if (!oldItem.Equals(horseInventory.GetSlot((byte)slot))) Log.Warn($"Cursor mismatch. Client reported old item as {oldItem} and it did not match existing the item {horseInventory.GetSlot((byte)slot)}");
							horseInventory.SetSlot(slot, newItem);
						}
					}

				}
				else if (record is CraftTransactionRecord)
				{
					var trans = (CraftTransactionRecord) record;
					int invId = trans.Action;
					int slot = trans.Slot;

					if (invId == (int) McpeInventoryTransaction.NormalAction.CraftUse)
					{
						_craftingInput[slot] = newItem;
					}
					else if (invId == (int) McpeInventoryTransaction.NormalAction.GetResult)
					{
						if (!VerifyRecipe(_craftingInput, oldItem))
						{
							if (Log.IsDebugEnabled) Log.Error($"Found NO matching recipe for player {Username}");
						}

						_craftingInput.Clear();
						_craftingInput.AddRange(new Item[9]);
					}
				}
				else if (record is CreativeTransactionRecord)
				{
					// 0 delete, 1 create
				}
				else if (record is WorldInteractionTransactionRecord)
				{
					if (record.Slot == 0)
					{
						// Drop
						DropItem(record.NewItem);
					}
					else if (record.Slot == 1)
					{
						// Pickup
					}
				}
			}
		}

		public virtual void DropItem(Item item)
		{
			var itemEntity = new ItemEntity(Level, item)
			{
				Velocity = KnownPosition.GetDirection().Normalize() * 0.3f,
				KnownPosition = KnownPosition + new Vector3(0f, 1.62f, 0f)
			};
			itemEntity.SpawnEntity();
		}

		private bool VerifyRecipe(List<Item> craftingInput, Item result)
		{
			List<Item> shapedInput = new List<Item>();
			foreach (var item in craftingInput)
			{
				shapedInput.Add(item ?? new Item(0));
			}

			List<Item> shapelessInput = new List<Item>();
			foreach (var item in craftingInput)
			{
				if (item == null) continue;
				shapelessInput.Add(item);
			}

			Log.Debug($"Looking for matching recipes with the result {result}");
			var recipes = RecipeManager.Recipes.Where(r => r is ShapedRecipe).Where(r => ((ShapedRecipe) r).Result.Id == result.Id && ((ShapedRecipe) r).Result.Metadata == result.Metadata).ToList();
			recipes.AddRange(RecipeManager.Recipes.Where(r => r is ShapelessRecipe).Where(r => ((ShapelessRecipe) r).Result.Id == result.Id && ((ShapelessRecipe) r).Result.Metadata == result.Metadata).ToList());
			Log.Debug($"Found {recipes.Count} matching recipes with the result {result}");
			foreach (var r in recipes)
			{
				if (r is ShapedRecipe)
				{
					var recipe = (ShapedRecipe) r;
					int rowOffset = -1;
					int colOffset = -1;
					int dim = UsingCraftingTable ? 3 : 2;
					for (int row = 0; row < dim; row++)
					{
						for (int col = 0; col < dim; col++)
						{
							var item = craftingInput[col + (dim*row)];
							if (item == null) continue;

							if (rowOffset == -1 && item.Id != 0)
							{
								rowOffset = row;
							}

							if (colOffset == -1 && item.Id != 0)
							{
								colOffset = col;
							}
						}
					}

					List<Item> shapedInputSmall = new List<Item>(new Item[recipe.Height*recipe.Width]);
					for (int row = 0; row < recipe.Height; row++)
					{
						for (int col = 0; col < recipe.Width; col++)
						{
							shapedInputSmall[col + (recipe.Width*row)] = craftingInput[(colOffset + col) + (dim*(row + rowOffset))];
						}
					}

					Log.Debug($"Items input={ToJson(craftingInput)}");
					Log.Debug($"Items input={ToJson(shapedInputSmall)}");
					Log.Debug($"Recipe input={ToJson(recipe.Input)}");

					var match = (recipe.Input.Length == shapedInputSmall.Count);
					Log.Debug($"ShapedRecipe Count match={match}");
					match = match && !shapedInputSmall.Except(recipe.Input, new ItemCompare()).Any();
					Log.Debug($"Items match={match}");
					if (match) return true;
				}
				else
				{
					var recipe = (ShapelessRecipe) r;
					var match = (recipe.Input.Count == shapelessInput.Count);
					Log.Debug($"ShapelessRecipe Count match={match}");
					match = match && !shapelessInput.Except(recipe.Input, new ItemCompare()).Any();
					Log.Debug($"Items match={match}");
					if (match) return true;
				}
			}

			return false;
		}

		private string ToJson(object obj)
		{
			var jsonSerializerSettings = new JsonSerializerSettings
			{
				PreserveReferencesHandling = PreserveReferencesHandling.Arrays,

				Formatting = Formatting.Indented,
			};
			jsonSerializerSettings.Converters.Add(new NbtIntConverter());
			jsonSerializerSettings.Converters.Add(new NbtStringConverter());

			return JsonConvert.SerializeObject(obj, jsonSerializerSettings);
		}

		private class ItemCompare : IEqualityComparer<Item>
		{
			public bool Equals(Item x, Item y)
			{
				if (ReferenceEquals(null, x)) return false;
				if (ReferenceEquals(null, y)) return false;
				if (ReferenceEquals(x, y)) return true;

				return x.Id == y.Id && (x.Metadata == y.Metadata || x.Metadata == -1 || y.Metadata == -1);
			}

			public int GetHashCode(Item obj)
			{
				return 0;
			}
		}

		public virtual void HandleMcpeContainerClose(McpeContainerClose message)
		{
			UsingCraftingTable = false;

			lock (_inventorySync)
			{
				if (_openInventory is Inventory inventory)
				{
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
				else if (_openInventory is HorseInventory horseInventory)
				{
					_openInventory = null;
				}
			}
		}

		public void HandleMcpePlayerHotbar(McpePlayerHotbar message)
		{
		}

		public void HandleMcpeInventoryContent(McpeInventoryContent message)
		{
		}

		/// <summary>
		///     Handles the interact.
		/// </summary>
		/// <param name="message">The message.</param>
		public virtual void HandleMcpeInteract(McpeInteract message)
		{
			if (!Level.TryGetEntity(message.targetRuntimeEntityId, out Entity target)) return;

			if (message.actionId != 4)
			{
				Log.Debug($"Interact Action ID: {message.actionId}");
				Log.Debug($"Interact Target Entity ID: {message.targetRuntimeEntityId}");
			}

			if (target == null) return;
			switch (message.actionId)
			{
				case 3:
				{
					if (Level.TryGetEntity(Vehicle, out Mob mob))
					{
						mob.Unmount(this);
					}

					break;
				}
				case 4:
				{
					// Mouse over
					DoMouseOverInteraction(message.actionId, this);
					target.DoMouseOverInteraction(message.actionId, this);
					break;
				}
				case 6:
				{
					// Riding; Open inventory
					if (IsRiding)
					{
						if (Level.TryGetEntity(Vehicle, out Mob mob) && mob is Horse horse)
						{
							horse.Inventory.Open(this);
						}
					}

					break;
				}
			}
		}

		public long Vehicle { get; set; }

		public virtual void HandleMcpeBlockPickRequest(McpeBlockPickRequest message)
		{
			if (GameMode != GameMode.Creative)
			{
				return;
			}

			Block block = Level.GetBlock(message.x, message.y, message.z);

			Item item = ItemFactory.GetItem((short) block.Id, block.Metadata);

			Inventory.SetInventorySlot(Inventory.InHandSlot, item);
		}

		public virtual void HandleMcpeEntityPickRequest(McpeEntityPickRequest message)
		{
			if (GameMode != GameMode.Creative)
			{
				return;
			}

			if (Level.Entities.TryGetValue((long) message.runtimeEntityId, out var entity))
			{
				Item item = ItemFactory.GetItem(383, (short) entity.EntityTypeId);

				Inventory.SetInventorySlot(Inventory.InHandSlot, item);
			}
		}

		protected virtual int CalculateDamage(Entity target)
		{
			int damage = Inventory.GetItemInHand().GetDamage(); //Item Damage.

			damage = (int) Math.Floor(damage*(1.0));

			return damage;
		}


		public virtual void HandleMcpeEntityEvent(McpeEntityEvent message)
		{
			if (Log.IsDebugEnabled)
			{
				Log.Debug("Entity Id:" + message.runtimeEntityId);
				Log.Debug("Entity Event Id:" + message.eventId);
				Log.Debug("Entity Event unknown:" + message.data);
			}

			switch (message.eventId)
			{
				case 34:
					RemoveExperienceLevels(message.data);
					break;
				case 57:
					var data = message.data;
					if (data != 0) BroadcastEntityEvent(57, data);
					break;
			}
		}

		public void SendRespawn()
		{
			McpeRespawn mcpeRespawn = McpeRespawn.CreateObject();
			mcpeRespawn.x = SpawnPosition.X;
			mcpeRespawn.y = SpawnPosition.Y;
			mcpeRespawn.z = SpawnPosition.Z;
			SendPacket(mcpeRespawn);
		}

		public void SendStartGame()
		{
			McpeStartGame startGame = McpeStartGame.CreateObject();
			startGame.entityIdSelf = EntityId;
			startGame.runtimeEntityId = EntityManager.EntityIdSelf;
			startGame.playerGamemode = (int) GameMode;
			startGame.spawn = SpawnPosition;
			startGame.unknown1 = new Vector2(KnownPosition.HeadYaw, KnownPosition.Pitch);
			startGame.seed = 12345;
			startGame.dimension = 0;
			startGame.generator = 1;
			startGame.gamemode = (int) GameMode;
			startGame.x = (int) SpawnPosition.X;
			startGame.y = (int) (SpawnPosition.Y + Height);
			startGame.z = (int) SpawnPosition.Z;
			startGame.hasAchievementsDisabled = true;
			startGame.dayCycleStopTime = (int) Level.WorldTime;
			startGame.eduMode = PlayerInfo.Edition == 1;
			startGame.rainLevel = 0;
			startGame.lightningLevel = 0;
			startGame.isMultiplayer = true;
			startGame.broadcastToLan = true;
			startGame.broadcastToXbl = true;
			startGame.enableCommands = EnableCommands;
			startGame.isTexturepacksRequired = false;
			startGame.gamerules = Level.GetGameRules();
			startGame.bonusChest = false;
			startGame.mapEnabled = false;
			startGame.trustPlayers = false;
			startGame.permissionLevel = (int) PermissionLevel;
			startGame.gamePublishSetting = 3;
			startGame.levelId = "1m0AAMIFIgA=";
			startGame.worldName = Level.LevelName;
			startGame.premiumWorldTemplateId = "";
			startGame.isTrial = false;
			startGame.currentTick = Level.TickTime;
			startGame.enchantmentSeed = 123456;

			startGame.blockstates = BlockFactory.Blockstates;

			SendPacket(startGame);
		}

		/// <summary>
		///     Sends the set spawn position packet.
		/// </summary>
		public void SendSetSpawnPosition()
		{
			McpeSetSpawnPosition mcpeSetSpawnPosition = McpeSetSpawnPosition.CreateObject();
			mcpeSetSpawnPosition.spawnType = 1;
			mcpeSetSpawnPosition.coordinates = (BlockCoordinates) SpawnPosition;
			SendPacket(mcpeSetSpawnPosition);
		}

		private object _sendChunkSync = new object();

		private void ForcedSendChunk(PlayerLocation position)
		{
			lock (_sendChunkSync)
			{
				var chunkPosition = new ChunkCoordinates(position);

				McpeWrapper chunk = Level.GetChunk(chunkPosition)?.GetBatch();
				if (!_chunksUsed.ContainsKey(chunkPosition))
				{
					_chunksUsed.Add(chunkPosition, chunk);
				}

				if (chunk != null)
				{
					SendPacket(chunk);
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

				for (int x = -1; x <= 1; x++)
				{
					for (int z = -1; z <= 1; z++)
					{
						McpeFullChunkData chunk = new McpeFullChunkData();
						chunk.chunkX = chunkPosition.X + x;
						chunk.chunkZ = chunkPosition.Z + z;
						chunk.chunkData = new byte[0];
						SendPacket(chunk);
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
				foreach (McpeWrapper chunk in Level.GenerateChunks(_currentChunkPosition, _chunksUsed, ChunkRadius))
				{
					if (chunk != null) SendPacket(chunk);

					if (++packetCount%16 == 0) Thread.Sleep(12);
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

				foreach (McpeWrapper chunk in Level.GenerateChunks(_currentChunkPosition, _chunksUsed, ChunkRadius))
				{
					if (chunk != null) SendPacket(chunk);

					if (++packetCount%16 == 0) Thread.Sleep(12);

					if (!IsSpawned && packetCount == 56)
					{
						InitializePlayer();
					}
				}

				Log.Debug($"Sent {packetCount} chunks for {chunkPosition}");
			}
			catch (Exception e)
			{
				Log.Error($"Failed sending chunks for {KnownPosition}", e);
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
				MaxValue = HealthManager.MaxHearts,
				Value = HealthManager.Hearts,
				Default = HealthManager.MaxHearts,
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
			attributesPackate.runtimeEntityId = EntityManager.EntityIdSelf;
			attributesPackate.attributes = attributes;
			SendPacket(attributesPackate);
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

		public virtual void SendForm(Form form)
		{
			CurrentForm = form;

			McpeModalFormRequest message = McpeModalFormRequest.CreateObject();
			message.formId = form.Id; // whatever
			message.data = form.ToJson();
			SendPacket(message);
		}

		public virtual void SendSetTime()
		{
			SendSetTime((int) Level.WorldTime);
		}

		public virtual void SendSetTime(int time)
		{
			McpeSetTime message = McpeSetTime.CreateObject();
			message.time = time;
			SendPacket(message);
		}

		public virtual void SendSetDownfall(int downfall)
		{
			McpeLevelEvent levelEvent = McpeLevelEvent.CreateObject();
			levelEvent.eventId = 3001;
			levelEvent.data = downfall;
			SendPacket(levelEvent);
		}

		public virtual void SendMovePlayer(bool teleport = false)
		{
			var packet = McpeMovePlayer.CreateObject();
			packet.runtimeEntityId = EntityManager.EntityIdSelf;
			packet.x = KnownPosition.X;
			packet.y = KnownPosition.Y + 1.62f;
			packet.z = KnownPosition.Z;
			packet.yaw = KnownPosition.Yaw;
			packet.headYaw = KnownPosition.HeadYaw;
			packet.pitch = KnownPosition.Pitch;
			packet.mode = (byte) (teleport ? 1 : 0);

			SendPacket(packet);
		}

		public override void OnTick(Entity[] entities)
		{
			OnTicking(new PlayerEventArgs(this));

			if (DetectInPortal())
			{
				if (PortalDetected == Level.TickTime)
				{
					PortalDetected = -1;

					Dimension dimension = Level.Dimension == Dimension.Overworld ? Dimension.Nether : Dimension.Overworld;
					Log.Debug($"Dimension change to {dimension} from {Level.Dimension} initiated, Game mode={GameMode}");

					ThreadPool.QueueUserWorkItem(delegate
					{
						Level oldLevel = Level;

						ChangeDimension(null, null, dimension, delegate
						{
							Level nextLevel = dimension == Dimension.Overworld ? oldLevel.OverworldLevel :
								dimension == Dimension.Nether ? oldLevel.NetherLevel : oldLevel.TheEndLevel;
							return nextLevel;
						});
					});
				}
				else if (PortalDetected == 0)
				{
					PortalDetected = Level.TickTime + (GameMode == GameMode.Creative ? 1 : 4*20);
				}
			}
			else
			{
				if (PortalDetected != 0) Log.Debug($"Reset portal detected");
				if (IsSpawned) PortalDetected = 0;
			}

			HungerManager.OnTick();

			base.OnTick(entities);

			if (LastAttackTarget != null && LastAttackTarget.HealthManager.IsDead)
			{
				LastAttackTarget = null;
			}

			foreach (var effect in Effects)
			{
				effect.Value.OnTick(this);
			}

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
								if (popup.CurrentTick%20 == 0 || popup.CurrentTick == popup.Duration + popup.DisplayDelay - 30)
									SendMessage(popup.Message, type: popup.MessageType);
							hasDisplayedTip = true;
						}

						// Popup is below
						if (popup.MessageType == MessageType.Popup && !hasDisplayedPopup)
						{
							if (popup.CurrentTick <= popup.Duration + popup.DisplayDelay - 30)
								if (popup.CurrentTick%20 == 0 || popup.CurrentTick == popup.Duration + popup.DisplayDelay - 30)
									SendMessage(popup.Message, type: popup.MessageType);
							hasDisplayedPopup = true;
						}
					}

					popup.CurrentTick++;
				}
			}

			OnTicked(new PlayerEventArgs(this));
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
			motions.runtimeEntityId = EntityManager.EntityIdSelf;
			motions.velocity = velocity;
			SendPacket(motions);
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
			metadata[27] = new MetadataByte((byte) (IsSleeping ? 0b10 : 0));
			//metadata[28] = new MetadataInt(1);
			metadata[(int) MetadataFlags.BedPosition] = new MetadataIntCoordinates((int) SpawnPosition.X, (int) SpawnPosition.Y, (int) SpawnPosition.Z);
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
				Level.RelayBroadcast(Level.CreateMcpeBatch(playerList.Encode()));
				playerList.records = null;
				playerList.PutPool();
			}
			{
				McpePlayerList playerList = McpePlayerList.CreateObject();
				playerList.records = new PlayerAddRecords {this};
				Level.RelayBroadcast(Level.CreateMcpeBatch(playerList.Encode()));
				playerList.records = null;
				playerList.PutPool();
			}
		}

		[Wired]
		public void SetEffect(Effect effect, bool ignoreIfLowerLevel = false)
		{
			if (Effects.ContainsKey(effect.EffectId))
			{
				if (ignoreIfLowerLevel && Effects[effect.EffectId].Level > effect.Level) return;

				effect.SendUpdate(this);
			}
			else
			{
				effect.SendAdd(this);
			}

			Effects[effect.EffectId] = effect;

			UpdatePotionColor();
		}

		[Wired]
		public void RemoveEffect(Effect effect, bool recalcColor = true)
		{
			if (Effects.ContainsKey(effect.EffectId))
			{
				effect.SendRemove(this);
				Effects.TryRemove(effect.EffectId, out effect);
			}


			if (recalcColor) UpdatePotionColor();
		}

		[Wired]
		public void RemoveAllEffects()
		{
			foreach (var effect in Effects)
			{
				RemoveEffect(effect.Value, false);
			}

			UpdatePotionColor();
		}

		public virtual void UpdatePotionColor()
		{
			if (Effects.Count == 0)
			{
				PotionColor = 0;
			}
			else
			{
				int a = 0xff;
				int r = 0, g = 0, b = 0;
				int levels = 0;
				foreach (var effect in Effects.Values)
				{
					var color = effect.ParticleColor;
					int level = effect.Level + 1;
					r += color.R*level;
					g += color.G*level;
					b += color.B*level;
					levels += level;
				}

				r /= levels;
				g /= levels;
				b /= levels;

				PotionColor = (int) (0xff000000 | (r << 16) | (g << 8) | b);
			}

			BroadcastSetEntityData();
		}

		public override void DespawnEntity()
		{
			IsSpawned = false;
			Level.DespawnFromAll(this);
		}

		public virtual void SendTitle(string text, TitleType type = TitleType.Title, int fadeIn = 6, int fadeOut = 6, int stayTime = 20, Player sender = null)
		{
			Level.BroadcastTitle(text, type, fadeIn, fadeOut, stayTime, sender, new[] {this});
		}

		public virtual void SendMessage(string text, MessageType type = MessageType.Chat, Player sender = null, bool needsTranslation = false, string[] parameters = null)
		{
			Level.BroadcastMessage(text, type, sender, new[] {this}, needsTranslation, parameters);
		}

		public override void BroadcastEntityEvent()
		{
			BroadcastEntityEvent(HealthManager.Health <= 0 ? 3 : 2);

			if (HealthManager.IsDead)
			{
				Player player = HealthManager.LastDamageSource as Player;
				BroadcastDeathMessage(player, HealthManager.LastDamageCause);
			}
		}

		public void BroadcastEntityEvent(int eventId, int data = 0)
		{
			{
				var entityEvent = McpeEntityEvent.CreateObject();
				entityEvent.runtimeEntityId = EntityManager.EntityIdSelf;
				entityEvent.eventId = (byte) eventId;
				entityEvent.data = data;
				SendPacket(entityEvent);
			}
			{
				var entityEvent = McpeEntityEvent.CreateObject();
				entityEvent.runtimeEntityId = EntityId;
				entityEvent.eventId = (byte) eventId;
				entityEvent.data = data;
				Level.RelayBroadcast(this, entityEvent);
			}
		}

		public virtual void BroadcastDeathMessage(Player player, DamageCause lastDamageCause)
		{
			string deathMessage = string.Format(HealthManager.GetDescription(lastDamageCause), Username, player == null ? "" : player.Username);
			Level.BroadcastMessage(deathMessage, type: MessageType.Raw);
			Log.Debug(deathMessage);
		}

		/// <summary>
		///     Very important litle method. This does all the sending of packets for
		///     the player class. Treat with respect!
		/// </summary>
		public void SendPacket(Packet packet)
		{
			if (NetworkHandler == null)
			{
				packet.PutPool();
			}
			else
			{
				NetworkHandler?.SendPacket(packet);
			}
		}

		private object _sendMoveListSync = new object();
		private DateTime _lastMoveListSendTime = DateTime.UtcNow;

		public void SendMoveList(McpeWrapper batch, DateTime sendTime)
		{
			if (sendTime < _lastMoveListSendTime || !Monitor.TryEnter(_sendMoveListSync))
			{
				batch.PutPool();
				return;
			}

			_lastMoveListSendTime = sendTime;

			try
			{
				SendPacket(batch);
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
			mcpeAddPlayer.entityIdSelf = EntityId;
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
			mcpeAddPlayer.flags = GetAdventureFlags();
			mcpeAddPlayer.commandPermission = (uint) CommandPermission;
			mcpeAddPlayer.actionPermissions = (uint) ActionPermissions;
			mcpeAddPlayer.permissionLevel = (uint) PermissionLevel;
			mcpeAddPlayer.userId = -1;

			int[] a = new int[5];

			//NOT WORKING: Reported to Mojang
			//if (IsRiding)
			//{
			//	mcpeAddPlayer.links = new Links()
			//	{
			//		new Tuple<long, long>(Vehicle, EntityId)
			//	};
			//}

			Level.RelayBroadcast(this, players, mcpeAddPlayer);

			if (IsRiding)
			{
				// This works if entities are spawned before players.

				McpeSetEntityLink link = McpeSetEntityLink.CreateObject();
				link.linkType = (byte) McpeSetEntityLink.LinkActions.Ride;
				link.riderId = EntityId;
				link.riddenId = Vehicle;
				Level.RelayBroadcast(players, link);
			}

			SendEquipmentForPlayer(players);

			SendArmorForPlayer(players);
		}

		public virtual void SendEquipmentForPlayer(Player[] receivers = null)
		{
			McpeMobEquipment mcpePlayerEquipment = McpeMobEquipment.CreateObject();
			mcpePlayerEquipment.runtimeEntityId = EntityId;
			mcpePlayerEquipment.item = Inventory.GetItemInHand();
			mcpePlayerEquipment.slot = 0;
			if (receivers == null)
			{
				Level.RelayBroadcast(this, mcpePlayerEquipment);
			}
			else
			{
				Level.RelayBroadcast(this, receivers, mcpePlayerEquipment);
			}
		}

		public virtual void SendArmorForPlayer(Player[] receivers = null)
		{
			McpeMobArmorEquipment mcpePlayerArmorEquipment = McpeMobArmorEquipment.CreateObject();
			mcpePlayerArmorEquipment.runtimeEntityId = EntityId;
			mcpePlayerArmorEquipment.helmet = Inventory.Helmet;
			mcpePlayerArmorEquipment.chestplate = Inventory.Chest;
			mcpePlayerArmorEquipment.leggings = Inventory.Leggings;
			mcpePlayerArmorEquipment.boots = Inventory.Boots;
			if (receivers == null)
			{
				Level.RelayBroadcast(this, mcpePlayerArmorEquipment);
			}
			else
			{
				Level.RelayBroadcast(this, receivers, mcpePlayerArmorEquipment);
			}
		}

		public override void DespawnFromPlayers(Player[] players)
		{
			McpeRemoveEntity mcpeRemovePlayer = McpeRemoveEntity.CreateObject();
			mcpeRemovePlayer.entityIdSelf = EntityId;
			Level.RelayBroadcast(this, players, mcpeRemovePlayer);
		}


		// Events

		public event EventHandler<PlayerEventArgs> PlayerJoining;

		protected virtual void OnPlayerJoining(PlayerEventArgs e)
		{
			PlayerJoining?.Invoke(this, e);
		}

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

		public event EventHandler<PlayerEventArgs> Ticking;

		protected virtual void OnTicking(PlayerEventArgs e)
		{
			Ticking?.Invoke(this, e);
		}

		public event EventHandler<PlayerEventArgs> Ticked;

		protected virtual void OnTicked(PlayerEventArgs e)
		{
			Ticked?.Invoke(this, e);
		}
	}

	public class PlayerEventArgs : EventArgs
	{
		public Player Player { get; }
		public Level Level { get; }

		public PlayerEventArgs(Player player)
		{
			Player = player;
			Level = player?.Level;
		}
	}
}
