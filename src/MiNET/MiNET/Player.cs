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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading;
using fNbt;
using log4net;
using Microsoft.IO;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Camera;
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
using MiNET.Utils.Diagnostics;
using MiNET.Utils.Metadata;
using MiNET.Utils.Nbt;
using MiNET.Utils.Skins;
using MiNET.Utils.Vectors;
using MiNET.Worlds;
using MiNET.Worlds.BlobCache;
using Newtonsoft.Json;

namespace MiNET
{
	public class Player : Entity, IMcpeMessageHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Player));

		private MiNetServer Server { get; set; }
		public IPEndPoint EndPoint { get; private set; }
		public INetworkHandler NetworkHandler { get; set; }

		// Which columns this player already holds, and which version of each. A column is pushed
		// once and never again until its content actually changes, which is what the version says.
		private Dictionary<ChunkCoordinates, long> _chunksUsed = new Dictionary<ChunkCoordinates, long>();
		private ChunkCoordinates _currentChunkPosition;

		/// <summary>What the player has open. Never null: closing everything leaves the player's own
		/// inventory screen.</summary>
		public Screen Screen { get; private set; } = new Screen(ScreenKind.Inventory);

		public PlayerInventory Inventory { get; set; }
		public ItemStackInventoryManager ItemStackInventoryManager { get; set; }

		public PlayerLocation SpawnPosition { get; set; }
		public bool IsSleeping { get; set; } = false;

		public int MaxViewDistance { get; set; } = 22;
		public int MoveRenderDistance { get; set; } = 1;

		/// <summary>
		///     Chunks sent between pauses while streaming, and how long to pause. The pause exists
		///     to keep a join burst from burying the session's send queue under thousands of ordered
		///     datagrams while everything else on the session waits behind them. Measured on a
		///     radius 32 join: 3209 chunks took 3.4s, of which 2.4s was these sleeps. Set the delay
		///     to 0 to stream flat out.
		/// </summary>

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

		// player_list add-record colour, ARGB (protocol 800+). Vanilla BDS 1.26.34 sends 0xFFEDEDED
		// for an ordinary player, verified against a live capture. It used to default to 0, which is
		// not "no colour" but fully transparent black, and the client has to draw the row in it.
		public int PlayerListColor { get; set; } = unchecked((int) 0xFFEDEDED);

		public Skin Skin { get; set; }

		public float MovementSpeed { get; set; } = 0.1f;
		public float FlySpeed { get; set; } = 0.05f;
		public float VerticalFlySpeed { get; set; } = 1.0f;

		// Player attribute values for SendUpdateAttributes, vanilla defaults. The ranges and
		// defaults in the attribute table are protocol constants; these are the live values,
		// plugin-settable like any other player state.
		public float FollowRange { get; set; } = 16;
		public float KnockbackResistance { get; set; } = 0;
		public float UnderwaterMovementSpeed { get; set; } = 0.02f;
		public float LavaMovementSpeed { get; set; } = 0.02f;
		public float Luck { get; set; } = 0;
		public float FrictionModifier { get; set; } = 1;
		public float Bounciness { get; set; } = 0;
		public float AirDragModifier { get; set; } = 1;

		public ConcurrentDictionary<EffectType, Effect> Effects { get; set; } = new ConcurrentDictionary<EffectType, Effect>();

		public HungerManager HungerManager { get; set; }
		public ExperienceManager ExperienceManager { get; set; }
		public CameraManager CameraManager { get; set; }

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
			CameraManager = new CameraManager(this);
			ItemStackInventoryManager = new ItemStackInventoryManager(this);

			AttackDamage = 1; // vanilla player base (Entity defaults to the mob value 2)

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

		// The join clock and how far this join has got. Started when the player object exists, which
		// is the first moment the server owns the join; everything before it is the transport's and
		// is measured there. _joinStage is what join.abandoned is tagged with, so a join that dies
		// names the stage it died after rather than only that it died.
		private readonly long _joinStartedAt = Stopwatch.GetTimestamp();
		private JoinStage _joinStage = JoinStage.None;

		private void CompleteJoinStage(JoinStage stage)
		{
			_joinStage = stage;
			EngineMetrics.RecordJoinStage(stage, Username, _joinStartedAt);
		}

		public virtual void HandleMcpeClientToServerHandshake(McpeClientToServerHandshake message)
		{
			// Beware that message might be null here.

			var serverInfo = Server.ConnectionInfo;
			Interlocked.Increment(ref serverInfo.ConnectionsInConnectPhase);

			SendPlayerStatus(McpePlayStatus.PlayStatus.LoginSuccess);

			{
				SendResourcePacksInfo();
			}

			CompleteJoinStage(JoinStage.Handshake);

			//MiNetServer.FastThreadPool.QueueUserWorkItem(() => { Start(null); });
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
			InvalidateRosterSlices();
			this.Level.RelayBroadcast(pk);
		}

		private PlayerListRecordSlices _rosterSlices;

		/// <summary>
		///     This player's cached player-list Add record fragments, built on first roster
		///     inclusion. Two racing builders both produce correct slices; the loser releases its
		///     skin-store acquisition so refcounts stay exact.
		/// </summary>
		public PlayerListRecordSlices GetOrBuildRosterSlices()
		{
			PlayerListRecordSlices slices = _rosterSlices;
			if (slices != null) return slices;

			slices = PlayerListRecordSlices.Build(this);
			PlayerListRecordSlices raced = Interlocked.CompareExchange(ref _rosterSlices, slices, null);
			if (raced != null)
			{
				slices.Release();
				return raced;
			}

			return slices;
		}

		/// <summary>
		///     Drops the cached record fragments after anything they encode changes (display name,
		///     skin) or when the player leaves. Rosters already borrowing the old arrays stay
		///     valid: the arrays are GC-owned and only the store refcount moves.
		/// </summary>
		public void InvalidateRosterSlices()
		{
			Interlocked.Exchange(ref _rosterSlices, null)?.Release();
		}

		public virtual void HandleMcpePhotoTransfer(McpePhotoTransfer message)
		{
			// Handle photos from the camera. Override to provide your own implementaion because
			// no sensible default for MiNET.
		}

		protected Form CurrentForm { get; set; } = null;

		public virtual void HandleMcpeModalFormResponse(McpeModalFormResponse message)
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

		public virtual void HandleMcpeServerSettingsRequest(McpeServerSettingsRequest message)
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
			// Fallback is the "inherit the level's mode" sentinel StartGame sends, and the client
			// acknowledges it verbatim. It is not a mode: storing it leaves GameMode matching
			// nothing at all, which silently disables every creative-gated path.
			var requested = (GameMode) message.gamemode;
			GameMode gameMode = requested == GameMode.Fallback ? Level.GameMode : requested;
			if (!Enum.IsDefined(gameMode))
			{
				Log.Warn($"Ignoring SetPlayerGameType with unknown game mode {message.gamemode}");
				return;
			}

			SetGameMode(gameMode);
		}

		public virtual void HandleMcpeLabTable(McpeLabTable message)
		{
		}

		private readonly ManualResetEventSlim _localPlayerInitialized = new(false);

		public virtual void HandleMcpeSetLocalPlayerAsInitialized(McpeSetLocalPlayerAsInitialized message)
		{
			// The client sends this in the same instant it closes its loading screen (traced
			// 2026-08-17): the true "I am in the world" edge. The join's chunk flood holds for it.
			_localPlayerInitialized.Set();

			OnLocalPlayerIsInitialized(new PlayerEventArgs(this));
		}

		private bool _serverHaveResources = false;

		public virtual void HandleMcpeResourcePackClientResponse(McpeResourcePackClientResponse message)
		{
			if (Log.IsDebugEnabled) Log.Debug($"Handled packet 0x{message.Id:X2}");

			if (message.response is ResourcePackClientResponseDownloading)
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
			else if (message.response is ResourcePackClientResponseDownloadingFinished)
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
			else if (message.response is ResourcePackClientResponseResourcePackStackFinished)
			{
				CompleteJoinStage(JoinStage.Packs);

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
			packInfo.worldTemplateIdAndVersion = new PackIdVersion
			{
				packUuid = (UUID) Guid.Empty,
				packVersion = "0.0.0" // vanilla sends this, not an empty string
			};
			packInfo.resourcePacks = new List<PackInfoData>();
			if (_serverHaveResources)
			{
				packInfo.mustAccept = false;
				packInfo.resourcePacks.Add(new PackInfoData
				{
					packIdVersion = new PackIdVersion
					{
						packUuid = new UUID("5abdb963-4f3f-4d97-8482-88e2049ab149"),
						packVersion = "0.0.1"
					},
					packSize = 359901,
					contentKey = "",
					subpackName = "",
					contentIdentity = "",
					cdnUrl = ""
				});
			}

			SendPacket(packInfo);
		}

		public virtual void SendResourcePackStack()
		{
			McpeResourcePackStack packStack = McpeResourcePackStack.CreateObject();
			// Vanilla sends "*" here, not the concrete game version.
			packStack.gameVersion = "*";
			
			if (_serverHaveResources)
			{
				packStack.mustAccept = false;
				packStack.resourcepackidversions = new ResourcePackIdVersions
				{
					new LegacyPackIdVersion()
					{
						Id = "5abdb963-4f3f-4d97-8482-88e2049ab149",
						Version = "0.0.1"
					},
				};
			}

			SendPacket(packStack);
		}

		public virtual void HandleMcpeSetEntityData(McpeSetEntityData message)
		{
			// Only used by EDU NPC so far.
			if (Level.TryGetEntity(message.runtimeEntityId, out Entity entity))
			{
				entity.SetEntityData(message.metadata);
			}
		}

		public virtual void HandleMcpeNpcRequest(McpeNpcRequest message)
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
			SendPacket(McpeClientboundMapItemData.FromMapInfo(mapInfo));
		}

		/// <summary>Chunk radius vanilla publishes during the join burst, before negotiation.</summary>
		/// <summary>
		///     Pacing for the skeleton stream. Each column is pre-compressed into its own
		///     <see cref="McpeWrapper" />, and a wrapper cannot nest inside another, so the send lane
		///     passes every one through as its own SCTP message: a radius-64 pass is 16,641 messages
		///     handed to one session back to back, with everything else that player needs queued behind
		///     them.
		///     <para>
		///     This pacing was removed once on the grounds that the send queue already paces it. It does
		///     not: the queue is an unbounded channel, so the producer never blocks and never feels the
		///     transport's backpressure at all. The only real limit is the SCTP window, which stalls the
		///     LANE rather than the loop feeding it, which is how send queue depth reached 1,284 packets
		///     under load with the producer already finished.
		///     </para>
		///     <para>Set <see cref="ChunkSendDelayMs" /> to 0 to send unpaced.</para>
		/// </summary>
		public int ChunkSendBatchSize { get; set; } = 16;

		/// <inheritdoc cref="ChunkSendBatchSize" />
		public int ChunkSendDelayMs { get; set; } = 12;

		public const int JoinBurstChunkRadius = 4;

		/// <summary>
		///     The most blob ids one ClientCacheBlobStatus may carry (Tomcc's design gist; bigger
		///     packets can be rejected).
		/// </summary>
		private const int MaxBlobStatusIds = 4095;

		/// <summary>
		///     Hash count at which a chunk group flushes mid-sweep: what one ClientCacheBlobStatus
		///     can answer. Tick-sized blocks (250) were tried 2026-08-17 and made the join WORSE,
		///     so small is not better here. int.MaxValue turns grouping off.
		/// </summary>
		private const int GroupFlushHashes = MaxBlobStatusIds;

		public int ChunkRadius { get; private set; } = -1;

		/// <summary>
		///     The radius the client last asked for, before any cap this server applies. Kept so an
		///     adaptive reduction can be walked back up toward what the player actually wants rather
		///     than toward whatever we last forced on them.
		/// </summary>
		private int _requestedChunkRadius = -1;

		// Columns the client has come back for. Kept for the request-latency metric.
		private long _columnsDrained;

		/// <summary>Smallest radius the adaptive cap will ever impose. Below this the world is unplayable regardless of streaming.</summary>
		public static int AdaptiveRadiusFloor { get; set; } = 8;

		/// <summary>Seconds between adaptive evaluations. Each change costs the client a renegotiation, so this is deliberately slow.</summary>
		public static double AdaptiveRadiusIntervalSeconds { get; set; } = 5;

		/// <summary>
		///     Seconds to wait after changing the radius before judging again. Reducing does not drain
		///     the queue the client already holds, it only stops adding to it, so a backlog stays high
		///     long after a reduction that will eventually fix it. Without this the loop measured its
		///     own lag and cut again - 38 to 29 to 22 in a few seconds, each step deleting more of the
		///     player's view for no gain.
		/// </summary>
		public static double AdaptiveRadiusCooldownSeconds { get; set; } = 20;


		/// <summary>Set false to disable adaptation entirely and honour the client's request up to MaxViewDistance.</summary>
		public static bool AdaptiveRadiusEnabled { get; set; } = true;

		private long _lastAdaptiveCheck;
		private int _healthyChecks;

		public void SetChunkRadius(int radius)
		{
			_requestedChunkRadius = radius;

			int updated = Math.Max(5, Math.Min(radius, MaxViewDistance));
			if (updated == ChunkRadius) return;

			ChunkRadius = updated;

			// Just run a pass. The seen set is deliberately NOT cleared: the sweep already prunes
			// whatever fell outside the new radius, and widening only adds columns the client does not
			// have yet. Clearing would re-push the entire radius for a change that invalidates almost
			// none of it.
			//
			// Without this the pass would bail on the standing-still guard, so a render-distance
			// change took effect only once the player next moved.
			Volatile.Write(ref _forceChunkPass, 1);
		}

		// Set when something other than movement invalidated the streamed area. Cleared by the pass
		// that honours it; see SendChunksForKnownPosition's same-position early-out.
		private int _forceChunkPass;

		/// <summary>
		///     Matches the streamed radius to what this client can actually consume, per player.
		///     <para>
		///     The signal is the backlog: columns whose skeleton we pushed and which the client has
		///     never asked a single sub-chunk for. A client that is keeping up holds a backlog of about
		///     one pass; one that cannot holds thousands and falls minutes behind, at which point the
		///     columns it does request are for terrain the player left long ago. Measured on a real
		///     client: radius 16 held a backlog of ~30 with 91ms turnaround, radius 50 reached 8,512
		///     with 41 SECONDS of turnaround, on identical server code.
		///     </para>
		///     <para>
		///     Reductions are immediate, recoveries are slow and need several consecutive healthy
		///     checks, because every change costs the client a renegotiation and re-sending a skeleton
		///     makes it rebuild that column - Mojang's own SubChunk Request System doc warns of visual
		///     artifacts when that happens. Oscillating here would look worse than any fixed cap.
		///     </para>
		/// </summary>
		private void AdaptChunkRadius()
		{
			if (!AdaptiveRadiusEnabled || !IsSpawned || _requestedChunkRadius <= 0) return;

			long now = Stopwatch.GetTimestamp();
			if (_lastAdaptiveCheck != 0 && (now - _lastAdaptiveCheck) / (double) Stopwatch.Frequency < AdaptiveRadiusIntervalSeconds) return;
			_lastAdaptiveCheck = now;

			// The whole signal, and it is not a proxy for the failure - it IS the failure. The player
			// is standing in a column whose skeleton we pushed and which the client has never asked a
			// single sub-chunk for, so there is nothing to draw where they are. No rate, no threshold,
			// no distribution to calibrate: if you are inside it and the client never asked, the client
			// does not have it, whatever the reason.
			var here = new ChunkCoordinates(KnownPosition);
			bool standingInUnrequested = _skeletonSentAt.ContainsKey(here);

			// Nothing is judged until the client has once caught up with where the player stands.
			// IsSpawned is true long before that: the join burst has just pushed the columns around
			// spawn and the client has not asked for a sub-chunk of any of them yet, which is exactly
			// the state below reads as failure. Every player would be told their view distance was
			// reduced, on arrival, every time. The arming signal is the same one the adaptor runs on,
			// so there is no timer to tune: the first check that finds the player's own column already
			// requested means streaming is level with them, and from then on the signal means what it
			// says.
			if (!_adaptiveArmed)
			{
				if (standingInUnrequested) return;

				_adaptiveArmed = true;
				_healthyChecks = 0;
				return;
			}

			int ceiling = Math.Min(_requestedChunkRadius, MaxViewDistance);
			int backlog = _skeletonSentAt.Count;

			if (standingInUnrequested && ChunkRadius > AdaptiveRadiusFloor)
			{
				_healthyChecks = 0;

				// A quarter at a time rather than straight to the floor: the aim is the largest radius
				// this client can hold, not the smallest one that works.
				int reduced = Math.Max(AdaptiveRadiusFloor, ChunkRadius - Math.Max(1, ChunkRadius / 4));
				ApplyAdaptiveRadius(reduced, backlog, lowered: true);
				return;
			}

			// Recovery is the same signal held clean: the player has walked for several checks without
			// once arriving somewhere the client had not asked about.
			if (!standingInUnrequested && ChunkRadius < ceiling)
			{
				if (++_healthyChecks < 3) return;

				_healthyChecks = 0;
				ApplyAdaptiveRadius(Math.Min(ceiling, ChunkRadius + 2), backlog, lowered: false);
				return;
			}

			_healthyChecks = 0;
		}

		private void ApplyAdaptiveRadius(int radius, int backlog, bool lowered)
		{
			if (radius == ChunkRadius) return;

			ChunkRadius = radius;
			SendChunkRadiusUpdate();
			Volatile.Write(ref _forceChunkPass, 1);

			// Hold off judging again until the client has had time to work down what it already holds.
			_lastAdaptiveCheck = Stopwatch.GetTimestamp() + (long) (AdaptiveRadiusCooldownSeconds * Stopwatch.Frequency);
			_healthyChecks = 0;

			SendMessage(lowered
				? $"§eView distance reduced to §f{radius}§e: you walked into terrain the client had not loaded."
				: $"§aView distance raised to §f{radius}§a: streaming is keeping up.");

			Log.Info($"Adaptive chunk radius for {Username}: {(lowered ? "lowered" : "raised")} to {radius} ({backlog} columns pushed and unrequested, client asked for {_requestedChunkRadius})");
		}
		
		public virtual void HandleMcpeRequestChunkRadius(McpeRequestChunkRadius message)
		{
			Log.Debug($"Requested chunk radius of: {message.chunkRadius}");

			SetChunkRadius(message.chunkRadius);
			// The radius confirmation is sent from the gated chunk task (vanilla answers after
			// the join burst, not in the middle of it).
			MiNetServer.FastThreadPool.QueueUserWorkItem(SendChunksForKnownPosition);
		}

		public virtual void HandleMcpeSetEntityMotion(McpeSetEntityMotion message)
		{
			//Level.RelayBroadcast((McpeSetEntityMotion) message.Clone());
		}

		public virtual void HandleMcpeMoveEntity(McpeMoveEntity message)
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
			msg.data = message.data;
			msg.swingSource = message.swingSource;

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
				case PlayerAction.ContinueDestroyBlock: // same as StartBreak, sent when block breaking is server authoritative
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
						if (target.IsUnbreakable) break;

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
				case PlayerAction.PredictDestroyBlock: // end of breaking; the block itself is broken by the Destroy transaction
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
				case PlayerAction.CreativeDestroy: // redundant: PredictDestroyBlock arrives too when breaking is server authoritative
				{
					break;
				}
				case PlayerAction.StartItemUseOn:
				case PlayerAction.StopItemUseOn: // vanilla only uses these for analytics
				{
					break;
				}
				case PlayerAction.HandledTeleport: // client acknowledging our teleport, nothing to do
				case PlayerAction.MissedSwing: // arrives on PlayerAuthInput as well, handled there
				case PlayerAction.StartCrawling:
				case PlayerAction.StopCrawling: // pose only, movement already comes from PlayerAuthInput
				case PlayerAction.StartFlying:
				case PlayerAction.StopFlying: // flight is granted by abilities, not asked for here
				case PlayerAction.ReceivedServerData: // client confirming it has our data
				case PlayerAction.StartUsingItem: // arrives on PlayerAuthInput as well, handled there
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
					// Not implemented is not a protocol error. Throwing here abandons the rest of the
					// batch this packet arrived in, so one unhandled action drops the movement and
					// transactions sent with it.
					Log.Debug($"Unhandled player action {(PlayerAction) message.actionId} ({message.actionId})");
					break;
				}
			}

			IsUsingItem = false;

			BroadcastSetEntityData();
		}

		private float _baseSpeed;

		// No lock: only this player's own handlers call it (PlayerAction and PlayerAuthInput),
		// and the session's single dispatch consumer serializes them, so the base/boosted-speed
		// read-modify-write below cannot interleave.
		public void SetSprinting(bool isSprinting)
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
		public bool ShowNameTags { get; set; } = true;
		public bool IsNoPvm { get; set; }
		public bool IsNoMvp { get; set; }
		public bool IsNoClip { get; set; }
		public bool IsFlying { get; set; }

		public virtual void SendGameRules()
		{
			McpeGameRulesChanged gameRulesChanged = McpeGameRulesChanged.CreateObject();
			gameRulesChanged.rules = Level.GetGameRules();
			SendPacket(gameRulesChanged);
		}

		public virtual void SendAdventureSettings()
		{
			// Protocol 1.19.30+ replaced the single AdventureSettings packet with UpdateAdventureSettings
			// (world rules) and UpdateAbilities (ability layers). The 1.26 client no longer knows the old
			// AdventureSettings id, so sending it during join is a hard reject.
			var adventure = McpeUpdateAdventureSettings.CreateObject();
			adventure.noPvm = IsNoPvm || IsSpectator || GameMode == GameMode.Spectator;
			adventure.noMvp = IsNoMvp || IsSpectator || GameMode == GameMode.Spectator;
			adventure.immutableWorld = IsWorldImmutable || GameMode == GameMode.Adventure;
			adventure.showNameTags = ShowNameTags;
			adventure.autoJump = IsAutoJump;
			SendPacket(adventure);

			SendUpdateAbilitiesPacket();
		}

		public virtual void SendUpdateAbilitiesPacket()
		{
			var abilities = McpeUpdateAbilities.CreateObject();
			abilities.entityUniqueId = EntityId;
			abilities.permissionLevel = (byte) PermissionLevel;
			abilities.commandPermission = (byte) CommandPermission;
			abilities.abilities = new List<AbilityLayer> { BuildBaseAbilityLayer() };
			SendPacket(abilities);
		}

		private AbilityLayer BuildBaseAbilityLayer()
		{
			bool spectator = IsSpectator || GameMode == GameMode.Spectator;
			bool creative = GameMode == GameMode.Creative;
			bool op = PermissionLevel >= PermissionLevel.Operator;

			AbilitySet set = 0;
			if (!spectator)
			{
				set |= AbilitySet.Build | AbilitySet.Mine | AbilitySet.DoorsAndSwitches
					| AbilitySet.OpenContainers | AbilitySet.AttackPlayers | AbilitySet.AttackMobs;
			}
			if (op) set |= AbilitySet.OperatorCommands | AbilitySet.Teleport;
			if (creative || spectator) set |= AbilitySet.Invulnerable | AbilitySet.InstantBuild;
			if (AllowFly || creative || spectator) set |= AbilitySet.MayFly;
			if (IsFlying || spectator) set |= AbilitySet.Flying;
			if (IsNoClip || spectator) set |= AbilitySet.NoClip;
			if (IsWorldBuilder) set |= AbilitySet.WorldBuilder;
			if (IsMuted) set |= AbilitySet.Muted;

			return new AbilityLayer
			{
				Type = AbilityLayerType.Base,
				// Vanilla marks every ability as allowed on the base layer and gates behavior
				// through the enabled set only (verified against BDS 1.26.34 bytes).
				Allowed = (AbilitySet) 0xFFFFF,
				Enabled = set,
				FlySpeed = FlySpeed,
				VerticalFlySpeed = VerticalFlySpeed,
				WalkSpeed = MovementSpeed,
			};
		}

		public PermissionLevel PermissionLevel { get; set; } = PermissionLevel.Operator;

		public CommandPermission CommandPermission { get; set; } = CommandPermission.Normal;

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

		public virtual void HandleMcpeRequestNetworkSettings(McpeRequestNetworkSettings message)
		{
			// Do nothing. Handled by LoginMessageHandler before the Player exists.
		}

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

				// A name is one seat: evict whoever already holds it (see Level.RemoveDuplicatePlayers).
				Level.RemoveDuplicatePlayers(this);

				Level.EntityManager.AddEntity(this);

				GameMode = Config.GetProperty("Player.GameMode", Level.GameMode);

				// The client requires this burst as an exact set in an exact order: a hole or a
				// reorder is rejected with no diagnostic. The frame numbers map to the vanilla
				// join it mirrors. Content is built from live state and the committed data files
				// (see JoinSequenceData); no captured bytes are replayed.
				SendSleepStatus(); // frame 6, LevelEventGeneric

				SendPlayerListSelf(); // frame 7, vanilla 1st player list, before StartGame

				SendWorldClockState(); // frame 8

				SendJigsawStructureData(); // frame 9

				SendVoxelShapes(); // frame 10

				SendStartGame(); // frame 11

				SendSyncEntityProperty(); // frames 12-24, one per entity type

				SendItemRegistry(); // frame 25

				SendPlayerSpawnPosition(); // frame 26, undefined-position sentinel: no personal (bed) spawn at join

				SendWorldClockRegistry(); // frame 27

				SendSetDificulty(); // frame 28

				SendSetCommandsEnabled(); // frame 29

				SendAdventureSettings(); // frames 30-31, adventure settings + first abilities

				SendGameRules(); // frame 32

				Level.AddPlayer(this, false); // frame 33, vanilla 2nd player list

				SendUpdateAbilitiesPacket(); // frame 34, abilities again after the 2nd player list

				SendBiomeDefinitionList(); // frame 35

				SendAvailableEntityIdentifiers(); // frame 36

				SendPlayerFog(); // frame 37
				SendCameraPresets(); // frame 38
				SendCameraAimAssistPresets(); // frame 39
				SendCameraSpline(); // frame 40

				if (ChunkRadius == -1) ChunkRadius = JoinBurstChunkRadius;

				SendUpdateAttributes(); // frame 41

				SendCreativeInventory(); // frame 42

				SendTrimData(); // frame 43

				SendPlayerInventory(); // frames 44-47, four InventoryContent

				SendPlayerHotbar(); // frame 48

				SendCraftingRecipes(); // frame 49

				SendAvailableCommands(); // frame 50 - the server's REAL command registry, never the captured vanilla list

				// Frames 51-53: vanilla sends THREE searching-state respawns before chunk streaming.
				SendRespawn();
				// SendRespawn();
				// SendRespawn();

				// frame 54; skeleton chunks stream from frame 55. Fixed 4 chunks (64 blocks) like
				// vanilla, regardless of any radius the client has already asked for.
				SendNetworkChunkPublisherUpdate(JoinBurstChunkRadius);

				BroadcastSetEntityData();

				SendCurrentStructureFeature(); // vanilla sends this just after the first chunks (frame 58)
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
			finally
			{
				// Unblocks chunk streaming (see SendChunksForKnownPosition). Set even on error so
				// a failed sequence can't leave the chunk task waiting forever.
				_loginSequenceCompleted.Set();
				Interlocked.Decrement(ref serverInfo.ConnectionsInConnectPhase);
				CompleteJoinStage(JoinStage.Burst);
			}

			LastUpdatedTime = DateTime.UtcNow;
			Log.InfoFormat("Login complete by: {0} from {2} in {1}ms", Username, watch.ElapsedMilliseconds, EndPoint);
		}

		// The joining player's own player-list entry, sent before StartGame exactly like vanilla
		// BDS, which sends self alone here and the full roster later from Level.AddPlayer.
		//
		// This used to copy the player into a detached stub, which silently dropped whatever the
		// stub forgot: the record writer emits DisplayName ?? Username and the stub set neither,
		// so every join announced the player with an empty name. Sending the player itself cannot
		// drift out of sync with the fields the writer reads.
		public virtual void SendPlayerListSelf()
		{
			var playerList = McpePlayerList.CreateObject();
			playerList.records = McpePlayerList.Added(this);
			SendPacket(playerList);
		}

		// Level event 19602: sleep status. Payload is an unframed network-NBT compound body:
		// ableToSleep, overworldPlayerCount, sleepingPlayerCount (varint-NBT int tags, no root
		// compound header). MiNET does not model sleeping yet, so sleeping count is 0.
		public virtual void SendSleepStatus()
		{
			var root = new NbtCompound("")
			{
				new NbtInt("ableToSleep", 1),
				new NbtInt("overworldPlayerCount", Math.Max(1, Level.PlayerCount)),
				new NbtInt("sleepingPlayerCount", 0)
			};

			var packet = McpeLevelEventGeneric.CreateObject();
			packet.eventId = (int) LevelEventType.SleepingPlayers;
			packet.eventData = root;
			SendPacket(packet);
		}

		public virtual void SendWorldClockState()
		{
			Level.Clock.SendStateTo(this);
		}

		// initialize_registry payload: the overworld clock and its day-cycle markers.
		public virtual void SendWorldClockRegistry()
		{
			Level.Clock.SendRegistryTo(this);
		}

		// The player's personal (bed/anchor) spawn. MiNET does not track one yet, so this is
		// vanilla's undefined-position sentinel (INT32_MIN, -1, INT32_MIN in dimension 3).
		// Distinct from SendSetSpawnPosition, which announces the WORLD spawn (type 1).
		public virtual void SendPlayerSpawnPosition()
		{
			var undefined = new BlockCoordinates(int.MinValue, -1, int.MinValue);

			var packet = McpeSetSpawnPosition.CreateObject();
			packet.spawnType = 0; // player spawn
			packet.coordinates = undefined;
			packet.dimension = 3; // undefined
			packet.unknownCoordinates = undefined;
			SendPacket(packet);
		}

		public virtual void SendPlayerHotbar()
		{
			var packet = McpePlayerHotbar.CreateObject();
			packet.selectedSlot = (uint) Inventory.InHandSlot;
			packet.windowId = 0;
			packet.selectSlot = true;
			SendPacket(packet);
		}

		// MiNET does not generate structures, so the player is never inside one.
		public virtual void SendCurrentStructureFeature()
		{
			var packet = McpeCurrentStructureFeature.CreateObject();
			packet.currentFeature = "";
			SendPacket(packet);
		}

		public virtual void SendItemRegistry()
		{
			// Since 1.21.60 the item type dictionary lives in its own packet (id 0xa2,
			// "item_registry", formerly item_component) instead of StartGame's itemstates.
			// Without it the client cannot interpret any item network id we send and drops the
			// connection during join. Sent right after StartGame, matching PMMP's
			// PreSpawnPacketHandler and vanilla BDS 1.26.34. Entry data comes from the generated
			// ItemRegistry, whose component blobs are already the bytes BDS puts on the wire.
			// REFCT: rebuilt per player, and the registry is immutable after startup. Every join walks
			// the whole ItemRegistry and allocates an ItemComponent per item, plus an NbtFile, an
			// NbtCompound and its backing dictionary for each item with no components, producing the
			// identical bytes every time. Measured on a 2000 bot join burst this and the entity
			// identifiers below are most of the NBT allocation on the login path. It should be built
			// once and kept as a pre-encoded payload; note also that a plugin suppressing this packet
			// suppresses it at SEND, so today the work is done and then discarded.
			var entries = new ItemComponentList();
			foreach (ItemRegistryEntry entry in ItemFactory.ItemRegistry)
			{
				var component = new ItemComponent
				{
					Name = entry.Name,
					RuntimeId = entry.NetworkId,
					ComponentBased = entry.ComponentBased,
					Version = entry.Version,
					RawNbt = entry.ComponentNbt
				};

				// An item with no components still carries an (empty) compound on the wire.
				if (component.RawNbt == null)
				{
					component.Nbt = new Nbt {NbtFile = new NbtFile {BigEndian = false, UseVarInt = true, RootTag = new NbtCompound("")}};
				}

				entries.Add(component);
			}

			var packet = McpeItemComponent.CreateObject();
			packet.entries = entries;
			SendPacket(packet);
		}

		// Entity identifier registry, built from our own EntityType registry (EntityHelpers).
		//
		// REFCT: same as SendItemComponents above. Regenerated per player from a registry that cannot
		// change after startup, so every join builds the same NBT tree and encodes the same bytes.
		public virtual void SendAvailableEntityIdentifiers()
		{
			var root = new NbtCompound("") {EntityHelpers.GenerateEntityIdentifiers()};

			var pk = McpeAvailableEntityIdentifiers.CreateObject();
			pk.namedtag = new Nbt {NbtFile = new NbtFile(root) {BigEndian = false, UseVarInt = true}};
			SendPacket(pk);
		}

		// Biome definitions, byte-identical to vanilla BDS 1.26.34 (Data/biome_definitions.json,
		// exported from a decoded wire capture; see JoinSequenceData). MiNET's own Biome data
		// (BiomeUtils) does not carry every wire field (snow/foliage colour, depth, scale, map
		// water colour, tags), so the captured definitions are sent verbatim.
		public virtual void SendBiomeDefinitionList()
		{
			SendPacket(Worlds.BiomeDefinitions.CreatePacket());
		}

		// Jigsaw structure sync data, byte-identical to vanilla BDS 1.26.34 (Data/jigsaw_structures.json).
		// MiNET does not generate structures, so this is the captured document sent verbatim.
		public virtual void SendJigsawStructureData()
		{
			var pk = McpeJigsawStructureData.CreateObject();
			pk.structureData = JoinSequenceData.NbtFromBase64(JoinSequenceData.JigsawStructureData.Value.NbtB64);
			SendPacket(pk);
		}

		// Voxel collision shapes, byte-identical to vanilla BDS 1.26.34 (Data/voxel_shapes.json).
		public virtual void SendVoxelShapes()
		{
			var data = JoinSequenceData.VoxelShapes.Value;
			var pk = McpeVoxelShapes.CreateObject();
			pk.Shapes = data.Shapes;
			pk.NameMap = data.NameMap;
			pk.CustomShapeCount = data.CustomShapeCount;
			SendPacket(pk);
		}

		// One SyncEntityProperty frame per entity type, byte-identical to vanilla BDS 1.26.34
		// (Data/entity_properties.json), sent in capture order (0012..0024).
		public virtual void SendSyncEntityProperty()
		{
			foreach (var entry in JoinSequenceData.EntityProperties.Value.Entries)
			{
				var pk = McpeSyncEntityProperty.CreateObject();
				pk.namedtag = JoinSequenceData.NbtFromBase64(entry.NbtB64);
				SendPacket(pk);
			}
		}

		// Client fog stack, byte-identical to vanilla BDS 1.26.34 (Data/player_fog.json).
		/// <summary>
		///     No fog stack. Vanilla sends an empty one at join and so do we; fog is applied later
		///     by gameplay, not declared here. Was reading a data file that held nothing.
		/// </summary>
		public virtual void SendPlayerFog()
		{
			var pk = McpePlayerFog.CreateObject();
			SendPacket(pk);
		}

		// Armor trim patterns/materials, byte-identical to vanilla BDS 1.26.34 (Data/trim_data.json).
		public virtual void SendTrimData()
		{
			var data = JoinSequenceData.TrimData.Value;
			var pk = McpeTrimData.CreateObject();
			pk.Patterns = data.Patterns;
			pk.Materials = data.Materials;
			SendPacket(pk);
		}

		public virtual void SendCameraPresets()
		{
			CameraManager.SendPresets();
		}

		// Aim-assist categories/presets, byte-identical to vanilla BDS 1.26.34
		// (Data/camera_aim_assist_presets.json).
		public virtual void SendCameraAimAssistPresets()
		{
			var data = JoinSequenceData.CameraAimAssistPresets.Value;
			var pk = McpeCameraAimAssistPresets.CreateObject();
			pk.Categories = data.Categories;
			pk.Presets = data.Presets;
			pk.Operation = data.Operation;
			SendPacket(pk);
		}

		// Camera splines, byte-identical to vanilla BDS 1.26.34 (Data/camera_spline.json). Vector3
		// control/rotation points are stored as plain x/y/z DTOs, converted here (see SendCameraPresets).
		/// <summary>
		///     No splines. A server declares camera splines when it wants scripted camera moves,
		///     and we have none, which is what vanilla sends at join too. Was thirty lines of
		///     projection over a data file holding an empty list.
		/// </summary>
		public virtual void SendCameraSpline()
		{
			var pk = McpeCameraSpline.CreateObject();
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
			Log.Debug($"UUID: {message.origin?.UUID}");

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
			// Reaching here means chunk streaming published enough for the client to initialize; the
			// rest of this method is the spawn itself.
			CompleteJoinStage(JoinStage.Chunks);

			// Vanilla join tail: a ready-state respawn during chunk streaming, then set health,
			// the spawn play-status, and entity data. No SetTime and no MovePlayer here; the
			// client has the position from StartGame.
			var respawn = McpeRespawn.CreateObject();
			respawn.x = SpawnPosition.X;
			respawn.y = SpawnPosition.Y + 1.62f;
			respawn.z = SpawnPosition.Z;
			respawn.state = (byte) McpeRespawn.RespawnState.Ready;
			SendPacket(respawn);

			var setHealth = McpeSetHealth.CreateObject();
			setHealth.health = HealthManager.Hearts;
			SendPacket(setHealth);

			SendSetEntityData();

			SendPlayerStatus(McpePlayStatus.PlayStatus.PlayerSpawn);

			IsSpawned = true;

			KnownPosition = (PlayerLocation) SpawnPosition.Clone();

			LastUpdatedTime = DateTime.UtcNow;
			_haveJoined = true;

			CompleteJoinStage(JoinStage.Spawn);
			EngineMetrics.RecordJoinCompleted(_joinStartedAt);

			OnPlayerJoin(new PlayerEventArgs(this));

			// Temporary: proves the camera API against a real client. Delete with Camera/CameraDemo.cs.
			CameraDemo.Run(this);
		}

		//public virtual void HandleMcpeRespawn()
		//{
		//	HandleMcpeRespawn(null);
		//}

		public virtual void HandleMcpeRespawn(McpeRespawn message)
		{
			if (message.state == (byte) McpeRespawn.RespawnState.ClientReady)
			{
				// The client tears its own window down when the player dies, so this is the server
				// catching up rather than telling it anything.
				ReleaseScreen();

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

				MiNetServer.FastThreadPool.QueueUserWorkItem(() =>
				{
					if (_loginSequenceCompleted.Wait(15000)) ForcedSendChunks();
				});

				//SendPlayerStatus(McpePlayStatus.PlayStatus.PlayerSpawn);

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
			packet.position = new Vector3(position.X, position.Y + 1.62f, position.Z);
			packet.rotation = new Vector2(position.Pitch, position.Yaw);
			packet.headYaw = position.HeadYaw;
			packet.mode = teleport ? McpeMovePlayer.PositionMode.Respawn : McpeMovePlayer.PositionMode.Normal;

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
					// Straight up over the DESTINATION, not over where we are leaving: the column we
					// are about to send is the destination's, and a client standing in a different
					// column has no use for it. High and in open air because NoAi holds the player
					// still but the client still has no sections beneath them yet.
					SetPosition(new PlayerLocation
					{
						X = newPosition.X,
						Y = 4000,
						Z = newPosition.Z,
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


			string portalName = new Portal().Name;
			string obsidianName = new Obsidian().Name;

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

						bool b = level.IsBlock(coord, portalName);
						b &= level.IsBlock(coord.BlockDown(), obsidianName);
						if (b)
						{
							var portal = (Portal) level.GetBlock(coord);
							if (portal.PortalAxis == "z")
							{
								b &= level.IsBlock(coord.BlockNorth(), portalName);
							}
							else
							{
								b &= level.IsBlock(coord.BlockEast(), portalName);
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
			// A screen carried into another level would still resolve slot clicks against a block in the
			// one being left. The client is told as well as the bookkeeping being done, because unlike
			// dying, changing level does not make the client tear its own window down.
			if (Screen.Kind != ScreenKind.Inventory) HandleMcpeContainerClose(null);

			bool oldNoAi = NoAi;
			SetNoAi(true);
			int oldChunkRadius = ChunkRadius;
			SendChunkRadiusUpdate(1);

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

				// Level.AddPlayer(this, true);

				// KnownPosition = SpawnPosition;

				// SendChunkRadiusUpdate(oldChunkRadius);

				// SendSetTime();

				// // send teleport to spawn
				// SetPosition(SpawnPosition);
				// SetNoAi(oldNoAi);

				// Log.InfoFormat("Respawn player {0} on level {1}", Username, Level.LevelId);

				// postSpawnAction?.Invoke();

				MiNetServer.FastThreadPool.QueueUserWorkItem(() =>
				{
					Level.AddPlayer(this, true);

					KnownPosition = SpawnPosition;

					SendChunkRadiusUpdate(oldChunkRadius);

					ForcedSendChunks(() =>
					{
						Log.InfoFormat("Respawn player {0} on level {1}", Username, Level.LevelId);

						SendSetTime();

						// send teleport to spawn
						SetPosition(SpawnPosition);
						SetNoAi(oldNoAi);

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

			// 1.26 container sizes (verified against BDS): main inventory 36 slots (hotbar is
			// slots 0-8 of it, not appended), armor 5 (the body/harness slot was added), ui 54.
			// MiNET's internal lists still use the old sizes; slice/pad at the wire.
			static ItemStacks Resize(ItemStacks src, int size)
			{
				var result = new ItemStacks();
				for (int i = 0; i < size; i++) result.Add(i < src.Count ? src[i] : new ItemAir());
				return result;
			}

			var inventoryContent = McpeInventoryContent.CreateObject();
			inventoryContent.inventoryId = (byte) 0x00;
			inventoryContent.input = Resize(Inventory.GetSlots(), 36);
			SendPacket(inventoryContent);

			var armorContent = McpeInventoryContent.CreateObject();
			armorContent.inventoryId = 0x78;
			armorContent.input = Resize(Inventory.GetArmor(), 5);
			SendPacket(armorContent);

			var uiContent = McpeInventoryContent.CreateObject();
			uiContent.inventoryId = 0x7c;
			uiContent.input = Resize(Inventory.GetUiSlots(), CursorInventory.Size);
			SendPacket(uiContent);

			var offHandContent = McpeInventoryContent.CreateObject();
			offHandContent.inventoryId = 0x77;
			offHandContent.input = Inventory.GetOffHand();
			SendPacket(offHandContent);

			// No self-targeted MobEquipment here: the client owns its hotbar selection and
			// vanilla never sends this at join (server->client MobEquipment is for OTHER
			// entities' visible held items).
		}

		public virtual void SendCraftingRecipes()
		{
			// The 1.26 client expects a CraftingData packet during join (both vanilla BDS and PMMP
			// always send one). It is a projection of the server's recipe registry, which is also what
			// crafting requests are validated against, so a plugin that adds a recipe changes both.

			SendPacket(RecipeManager.CreateCraftingDataPacket());
		}

		public virtual void SendCreativeInventory()
		{
			if (!UseCreativeInventory) return;

			var creativeContent = McpeCreativeContent.CreateObject();
			creativeContent.groups = new List<CreativeGroupInfoPayload>();
			creativeContent.entries = new List<CreativeItemEntryPayload>();

			// Vanilla tab groups: groups with category/name/icon, and each entry referencing its
			// group by index. Without correct groups the client shows empty creative tabs.
			CreativeGroupData groupData = InventoryUtils.CreativeGroups.Value;
			foreach (CreativeGroupDef def in groupData.Groups)
			{
				Item icon = null;
				if (def.IconNetworkId != 0)
				{
					// The captured icon identity, resolved back through the item registry so the
					// item carries a real name. An unresolved icon (network id -1) crashes the client
					// when it rebuilds the creative UI, so the id must always land on a registry entry.
					icon = ItemFactory.GetItemByNetworkId(def.IconNetworkId, def.IconMetadata);
					icon.NetworkMetadata = def.IconMetadata;
					icon.RuntimeId = def.IconRuntimeId;
					if (def.IconNbt != null) icon.ExtraData = TypedNbtJson.ReadCompound(def.IconNbt);
				}

				creativeContent.groups.Add(new CreativeGroupInfoPayload
				{
					creativeCategory = (CreativeGroupInfoPayload.CreativeCategory) def.Category,
					name = def.Name ?? string.Empty,
					groupIconItem = icon,
				});
			}

			for (int i = 0; i < groupData.Entries.Count; i++)
			{
				CreativeEntryDef def = groupData.Entries[i];
				Item item = ItemFactory.GetItemByNetworkId(def.NetworkId, def.Metadata);
				item.NetworkMetadata = def.Metadata;
				item.RuntimeId = def.RuntimeId;
				if (def.Nbt != null) item.ExtraData = TypedNbtJson.ReadCompound(def.Nbt);

				creativeContent.entries.Add(new CreativeItemEntryPayload
				{
					groupIndex = (uint) def.GroupIndex,
					creativeNetId = (uint) (i + 1),
					itemInstance = item,
				});
			}

			SendPacket(creativeContent);
		}

		private void SendChunkRadiusUpdate()
		{
			McpeChunkRadiusUpdate packet = McpeChunkRadiusUpdate.CreateObject();
			packet.chunkRadius = ChunkRadius;

			SendPacket(packet);
		}
		private void SendChunkRadiusUpdate(int radius)
		{
			McpeChunkRadiusUpdate packet = McpeChunkRadiusUpdate.CreateObject();
			packet.chunkRadius = radius;

			SendPacket(packet);
		}

		public void SendPlayerStatus(McpePlayStatus.PlayStatus status)
		{
			McpePlayStatus mcpePlayerStatus = McpePlayStatus.CreateObject();
			mcpePlayerStatus.status = (int) status;
			SendPacket(mcpePlayerStatus);
		}

		[Wired]
		public void SetGameMode(GameMode gameMode)
		{
			GameMode = gameMode;

			SendUpdatePlayerGameType();
		}


		/// <summary>
		///     SetPlayerGameType (0x3e) is the CLIENT's request; the server answers with
		///     UpdatePlayerGameType (0x97), which also carries who changed and when. Sending 0x3e
		///     back at the client is not legal in that direction.
		/// </summary>
		public void SendUpdatePlayerGameType()
		{
			McpeUpdatePlayerGameType gametype = McpeUpdatePlayerGameType.CreateObject();
			gametype.playerGameType = (int) GameMode;
			gametype.targetPlayerUniqueId = EntityId;
			gametype.tick = 0;
			SendPacket(gametype);
		}

		[Wired]
		public void StrikeLightning()
		{
			// Through the level, so this gets the same centring correction as every other caller
			// instead of spawning the bolt itself and landing half a block off.
			Level?.StrikeLightning(KnownPosition.ToVector3());
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
							disconnect.reason = (int) McpeDisconnect.FailReason.LegacyDisconnect;
							disconnect.message = reason;
							NetworkHandler.SendPacket(disconnect);
						}

						NetworkHandler.Close();
						NetworkHandler = null;

						IsConnected = false;
					}

					// While the level is still there to hear it. A chest whose last viewer left without
					// closing it reads as open forever: the lid stays up for everyone, and the inventory
					// holds a departed player it keeps sending slot changes to.
					ReleaseScreen();

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
						EngineMetrics.RecordJoinAbandoned(_joinStage, Username, _joinStartedAt);
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

		public virtual void HandleMcpeMovePlayer(McpeMovePlayer message)
		{
			// No ordering guard here: ordered delivery is the transport's guarantee (the session's
			// single dispatch consumer), so handlers run sequentially and in sequence by
			// construction. A handler-level lock duplicating that would also cost this method its
			// verified label.
			if (!IsSpawned || HealthManager.IsDead) return;

			var origin = KnownPosition.ToVector3();
			double distanceTo = Vector3.Distance(origin, new Vector3(message.position.X, message.position.Y - 1.62f, message.position.Z));

			CurrentSpeed = distanceTo / ((double) (DateTime.UtcNow - LastUpdatedTime).Ticks / TimeSpan.TicksPerSecond);

			double verticalMove = message.position.Y - 1.62 - KnownPosition.Y;

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
			if (!IsGliding) HungerManager.Move(Vector3.Distance(new Vector3(KnownPosition.X, 0, KnownPosition.Z), new Vector3(message.position.X, 0, message.position.Z)));

			KnownPosition = new PlayerLocation
			{
				X = message.position.X,
				Y = message.position.Y - 1.62f,
				Z = message.position.Z,
				Pitch = message.rotation.X,
				Yaw = message.rotation.Y,
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
			double d = Math.Abs(KnownPosition.Y - (message.position.Y - 1.62f));
			return !(AllowFly || IsOnGround || isOnGround || d > 0.001);
		}

		private static readonly int[] Layers = {-1, 0};
		private static readonly int[] Arounds = {0, 1, -1};

		public bool CheckOnGround(McpeMovePlayer message)
		{
			if (Level == null)
				return true;

			BlockCoordinates pos = new Vector3(message.position.X, message.position.Y - 1.62f, message.position.Z);

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

		/// <summary>
		///     Whether this client gets chunks as blobs. Both ends have to agree: the client tells
		///     us it keeps a cache, and BlobCacheEnabled says we are willing to serve one. Some
		///     clients never opt in, so the plain chunk path is not optional and stays the default.
		/// </summary>
		/// <summary>
		///     The emotes this client owns, sent once after login. Parsed but unused: acting on it
		///     means validating an incoming Emote against the list and relaying it to the other
		///     players, and we do neither yet. Handled so it stops arriving as an unknown packet.
		/// </summary>
		public virtual void HandleMcpeEmoteList(McpeEmoteList message)
		{
			if (Log.IsDebugEnabled) Log.Debug($"Emote list from {Username}: {message.emotePieceIds.Length} emotes");
		}

		/// <summary>
		///     The client announces whether it caches chunk blobs. We require that it does.
		///     <para>
		///         Every Mojang client supports it, so a client saying no is either something we did
		///         not write or something pretending. Serving it would mean carrying a second chunk
		///         path for ever: the same column serialized twice, biomes inline instead of blob
		///         addressed, and the whole terrain re-sent to a player who already walked here. One
		///         path is the point of this check, and refusing is a server protecting itself rather
		///         than a judgement about the client.
		///     </para>
		/// </summary>
		public virtual void HandleMcpeClientCacheStatus(McpeClientCacheStatus message)
		{
			if (!message.enabled)
			{
				Log.Warn($"Refusing {Username}: the client says it does not cache chunks, which this server requires.");
				Disconnect("This server requires a client that caches chunks.");
				return;
			}

			Log.Info($"Cache status from {Username}: client caches, as this server requires.");
		}

		/// <summary>
		///     Answers a client's report of which blobs it already had. Hits cost nothing; misses
		///     get the bytes the hash was taken over. A hash we cannot resolve means the blob aged
		///     out of the store after we advertised it, which would strand the chunk, so it is
		///     logged rather than passed over.
		/// </summary>
		private readonly ManualResetEventSlim _clientCacheBlobStatusReceived = new(false);

		public virtual void HandleMcpeClientCacheBlobStatus(McpeClientCacheBlobStatus message)
		{
			// First status of the session = the client has verified the spawn block's hashes.
			// The join's flood also waits for this edge; once set it stays set.
			_clientCacheBlobStatusReceived.Set();

			// The client's own cache verdict, hash by hash: the hit/miss ratio here is the direct
			// measure of how much the blob cache is actually saving.
			EngineMetrics.BlobCacheReport("hit", message.hashHits?.Length ?? 0);
			EngineMetrics.BlobCacheReport("miss", message.hashMisses?.Length ?? 0);

			if (message.hashMisses == null || message.hashMisses.Length == 0) return;

			int unresolved = 0;
			var blobs = new Dictionary<ulong, byte[]>();
			foreach (ulong hash in message.hashMisses)
			{
				if (BlobStore.TryGet(hash, out byte[] blob)) blobs[hash] = blob;
				else
				{
					unresolved++;
					Log.Warn($"No blob for hash {hash:X16} requested by {Username}");
				}
			}
			EngineMetrics.BlobCacheReport("unresolved", unresolved);

			if (blobs.Count == 0) return;

			var response = McpeClientCacheMissResponse.CreateObject();
			response.blobs = blobs;
			SendPacket(response);
		}

		public virtual void HandleMcpeNetworkSettings(McpeNetworkSettings message)
		{
		}

		public virtual void HandleMcpeEmote(McpeEmote message)
		{
		}

		public virtual void HandleMcpeMultiplayerSettings(McpeMultiplayerSettings message)
		{
		}

		public virtual void HandleMcpeSettingsCommand(McpeSettingsCommand message)
		{
		}

		public virtual void HandleMcpeAnvilDamage(McpeAnvilDamage message)
		{
		}

		/// <inheritdoc />
		public virtual void HandleMcpePlayerAuthInput(McpePlayerAuthInput message)
		{
			// The 1.26 client sends PlayerAuthInput every tick as its only movement packet
			// (MovePlayer is server->client only now). Position is at eye height, like
			// MovePlayer's was. No ordering guard: see HandleMcpeMovePlayer's remarks, ordering
			// is the transport's guarantee on both paths.
			if (!IsSpawned || HealthManager.IsDead) return;

			var newPosition = new PlayerLocation
			{
				X = message.position.X,
				Y = message.position.Y - 1.62f,
				Z = message.position.Z,
				Pitch = message.playerRotation.X,
				Yaw = message.playerRotation.Y,
				HeadYaw = message.playerHeadRotation
			};

			double distanceTo = Vector3.Distance(KnownPosition.ToVector3(), newPosition.ToVector3());
			CurrentSpeed = distanceTo / ((double) (DateTime.UtcNow - LastUpdatedTime).Ticks / TimeSpan.TicksPerSecond);

			// The input flags carry collision state directly; vertical collision while not
			// jumping is standing on ground.
			IsOnGround = (message.inputData & AuthInputFlags.VerticalCollision) != 0;

			if (!IsGliding) HungerManager.Move(Vector3.Distance(new Vector3(KnownPosition.X, 0, KnownPosition.Z), new Vector3(newPosition.X, 0, newPosition.Z)));

			KnownPosition = newPosition;
			LastUpdatedTime = DateTime.UtcNow;

			// Republish the live area the moment the player crosses into a new column, ahead of any
			// streaming. The publisher update names the centre and radius the client treats as live,
			// and it used to go out only once per streaming pass - so while a pass ran for tens of
			// seconds the published centre stayed where the player had been, and they could walk
			// outside their own publish area. Vanilla republishes far more often than we did. The
			// packet is a coordinate and a varint.
			MaybePublishChunkArea();


			// Keep chunk streaming following the player; this used to hang off MovePlayer, which the
			// 1.26 client no longer sends.
			//
			// Guarded HERE, not left to the callee. SendChunksForKnownPosition does self-guard, but
			// by then the work item is already allocated and scheduled, and auth input arrives twenty
			// times a second per player whether they moved or not: 2000 stationary players queued
			// 40000 pool items a second to discover they had nothing to do. Same test the move
			// handler has always used.
			var authChunkPosition = new ChunkCoordinates(KnownPosition);
			if (_currentChunkPosition != authChunkPosition && _currentChunkPosition.DistanceTo(authChunkPosition) >= MoveRenderDistance)
			{
				MiNetServer.FastThreadPool.QueueUserWorkItem(SendChunksForKnownPosition);
			}

			// Movement state transitions arrive as input flags now (the old PlayerAction
			// start/stop sprint/sneak/glide packets are gone in 1.26). Route them to the same
			// behaviors; without this the server keeps broadcasting stale entity state against
			// the client's prediction and sprint/sneak stutter and cancel.
			AuthInputFlags flags = message.inputData;
			if ((flags & (AuthInputFlags.StartSprinting | AuthInputFlags.StopSprinting | AuthInputFlags.StartSneaking | AuthInputFlags.StopSneaking | AuthInputFlags.StartGliding | AuthInputFlags.StopGliding)) != 0)
			{
				if ((flags & AuthInputFlags.StartSprinting) != 0) SetSprinting(true);
				if ((flags & AuthInputFlags.StopSprinting) != 0) SetSprinting(false);

				if ((flags & AuthInputFlags.StartSneaking) != 0)
				{
					SetSprinting(false);
					IsSneaking = true;
				}
				if ((flags & AuthInputFlags.StopSneaking) != 0)
				{
					SetSprinting(false);
					IsSneaking = false;
				}

				if ((flags & AuthInputFlags.StartGliding) != 0)
				{
					IsGliding = true;
					Height = 0.6;
				}
				if ((flags & AuthInputFlags.StopGliding) != 0)
				{
					IsGliding = false;
					Height = 1.8;
				}

				BroadcastSetEntityData();
			}

			// Placing and using a block arrives here on a server-authoritative client, folded into the
			// movement tick, and NOT as its own McpeInventoryTransaction: the client sends one or the
			// other, never both. Same transaction, same handler, so a right-click reaches a chest the
			// same way whichever packet carried it.
			//
			// Hesitate, then thread: the transaction branch reaches world locks (Level.Interact,
			// UI screens), so it runs as a pool work item instead of on the movement path. A click
			// lands a beat later; the 20/s movement tail stays provably lock-free.
			if (message.itemUseTransaction?.itemUseTransaction != null)
			{
				ItemUseInventoryTransaction transaction = message.itemUseTransaction.itemUseTransaction;

				// The transaction belongs to the packet, and the packet is released the moment this
				// handler returns: Reset hands back its leases and this would be reading memory that
				// went with them. A reference keeps it alive until the queued work is done with it.
				message.AddReferences(1);
				MiNetServer.FastThreadPool.QueueUserWorkItem(() =>
				{
					try { HandleItemUseTransaction(transaction); }
					finally { message.PutPool(); }
				});
			}

			if (message.itemStackRequest != null)
			{
				// Hesitate, then thread, same as the transaction above: crafting resolution takes
				// the recipe-manager lock, so an inventory click folded into a movement tick runs
				// as a pool work item too.
				ItemStackRequest request = message.itemStackRequest;

				// Same ownership problem as the transaction above.
				message.AddReferences(1);
				MiNetServer.FastThreadPool.QueueUserWorkItem(() =>
				{
					try { HandleSingleItemStackRequest(request); }
					finally { message.PutPool(); }
				});
			}

			if (message.playerBlockActions != null)
			{
				// Server-authoritative block breaking (StartGame flag): break progress and the
				// actual destroy arrive as auth-input block actions instead of the old
				// PlayerAction/InventoryTransaction path.
				foreach (PlayerBlockActionData action in message.playerBlockActions)
				{
					BlockCoordinates coordinates = action.position;

					// PMMP's PlayerAction numbers, not the generated PlayerActionType enum: that is the
					// standalone PlayerAction packet's list under the same name.
					switch ((int) action.playerActionType)
					{
						case 0: // start_break
						{
							if (GameMode == GameMode.Survival)
							{
								Block target = Level.GetBlock(coordinates);
								// Unbreakable blocks report negative hardness, which would come out as a
								// negative break time and read to the client as instant.
								if (!target.IsUnbreakable)
								{
									var drops = target.GetDrops(Inventory.GetItemInHand());
									float tooltypeFactor = drops == null || drops.Length == 0 ? 5f : 1.5f; // 1.5 if proper tool
									double breakTime = Math.Ceiling(target.Hardness * tooltypeFactor * 20);

									McpeLevelEvent breakEvent = McpeLevelEvent.CreateObject();
									breakEvent.eventId = 3600;
									breakEvent.position = coordinates;
									breakEvent.data = (int) (65535 / breakTime);
									Level.RelayBroadcast(breakEvent);
								}
							}
							break;
						}
						case 1: // abort_break
						{
							McpeLevelEvent breakEvent = McpeLevelEvent.CreateObject();
							breakEvent.eventId = 3601;
							breakEvent.position = coordinates;
							Level.RelayBroadcast(breakEvent);
							break;
						}
						case 18: // crack_break
						case 27: // continue_break
						{
							Block target = Level.GetBlock(coordinates);
							McpeLevelEvent breakEvent = McpeLevelEvent.CreateObject();
							breakEvent.eventId = 2014;
							breakEvent.position = coordinates;
							breakEvent.data = ((int) target.GetRuntimeId()) | ((byte) (action.facing << 24));
							Level.RelayBroadcast(breakEvent);
							break;
						}
						case 26: // predict_break: the client predicted the destroy; perform it
						{
							Level.BreakBlock(this, coordinates, (BlockFace) action.facing);
							break;
						}
					}
				}
			}
		}

		public virtual void HandleMcpePlayerToggleCrafterSlotRequest(McpePlayerToggleCrafterSlotRequest message)
		{
		}

		public virtual void HandleMcpeServerBoundLoadingScreen(McpeServerBoundLoadingScreen message)
		{
			// Client informs the server which loading screen it is on. No server action needed.
		}

		public virtual void HandleMcpeServerBoundDiagnostics(McpeServerBoundDiagnostics message)
		{
			// Client performance telemetry (creator diagnostics setting). Ignored.
		}

		public virtual void HandleMcpeClientCameraAimAssist(McpeClientCameraAimAssist message)
		{
			// Client-side aim assist state report. Ignored.
		}

		public virtual void HandleMcpeClientMovementPredictionSync(McpeClientMovementPredictionSync message)
		{
			// Client-authoritative movement correction sync. No server action needed.
		}

		public virtual void HandleMcpeUpdateClientOptions(McpeUpdateClientOptions message)
		{
			// Client graphics/profanity-filter option change notification. Ignored.
		}

		public virtual void HandleMcpeServerboundPackSettingChange(McpeServerboundPackSettingChange message)
		{
			// Client resource pack setting (slider/toggle) change. Ignored.
		}

		public virtual void HandleMcpeSetPlayerFurnaceOptions(McpeSetPlayerFurnaceOptions message)
		{
			// Client furnace UI layout preference (new at 2192). Ignored.
		}

		public virtual void HandleMcpeServerboundDataStore(McpeServerboundDataStore message)
		{
			// Client data store update request. Ignored.
		}

		public virtual void HandleMcpePartyDestinationCookieResponse(McpePartyDestinationCookieResponse message)
		{
			// Client response to a party destination cookie. Ignored.
		}

		public virtual void HandleMcpeResourcePacksReadyForValidation(McpeResourcePacksReadyForValidation message)
		{
		}

		public virtual void HandleMcpePartyChanged(McpePartyChanged message)
		{
		}

		public virtual void HandleMcpeServerboundDataDrivenScreenClosed(McpeServerboundDataDrivenScreenClosed message)
		{
		}

		public virtual void HandleMcpeSetPlayerInventoryOptions(McpeSetPlayerInventoryOptions message)
		{
			// Client UI preferences (tabs, filtering, layout). Nothing to do server-side.
		}

		public virtual void HandleMcpeCreatePhoto(McpeCreatePhoto message)
		{
			// Client notification that it captured a portfolio/education photo. Ignored.
		}


		// Single request embedded in McpePlayerAuthInput (item_stack_request input flag); same
		// processing and response flow as the standalone McpeItemStackRequest packet.
		private void HandleSingleItemStackRequest(ItemStackRequest request)
		{
			var response = McpeItemStackResponse.CreateObject();
			response.responses = new List<ItemStackResponseInfo>();

			var stackResponse = new ItemStackResponseInfo
			{
				result = ItemStackResponseInfo.Result.Success,
				clientRequestId = request.clientRequestId,
				containers = new List<ItemStackResponseContainerInfo>()
			};
			response.responses.Add(stackResponse);

			try
			{
				stackResponse.containers.AddRange(ItemStackInventoryManager.HandleItemStackActions(request.clientRequestId, request));
			}
			catch (Exception e)
			{
				Log.Warn($"Failed to process inventory actions", e);
				stackResponse.result = ItemStackResponseInfo.Result.Error;
				stackResponse.containers.Clear();
			}

			SendPacket(response);
			if (stackResponse.result != ItemStackResponseInfo.Result.Success) ResyncInventoryAfterFailedStackRequest();
		}

		public virtual void HandleMcpeItemStackRequest(McpeItemStackRequest message)
		{
			var response = McpeItemStackResponse.CreateObject();
			response.responses = new List<ItemStackResponseInfo>();
			bool anyFailed = false;
			foreach (ItemStackRequest request in message.requests)
			{
				var stackResponse = new ItemStackResponseInfo
				{
					result = ItemStackResponseInfo.Result.Success,
					clientRequestId = request.clientRequestId,
					containers = new List<ItemStackResponseContainerInfo>()
				};

				response.responses.Add(stackResponse);

				try
				{
					stackResponse.containers.AddRange(ItemStackInventoryManager.HandleItemStackActions(request.clientRequestId, request));
				}
				catch (Exception e)
				{
					Log.Warn($"Failed to process inventory actions", e);
					stackResponse.result = ItemStackResponseInfo.Result.Error;
					stackResponse.containers.Clear();
					anyFailed = true;
				}
			}

			SendPacket(response);
			if (anyFailed) ResyncInventoryAfterFailedStackRequest();
		}

		// After rejecting a stack request BDS repairs the client's view of the inventory:
		// InventoryContent for windows 0/0x78/0x7c/0x77 followed by PlayerHotbar (observed
		// live against BDS 1.26.34 answering an invalid CraftCreative request).
		private void ResyncInventoryAfterFailedStackRequest()
		{
			SendPlayerInventory();
			SendPlayerHotbar();
		}

		/// <summary>
		///     A container's slot, addressed the way the client does, by ContainerEnumName. Which store
		///     that name reaches depends on the open screen, so the mapping lives in <see cref="Screen" />.
		/// </summary>
		protected internal virtual Item GetContainerItem(FullContainerName.ContainerEnumName containerId, int slot)
		{
			SlotBinding binding = Screen.Bind(containerId, slot);

			switch (binding.Store)
			{
				case SlotStore.Ui:
					return Inventory.UiInventory.Slots[binding.Index];
				case SlotStore.Main:
					return Inventory.Slots[binding.Index];
				case SlotStore.Offhand:
					return Inventory.OffHand;
				case SlotStore.Armor:
					return binding.Index switch
					{
						0 => Inventory.Helmet,
						1 => Inventory.Chest,
						2 => Inventory.Leggings,
						3 => Inventory.Boots,
						_ => throw new InvalidOperationException($"Armor has no slot {binding.Index}")
					};
				case SlotStore.Block:
					return Screen.BlockInventory.GetSlot((byte) binding.Index);
				default:
					throw new InvalidOperationException($"Container {containerId} resolved to {binding.Store}, which cannot be read");
			}
		}

		protected internal virtual void SetContainerItem(FullContainerName.ContainerEnumName containerId, int slot, Item item)
		{
			SlotBinding binding = Screen.Bind(containerId, slot);

			switch (binding.Store)
			{
				case SlotStore.Ui:
					Inventory.UiInventory.Slots[binding.Index] = item;
					break;
				case SlotStore.Main:
					Inventory.Slots[binding.Index] = item;
					break;
				case SlotStore.Offhand:
					Inventory.OffHand = item;
					break;
				case SlotStore.Armor:
					switch (binding.Index)
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
						default:
							throw new InvalidOperationException($"Armor has no slot {binding.Index}");
					}
					break;
				case SlotStore.Block:
					Screen.BlockInventory.SetSlot(this, (byte) binding.Index, item);
					break;
				default:
					throw new InvalidOperationException($"Container {containerId} resolved to {binding.Store}, which cannot be written");
			}
		}

		public virtual void HandleMcpeUpdatePlayerGameType(McpeUpdatePlayerGameType message)
		{
		}

		public virtual void HandleMcpePositionTrackingDbClientRequest(McpePositionTrackingDbClientRequest message)
		{
		}

		public virtual void HandleMcpeDebugInfo(McpeDebugInfo message)
		{
		}

		public virtual void HandleMcpePacketViolationWarning(McpePacketViolationWarning message)
		{
			Log.Error($"Client reported a level {message.severity} packet violation of type {message.violationType} for packet 0x{message.packetId:X2}: {message.reason}");
		}

		/// <inheritdoc />
		public virtual void HandleMcpeUpdateSubChunkBlocksPacket(McpeUpdateSubChunkBlocksPacket message)
		{
			
		}

		/// <inheritdoc />
		public virtual void HandleMcpeSubChunkRequestPacket(McpeSubChunkRequestPacket message)
		{
			// The block half of the chunk flow: the skeleton LevelChunk carried only biomes, and
			// the client asks here for the sections it wants, as offsets from an origin in absolute
			// sub-chunk coordinates. One entry is answered per offset; the column serializes it.
			// The client rejects a SubChunkPacket with more than 8192 entries (packet violation
			// 0xAE, "too many input elements"), so a mass re-request is answered as a run of
			// packets at the cap instead of one mirror of the request.
			const int maxEntriesPerPacket = 8192;

			McpeSubChunkPacket response = null;

			McpeSubChunkPacket NewResponse()
			{
				var packet = McpeSubChunkPacket.CreateObject();
				packet.cacheEnabled = true;
				packet.dimensionType = message.dimension;
				packet.centerPos = new SubChunkPos {subchunkPositionX = message.originX, subchunkPositionY = message.originY, subchunkPositionZ = message.originZ};
				packet.subchunkData = new List<SubChunkPacketData>();
				return packet;
			}

			foreach (SubChunkPosOffset offset in message.offsets)
			{
				SubChunkPacketData entry = BuildSubChunkEntry(message, offset);

				// Counted per entry, not per request: one request carries many offsets and they can
				// have different outcomes, which is exactly the distinction worth having.
				EngineMetrics.SubChunkResult(entry.subchunkRequestResult.ToString().ToLowerInvariant());
				if (entry.serializedSubChunk != null) EngineMetrics.SubChunkBytes(entry.serializedSubChunk.Length);

				response ??= NewResponse();
				response.subchunkData.Add(entry);

				if (response.subchunkData.Count >= maxEntriesPerPacket)
				{
					SendPacket(response);
					response = null;
				}
			}

			if (response != null) SendPacket(response);
		}

		private SubChunkPacketData BuildSubChunkEntry(McpeSubChunkRequestPacket message, SubChunkPosOffset offset)
		{
			SubChunkPacketData Rejected(SubChunkPacketData.SubchunkRequestResult result) => new SubChunkPacketData
			{
				subchunkPosOffset = offset,
				subchunkRequestResult = result,
				heightMapData = new SubChunkHeightmapData
				{
					heightMapType = SubChunkHeightmapData.HeightMapType.Nodata,
					renderHeightMapType = SubChunkHeightmapData.RenderHeightMapType.Nodata
				}
			};

			if (message.dimension != (int) Level.Dimension) return Rejected(SubChunkPacketData.SubchunkRequestResult.Wrongdimension);

			int sectionY = message.originY + offset.subchunkOffsetY;
			if (!ChunkColumn.IsSectionInBounds(sectionY)) return Rejected(SubChunkPacketData.SubchunkRequestResult.Indexoutofbounds);

			var coordinates = new ChunkCoordinates(message.originX + offset.subchunkOffsetX, message.originZ + offset.subchunkOffsetZ);

			// First request for this column: close the loop on when its skeleton went out. Removed on
			// the first match, so what remains in the dictionary is exactly the set of columns the
			// client has been told about and never come back for.
			if (_skeletonSentAt.TryRemove(coordinates, out long sentAt))
			{
				double millis = (Stopwatch.GetTimestamp() - sentAt) * 1000d / Stopwatch.Frequency;
				EngineMetrics.RecordChunkRequestLatency(millis);

				// Feeds the distribution the stale threshold is derived from: only columns the client
				// actually came back for describe how long coming back normally takes.
				Interlocked.Increment(ref _columnsDrained);
			}

			// Loads the column if it is not resident yet, rather than answering "does not exist" for
			// anything the streaming path has not reached. cacheOnly was here to keep disk IO off the
			// transport receive thread; that is no longer the reason it needs, because this handler
			// does IO and so is disqualified from inline dispatch outright. Rejecting instead of
			// loading is what left a dense hand-built map rendering as void with ore and lava floating
			// in it: the client asks for sub-chunks faster than the publisher loads columns, and it
			// does not come back for a rejection.
			ChunkColumn chunkColumn = Level.GetChunk(coordinates);
			if (chunkColumn == null) return Rejected(SubChunkPacketData.SubchunkRequestResult.Levelchunkdoesntexist);

			// A section asked for twice was evicted client-side and re-marked by a fresh skeleton;
			// counting those is how the client's eviction behaviour stays visible.
			bool seenBefore;
			lock (_subChunksServed) seenBefore = !_subChunksServed.Add((coordinates, sectionY));
			if (seenBefore) EngineMetrics.SubChunkReRequested();

			return chunkColumn.GetSubChunkData(offset, sectionY);
		}

		public virtual void HandleMcpeMobArmorEquipment(McpeMobArmorEquipment message)
		{
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

		public virtual void OpenScreen(Screen screen)
		{
			Screen = screen;
		}

		/// <summary>Block inventories carry a fixed window id per kind (see InventoryManager), all of
		/// them below this, and the client reserves 0x77 and up for its own windows. Everything in
		/// between is the server's to hand out.</summary>
		private const byte FirstScreenWindowId = 30;

		private byte _nextScreenWindowId = FirstScreenWindowId;

		private byte NextScreenWindowId()
		{
			if (_nextScreenWindowId >= 0x77) _nextScreenWindowId = FirstScreenWindowId;

			return _nextScreenWindowId++;
		}

		/// <summary>Opens a screen the client drives on its own: an anvil, a loom, a smithing table.
		/// Every slot in one of these lives in the flat UI window, so the server holds nothing and
		/// opening it is just naming it. Whatever was open is closed first, because the client refuses
		/// to open a second window while it believes one is still up.</summary>
		public virtual void OpenUiScreen(ScreenKind kind, ContainerType type, BlockCoordinates coordinates)
		{
			lock (_inventorySync)
			{
				if (Screen.Kind != ScreenKind.Inventory) HandleMcpeContainerClose(null);

				byte windowId = NextScreenWindowId();
				OpenScreen(new Screen(kind, type, windowId, coordinates, null));

				var containerOpen = McpeContainerOpen.CreateObject();
				containerOpen.windowId = windowId;
				containerOpen.type = (byte) type;
				containerOpen.coordinates = coordinates;
				// -1, the same as every block container BDS opens. The anvil, the crafting table and
				// the loom used to send EntityIdSelf here and the client took that too, so the field
				// looks unread for a screen that names a block.
				containerOpen.actorUniqueId = -1;
				SendPacket(containerOpen);
			}
		}

		private static ScreenKind ScreenKindOf(BlockEntity blockEntity)
		{
			return blockEntity switch
			{
				FurnaceBlockEntity => ScreenKind.Furnace,
				BlastFurnaceBlockEntity => ScreenKind.BlastFurnace,
				SmokerBlockEntity => ScreenKind.Smoker,
				BrewingStandBlockEntity => ScreenKind.BrewingStand,
				EnchantingTableBlockEntity => ScreenKind.EnchantingTable,
				_ => ScreenKind.Container
			};
		}

		public void OpenInventory(BlockCoordinates inventoryCoord)
		{
			// https://github.com/pmmp/PocketMine-MP/blob/stable/src/pocketmine/network/mcpe/protocol/types/WindowTypes.php
			lock (_inventorySync)
			{
				if (Screen.Kind != ScreenKind.Inventory)
				{
					if (Screen.BlockInventory != null && Screen.Coordinates.Equals(inventoryCoord)) return;
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

				OpenScreen(new Screen(ScreenKindOf(inventory.BlockEntity), (ContainerType) inventory.Type, inventory.WindowsId, inventoryCoord, inventory));

				// A barrel has no open animation to broadcast: its lid is a block state, so it opens by
				// changing the block. Both of these ask "is anyone else already looking at it" before
				// the subscription below, and answer again after the unsubscription in the close.
				if (inventory.BlockEntity is BarrelBlockEntity && !inventory.IsOpen()) SetBarrelLid(inventoryCoord, true);

				// Chest open animation.
				if (inventory.BlockEntity is ChestBlockEntity or ShulkerBoxBlockEntity && !inventory.IsOpen())
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
				containerOpen.actorUniqueId = -1;
				SendPacket(containerOpen);

				var containerSetContent = McpeInventoryContent.CreateObject();
				containerSetContent.inventoryId = inventory.WindowsId;
				containerSetContent.input = inventory.Slots;
				SendPacket(containerSetContent);
			}
		}

		/// <summary>The barrel's open lid, which is a block state rather than the block event a chest
		/// answers with. Everyone in range sees it, so it follows whether ANY player has the barrel
		/// open, not this one.</summary>
		private void SetBarrelLid(BlockCoordinates coordinates, bool open)
		{
			if (Level?.GetBlock(coordinates) is not Barrel barrel || barrel.OpenBit == open) return;

			barrel.OpenBit = open;

			// Same block, same opacity, one state bit: nothing to relight and nothing to fall, so a
			// barrel being opened does not drag a skylight recalculation behind it.
			Level.SetBlock(barrel, applyPhysics: false, calculateLight: false);
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


		public virtual void HandleMcpeInventorySlot(McpeInventorySlot message)
		{
		}

		public virtual void HandleMcpeInventoryTransaction(McpeInventoryTransaction message)
		{
			switch (message.transaction)
			{
				case InventoryMismatchData inventoryMismatchTransaction:
					HandleInventoryMismatchTransaction(inventoryMismatchTransaction);
					break;
				case ItemReleaseInventoryTransaction itemReleaseTransaction:
					HandleItemReleaseTransaction(itemReleaseTransaction);
					break;
				case ItemUseOnActorInventoryTransaction itemUseOnEntityTransaction:
					HandleItemUseOnEntityTransaction(itemUseOnEntityTransaction);
					break;
				case ItemUseInventoryTransaction itemUseTransaction:
					HandleItemUseTransaction(itemUseTransaction);
					break;
				case NormalTransactionData normalTransaction:
					HandleNormalTransaction(normalTransaction);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		protected virtual void HandleItemUseOnEntityTransaction(ItemUseOnActorInventoryTransaction transaction)
		{
			switch (transaction.actionType)
			{
				case ItemUseOnActorInventoryTransaction.ItemUseOnActorActionType.Interact: // Right click
					EntityInteract(transaction);
					break;
				case ItemUseOnActorInventoryTransaction.ItemUseOnActorActionType.Attack: // Left click
					EntityAttack(transaction);
					break;
				case ItemUseOnActorInventoryTransaction.ItemUseOnActorActionType.ItemInteract:
					Log.Warn($"Got Entity ItemInteract. Was't sure it existed, but obviously it does :-o");
					EntityItemInteract(transaction);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void EntityItemInteract(ItemUseOnActorInventoryTransaction transaction)
		{
			Item itemInHand = Inventory.GetItemInHand();
			if (!itemInHand.Name.Equals(transaction.item.Name, StringComparison.OrdinalIgnoreCase) || itemInHand.Metadata != transaction.item.Metadata)
			{
				Log.Warn($"Attack item mismatch. Expected {itemInHand}, but client reported {transaction.item}");
			}

			if (!Level.TryGetEntity(transaction.runtimeId, out Entity target)) return;
			target.DoItemInteraction(this, itemInHand);
		}

		protected virtual void EntityInteract(ItemUseOnActorInventoryTransaction transaction)
		{
			DoInteraction((int) transaction.actionType, this);

			if (!Level.TryGetEntity(transaction.runtimeId, out Entity target)) return;
			target.DoInteraction((int) transaction.actionType, this);
		}

		protected virtual void EntityAttack(ItemUseOnActorInventoryTransaction transaction)
		{
			Item itemInHand = Inventory.GetItemInHand();
			if (!itemInHand.Name.Equals(transaction.item.Name, StringComparison.OrdinalIgnoreCase) || itemInHand.Metadata != transaction.item.Metadata)
			{
				Log.Warn($"Attack item mismatch. Expected {itemInHand}, but client reported {transaction.item}");
			}

			if (!Level.TryGetEntity(transaction.runtimeId, out Entity target)) return;


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

		protected virtual void HandleInventoryMismatchTransaction(InventoryMismatchData transaction)
		{
			Log.Warn($"Transaction mismatch");
		}

		protected virtual void HandleItemReleaseTransaction(ItemReleaseInventoryTransaction transaction)
		{
			Item itemInHand = Inventory.GetItemInHand();

			switch (transaction.actionType)
			{
				case ItemReleaseInventoryTransaction.ItemReleaseActionType.Release:
				{
					itemInHand.Release(Level, this, transaction.fromPosition);
					break;
				}
				case ItemReleaseInventoryTransaction.ItemReleaseActionType.Use:
				{
					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
			}

			HandleTransactionRecords(transaction.actions);
		}

		protected virtual void HandleItemUseTransaction(ItemUseInventoryTransaction transaction)
		{
			var itemInHand = Inventory.GetItemInHand();

			switch (transaction.actionType)
			{
				case ItemUseInventoryTransaction.ItemUseActionType.Place:
				{
					Level.Interact(this, itemInHand, transaction.position, (BlockFace) transaction.face, transaction.clickPosition);
					break;
				}
				case ItemUseInventoryTransaction.ItemUseActionType.Use:
				{
					itemInHand.UseItem(Level, this, transaction.position);
					break;
				}
				case ItemUseInventoryTransaction.ItemUseActionType.Destroy:
				{
					//TODO: Add face and other parameters to break. For logic in break block.
					Level.BreakBlock(this, transaction.position, (BlockFace) transaction.face);
					break;
				}
			}

			HandleTransactionRecords(transaction.actions);
		}

		protected virtual void HandleNormalTransaction(NormalTransactionData transaction)
		{
			HandleTransactionRecords(transaction.actions);
		}

		/// <summary>
		///     The actions a transaction carries. Since the packet moved onto the schemas these are
		///     InventoryAction, where the kind is the source's own type rather than a subclass, and
		///     the item the client says the slot ends up holding is toItem.
		/// </summary>
		/// <summary>
		///     Only the world-interaction source still arrives here. Slot movement moved to
		///     ItemStackRequest with the stack net id, which is a stronger claim than "the item I
		///     believe is in that slot", so container records stopped being sent and the code that
		///     verified them against the server's own slots went with them. Dropping an item is
		///     what is left.
		/// </summary>
		protected virtual void HandleTransactionRecords(List<InventoryAction> records)
		{
			if (records == null || records.Count == 0) return;

			foreach (InventoryAction record in records)
			{
				Item newItem = record.toItem;

				switch (record.source?.sourceType)
				{
					case InventorySource.InventorySourceType.CreativeInventory:
					{
						throw new Exception($"This should never happen with new inventory transactions");
					}
					case InventorySource.InventorySourceType.WorldInteraction:
					{
						// Drop
						Item sourceItem = Inventory.GetItemInHand();

						if (!newItem.Name.Equals(sourceItem.Name, StringComparison.OrdinalIgnoreCase)) Log.Warn($"Inventory mismatch. Client reported drop item as {newItem} and it did not match existing item {sourceItem}");

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
				.Where(r => ((ShapedRecipe) r).Result.First().Name.Equals(result.Name, StringComparison.OrdinalIgnoreCase) && ((ShapedRecipe) r).Result.First().Metadata == result.Metadata).ToList();

			recipes.AddRange(RecipeManager.Recipes
				.Where(r => r is ShapelessRecipe)
				.Where(r => ((ShapelessRecipe) r).Result.First().Name.Equals(result.Name, StringComparison.OrdinalIgnoreCase) && ((ShapelessRecipe) r).Result.First().Metadata == result.Metadata).ToList());

			Log.Debug($"Found {recipes.Count} matching recipes with the result {result}");

			if (recipes.Count == 0) return false;

			var input = craftingInput.Where(i => i != null && !i.IsAir).ToList();

			foreach (var recipe in recipes)
			{
				List<Item> ingredients = null;
				switch (recipe)
				{
					case ShapedRecipe shapedRecipe:
					{
						ingredients = shapedRecipe.Input.Where(i => i != null && !i.IsAir).ToList();
						break;
					}
					case ShapelessRecipe shapelessRecipe:
					{
						ingredients = shapelessRecipe.Input.Where(i => i != null && !i.IsAir).ToList();
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

				return x.Name.Equals(y.Name, StringComparison.OrdinalIgnoreCase) && (x.Metadata == y.Metadata || x.Metadata == short.MaxValue || y.Metadata == short.MaxValue);
			}

			public int GetHashCode(Item obj)
			{
				return 0;
			}
		}

		/// <summary>Lets go of whatever screen is open, server side only, and returns what was let go
		/// of. Everything here says "this player is no longer looking": the subscription, the observer
		/// registration, and the lid or animation that told the room. Answering the client is the
		/// caller's business, because a player who has just disconnected has nothing left to answer to,
		/// and a shared inventory that keeps a departed player as an observer counts as open forever.</summary>
		protected virtual Screen ReleaseScreen()
		{
			lock (_inventorySync)
			{
				Screen closing = Screen;
				Screen = new Screen(ScreenKind.Inventory);

				if (closing.BlockInventory is not Inventory inventory) return closing;

				// unsubscribe to inventory changes
				inventory.InventoryChange -= OnInventoryChange;
				inventory.RemoveObserver(this);

				if (inventory.BlockEntity is BarrelBlockEntity && !inventory.IsOpen()) SetBarrelLid(inventory.Coordinates, false);

				// close container
				if (inventory.BlockEntity is ChestBlockEntity or ShulkerBoxBlockEntity && !inventory.IsOpen())
				{
					var tileEvent = McpeBlockEvent.CreateObject();
					tileEvent.coordinates = inventory.Coordinates;
					tileEvent.case1 = 1;
					tileEvent.case2 = 0;
					Level.RelayBroadcast(tileEvent);
				}

				return closing;
			}
		}

		public virtual void HandleMcpeContainerClose(McpeContainerClose message)
		{
			lock (_inventorySync)
			{
				Screen closing = ReleaseScreen();

				if (closing.Kind == ScreenKind.Horse) return;

				// The player's own screen is answered with an allocated id (see the self open-inventory
				// case in HandleMcpeInteract) that is never recorded here, so only a screen the server
				// opened has an id worth comparing.
				if (message != null && closing.Kind != ScreenKind.Inventory && message.windowId != closing.WindowId)
				{
					Log.Warn($"Client closed window {message.windowId} while {closing.Kind} held window {closing.WindowId}");
				}

				// A close the client asked for is ALWAYS answered, with the id it named. Skipping the
				// answer when the ids disagree leaves the client believing its window is still open,
				// and it then refuses to open any other: no inventory, no chest, and the lid of the
				// one it thinks is open stays up because the block event above never ran either.
				var closePacket = McpeContainerClose.CreateObject();
				closePacket.windowId = message?.windowId ?? closing.WindowId;
				closePacket.windowType = message?.windowType ?? (byte) ContainerType.None;
				closePacket.server = message == null;
				SendPacket(closePacket);
			}
		}

		public virtual void HandleMcpePlayerHotbar(McpePlayerHotbar message)
		{
		}

		public virtual void HandleMcpeInventoryContent(McpeInventoryContent message)
		{
		}

		/// <summary>
		///     Handles the interact.
		/// </summary>
		/// <param name="message">The message.</param>
		public virtual void HandleMcpeInteract(McpeInteract message)
		{
			//Log.Info($"Interact. Target={message.targetRuntimeEntityId} Action={message.actionId} Position={message.position}");
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
						// Opening the player's own screen replaces whatever was open, so a block
						// screen still recorded here has to be torn down first: leaving it makes the
						// player's next close carry window id 2 against a chest's window id.
						if (Screen.Kind != ScreenKind.Inventory) HandleMcpeContainerClose(null);

						// Mirrors vanilla's answer to a self open-inventory request (captured live
						// from BDS 1.26.34): an ALLOCATED window id (2, never the reserved
						// inventory id 0), type 255 (none), the player's block position and
						// runtime entity id -1. Sending window id 0 here corrupts the client's
						// own-inventory screen state.
						var containerOpen = McpeContainerOpen.CreateObject();
						containerOpen.windowId = 2;
						containerOpen.type = 255;
						containerOpen.coordinates = (BlockCoordinates) KnownPosition;
						containerOpen.actorUniqueId = -1;
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
				Item item = ItemFactory.GetItemByName("minecraft:spawn_egg", (short) EntityHelpers.ToEntityType(entity.EntityTypeId));

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
			var levelSettings = new LevelSettings
			{
				spawnSettings = new SpawnSettings
				{
					dimension = (int) (Level?.Dimension ?? 0),
					userDefinedBiomeName = Level.SpawnBiomeName,
					spawnBiomeType = (SpawnSettings.SpawnBiomeType) Level.SpawnBiomeType
				},
				seed = (ulong) Level.Seed,
				generatorType = (LevelSettings.GeneratorType) Level.GeneratorType,
				gameType = (LevelSettings.GameType) GameMode,
				gameDifficulty = (LevelSettings.Difficulty) Level.Difficulty,
				// The LEVEL spawn, not this player's: SpawnPosition is per-player and plugins
				// (Plotter) persist it, so it is wherever this player last was. Vanilla puts the
				// world's fixed spawn block here.
				defaultSpawnBlockPosition = new BlockCoordinates((int) Level.SpawnPoint.X, (int) Level.SpawnPoint.Y, (int) Level.SpawnPoint.Z),
				achievementsDisabled = Level.AchievementsDisabled,
				dayCycleStopTime = (int) Level.WorldTime,
				educationEditionOffer = PlayerInfo.Edition == 1 ? LevelSettings.EducationEditionOffer.Restofworld : LevelSettings.EducationEditionOffer.None,
				educationProductId = "",
				rainLevel = Level.RainLevel,
				lightningLevel = Level.LightningLevel,
				multiplayerGameIntent = Level.IsMultiplayer,
				lanBroadcastIntent = Level.BroadcastToLan,
				commandsEnabled = EnableCommands,
				texturePacksRequired = Level.IsTexturepacksRequired,
				gamerules = Level.GetGameRules(),
				experiments = new Experiments(),
				hasBonusChestEnabled = Level.BonusChest,
				startWithMapEnabled = Level.MapEnabled,
				playerPermissions = (LevelSettings.PlayerPermissionLevel) PermissionLevel,
				// "*" is what vanilla sends here, not the version string and not empty.
				baseGameVersion = "*",

				// This server is not Education Edition. Sending true put every client into edu mode,
				// which changes chat, permissions and the player roster UI.
				educationFeaturesEnabled = false,
				eduSharedUriResource = new EduSharedUriResource {buttonName = "", linkUri = ""},

				serverChunkTickRange = Level.ServerChunkTickRange,
				useMsaGamertagsOnly = Level.UseMsaGamertagsOnly,
				limitedWorldWidth = Level.LimitedWorldWidth,
				limitedWorldDepth = Level.LimitedWorldLength,
				xboxLiveBroadcastSetting = Level.XboxLiveBroadcastMode,
				platformBroadcastSetting = Level.PlatformBroadcastMode,
			};

			var startGame = McpeStartGame.CreateObject();
			startGame.settings = levelSettings;
			startGame.entityIdSelf = EntityId;
			startGame.runtimeEntityId = EntityManager.EntityIdSelf;
			startGame.gameType = McpeStartGame.GameType.Default; // fallback: use the level's game mode, like vanilla
			// Eye height, like every other position we send. SpawnPosition is feet, and the client
			// subtracts the offset to place them, so sending it raw spawns the player 1.62 low,
			// which is inside the ground when the spawn is snapped to the surface.
			startGame.position = new Vector3(SpawnPosition.X, SpawnPosition.Y + 1.62f, SpawnPosition.Z);
			startGame.rotation = new Vector2(KnownPosition.Pitch, KnownPosition.HeadYaw);

			// A stable but non-legacy level id: the client keys local caches on world identity,
			// and the old constant id may pin poisoned cache entries from early broken sessions.
			startGame.levelId = "minet-" + (Level.LevelName ?? "world");
			startGame.levelName = string.IsNullOrEmpty(Level.LevelName) ? "MiNET" : Level.LevelName;
			startGame.templateContentIdentity = "00000000-0000-0000-0000-000000000000"; // vanilla sends the zero uuid as a string, not empty
			startGame.isTrial = Level.IsTrial;
			// How SubChunk.WriteStore encodes chunk palettes. The two must always agree, so both
			// come from the one setting. The palette itself is never sent, in either mode: vanilla
			// stopped sending it, and index mode indexes the client's own canonical palette. That
			// is what makes index mode demanding: our palette order has to be the client's, exactly.
			startGame.blockNetworkIdsAreHashes = SubChunk.BlockNetworkIdsAreHashes;
			// Wire behaviour switches, not world settings: server-auth block breaking must match how
			// this server actually handles breaking, so it is not somebody's to configure.
			startGame.movementSettings = new SyncedPlayerMovementSettings
			{
				rewindHistorySize = Level.MovementRewindHistorySize,
				serverAuthoritativeBlockBreaking = true
			};
			startGame.serverAuthSoundEnabled = true;
			startGame.levelCurrentTime = (ulong) Level.TickTime;
			startGame.enchantmentSeed = Level.EnchantmentSeed;
			startGame.enableItemStackNetManager = true;
			startGame.blockProperties = new List<ServerBlockProperty>();
			startGame.playerPropertyData = new Nbt {NbtFile = new NbtFile(new NbtCompound("")) {BigEndian = false, UseVarInt = true}};
			// 0 disables the client's palette-checksum verification. NEVER mirror BDS's value:
			// the client recomputes the checksum locally and rejects the join with "Blocks
			// between client and server do not match" on any mismatch (observed live 2026-07-31;
			// the mirrored 1.26.34 value failed a 1.26.33 client). PMMP ships 0 for the same
			// reason. Computing the real value needs the exact vanilla algorithm; until then 0.
			startGame.serverBlockTypeRegistryChecksum = 0;
			startGame.serverVersion = McpeProtocolInfo.GameVersion;
			startGame.worldTemplateId = new UUID(new byte[16]);
			// Session correlation id in vanilla's "<raknet>xxxx-xxxx-xxxx-xxxx" shape.
			startGame.multiplayerCorrelationId = "<raknet>" + Guid.NewGuid().ToString("N").Substring(0, 16).Insert(4, "-").Insert(9, "-").Insert(14, "-");
			startGame.serverEnabledClientsideGeneration = false;
			// Vanilla sends the join-info block with all three optional sub-blocks absent.
			startGame.serverConfigurationJoinInfo = new ServerConfig();
			startGame.serverTelemetryData = new ServerTelemetryData {serverId = "", scenarioId = "", worldId = "", ownerId = ""};

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
			mcpeSetSpawnPosition.dimension = (int) Level.Dimension;
			SendPacket(mcpeSetSpawnPosition);
		}

		private object _sendChunkSync = new object();

		// Gate keeping chunk streaming behind the login send-sequence. The client's
		// RequestChunkRadius arrives while the login thread is still sending the pre-spawn burst
		// (item registry, biome definitions, creative content, commands), and the chunk task would
		// otherwise race past it on the same ordered channel and deliver chunks + PlayStatus(3)
		// before the registries. BDS sends the registries first and PlayStatus(3) last; a strict
		// 1.26 client disconnects when told to spawn without an item registry.
		private readonly ManualResetEventSlim _loginSequenceCompleted = new ManualResetEventSlim(false);

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
						chunk.chunkPosition = new ChunkPos {x = chunkPosition.X + x, z = chunkPosition.Z + z};
						chunk.cacheMetadata = new List<ulong>();
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
			SendNetworkChunkPublisherUpdate(ChunkRadius);
		}

		/// <summary>Publishes an area centred somewhere other than where the player currently is.</summary>
		public void SendNetworkChunkPublisherUpdate(PlayerLocation position)
		{
			SendNetworkChunkPublisherUpdate(ChunkRadius, position);
		}

		// The column the publish area was last centred on. Written only from the dispatch thread that
		// handles movement, so it needs no synchronization of its own.
		private ChunkCoordinates _lastPublishedChunk = new ChunkCoordinates(int.MaxValue, int.MaxValue);

		/// <summary>
		///     Republishes the live chunk area when the player enters a new column, and does nothing
		///     otherwise. Cheap enough to sit on the movement path: one coordinate and one varint,
		///     against a client that otherwise keeps treating a stale centre as authoritative.
		/// </summary>
		private void MaybePublishChunkArea()
		{
			if (!IsSpawned || ChunkRadius <= 0) return;

			var chunkPosition = new ChunkCoordinates(KnownPosition);
			if (chunkPosition == _lastPublishedChunk) return;

			_lastPublishedChunk = chunkPosition;
			SendNetworkChunkPublisherUpdate();
		}

		/// <summary>
		///     Radius is in BLOCKS. Vanilla publishes a fixed <see cref="JoinBurstChunkRadius" /> for
		///     the whole join burst and switches to the negotiated radius only afterwards. The burst
		///     value must not depend on the negotiated one, because RequestChunkRadius can arrive
		///     before the burst reaches this point.
		/// </summary>
		public void SendNetworkChunkPublisherUpdate(int chunkRadius, PlayerLocation position = null)
		{
			SendPacket(CreateNetworkChunkPublisherUpdate(chunkRadius, position));
		}

		/// <summary>
		///     Built rather than sent so the chunk sweeps can put it at the head of their first
		///     skeleton group: the publish and the columns it covers then ride one wrapper.
		/// </summary>
		private McpeNetworkChunkPublisherUpdate CreateNetworkChunkPublisherUpdate(int chunkRadius, PlayerLocation position = null)
		{
			var pk = McpeNetworkChunkPublisherUpdate.CreateObject();

			// The centre is where the chunks being published ARE, which is not always where the
			// player is. Respawn, teleport and both dimension paths stream the destination before
			// SetPosition moves anyone onto it, so taking KnownPosition there publishes an area
			// around the place they are leaving and the client drops everything just sent.
			pk.coordinates = (position ?? KnownPosition).GetCoordinates3D();

			// The radius the view actually covers, in blocks. Every captured BDS value is exactly
			// view x 16 (64/80/160/192 for views 4/5/10/12), no rounding or padding for rim
			// coverage, so the client converts to chunk space itself rather than testing
			// block-precise containment.
			pk.radius = (uint) (chunkRadius * 16);
			return pk;
		}

		/// <summary>
		///     One column, the one the player is about to be put on, so there is ground under them
		///     the moment they arrive. Respawn, teleport and both dimension paths all send terrain
		///     before SetPosition moves anyone onto it, which is why the position is a parameter and
		///     not KnownPosition: that is still the place they are leaving.
		///     <para>
		///         A single column rather than the radius on purpose. The rest streams asynchronously
		///         once the player is placed, whereas generating a whole radius here runs inside the
		///         teleport lock and blocks the arrival: at radius 12 that is some 450 columns out of
		///         the world provider before the client is told anything at all.
		///     </para>
		///     <para>
		///         The publish has to move with it. The client drops chunks outside the area it was
		///         last told about, and that area is centred wherever this says, not on the player.
		///     </para>
		/// </summary>
		private void ForcedSendChunk(PlayerLocation position)
		{
			lock (_sendChunkSync)
			{
				var chunkPosition = new ChunkCoordinates(position);
				ChunkColumn column = Level.GetChunk(chunkPosition);

				if (column == null) return;

				// Pushed, not announced. This is the column the player is standing in during a join
				// or a teleport, and it has to be there when they arrive rather than after a round
				// trip they have to wait for.
				McpeLevelChunk chunk = column.CreateCachedPushChunk();
				_chunksUsed[chunkPosition] = column.Version;

				SendNetworkChunkPublisherUpdate(position);
				_lastPublishedChunk = chunkPosition;

				SendPacket(chunk);
			}
		}

		/// <summary>
		///     Streams the radius around <paramref name="position" />, defaulting to where the player
		///     currently is.
		/// </summary>
		public void ForcedSendChunks(Action postAction = null, PlayerLocation position = null)
		{
			Monitor.Enter(_sendChunkSync);
			try
			{
				var chunkPosition = new ChunkCoordinates(position ?? KnownPosition);

				_currentChunkPosition = chunkPosition;

				if (Level == null) return;

				// Centred on what is being sent, not on where the player stands. See the overload.
				// The publish centre and the streamed centre must agree, so the movement path's
				// "have I already published this column" memory is updated to match rather than
				// left claiming the old one.
				_lastPublishedChunk = chunkPosition;

				// The whole pass accumulates into list-form sends, publisher update at the head:
				// the transport decides what shares a wrapper and a datagram. Groups flush at
				// the blob-status boundary like the streamer does.
				// Small publisher + spawn block first, full publisher ahead of the rest: the
				// same join-burst ordering as the streamer, so the client can complete and draw
				// the near area before being told about the whole view.
				var group = new List<Packet> {CreateNetworkChunkPublisherUpdate(JoinBurstChunkRadius, position ?? KnownPosition)};
				int groupHashes = 0;

				// A forced pass is a join or a level change, so it happens once and covers the whole
				// disc. What it cost is worth saying out loud: at a large view distance this is the
				// wait a player experiences, and split by phase it says whether the time went into
				// reading columns or into handing them to the transport.
				long passStarted = Stopwatch.GetTimestamp();
				int spawnColumns = 0, streamColumns = 0;

				foreach ((ChunkCoordinates _, McpeLevelChunk chunk) in Level.GenerateChunks(_currentChunkPosition, _chunksUsed, Math.Min(JoinBurstChunkRadius, ChunkRadius), prune: false, cachedPush: true))
				{
					if (chunk != null)
					{
						group.Add(chunk);
						spawnColumns++;
					}
				}

				long spawnBuilt = Stopwatch.GetTimestamp();
				SendPackets(group);
				long spawnSent = Stopwatch.GetTimestamp();
				group = new List<Packet> {CreateNetworkChunkPublisherUpdate(ChunkRadius, position ?? KnownPosition)};

				// The rest of the disc at the blob-status boundary (see the streamer).
				foreach ((ChunkCoordinates _, McpeLevelChunk chunk) in Level.GenerateChunks(_currentChunkPosition, _chunksUsed, ChunkRadius, cachedPush: true))
				{
					if (chunk == null) continue;

					group.Add(chunk);
					streamColumns++;
					groupHashes += chunk.cacheMetadata?.Count ?? 0;

					if (groupHashes >= GroupFlushHashes)
					{
						SendPackets(group);
						group = new List<Packet>();
						groupHashes = 0;
					}
				}
				if (group.Count > 0) SendPackets(group);

				long passEnded = Stopwatch.GetTimestamp();
				double Ms(long from, long to) => (to - from) * 1000d / Stopwatch.Frequency;
				Log.Info($"Forced chunk pass for {Username} on {Level?.LevelId}: radius {ChunkRadius}, spawn block {spawnColumns} columns in {Ms(passStarted, spawnBuilt):F0}ms (send {Ms(spawnBuilt, spawnSent):F0}ms), rest {streamColumns} columns in {Ms(spawnSent, passEnded):F0}ms, total {Ms(passStarted, passEnded):F0}ms");
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
			// See _loginSequenceCompleted: never stream chunks (or the PlayStatus 3 they trigger)
			// before the pre-spawn sequence has gone out. No-op after login.
			if (!_loginSequenceCompleted.Wait(15000)) return;

			// A pass already running does not mean this trigger can be thrown away: the player has
			// moved somewhere the running pass was not computed for, and nothing else will ever come
			// back for it - the next trigger only fires on the NEXT chunk boundary crossed. Measured at
			// 386 lost triggers in 45 seconds on a big Anvil map, which is exactly as many patches of
			// world that silently never streamed. Remembered here and re-run once below instead, so
			// any number of triggers during one pass coalesce into exactly one more pass.
			if (!Monitor.TryEnter(_sendChunkSync))
			{
				Volatile.Write(ref _chunkPassPending, 1);
				EngineMetrics.ChunkPassSkipped();
				return;
			}

			try
			{
				if (ChunkRadius <= 0) return;

				if (!IsSpawned) SendChunkRadiusUpdate();


				// Consumed before the position guards below: a forced pass is one where the streamed
				// area was invalidated without the player moving, which is exactly the case those
				// guards would otherwise throw away.
				bool forced = Interlocked.Exchange(ref _forceChunkPass, 0) == 1;

				var chunkPosition = new ChunkCoordinates(KnownPosition);
				if (!forced && IsSpawned && _currentChunkPosition == chunkPosition) return;

				if (!forced && IsSpawned && _currentChunkPosition.DistanceTo(chunkPosition) < MoveRenderDistance)
				{
					return;
				}

				_currentChunkPosition = chunkPosition;

				int packetCount = 0;

				if (Level == null) return;

				// HeadYaw, not Yaw: the head is what the player is actually looking along, and the body
				// lags it. The sweep stays radial; this only decides which side of the circle is served
				// first, which is what a player perceives while a large pass is still running.
				//
				// The pass accumulates into a list-form send, publisher update at its head, and
				// the transport decides what shares a wrapper and a datagram. The mid-pass
				// flushes are the spawn gate below (ordering, not grouping) and the blob-status
				// boundary in the loop (grouping matched to what the client can answer).
				var group = new List<Packet>();
				var groupCoordinates = new List<ChunkCoordinates>();
				int groupHashes = 0;

				void FlushGroup()
				{
					if (group.Count == 0) return;

					// Stamped at the actual send, not at build: the stamp times the client's
					// turnaround from skeleton to first sub-chunk request, and buffering time in
					// the list is ours, not the client's. groupCoordinates carries only the
					// skeletons, so the non-skeleton head of the list is never stamped.
					long now = Stopwatch.GetTimestamp();
					foreach (ChunkCoordinates stamped in groupCoordinates)
					{
						EngineMetrics.SkeletonSent();
						_skeletonSentAt[stamped] = now;
					}

					SendPackets(group);
					group = new List<Packet>();
					groupCoordinates.Clear();
					groupHashes = 0;
				}

				// Delivery is a per-player STATE: the first pass streams the whole burst in the
				// pull flow, and only after those first sends does the player switch to the
				// push phase. The flag flips at the end of the pass that streamed the burst -
				// send-driven, deliberately not derived from IsSpawned, which the client can
				// flip mid-pass (a bot acknowledges PlayStatus(3) within milliseconds).
				bool spawningPass = !_firstBurstSent;

				// The spawn block exists ONLY on the initial spawn pass. Movement passes never
				// re-enter this: re-declaring the small area mid-session momentarily shrinks the
				// published area, and a faithfully evicting client obeys it - measured at 200
				// walking bots as 600k+ sub-chunk re-fetches of ring columns evicted and re-sent
				// on every boundary crossing. Vanilla never shrinks the area after the burst.
				if (spawningPass)
				{
					// The join-burst publisher first, the way vanilla does it: the client is told
					// about the small spawn area only, so the columns that follow COMPLETE it and
					// it can draw. prune: false because this radius is not the published area,
					// and the spawn goes out right after it: block on the queue first,
					// PlayStatus(3) second.
					group.Add(CreateNetworkChunkPublisherUpdate(JoinBurstChunkRadius));
					foreach ((ChunkCoordinates coordinates, McpeLevelChunk chunk) in Level.GenerateChunks(_currentChunkPosition, _chunksUsed, Math.Min(JoinBurstChunkRadius + 2, ChunkRadius), prune: false))
					{
						if (chunk == null) continue;

						group.Add(chunk);
						groupCoordinates.Add(coordinates);
						packetCount++;
					}

					FlushGroup();

					if (!IsSpawned) InitializePlayer();
				}

				// Now the world grows: the full-radius publisher rides at the head of the next
				// group, exactly where vanilla widens the area after the join burst.
				group.Add(CreateNetworkChunkPublisherUpdate(ChunkRadius));

				// The big sweep, cost-ordered with the view bias. Everything block 1 sent is
				// already versioned in _chunksUsed, so it yields as null here and only the rest
				// of the disc travels, at the blob-status boundary: one ClientCacheBlobStatus
				// answers at most 4095 ids, so each block is one the client can settle with a
				// single status packet.
				//
				// Delivery mode is phased per player: the spawn pass streams the pull flow (the
				// client's request selectivity tames the cold disc), and once spawned the rim
				// delta switches to cached push - a walking player takes every rim column
				// anyway, so pull's skeleton+request+response per column collapses to one
				// pushed packet. Pushed columns stay OUT of the request-latency bookkeeping
				// (groupCoordinates): nothing will ever request them, and stamping them would
				// false-positive the adaptive radius's never-requested signal.
				// The streamer's own call: a spawning pass leaves the client to ask, because its
				// request selectivity is what makes a cold horizon bearable, and every pass after
				// that pushes, because a walking player takes the whole rim anyway.
				bool pushRim = !spawningPass;
				foreach ((ChunkCoordinates coordinates, McpeLevelChunk chunk) in Level.GenerateChunks(_currentChunkPosition, _chunksUsed, ChunkRadius, () => KnownPosition, KnownPosition.HeadYaw, cachedPush: pushRim))
				{
					if (chunk != null)
					{
						group.Add(chunk);
						if (!pushRim) groupCoordinates.Add(coordinates);
						groupHashes += chunk.cacheMetadata?.Count ?? 0;

						if (groupHashes >= GroupFlushHashes)
						{
							FlushGroup();
							group.Add(CreateNetworkChunkPublisherUpdate(ChunkRadius));
						}
					}

					packetCount++;
				}

				FlushGroup();

				// The first sends are done, so from the next pass on this player's rim is pushed.
				_firstBurstSent = true;

				// A client does not request sub-chunks until it has been told to spawn, so gating the
				// spawn on sub-chunk responses would deadlock the join.
				EngineMetrics.ChunkPassCompleted(packetCount);
				EngineMetrics.ChunkNeverAsked(_skeletonSentAt.Count);

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

			// Outside the lock, so the queued pass can take it immediately. Exchange rather than a
			// read-then-clear: a trigger arriving between those two steps would be dropped by exactly
			// the bug this exists to close.
			if (Interlocked.Exchange(ref _chunkPassPending, 0) == 1 && IsConnected)
			{
				MiNetServer.FastThreadPool.QueueUserWorkItem(SendChunksForKnownPosition);
			}
		}

		// Set when a streaming pass was triggered while another was already running; cleared by the
		// pass that honours it. See SendChunksForKnownPosition's TryEnter.
		private int _chunkPassPending;

		// Chunk delivery phase: false until the first burst has actually been SENT (flipped at
		// the end of the pass that streamed it), after which movement passes may switch to the
		// cached push form. Send-driven on purpose; never derived from client acknowledgments.
		private bool _firstBurstSent;


		// When each column's skeleton went out, so the first sub-chunk request for it can be timed
		// against that. Written by the streaming pass, read and cleared by the request handler on a
		// different thread, hence concurrent. An entry that is never removed is a column the client
		// was told about and never asked about - which is what a hole in the world looks like.
		private readonly System.Collections.Concurrent.ConcurrentDictionary<ChunkCoordinates, long> _skeletonSentAt = new();

		// Every section this player has been served, so a request for one of them again is
		// countable: the client only re-asks after a fresh skeleton re-marked the column, so each
		// re-request is a section the client evicted and came back for. Written by the request
		// handler on the dispatch thread, cleared by CleanCache from respawn/dimension paths,
		// hence locked on itself. Grows with the ground covered and is never pruned;
		// diagnostics-priced, a tuple per section.
		private readonly HashSet<(ChunkCoordinates Column, int SectionY)> _subChunksServed = new();

		/// <summary>Columns pushed to this player that no sub-chunk has ever been requested for.</summary>
		public int ColumnsNeverRequested => _skeletonSentAt.Count;

		// Whether streaming has ever been level with this player. False through the join burst and
		// after anything that restreams the world under them; see AdaptChunkRadius.
		private bool _adaptiveArmed;

		public virtual void SendUpdateAttributes()
		{
			// Exact attribute set, order and ranges as sent by vanilla BDS 1.26.34 for the local
			// player at join (decoded wire capture). Values come from the live managers.
			var attributes = new PlayerAttributes();
			void Add(string name, float min, float max, float value, float def)
			{
				// DefaultMin/DefaultMax are the attribute's own range, not zero: left at zero the
				// client is told every attribute has a default range of [0,0].
				attributes[name] = new PlayerAttribute
				{
					Name = name, MinValue = min, MaxValue = max, Value = value, Default = def,
					DefaultMinValue = min, DefaultMaxValue = max
				};
			}

			Add("minecraft:player.hunger", 0, 20, HungerManager.Hunger, 20);
			Add("minecraft:player.saturation", 0, 20, (float) HungerManager.Saturation, 5);
			Add("minecraft:player.exhaustion", 0, 20, (float) HungerManager.Exhaustion, 0);
			Add("minecraft:player.level", 0, 24791, ExperienceManager.ExperienceLevel, 0);
			Add("minecraft:player.experience", 0, 1, ExperienceManager.Experience, 0);
			Add("minecraft:health", 0, 20, HealthManager.Hearts, 20);
			Add("minecraft:follow_range", 0, 2048, FollowRange, 16);
			Add("minecraft:knockback_resistance", -2, 1, KnockbackResistance, 0);
			Add("minecraft:movement", 0, float.MaxValue, (float) MovementSpeed, 0.1f);
			Add("minecraft:underwater_movement", 0, float.MaxValue, UnderwaterMovementSpeed, 0.02f);
			Add("minecraft:lava_movement", 0, float.MaxValue, LavaMovementSpeed, 0.02f);
			Add("minecraft:attack_damage", 1, 1, AttackDamage, 1);
			Add("minecraft:absorption", 0, 16, HealthManager.Absorption, 0);
			Add("minecraft:luck", -1024, 1024, Luck, 0);
			Add("minecraft:friction_modifier", 0, 256, FrictionModifier, 1);
			Add("minecraft:bounciness", 0, 1, Bounciness, 0);
			Add("minecraft:air_drag_modifier", 0, 256, AirDragModifier, 1);

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
			// TODO: Wire format is a sound name string since protocol 993; verify the
			// name mapping against BDS traces when the server side is brought to 1001.
			packet.soundId = sound.ToString();
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
			packet.position = new Vector3(KnownPosition.X, KnownPosition.Y + 1.62f, KnownPosition.Z);
			packet.rotation = new Vector2(KnownPosition.Pitch, KnownPosition.Yaw);
			packet.headYaw = KnownPosition.HeadYaw;
			packet.mode = teleport ? McpeMovePlayer.PositionMode.Respawn : McpeMovePlayer.PositionMode.Normal;

			SendPacket(packet);
		}

		public override void OnTick(Entity[] entities)
		{
			OnTicking(new PlayerEventArgs(this));

			// On the tick, not at the end of a streaming pass: passes only run when the player crosses
			// into a new column, so a player standing still or walking through columns they already
			// have never re-evaluated - which is how a radius that had been walked down to the floor
			// stayed there. It self-throttles on its own interval.
			AdaptChunkRadius();

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
			// A player has no spawn position until the level assigns one, and metadata is broadcast
			// before that: any throw here takes down the whole wrapper batch it rides in.
			PlayerLocation bedPosition = SpawnPosition ?? KnownPosition;
			metadata[(int) MetadataFlags.BedPosition] = bedPosition == null
				? new MetadataIntCoordinates(0, 0, 0)
				: new MetadataIntCoordinates((int) bedPosition.X, (int) bedPosition.Y, (int) bedPosition.Z);

			// Players report their bounding box as a single CollisionBox vector3 (width, height, 0)
			// instead of the generic CollisionBoxWidth/Height floats used by mobs.
			metadata._entries.Remove((int) MetadataFlags.CollisionBoxWidth);
			metadata._entries.Remove((int) MetadataFlags.CollisionBoxHeight);
			metadata[(int) MetadataFlags.CollisionBox] = new MetadataVector3((float) Width, (float) Height, 0);

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
			InvalidateRosterSlices();

			{
				var playerList = McpePlayerList.CreateObject();
				playerList.records = McpePlayerList.Removed(this);
				Level.RelayBroadcast(Level.CreateMcpeBatch(playerList.EncodeAsMemory())); // Replace with records, to remove need for player and encode
				playerList.records = null;
				playerList.PutPool();
			}
			{
				var playerList = McpePlayerList.CreateObject();
				playerList.records = McpePlayerList.Added(this);
				Level.RelayBroadcast(Level.CreateMcpeBatch(playerList.EncodeAsMemory())); // Replace with records, to remove need for player and encode
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
				return;
			}

			// Plugins get the packet HERE, before it is queued, rather than in PrepareSend on the way
			// out. A suppressed packet then costs the object and nothing else: no channel write, no
			// drain, no coalesce pass, no encode. The handler may also replace it, which is why the
			// result is what gets sent.
			//
			// Not written as "handler(...) ?? packet": null is the handler SUPPRESSING the packet,
			// and coalescing that with "there is no handler" sends everything a plugin asked to drop.
			MiNET.Plugins.PluginManager plugins = Server?.PluginManager;
			if (plugins != null)
			{
				Packet outgoing = plugins.PluginPacketHandler(packet, false, this);
				if (outgoing != packet)
				{
					packet.PutPool();
					if (outgoing == null) return;

					packet = outgoing;
				}
			}

			NetworkHandler.SendPacket(packet);
		}

		/// <summary>
		///     The list form of <see cref="SendPacket" />: the packets reach the transport as one
		///     unit and normally leave in one wrapper instead of one each. Plugins see every packet
		///     exactly as they would through the single form, including suppression and
		///     replacement. The list and its packets are handed over.
		/// </summary>
		public void SendPackets(List<Packet> packets)
		{
			if (packets == null || packets.Count == 0) return;

			if (NetworkHandler == null)
			{
				foreach (Packet packet in packets) packet?.PutPool();
				return;
			}

			MiNET.Plugins.PluginManager plugins = Server?.PluginManager;
			if (plugins != null)
			{
				for (int i = packets.Count - 1; i >= 0; i--)
				{
					Packet outgoing = plugins.PluginPacketHandler(packets[i], false, this);
					if (outgoing == packets[i]) continue;

					packets[i].PutPool();
					if (outgoing == null) packets.RemoveAt(i);
					else packets[i] = outgoing;
				}
				if (packets.Count == 0) return;
			}

			NetworkHandler.SendPackets(packets);
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

			// Everything will be served afresh, so served-section history would count the entire
			// re-stream as re-requests and drown the signal it exists for.
			lock (_subChunksServed) _subChunksServed.Clear();

			// Everything around the player is about to be streamed again, so the adaptor is back where
			// it was at join: standing in columns the client has not asked about yet, through no fault
			// of the client's.
			_adaptiveArmed = false;
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

			if (!Inventory.Helmet.IsAir)
			{
				Level.DropItem(coordinates, Inventory.Helmet);
				Inventory.Helmet = new ItemAir();
			}

			if (!Inventory.Chest.IsAir)
			{
				Level.DropItem(coordinates, Inventory.Chest);
				Inventory.Chest = new ItemAir();
			}

			if (!Inventory.Leggings.IsAir)
			{
				Level.DropItem(coordinates, Inventory.Leggings);
				Inventory.Leggings = new ItemAir();
			}

			if (!Inventory.Boots.IsAir)
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
			mcpeAddPlayer.runtimeEntityId = EntityId;
			mcpeAddPlayer.position = KnownPosition.ToVector3();
			mcpeAddPlayer.velocity = Velocity;
			mcpeAddPlayer.rotation = new Vector2(KnownPosition.Pitch, KnownPosition.Yaw);
			mcpeAddPlayer.yHeadRotation = KnownPosition.HeadYaw;
			mcpeAddPlayer.gamemode = (McpeAddPlayer.GameType) GameMode;
			mcpeAddPlayer.metadata = GetMetadata();
			mcpeAddPlayer.deviceId = PlayerInfo.DeviceId;
			mcpeAddPlayer.buildPlatform = (McpeAddPlayer.BuildPlatform) PlayerInfo.DeviceOS;

			// A spawned player must arrive with its ability layers. This went out with none, where
			// vanilla BDS 1.26.34 sends two, leaving the receiving client a player it has no base
			// layer to read walk and fly speed from. The layer is the same one UpdateAbilities
			// sends for this player, so the two cannot describe the player differently.
			mcpeAddPlayer.abilitiesData = new SerializedAbilitiesData
			{
				targetPlayerRawId = EntityId,
				playerPermissions = (SerializedAbilitiesData.PlayerPermissionLevel) PermissionLevel,
				commandPermissions = (SerializedAbilitiesData.CommandPermissionLevel) CommandPermission,
				layers = new List<AbilityLayer> {BuildBaseAbilityLayer()}
			};

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

			// No equipment or armor here. Vanilla BDS 1.26.34 sends neither when it spawns a player,
			// and sending them is what dropped a real client the moment another player appeared.
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

		public virtual void HandleMcpeScriptMessage(McpeScriptMessage message)
		{
		}

		public virtual void HandleMcpeCodeBuilderSource(McpeCodeBuilderSource message)
		{
		}

		public virtual void HandleMcpeChangeMobProperty(McpeChangeMobProperty message)
		{
		}

		public virtual void HandleMcpeRequestAbility(McpeRequestAbility message)
		{
		}

		public virtual void HandleMcpeRequestPermissions(McpeRequestPermissions message)
		{
		}

		public virtual void HandleMcpeEditorNetwork(McpeEditorNetwork message)
		{
		}

		public virtual void HandleMcpeGameTestRequest(McpeGameTestRequest message)
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
