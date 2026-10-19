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
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using log4net;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Cryptography;
using MiNET.Utils.Metadata;
using MiNET.Utils.Skins;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Entities
{
	public class PlayerMob : Mob
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(PlayerMob));

		public UUID ClientUuid { get; private set; }
		public Skin Skin { get; set; }

		public Item ItemInHand { get; set; }

		/// <summary>
		///     The identity a real player carries into AddPlayer and the player-list record. A mob
		///     had none, so device id and platform chat id went out empty and the build platform as
		///     zero, where every real spawn carries real values. Copy a player's to spawn a stand-in
		///     that looks the same on the wire.
		/// </summary>
		public PlayerInfo PlayerInfo { get; set; } = new PlayerInfo {DeviceOS = 7, PlatformChatId = string.Empty};

		public GameMode GameMode { get; set; } = GameMode.Survival;

		public PlayerMob(string name, Level level) : base(EntityType.Player, level)
		{
			ClientUuid = new UUID(Guid.NewGuid().ToByteArray());

			Width = 0.6;
			Length = 0.6;
			Height = 1.80;

			IsSpawned = false;

			NameTag = name;


			// Every field the wire carries, not just the six this used to set. A skin built from a
			// real client's login (what MiNET.Client sends, and what a 1.26.40 client accepts)
			// carries an arm size, a skin colour, a geometry data version and an animation data
			// document; leaving them null sent empty strings where the format expects values, and
			// left the arm size contradicting the geometry the skin names.
			var resourcePatch = new SkinResourcePatch {Geometry = new GeometryIdentifier {Default = "geometry.humanoid.custom"}};
			Skin = new Skin
			{
				SkinId = $"{Guid.NewGuid()}.Custom",
				SkinResourcePatch = resourcePatch,
				Slim = false,
				ArmSize = "wide",
				SkinColor = "#0",
				GeometryDataVersion = "0.0.0",
				AnimationData = string.Empty,

				// The resource patch above names geometry.humanoid.custom, so the skin has to ship
				// the model that defines it. This was null, which is a skin pointing at a model it
				// does not carry: the same shape MiNET.Client's bot had before it was given a real
				// captured skin, and the client has nothing to draw from it.
				GeometryData = CryptoUtils.DefaultPlayerGeometry,

				// The same vouching ClientData.ToSkin does for every relayed skin. A skin built
				// here never went through that, so it went out untrusted, which is what the
				// client's "only allow trusted skins" setting rejects. Premium stays false: that
				// claims the skin came from a purchased pack, which this one did not.
				IsVerified = true,
				Height = 32,
				Width = 64,
				Data = Encoding.Default.GetBytes(new string('Z', 8192)),
			};

			ItemInHand = new ItemAir();

			HideNameTag = false;
			IsAlwaysShowName = true;

			IsInWater = true;
			NoAi = true;
			HealthManager.IsOnFire = false;
			Velocity = Vector3.Zero;
			PositionOffset = 1.62f;
		}

		[Wired]
		public void SetPosition(PlayerLocation position, bool teleport = true)
		{
			KnownPosition = position;
			LastUpdatedTime = DateTime.UtcNow;

			var package = McpeMovePlayer.CreateObject();
			package.runtimeEntityId = EntityId;
			package.position = new Vector3(position.X, position.Y + 1.62f, position.Z);
			package.rotation = new Vector2(position.Pitch, position.HeadYaw);
			package.headYaw = position.Yaw;
			package.mode = teleport ? McpeMovePlayer.PositionMode.Respawn : McpeMovePlayer.PositionMode.Normal;

			Level.RelayBroadcast(package);
		}

		public override MetadataDictionary GetMetadata()
		{
			var metadata = base.GetMetadata();

			// Players (and player-like mobs, eg. NPC bots) report their bounding box as a single
			// CollisionBox vector3 (width, height, 0) instead of the generic width/height floats.
			metadata[(int) MetadataFlags.CollisionBox] = new MetadataVector3((float) Width, (float) Height, 0);

			// Deliberate divergence from vanilla, which sends only CollisionBox (130) for players
			// and drops the older width/height pair. 130 alone does move the collision hull, but
			// the client anchors the nametag to 53/54: resizing an entity with only 130 leaves the
			// tag floating at its original height, and sending 53/54 as well brings it down with
			// the body. Confirmed against a real 1.26.40 client by shrinking a PlayerMob live.
			metadata[(int) MetadataFlags.CollisionBoxWidth] = new MetadataFloat((float) Width);
			metadata[(int) MetadataFlags.CollisionBoxHeight] = new MetadataFloat((float) Height);

			// The three player-only entries Player.GetMetadata sets and this override did not.
			// Vanilla BDS carries all three on every AddPlayer it sends (verified against captured
			// 1.26.40 frames), and a player entity that arrives without them is the one difference
			// left between this path and the Player path a real client accepts.
			metadata[(int) MetadataFlags.PlayerFlags] = new MetadataByte(0);
			metadata[(int) MetadataFlags.BedPosition] = new MetadataIntCoordinates(0, 0, 0);
			metadata[(int) MetadataFlags.ButtonText] = new MetadataString(string.Empty);

			return metadata;
		}

		public override void SpawnToPlayers(Player[] players)
		{
			// Exactly what Level.AddPlayer does for a real player: one player-list add record,
			// which is how the receiving client learns the appearance, and it stays. AddPlayer
			// below only references the identity. This used to be followed by a remove record so
			// the mob would not show in the tab list, which is the one thing no real player spawn
			// ever does.
			{
				var record = new Player(null, null)
				{
					ClientUuid = ClientUuid,
					EntityId = EntityId,
					NameTag = NameTag,
					DisplayName = NameTag,
					Username = NameTag,
					Skin = Skin,
					PlayerInfo = PlayerInfo
				};

				var playerList = McpePlayerList.CreateObject();
				playerList.records = McpePlayerList.Added(record);
				Level.RelayBroadcast(players, Level.CreateMcpeBatch(playerList.EncodeAsMemory()));
				playerList.records = null;
				playerList.PutPool();
			}

			{
				var message = McpeAddPlayer.CreateObject();
				message.uuid = ClientUuid;
				message.username = NameTag;
				message.runtimeEntityId = EntityId;
				message.position = KnownPosition.ToVector3();
				message.velocity = Velocity;
				message.rotation = new Vector2(KnownPosition.Pitch, KnownPosition.Yaw);
				message.yHeadRotation = KnownPosition.HeadYaw;
				message.gamemode = (McpeAddPlayer.GameType) GameMode;
				message.metadata = GetMetadata();
				message.deviceId = PlayerInfo.DeviceId;
				message.buildPlatform = (McpeAddPlayer.BuildPlatform) PlayerInfo.DeviceOS;

				// A spawned player must arrive with its ability layers, the same as
				// Player.SpawnToPlayers: with none, the receiving client has a player it cannot
				// read a walk or fly speed for, and a real 1.26.40 client drops the session about
				// 100ms later. A mob is not an operator and cannot fly, so the base layer says so.
				message.abilitiesData = new SerializedAbilitiesData
				{
					targetPlayerRawId = EntityId,
					playerPermissions = SerializedAbilitiesData.PlayerPermissionLevel.Member,
					commandPermissions = SerializedAbilitiesData.CommandPermissionLevel.Any,
					layers = new List<AbilityLayer>
					{
						new AbilityLayer
						{
							Type = AbilityLayerType.Base,
							// Vanilla marks everything allowed on the base layer and gates behaviour
							// through the enabled set alone.
							Allowed = (AbilitySet) 0xFFFFF,
							Enabled = AbilitySet.Build | AbilitySet.Mine | AbilitySet.DoorsAndSwitches
								| AbilitySet.OpenContainers | AbilitySet.AttackPlayers | AbilitySet.AttackMobs,
							FlySpeed = 0.05f,
							VerticalFlySpeed = 1.0f,
							WalkSpeed = 0.1f,
						}
					}
				};

				Level.RelayBroadcast(players, message);
			}

			// No equipment or armor here, the same as Player.SpawnToPlayers: vanilla BDS sends
			// neither when it spawns a player, and sending them is what dropped a real 1.26.40
			// client roughly 400ms after a PlayerMob appeared.

			{
				var setEntityData = McpeSetEntityData.CreateObject();
				setEntityData.runtimeEntityId = EntityId;
				setEntityData.metadata = GetMetadata();
				Level?.RelayBroadcast(players, setEntityData);
			}
		}

		public void RemoveFromPlayerList()
		{
			var fake = new Player(null, null)
			{
				ClientUuid = ClientUuid,
				EntityId = EntityId,
				NameTag = NameTag,
				Skin = Skin
			};

			var players = Level.GetSpawnedPlayers();

			var playerList = McpePlayerList.CreateObject();
			playerList.records = McpePlayerList.Removed(fake);
			Level.RelayBroadcast(players, Level.CreateMcpeBatch(playerList.EncodeAsMemory()));
			playerList.records = null;
			playerList.PutPool();
		}

		public void AddToPlayerList()
		{
			Player fake = new Player(null, null)
			{
				ClientUuid = ClientUuid,
				EntityId = EntityId,
				NameTag = NameTag,
				Skin = Skin,
				PlayerInfo = new PlayerInfo()
			};

			var players = Level.GetSpawnedPlayers();

			McpePlayerList playerList = McpePlayerList.CreateObject();
			playerList.records = McpePlayerList.Added(fake);
			Level.RelayBroadcast(players, Level.CreateMcpeBatch(playerList.EncodeAsMemory()));
			playerList.records = null;
			playerList.PutPool();
		}

		public override void DespawnFromPlayers(Player[] players)
		{
			{
				var fake = new Player(null, null)
				{
					ClientUuid = ClientUuid,
					EntityId = EntityId,
					NameTag = NameTag,
					Skin = Skin
				};

				McpePlayerList playerList = McpePlayerList.CreateObject();
				playerList.records = McpePlayerList.Removed(fake);
				Level.RelayBroadcast(players, Level.CreateMcpeBatch(playerList.EncodeAsMemory()));
				playerList.records = null;
				playerList.PutPool();
			}

			McpeRemoveEntity mcpeRemovePlayer = McpeRemoveEntity.CreateObject();
			mcpeRemovePlayer.targetActorId = EntityId;
			Level.RelayBroadcast(players, mcpeRemovePlayer);
		}

		public override void OnTick(Entity[] entities)
		{
			OnTicking(new PlayerEventArgs(null));

			// Do nothing of the mob stuff

			OnTicked(new PlayerEventArgs(null));
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


		protected virtual void SendEquipment()
		{
			McpeMobEquipment message = McpeMobEquipment.CreateObject();
			message.runtimeEntityId = EntityId;
			message.item = ItemInHand;
			message.slot = 0;
			Level.RelayBroadcast(message);
		}

		protected virtual void SendArmor()
		{
			McpeMobArmorEquipment armorEquipment = McpeMobArmorEquipment.CreateObject();
			armorEquipment.runtimeEntityId = EntityId;
			armorEquipment.helmet = Helmet;
			armorEquipment.chestplate = Chest;
			armorEquipment.leggings = Leggings;
			armorEquipment.boots = Boots;
			Level.RelayBroadcast(armorEquipment);
		}
	}
}