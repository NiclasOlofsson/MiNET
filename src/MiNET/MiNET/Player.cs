#region LICENSE

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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Buffers.Binary;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Threading;
using fNbt;
using log4net;
using Microsoft.IO;
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
using MiNET.Utils.Metadata;
using MiNET.Utils.Nbt;
using MiNET.Utils.Skins;
using MiNET.Utils.Vectors;
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

		internal IInventory _openInventory;
		public PlayerInventory Inventory { get; set; }
		public ItemStackInventoryManager ItemStackInventoryManager { get; set; }

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

		public float MovementSpeed { get; set; } = 0.1f;
		public ConcurrentDictionary<EffectType, Effect> Effects { get; set; } = new ConcurrentDictionary<EffectType, Effect>();

		public HungerManager HungerManager { get; set; }
		public ExperienceManager ExperienceManager { get; set; }

		public bool IsFalling { get; set; }
		public bool IsFlyingHorizontally { get; set; }

		public Entity LastAttackTarget { get; set; }

		public List<Popup> Popups { get; set; } = new List<Popup>();

		public Session Session { get; set; }

		public DamageCalculator DamageCalculator { get; set; } = new DamageCalculator();


		public Player(MiNetServer server, IPEndPoint endPoint) : base(EntityType.None, null)
		{
			Server = server;
			EndPoint = endPoint;

			Inventory = new PlayerInventory(this);
			HungerManager = new HungerManager(this);
			ExperienceManager = new ExperienceManager(this);
			ItemStackInventoryManager = new ItemStackInventoryManager(this);

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

			var serverInfo = Server.ConnectionInfo;
			Interlocked.Increment(ref serverInfo.ConnectionsInConnectPhase);

			SendPlayerStatus(0);

			{
				SendResourcePacksInfo();
			}

			//MiNetServer.FastThreadPool.QueueUserWorkItem(() => { Start(null); });
		}

		public virtual void HandleMcpeScriptCustomEvent(McpeScriptCustomEvent message)
		{
		}

		public virtual void HandleMcpeCommandBlockUpdate(McpeCommandBlockUpdate message)
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
			chunkData.payload = content;
			SendPacket(chunkData);
		}

		public virtual void HandleMcpePurchaseReceipt(McpePurchaseReceipt message)
		{
		}

		public virtual void HandleMcpePlayerSkin(McpePlayerSkin message)
		{
			McpePlayerSkin pk = McpePlayerSkin.CreateObject();
			pk.uuid = this.ClientUuid;
			pk.skin = message.skin;
			pk.oldSkinName = this.Skin.SkinId;
			pk.skinName = message.skinName;
			this.Skin = message.skin;
			this.Level.RelayBroadcast(pk);
		}

		public virtual void HandleMcpePhotoTransfer(McpePhotoTransfer message)
		{
			// Handle photos from the camera. Override to provide your own implementaion because
			// no sensible default for MiNET.
		}

		protected Form CurrentForm { get; set; } = null;

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
				new Input
				{
					Text = "",
					Placeholder = "Placeholder",
					Value = ""
				},
				new Toggle
				{
					Text = "A toggler",
					Value = true
				},
				new Slider
				{
					Text = "A slider",
					Min = 0,
					Max = 10,
					Step = 2,
					Value = 3
				},
				new StepSlider
				{
					Text = "A step slider",
					Steps = new List<string>()
					{
						"Step 1",
						"Step 2",
						"Step 3"
					},
					Value = 1
				},
				new Dropdown
				{
					Text = "A step slider",
					Options = new List<string>()
					{
						"Option 1",
						"Option 2",
						"Option 3"
					},
					Value = 1
				},
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

		public virtual void HandleMcpeSetLocalPlayerAsInitialized(McpeSetLocalPlayerAsInitialized message)
		{
			OnLocalPlayerIsInitialized(new PlayerEventArgs(this));
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
				dataInfo.hash = new byte[] {57, 38, 13, 50, 39, 63, 88, 63, 59, 27, 63, 63, 63, 63, 6, 63, 54, 7, 84, 63, 47, 91, 63, 120, 63, 120, 42, 5, 104, 2, 63, 18};
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
				packInfo.behahaviorpackinfos = new ResourcePackInfos
				{
					new ResourcePackInfo()
					{
						UUID = "5abdb963-4f3f-4d97-8482-88e2049ab149",
						Version = "0.0.1",
						Size = 359901
					},
				};
			}

			SendPacket(packInfo);
		}

		public virtual void SendResourcePackStack()
		{
			McpeResourcePackStack packStack = McpeResourcePackStack.CreateObject();
			packStack.gameVersion = McpeProtocolInfo.GameVersion;
			
			if (_serverHaveResources)
			{
				packStack.mustAccept = false;
				packStack.resourcepackidversions = new ResourcePackIdVersions
				{
					new PackIdVersion()
					{
						Id = "5abdb963-4f3f-4d97-8482-88e2049ab149",
						Version = "0.0.1"
					},
				};
			}

			SendPacket(packStack);
		}

		public virtual void HandleMcpePlayerInput(McpePlayerInput message)
		{
			Log.Debug($"Player input: x={message.motionX}, z={message.motionZ}, jumping={message.jumping}, sneaking={message.sneaking}");
		}

		public virtual void HandleMcpeRiderJump(McpeRiderJump message)
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

		public void HandleMcpeTickSync(McpeTickSync message)
		{
			var msg = McpeTickSync.CreateObject();
			msg.requestTime = message.requestTime;
			msg.responseTime = message.responseTime;

			SendPacket(msg);
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

		public virtual void HandleMcpeMapInfoRequest(McpeMapInfoRequest message)
		{
			lock (_mapInfoSync)
			{
				//if(_mapSender == null)
				//{
				//	_mapSender = new Timer(Callback);
				//}

				long mapId = message.mapId;

				Log.Trace($"Requested map with ID: {mapId} 0x{mapId:X2}");

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
						mapEntity = new MapEntity(Level, mapId);
						mapEntity.SpawnEntity();
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

		public void SetChunkRadius(int radius)
		{
			ChunkRadius = Math.Max(5, Math.Min(radius, MaxViewDistance));
		}
		
		public virtual void HandleMcpeRequestChunkRadius(McpeRequestChunkRadius message)
		{
			Log.Debug($"Requested chunk radius of: {message.chunkRadius}");

			SetChunkRadius(message.chunkRadius);
			SendChunkRadiusUpdate();

			//if (_completedStartSequence)
			{
				MiNetServer.FastThreadPool.QueueUserWorkItem(SendChunksForKnownPosition);
			}
		}

		public virtual void HandleMcpeSetEntityMotion(McpeSetEntityMotion message)
		{
			//Level.RelayBroadcast((McpeSetEntityMotion) message.Clone());
		}

		public void HandleMcpeMoveEntity(McpeMoveEntity message)
		{
			//Level.RelayBroadcast((McpeMoveEntity) message.Clone());
			if (Vehicle == message.runtimeEntityId && Level.TryGetEntity(message.runtimeEntityId, out Entity entity))
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
						double breakTime = Math.Ceiling(target.Hardness * tooltypeFactor * 20);

						McpeLevelEvent breakEvent = McpeLevelEvent.CreateObject();
						breakEvent.eventId = 3600;
						breakEvent.position = message.coordinates;
						breakEvent.data = (int) (65535 / breakTime);
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
				//case PlayerAction.Respawn:
				//{
				//	MiNetServer.FastThreadPool.QueueUserWorkItem(HandleMcpeRespawn);
				//	break;
				//}
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
				case PlayerAction.CreativeDestroy:
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
				case PlayerAction.InteractBlock:
				{
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
					MovementSpeed += MovementSpeed * 0.3f;
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

			blockEntity.SetCompound((NbtCompound) message.namedtag.NbtFile.RootTag);
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
			McpeUpdateAdventureSettings settings = McpeUpdateAdventureSettings.CreateObject();
			settings.noPvm = IsNoPvm;
			settings.noMvp = IsNoMvp;
			settings.autoJump = IsAutoJump;
			settings.immutableWorld = IsWorldImmutable;
			settings.showNametags = true;
			SendPacket(settings);
		}

		public virtual void SendAbilities()
		{
			McpeUpdateAbilities packet = McpeUpdateAbilities.CreateObject();
			packet.layers = GetAbilities();
			packet.commandPermissions = (byte) CommandPermission;
			packet.playerPermissions = (byte) PermissionLevel;
			packet.entityUniqueId = BinaryPrimitives.ReverseEndianness(EntityId);
			SendPacket(packet);
		}

		private AbilityLayers GetAbilities()
		{
			PlayerAbility abilities = 0;
			PlayerAbility values = 0;

			if (GameMode.AllowsFlying())
			{
				abilities |= PlayerAbility.MayFly;

				if (IsFlying)
				{
					abilities |= PlayerAbility.Flying;
				}
			}

			if (!GameMode.HasCollision())
			{
				abilities |= PlayerAbility.NoClip;
			}

			if (!GameMode.AllowsTakingDamage())
			{
				abilities |= PlayerAbility.Invulnerable;
			}

			if (GameMode.HasCreativeInventory())
			{
				abilities |= PlayerAbility.InstantBuild;
			}

			if (GameMode.AllowsEditing())
			{
				abilities |= PlayerAbility.Build | PlayerAbility.Mine;
			}

			if (GameMode.AllowsInteraction())
			{
				abilities |= PlayerAbility.DoorsAndSwitches | PlayerAbility.OpenContainers | PlayerAbility.AttackPlayers | PlayerAbility.AttackMobs;
			}

			if (PermissionLevel == PermissionLevel.Operator)
			{
				abilities |= PlayerAbility.OperatorCommands;
			}

			if (IsMuted)
			{
				abilities |= PlayerAbility.Muted;
			}

			var layers = new AbilityLayers();

			var baseLayer = new AbilityLayer()
			{
				Type = AbilityLayerType.Base,
				Abilities = abilities,
				Values = (uint) abilities,
				FlySpeed = 0.05f,
				WalkSpeed = 0.1f
			};

			layers.Add(baseLayer);

			return layers;
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

			var serverInfo = Server.ConnectionInfo;

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

				SendAvailableEntityIdentifiers();

				SendBiomeDefinitionList();

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

				SendNetworkChunkPublisherUpdate();
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

		public virtual void SendAvailableEntityIdentifiers()
		{
			var nbt = new Nbt
			{
				NbtFile = new NbtFile
				{
					BigEndian = false,
					UseVarInt = true,
					RootTag = new NbtCompound("") {EntityHelpers.GenerateEntityIdentifiers()}
				}
			};

			var pk = McpeAvailableEntityIdentifiers.CreateObject();
			pk.namedtag = nbt;
			SendPacket(pk);
		}

		public virtual void SendBiomeDefinitionList()
		{
			var nbt = new Nbt
			{
				NbtFile = new NbtFile
				{
					BigEndian = false,
					UseVarInt = true,
					RootTag = BiomeUtils.GenerateDefinitionList(),
				}
			};

			var pk = McpeBiomeDefinitionList.CreateObject();
			pk.namedtag = nbt;
			SendPacket(pk);
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
			//return;
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

		//public virtual void HandleMcpeRespawn()
		//{
		//	HandleMcpeRespawn(null);
		//}

		public virtual void HandleMcpeRespawn(McpeRespawn message)
		{
			if (message.state == (byte) McpeRespawn.RespawnState.ClientReady)
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

				var mcpeRespawn = McpeRespawn.CreateObject();
				mcpeRespawn.x = SpawnPosition.X;
				mcpeRespawn.y = SpawnPosition.Y;
				mcpeRespawn.z = SpawnPosition.Z;
				mcpeRespawn.state = (byte) McpeRespawn.RespawnState.Ready;
				mcpeRespawn.runtimeEntityId = EntityId;
				SendPacket(mcpeRespawn);

				////send time again
				//SendSetTime();
				//IsSpawned = true;
				//LastUpdatedTime = DateTime.UtcNow;
				//_haveJoined = true;
			}
			else
			{
				Log.Warn($"Unhandled respawn state = {message.state}");
			}
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
					if (!Level.WorldProvider.HaveNether())
					{
						Log.Warn($"This world doesn't have nether");
						return;
					}
					break;
				case Dimension.TheEnd:
					if (!Level.WorldProvider.HaveTheEnd())
					{
						Log.Warn($"This world doesn't have the end");
						return;
					}
					break;
			}

			switch (dimension)
			{
				case Dimension.Overworld:
				{
					var start = (BlockCoordinates) KnownPosition;
					start *= new BlockCoordinates(8, 1, 8);
					SendChangeDimension(dimension, false, start);
					break;
				}
				case Dimension.Nether:
				{
					var start = (BlockCoordinates) KnownPosition;
					start /= new BlockCoordinates(8, 1, 8);
					SendChangeDimension(dimension, false, start);
					break;
				}
				case Dimension.TheEnd:
				{
					var start = (BlockCoordinates) KnownPosition;
					SendChangeDimension(dimension, false, start);
					break;
				}
				default:
					throw new ArgumentOutOfRangeException(nameof(dimension), dimension, null);
			}

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
							var portal = (Portal) level.GetBlock(coord);
							if (portal.PortalAxis == "z")
							{
								b &= level.IsBlock(coord.BlockNorth(), portalId);
							}
							else
							{
								b &= level.IsBlock(coord.BlockEast(), portalId);
							}

							Log.Debug($"Found portal block at {coord}, axis={portal.PortalAxis}");
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
							closestPortal = new PortalInfo()
							{
								Coordinates = coord,
								Size = bbox
							};
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

			int midPoint = depth > 2 ? depth / 2 : 0;

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
								level.SetBlock(new Portal
								{
									Coordinates = coordinates,
									PortalAxis = "x"
								});
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
								level.SetBlock(new Portal
								{
									Coordinates = coordinates,
									PortalAxis = "z",
								});
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
						//if (z == min.Z) if (!Level.GetBlockId(new BlockCoordinates(x, y, z)).IsBuildable) return false;
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
			var changeDimension = McpeChangeDimension.CreateObject();
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
			SendPacket(mcpeSetEntityData);

			base.BroadcastSetEntityData(metadata);
		}

		public void SendSetEntityData()
		{
			McpeSetEntityData mcpeSetEntityData = McpeSetEntityData.CreateObject();
			mcpeSetEntityData.runtimeEntityId = EntityManager.EntityIdSelf;
			mcpeSetEntityData.metadata = GetMetadata();
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
			//McpeInventoryContent strangeContent = McpeInventoryContent.CreateObject();
			//strangeContent.inventoryId = (byte) 0x7b;
			//strangeContent.input = new ItemStacks();
			//SendPacket(strangeContent);

			var inventoryContent = McpeInventoryContent.CreateObject();
			inventoryContent.inventoryId = (byte) 0x00;
			inventoryContent.input = Inventory.GetSlots();
			SendPacket(inventoryContent);

			var armorContent = McpeInventoryContent.CreateObject();
			armorContent.inventoryId = 0x78;
			armorContent.input = Inventory.GetArmor();
			SendPacket(armorContent);

			var uiContent = McpeInventoryContent.CreateObject();
			uiContent.inventoryId = 0x7c;
			uiContent.input = Inventory.GetUiSlots();
			SendPacket(uiContent);

			var offHandContent = McpeInventoryContent.CreateObject();
			offHandContent.inventoryId = 0x77;
			offHandContent.input = Inventory.GetOffHand();
			SendPacket(offHandContent);

			var mobEquipment = McpeMobEquipment.CreateObject();
			mobEquipment.runtimeEntityId = EntityManager.EntityIdSelf;
			mobEquipment.item = Inventory.GetItemInHand();
			mobEquipment.slot = (byte) Inventory.InHandSlot;
			mobEquipment.selectedSlot = (byte) Inventory.InHandSlot;
			SendPacket(mobEquipment);
		}

		public virtual void SendCraftingRecipes()
		{
			//TODO: Fix crafting recipe sending.
			
			/*McpeCraftingData craftingData = McpeCraftingData.CreateObject();
			craftingData.recipes = RecipeManager.Recipes;
			SendPacket(craftingData);*/
		}

		public virtual void SendCreativeInventory()
		{
			if (!UseCreativeInventory) return;

			var creativeContent = McpeCreativeContent.CreateObject();
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
			SendAbilities();
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
							var disconnect = McpeDisconnect.CreateObject();
							disconnect.message = reason;
							NetworkHandler.SendPacket(disconnect);
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

		public virtual void HandleMcpeText(McpeText message)
		{
			string text = message.message;

			if (string.IsNullOrEmpty(text)) return;

			Level.BroadcastMessage(text, sender: this);
		}

		private int _lastOrderingIndex;
		private object _moveSyncLock = new object();

		public virtual void HandleMcpeMovePlayer(McpeMovePlayer message)
		{
			if (!IsSpawned || HealthManager.IsDead) return;

			if (Server.ServerRole != ServerRole.Node)
			{
				lock (_moveSyncLock)
				{
					if (_lastOrderingIndex > message.ReliabilityHeader.OrderingIndex) return;
					_lastOrderingIndex = message.ReliabilityHeader.OrderingIndex;
				}
			}

			var origin = KnownPosition.ToVector3();
			double distanceTo = Vector3.Distance(origin, new Vector3(message.x, message.y - 1.62f, message.z));

			CurrentSpeed = distanceTo / ((double) (DateTime.UtcNow - LastUpdatedTime).Ticks / TimeSpan.TicksPerSecond);

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

			if (IsFalling)
			{
				if (StartFallY == 0) StartFallY = KnownPosition.Y;
			}
			else
			{
				double damage = StartFallY - KnownPosition.Y;
				if ((damage - 3) > 0)
				{
					HealthManager.TakeHit(null, (int) DamageCalculator.CalculatePlayerDamage(null, this, null, damage, DamageCause.Fall), DamageCause.Fall);
				}
				StartFallY = 0;
			}

			LastUpdatedTime = DateTime.UtcNow;

			var chunkPosition = new ChunkCoordinates(KnownPosition);
			if (_currentChunkPosition != chunkPosition && _currentChunkPosition.DistanceTo(chunkPosition) >= MoveRenderDistance)
			{
				MiNetServer.FastThreadPool.QueueUserWorkItem(SendChunksForKnownPosition);
			}
		}

		public double CurrentSpeed { get; private set; } = 0;
		public double StartFallY { get; private set; } = 0;

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

		public virtual void HandleMcpeLevelSoundEventOld(McpeLevelSoundEventOld message)
		{
			var sound = McpeLevelSoundEventOld.CreateObject();
			sound.soundId = message.soundId;
			sound.position = message.position;
			sound.blockId = message.blockId;
			sound.entityType = message.entityType;
			sound.isBabyMob = message.isBabyMob;
			sound.isGlobal = message.isGlobal;
			Level.RelayBroadcast(sound);
		}

		public virtual void HandleMcpeLevelSoundEvent(McpeLevelSoundEvent message)
		{
			//TODO: This will require that sounds are sent by the server.

			//var sound = McpeLevelSoundEvent.CreateObject();
			//sound.soundId = message.soundId;
			//sound.position = message.position;
			//sound.blockId = message.blockId;
			//sound.entityType = message.entityType;
			//sound.isBabyMob = message.isBabyMob;
			//sound.isGlobal = message.isGlobal;
			//Level.RelayBroadcast(sound);
		}

		public void HandleMcpeClientCacheStatus(McpeClientCacheStatus message)
		{
			Log.Warn($"Cache status: {(message.enabled ? "Enabled" : "Disabled")}");
		}

		public void HandleMcpeNetworkSettings(McpeNetworkSettings message)
		{
		}

		/// <inheritdoc />
		public void HandleMcpePlayerAuthInput(McpePlayerAuthInput message)
		{
			
		}


		public bool UsingAnvil { get; set; }

		public void HandleMcpeItemStackRequest(McpeItemStackRequest message)
		{
			var response = McpeItemStackResponse.CreateObject();
			response.responses = new ItemStackResponses();
			foreach (ItemStackActionList request in message.requests)
			{
				var stackResponse = new ItemStackResponse()
				{
					Result = StackResponseStatus.Ok,
					RequestId = request.RequestId,
					ResponseContainerInfos = new List<StackResponseContainerInfo>()
				};

				response.responses.Add(stackResponse);

				try
				{
					stackResponse.ResponseContainerInfos.AddRange(ItemStackInventoryManager.HandleItemStackActions(request.RequestId, request));
				}
				catch (Exception e)
				{
					Log.Warn($"Failed to process inventory actions", e);
					stackResponse.Result = StackResponseStatus.Error;
					stackResponse.ResponseContainerInfos.Clear();
				}
			}

			SendPacket(response);
		}

		protected Item GetContainerItem(int containerId, int slot)
		{
			if (UsingAnvil && containerId < 3) containerId = 13;

			Item item = null;
			switch (containerId)
			{
				case 13: // crafting
				case 21: // enchanting
				case 22: // enchanting
				case 41: // loom
				case 58: // cursor
				case 59: // creative
					item = Inventory.UiInventory.Slots[slot];
					break;
				case 12: // auto
				case 27: // hotbar
				case 28: // player inventory
					item = Inventory.Slots[slot];
					break;
				case 6: // armor
					item = slot switch
					{
						0 => Inventory.Helmet,
						1 => Inventory.Chest,
						2 => Inventory.Leggings,
						3 => Inventory.Boots,
						_ => null
					};
					break;
				case 7: // chest/container
					if (_openInventory is Inventory inventory) item = inventory.GetSlot((byte) slot);
					break;
				default:
					Log.Warn($"Unknown containerId: {containerId}");
					break;
			}

			return item;
		}

		protected void SetContainerItem(int containerId, int slot, Item item)
		{
			if (UsingAnvil && containerId < 3) containerId = 13;

			switch (containerId)
			{
				case 13: // crafting
				case 21: // enchanting
				case 22: // enchanting
				case 41: // loom
				case 58: // cursor
				case 59: // creative
					Inventory.UiInventory.Slots[slot] = item;
					break;
				case 12: // auto
				case 27: // hotbar
				case 28: // player inventory
					Inventory.Slots[slot] = item;
					break;
				case 6: // armor
					switch (slot)
					{
						case 0:
							Inventory.Helmet = item;
							break;
						case 1:
							Inventory.Chest = item;
							break;
						case 2:
							Inventory.Leggings = item;
							break;
						case 3:
							Inventory.Boots = item;
							break;
					}
					break;
				case 7: // chest/container
					if (_openInventory is Inventory inventory) inventory.SetSlot(this, (byte) slot, item);
					break;
				default:
					Log.Warn($"Unknown containerId: {containerId}");
					break;
			}
		}

		public void HandleMcpeUpdatePlayerGameType(McpeUpdatePlayerGameType message)
		{
		}

		public void HandleMcpePacketViolationWarning(McpePacketViolationWarning message)
		{
			Log.Error($"Client reported a level {message.severity} packet violation of type {message.violationType} for packet 0x{message.packetId:X2}: {message.reason}");
		}

		/// <inheritdoc />
		public void HandleMcpeFilterTextPacket(McpeFilterTextPacket message)
		{
			// Allow anvil renaming to work - this packet must be sent in response
			// You could also modify the contents to change the outcome.
			var packet = McpeFilterTextPacket.CreateObject();
			packet.text = message.text;
			packet.fromServer = true;
			SendPacket(packet);
		}

		/// <inheritdoc />
		public void HandleMcpeUpdateSubChunkBlocksPacket(McpeUpdateSubChunkBlocksPacket message)
		{
			
		}

		/// <inheritdoc />
		public void HandleMcpeSubChunkRequestPacket(McpeSubChunkRequestPacket message)
		{
			/*McpeSubChunkPacket response = McpeSubChunkPacket.CreateObject();
			if (message.dimension != (int) Level.Dimension)
			{
				response.requestResult = (int) SubChunkRequestResult.WrongDimension;
			}
			else
			{
				var chunk = Level.GetChunk(message.subchunkCoordinates);

				if (chunk == null)
				{
					response.requestResult = (int) SubChunkRequestResult.NoSuchChunk;
				}
				else
				{
					try
					{
						var subChunk = chunk.GetSubChunk(message.subchunkCoordinates.Y);

						using (MemoryStream ms = new MemoryStream())
						{
							subChunk.Write(ms);
							response.data = ms.ToArray();
						}
						//subChunk.Write();

						response.dimension = message.dimension;
						response.heightmapData = new HeightMapData(chunk.height);
						
						response.requestResult = (int) SubChunkRequestResult.Success;
					}
					catch (IndexOutOfRangeException)
					{
						response.requestResult = (int) SubChunkRequestResult.YIndexOutOfBounds;
					}
				}
			}
			
			SendPacket(response);*/
		}

		/// <inheritdoc />
		public void HandleMcpeRequestAbility(McpeRequestAbility message)
		{
			//TODO: Implement ability requests
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

			if (message.windowsId == 0)
			{
				byte selectedHotbarSlot = message.selectedSlot;
				if (selectedHotbarSlot > 8)
				{
					Log.Error($"Player {Username} called set equipment with held hotbar slot {message.selectedSlot} with item {message.item}");
					return;
				}

				if (Log.IsDebugEnabled) Log.Debug($"Player {Username} called set equipment with held hotbar slot {message.selectedSlot} with item {message.item}");

				Inventory.SetHeldItemSlot(selectedHotbarSlot, false);
				if (Log.IsDebugEnabled)
					Log.Debug($"Player {Username} now holding {Inventory.GetItemInHand()}");
			}
			else if (message.windowsId == 119)
			{
				if (message.slot != 1)
				{
					Log.Error($"Player {Username} called set equipment with offhand slot {message.slot} with item {message.item}");
					return;
				}

				if (Log.IsDebugEnabled) Log.Debug($"Player {Username} called set equipment with offhand slot {message.slot} with item {message.item}");

				var offHandItem = Inventory.OffHand;
			}
		}

		private object _inventorySync = new object();

		public virtual void SetOpenInventory(IInventory inventory)
		{
			_openInventory = inventory;
		}

		public void OpenInventory(BlockCoordinates inventoryCoord)
		{
			// https://github.com/pmmp/PocketMine-MP/blob/stable/src/pocketmine/network/mcpe/protocol/types/WindowTypes.php
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

				var containerSetContent = McpeInventoryContent.CreateObject();
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
				var sendSlot = McpeInventorySlot.CreateObject();
				sendSlot.inventoryId = inventory.WindowsId;
				sendSlot.slot = slot;
				//sendSlot.uniqueid = itemStack.UniqueId;
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
			switch (message.transaction)
			{
				case InventoryMismatchTransaction inventoryMismatchTransaction:
					HandleInventoryMismatchTransaction(inventoryMismatchTransaction);
					break;
				case ItemReleaseTransaction itemReleaseTransaction:
					HandleItemReleaseTransaction(itemReleaseTransaction);
					break;
				case ItemUseOnEntityTransaction itemUseOnEntityTransaction:
					HandleItemUseOnEntityTransaction(itemUseOnEntityTransaction);
					break;
				case ItemUseTransaction itemUseTransaction:
					HandleItemUseTransaction(itemUseTransaction);
					break;
				case NormalTransaction normalTransaction:
					HandleNormalTransaction(normalTransaction);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		protected virtual void HandleItemUseOnEntityTransaction(ItemUseOnEntityTransaction transaction)
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

		private void EntityItemInteract(ItemUseOnEntityTransaction transaction)
		{
			Item itemInHand = Inventory.GetItemInHand();
			if (itemInHand.Id != transaction.Item.Id || itemInHand.Metadata != transaction.Item.Metadata)
			{
				Log.Warn($"Attack item mismatch. Expected {itemInHand}, but client reported {transaction.Item}");
			}

			if (!Level.TryGetEntity(transaction.EntityId, out Entity target)) return;
			target.DoItemInteraction(this, itemInHand);
		}

		protected virtual void EntityInteract(ItemUseOnEntityTransaction transaction)
		{
			DoInteraction((int) transaction.ActionType, this);

			if (!Level.TryGetEntity(transaction.EntityId, out Entity target)) return;
			target.DoInteraction((int) transaction.ActionType, this);
		}

		protected virtual void EntityAttack(ItemUseOnEntityTransaction transaction)
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
				if (reducedDamage < damage)
				{
					player.Inventory.DamageArmor();
				}
				var fireAspectLevel = itemInHand.GetEnchantingLevel(EnchantingType.FireAspect);
				if (fireAspectLevel > 0)
				{
					player.HealthManager.Ignite(fireAspectLevel * 80);
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

		protected virtual void HandleInventoryMismatchTransaction(InventoryMismatchTransaction transaction)
		{
			Log.Warn($"Transaction mismatch");
		}

		protected virtual void HandleItemReleaseTransaction(ItemReleaseTransaction transaction)
		{
			Item itemInHand = Inventory.GetItemInHand();

			switch (transaction.ActionType)
			{
				case McpeInventoryTransaction.ItemReleaseAction.Release:
				{
					itemInHand.Release(Level, this, transaction.FromPosition);
					break;
				}
				case McpeInventoryTransaction.ItemReleaseAction.Use:
				{
					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
			}

			HandleTransactionRecords(transaction.TransactionRecords);
		}

		protected virtual void HandleItemUseTransaction(ItemUseTransaction transaction)
		{
			var itemInHand = Inventory.GetItemInHand();

			switch (transaction.ActionType)
			{
				case McpeInventoryTransaction.ItemUseAction.Place:
				{
					Level.Interact(this, itemInHand, transaction.Position, (BlockFace) transaction.Face, transaction.ClickPosition);
					break;
				}
				case McpeInventoryTransaction.ItemUseAction.Use:
				{
					itemInHand.UseItem(Level, this, transaction.Position);
					break;
				}
				case McpeInventoryTransaction.ItemUseAction.Destroy:
				{
					//TODO: Add face and other parameters to break. For logic in break block.
					Level.BreakBlock(this, transaction.Position, (BlockFace) transaction.Face);
					break;
				}
			}

			HandleTransactionRecords(transaction.TransactionRecords);
		}

		protected virtual void HandleNormalTransaction(NormalTransaction transaction)
		{
			HandleTransactionRecords(transaction.TransactionRecords);
		}

		protected virtual void HandleTransactionRecords(List<TransactionRecord> records)
		{
			if (records.Count == 0) return;

			foreach (TransactionRecord record in records)
			{
				//Item oldItem = record.OldItem;
				Item newItem = record.NewItem;
				//int slot = record.Slot;

				switch (record)
				{
					//case ContainerTransactionRecord rec:
					//{
					//	int inventoryId = rec.InventoryId;

					//	switch (inventoryId)
					//	{
					//		case 0: // Player inventory
					//		{
					//			Item existingItem = Inventory.Slots[slot];

					//			if (!newItem.Equals(existingItem)) Log.Warn($"Inventory mismatch. Client reported new item as {oldItem} and it did not match existing item {existingItem}");
					//			else Log.Debug($"Verified new inventory slot {slot} to {Inventory.Slots[slot]}");
					//			break;
					//		}
					//		case 119: // Armor inventory
					//		case 120: // Armor inventory
					//		case 121: // Creative inventory
					//		case 124: // Cursor inventory
					//			throw new Exception($"This should never happen with new inventory transactions");
					//		default:
					//		{
					//			throw new Exception($"This should never happen with new inventory transactions");
					//			////TODO Handle custom items, like player inventory and cursor

					//			//if (_openInventory != null)
					//			//{
					//			//	if (_openInventory is Inventory inventory && inventory.WindowsId == inventoryId)
					//			//	{
					//			//		//if (!oldItem.Equals(inventory.CraftRemoveIngredient((byte)slot))) Log.Warn($"Cursor mismatch. Client reported old item as {oldItem} and it did not match existing the item {inventory.CraftRemoveIngredient((byte)slot)}");

					//			//		// block inventories of various kinds (chests, furnace, etc)
					//			//		inventory.SetSlot(this, (byte) slot, newItem);
					//			//	}
					//			//	else if (_openInventory is HorseInventory horseInventory)
					//			//	{
					//			//		//if (!oldItem.Equals(horseInventory.CraftRemoveIngredient((byte)slot))) Log.Warn($"Cursor mismatch. Client reported old item as {oldItem} and it did not match existing the item {horseInventory.CraftRemoveIngredient((byte)slot)}");
					//			//		horseInventory.SetSlot(slot, newItem);
					//			//	}
					//			//}
					//			//break;
					//		}
					//	}
					//	break;
					//}

					//TODO Handle custom items, like player inventory and cursor. Not entirely sure how to handle this for crafting and similar inventories.
					case CraftTransactionRecord _:
					{
						throw new Exception($"This should never happen with new inventory transactions");
					}
					case CreativeTransactionRecord _:
					{
						throw new Exception($"This should never happen with new inventory transactions");
					}
					case WorldInteractionTransactionRecord _:
					{
						// Drop
						Item sourceItem = Inventory.GetItemInHand();

						if (newItem.Id != sourceItem.Id) Log.Warn($"Inventory mismatch. Client reported drop item as {newItem} and it did not match existing item {sourceItem}");

						byte count = newItem.Count;

						Item dropItem;
						if (sourceItem.Count == count)
						{
							dropItem = sourceItem;
							Inventory.ClearInventorySlot((byte) Inventory.InHandSlot);
						}
						else
						{
							dropItem = (Item) sourceItem.Clone();
							sourceItem.Count -= count;
							dropItem.Count = count;
							dropItem.UniqueId = Environment.TickCount;
						}

						DropItem(dropItem);
						break;
					}
				}
			}
		}

		public virtual ItemEntity DropItem(Item item)
		{
			var itemEntity = new ItemEntity(Level, item)
			{
				Velocity = KnownPosition.GetDirection().Normalize() * 0.3f,
				KnownPosition = KnownPosition + new Vector3(0f, 1.62f, 0f)
			};
			itemEntity.SpawnEntity();

			return itemEntity;
		}

		public virtual bool PickUpItem(ItemEntity item)
		{
			return Inventory.SetFirstEmptySlot(item.Item, true);
		}

		private bool VerifyRecipe(List<Item> craftingInput, Item result)
		{
			Log.Debug($"Looking for matching recipes with the result {result}");

			var recipes = RecipeManager.Recipes
				.Where(r => r is ShapedRecipe)
				.Where(r => ((ShapedRecipe) r).Result.First().Id == result.Id && ((ShapedRecipe) r).Result.First().Metadata == result.Metadata).ToList();

			recipes.AddRange(RecipeManager.Recipes
				.Where(r => r is ShapelessRecipe)
				.Where(r => ((ShapelessRecipe) r).Result.First().Id == result.Id && ((ShapelessRecipe) r).Result.First().Metadata == result.Metadata).ToList());

			Log.Debug($"Found {recipes.Count} matching recipes with the result {result}");

			if (recipes.Count == 0) return false;

			var input = craftingInput.Where(i => i != null && i.Id != 0).ToList();

			foreach (var recipe in recipes)
			{
				List<Item> ingredients = null;
				switch (recipe)
				{
					case ShapedRecipe shapedRecipe:
					{
						ingredients = shapedRecipe.Input.Where(i => i != null && i.Id != 0).ToList();
						break;
					}
					case ShapelessRecipe shapelessRecipe:
					{
						ingredients = shapelessRecipe.Input.Where(i => i != null && i.Id != 0).ToList();
						break;
					}
				}

				if (ingredients == null) continue;

				var match = input.Count == ingredients.Count;
				Log.Debug($"Recipe number of ingredients match={match}");

				match = match && !input.Except(ingredients, new ItemCompare()).Union(ingredients.Except(input, new ItemCompare())).Any();

				Log.Debug($"Ingredients match={match}");
				if (match) return true;
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
			jsonSerializerSettings.Converters.Add(new IPAddressConverter());
			jsonSerializerSettings.Converters.Add(new IPEndPointConverter());

			return JsonConvert.SerializeObject(obj, jsonSerializerSettings);
		}

		private class ItemCompare : IEqualityComparer<Item>
		{
			public bool Equals(Item x, Item y)
			{
				if (ReferenceEquals(null, x)) return false;
				if (ReferenceEquals(null, y)) return false;
				if (ReferenceEquals(x, y)) return true;

				return x.Id == y.Id && (x.Metadata == y.Metadata || x.Metadata == short.MaxValue || y.Metadata == short.MaxValue);
			}

			public int GetHashCode(Item obj)
			{
				return 0;
			}
		}

		public virtual void HandleMcpeContainerClose(McpeContainerClose message)
		{
			UsingAnvil = false;

			lock (_inventorySync)
			{
				if (_openInventory is Inventory inventory)
				{
					_openInventory = null;

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

					var closePacket = McpeContainerClose.CreateObject();
					closePacket.windowId = inventory.WindowsId;
					closePacket.server = message == null ? true : false;
					SendPacket(closePacket);
				}
				else if (_openInventory is HorseInventory horseInventory)
				{
					_openInventory = null;
				}
				else
				{
					var closePacket = McpeContainerClose.CreateObject();
					closePacket.windowId = 0;
					closePacket.server = message == null ? true : false;
					SendPacket(closePacket);
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
			//Log.Info($"Interact. Target={message.targetRuntimeEntityId} Action={message.actionId} Position={message.Position}");
			Entity target = null;
			long runtimeEntityId = message.targetRuntimeEntityId;
			if (runtimeEntityId == EntityManager.EntityIdSelf)
			{
				target = this;
			}
			else if (!Level.TryGetEntity(runtimeEntityId, out target))
			{
				return;
			}

			if (message.actionId != 4)
			{
				Log.Debug($"Interact Action ID: {message.actionId}");
				Log.Debug($"Interact Target Entity ID: {runtimeEntityId}");
			}

			if (target == null) return;
			switch ((McpeInteract.Actions)message.actionId)
			{
				case McpeInteract.Actions.LeaveVehicle:
				{
					if (Level.TryGetEntity(Vehicle, out Mob mob))
					{
						mob.Unmount(this);
					}

					break;
				}
				case McpeInteract.Actions.MouseOver:
				{
					// Mouse over
					DoMouseOverInteraction(message.actionId, this);
					target.DoMouseOverInteraction(message.actionId, this);
					break;
				}
				case McpeInteract.Actions.OpenInventory:
				{
					if (target == this)
					{
						var containerOpen = McpeContainerOpen.CreateObject();
						containerOpen.windowId = 0;
						containerOpen.type = 255;
						containerOpen.runtimeEntityId = EntityManager.EntityIdSelf;
						SendPacket(containerOpen);
					}
					else if (IsRiding) // Riding; Open inventory
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
			Log.Debug($"Picked block {block.Name} from blockstate {block.GetRuntimeId()}. Expected block to be in slot {message.selectedSlot}");
			Item item = block.GetItem();
			if (item is ItemBlock blockItem)
			{
				Log.Debug($"Have BlockItem with block state {blockItem.Block.GetRuntimeId()}");
			}
			if (item == null) return;

			Inventory.SetInventorySlot(Inventory.InHandSlot, item, true);
		}

		public virtual void HandleMcpeEntityPickRequest(McpeEntityPickRequest message)
		{
			if (GameMode != GameMode.Creative)
			{
				return;
			}

			if (Level.Entities.TryGetValue((long) message.runtimeEntityId, out var entity))
			{
				Item item = ItemFactory.GetItem(383, (short) EntityHelpers.ToEntityType(entity.EntityTypeId));

				Inventory.SetInventorySlot(Inventory.InHandSlot, item);
			}
		}

		protected virtual int CalculateDamage(Entity target)
		{
			int damage = Inventory.GetItemInHand().GetDamage(); //Item Damage.

			damage = (int) Math.Floor(damage * (1.0));

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
				{
					ExperienceManager.RemoveExperienceLevels(message.data);
					break;
				}
				case 57:
				{
					int data = message.data;
					if (data != 0) BroadcastEntityEvent(57, data);
					break;
				}
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
			var levelSettings = new LevelSettings();
			levelSettings.spawnSettings = new SpawnSettings()
			{
				Dimension = (int)(Level?.Dimension ?? 0),
				BiomeName = "",
				BiomeType = 0
			};
			levelSettings.seed = 12345;
			levelSettings.generator = 1;
			levelSettings.gamemode = (int) GameMode;
			levelSettings.x = (int) SpawnPosition.X;
			levelSettings.y = (int) (SpawnPosition.Y + Height);
			levelSettings.z = (int) SpawnPosition.Z;
			levelSettings.hasAchievementsDisabled = true;
			levelSettings.time = (int) Level.WorldTime;
			levelSettings.eduOffer = PlayerInfo.Edition == 1 ? 1 : 0;
			levelSettings.rainLevel = 0;
			levelSettings.lightningLevel = 0;
			levelSettings.isMultiplayer = true;
			levelSettings.broadcastToLan = true;
			levelSettings.enableCommands = EnableCommands;
			levelSettings.isTexturepacksRequired = false;
			levelSettings.gamerules = Level.GetGameRules();
			levelSettings.bonusChest = false;
			levelSettings.mapEnabled = false;
			levelSettings.permissionLevel = (byte) PermissionLevel;
			levelSettings.gameVersion = "";
			levelSettings.hasEduFeaturesEnabled = true;
			
			var startGame = McpeStartGame.CreateObject();
			startGame.levelSettings = levelSettings;
			startGame.entityIdSelf = EntityId;
			startGame.runtimeEntityId = EntityManager.EntityIdSelf;
			startGame.playerGamemode = (int) GameMode;
			startGame.spawn = SpawnPosition;
			startGame.rotation = new Vector2(KnownPosition.HeadYaw, KnownPosition.Pitch);
			
			startGame.levelId = "1m0AAMIFIgA=";
			startGame.worldName = Level.LevelName;
			startGame.premiumWorldTemplateId = "";
			startGame.isTrial = false;
			startGame.currentTick = Level.TickTime;
			startGame.enchantmentSeed = 123456;
			startGame.movementType = 0;

			//startGame.blockPalette = BlockFactory.BlockPalette;
			startGame.itemstates = ItemFactory.Itemstates;

			startGame.enableNewInventorySystem = true;
			startGame.blockPaletteChecksum = 0;
			startGame.serverVersion = McpeProtocolInfo.GameVersion;
			startGame.propertyData = new Nbt() {NbtFile = new NbtFile()};
			startGame.worldTemplateId = new UUID(Guid.Empty.ToByteArray());

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
						var chunk = new McpeLevelChunk();
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

		public void SendNetworkChunkPublisherUpdate()
		{
			var pk = McpeNetworkChunkPublisherUpdate.CreateObject();
			pk.coordinates = KnownPosition.GetCoordinates3D();
			pk.radius = (uint) (MaxViewDistance * 16);
			SendPacket(pk);
		}

		public void ForcedSendChunks(Action postAction = null)
		{
			Monitor.Enter(_sendChunkSync);
			try
			{
				var chunkPosition = new ChunkCoordinates(KnownPosition);

				_currentChunkPosition = chunkPosition;

				if (Level == null) return;

				SendNetworkChunkPublisherUpdate();
				int packetCount = 0;
				foreach (McpeWrapper chunk in Level.GenerateChunks(_currentChunkPosition, _chunksUsed, ChunkRadius))
				{
					if (chunk != null) SendPacket(chunk);

					if (++packetCount % 16 == 0) Thread.Sleep(12);
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

				SendNetworkChunkPublisherUpdate();

				foreach (McpeWrapper chunk in Level.GenerateChunks(_currentChunkPosition, _chunksUsed, ChunkRadius, () => KnownPosition))
				{
					if (chunk != null) SendPacket(chunk);

					if (++packetCount % 16 == 0) Thread.Sleep(12);

					if (!IsSpawned && packetCount == 56)
					{
						InitializePlayer();
					}
				}

				Log.Debug($"Sent {packetCount} chunks for {chunkPosition} with view distance {MaxViewDistance}");
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
			attributes["minecraft:follow_range"] = new PlayerAttribute
			{
				Name = "minecraft:follow_range",
				MinValue = 0,
				MaxValue = 2048,
				Value = 16,
				Default = 16,
			};
			// Workaround, bad design.
			attributes = HungerManager.AddHungerAttributes(attributes);
			attributes = ExperienceManager.AddExperienceAttributes(attributes);

			McpeUpdateAttributes attributesPackate = McpeUpdateAttributes.CreateObject();
			attributesPackate.runtimeEntityId = EntityManager.EntityIdSelf;
			attributesPackate.attributes = attributes;
			SendPacket(attributesPackate);
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

		public void SendSound(BlockCoordinates position, LevelSoundEventType sound, int blockId = 0)
		{
			var packet = McpeLevelSoundEvent.CreateObject();
			packet.position = position;
			packet.soundId = (uint) sound;
			packet.blockId = blockId;
			SendPacket(packet);
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
					PortalDetected = Level.TickTime + (GameMode == GameMode.Creative ? 1 : 4 * 20);
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
								if (popup.CurrentTick % 20 == 0 || popup.CurrentTick == popup.Duration + popup.DisplayDelay - 30)
									SendMessage(popup.Message, type: popup.MessageType);
							hasDisplayedTip = true;
						}

						// Popup is below
						if (popup.MessageType == MessageType.Popup && !hasDisplayedPopup)
						{
							if (popup.CurrentTick <= popup.Duration + popup.DisplayDelay - 30)
								if (popup.CurrentTick % 20 == 0 || popup.CurrentTick == popup.Duration + popup.DisplayDelay - 30)
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
			metadata[(int) MetadataFlags.NameTag] = new MetadataString(NameTag ?? Username);
			metadata[(int) MetadataFlags.ButtonText] = new MetadataString(ButtonText ?? string.Empty);
			metadata[(int) MetadataFlags.PlayerFlags] = new MetadataByte((byte) (IsSleeping ? 0b10 : 0));
			metadata[(int) MetadataFlags.BedPosition] = new MetadataIntCoordinates((int) SpawnPosition.X, (int) SpawnPosition.Y, (int) SpawnPosition.Z);

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
				var playerList = McpePlayerList.CreateObject();
				playerList.records = new PlayerRemoveRecords {this};
				Level.RelayBroadcast(Level.CreateMcpeBatch(playerList.Encode())); // Replace with records, to remove need for player and encode
				playerList.records = null;
				playerList.PutPool();
			}
			{
				var playerList = McpePlayerList.CreateObject();
				playerList.records = new PlayerAddRecords {this};
				Level.RelayBroadcast(Level.CreateMcpeBatch(playerList.Encode())); // Replace with records, to remove need for player and encode
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
				int r = 0, g = 0, b = 0;
				int levels = 0;
				foreach (var effect in Effects.Values)
				{
					if (!effect.Particles) continue;

					var color = effect.ParticleColor;
					int level = effect.Level + 1;
					r += color.R * level;
					g += color.G * level;
					b += color.B * level;
					levels += level;
				}

				if (levels == 0)
				{
					PotionColor = 0;
				}
				else
				{
					r /= levels;
					g /= levels;
					b /= levels;

					PotionColor = (int) (0xff000000 | (r << 16) | (uint) (g << 8) | (uint) b);
				}
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

		public void CleanCache(ChunkColumn chunk)
		{
			lock (_sendChunkSync)
			{
				_chunksUsed.Remove(new ChunkCoordinates(chunk.X, chunk.Z));
			}
		}

		public virtual void DropInventory()
		{
			var slots = Inventory.Slots;
			var uiSlots = Inventory.UiInventory.Slots;

			Vector3 coordinates = KnownPosition.ToVector3();
			coordinates.Y += 0.5f;

			foreach (var stack in slots.ToArray())
			{
				Level.DropItem(coordinates, stack);
			}

			foreach (var stack in uiSlots.ToArray())
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
			/*mcpeAddPlayer.flags = GetAdventureFlags();
			mcpeAddPlayer.commandPermission = (uint) CommandPermission;
			mcpeAddPlayer.actionPermissions = (uint) ActionPermissions;
			mcpeAddPlayer.permissionLevel = (uint) PermissionLevel;
			mcpeAddPlayer.userId = -1;*/
			mcpeAddPlayer.deviceId = PlayerInfo.DeviceId;
			mcpeAddPlayer.deviceOs = PlayerInfo.DeviceOS;
			mcpeAddPlayer.gameType = (uint) GameMode;
			mcpeAddPlayer.layers = GetAbilities();

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
			var mcpePlayerEquipment = McpeMobEquipment.CreateObject();
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

		public event EventHandler<PlayerEventArgs> LocalPlayerIsInitialized;

		protected virtual void OnLocalPlayerIsInitialized(PlayerEventArgs e)
		{
			LocalPlayerIsInitialized?.Invoke(this, e);
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

		public virtual void HandleMcpeNetworkStackLatency(McpeNetworkStackLatency message)
		{
			var packet = McpeNetworkStackLatency.CreateObject();
			packet.timestamp = message.timestamp; // don't know what is it
			packet.unknownFlag = 1;
			SendPacket(packet);
		}

		public virtual void HandleMcpeLevelSoundEventV2(McpeLevelSoundEventV2 message)
		{
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
