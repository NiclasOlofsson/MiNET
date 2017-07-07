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
// The Original Code is Niclas Olofsson.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Collections;
using System.Numerics;
using log4net;
using MiNET.Blocks;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities
{
	public class Entity
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Entity));


		public Level Level { get; set; }

		public int EntityTypeId { get; private set; }
		public long EntityId { get; set; }
		public bool IsSpawned { get; set; }

		public DateTime LastUpdatedTime { get; set; }
		public PlayerLocation KnownPosition { get; set; }
		public Vector3 Velocity { get; set; }
		public float PositionOffset { get; set; }

		public HealthManager HealthManager { get; set; }

		public string NameTag { get; set; }

		public bool NoAi { get; set; }
		public bool HideNameTag { get; set; } = true;
		public bool Silent { get; set; }
		public bool IsInWater { get; set; } = false;
		public bool IsOutOfWater => !IsInWater;

		public long Age { get; set; }
		public double Scale { get; set; } = 1.0;
		public double Height { get; set; } = 1;
		public double Width { get; set; } = 1;
		public double Length { get; set; } = 1;
		public double Drag { get; set; } = 0.02;
		public double Gravity { get; set; } = 0.08;
		public int Data { get; set; }

		public long PortalDetected { get; set; }

		public Entity(int entityTypeId, Level level)
		{
			EntityId = EntityManager.EntityIdUndefined;
			Level = level;
			EntityTypeId = entityTypeId;
			KnownPosition = new PlayerLocation();
			HealthManager = new HealthManager(this);
		}

		public enum MetadataFlags
		{
			EntityFlags = 0,
			HideNameTag = 3,
			NameTag = 4,
			AvailableAir = 7,
			EatingHaystack = 16,
			MaybeAge = 25,
			Scale = 39,
			MaxAir = 43,
			CollisionBoxWidth = 54,
			CollisionBoxHeight = 55,
		}

		public virtual MetadataDictionary GetMetadata()
		{
			MetadataDictionary metadata = new MetadataDictionary();
			metadata[(int) MetadataFlags.EntityFlags] = new MetadataLong(GetDataValue());
			metadata[1] = new MetadataInt(1);
			metadata[2] = new MetadataInt(0);
			metadata[(int) MetadataFlags.HideNameTag] = new MetadataByte(!HideNameTag);
			metadata[(int) MetadataFlags.NameTag] = new MetadataString(NameTag ?? string.Empty);
			metadata[(int) MetadataFlags.AvailableAir] = new MetadataShort(HealthManager.Air);
			//metadata[4] = new MetadataByte(Silent);
			//metadata[7] = new MetadataInt(0); // Potion Color
			//metadata[8] = new MetadataByte(0); // Potion Ambient
			//metadata[15] = new MetadataByte(NoAi);
			//metadata[16] = new MetadataByte(0); // Player flags
			////metadata[17] = new MetadataIntCoordinates(0, 0, 0);
			//metadata[23] = new MetadataLong(-1); // Leads EID (target or holder?)
			//metadata[23] = new MetadataLong(-1); // Leads EID (target or holder?)
			//metadata[24] = new MetadataByte(0); // Leads on/off
			//metadata[(int)MetadataFlags.MaybeAge] = new MetadataInt(0); // Scale
			metadata[(int) MetadataFlags.Scale] = new MetadataFloat(Scale); // Scale
			metadata[(int) MetadataFlags.MaxAir] = new MetadataShort(HealthManager.MaxAir);
			metadata[(int) MetadataFlags.CollisionBoxHeight] = new MetadataFloat(Height); // Collision box width
			metadata[(int) MetadataFlags.CollisionBoxWidth] = new MetadataFloat(Width); // Collision box height
			return metadata;
		}

		public virtual long GetDataValue()
		{
			//Player: 10000000000000011001000000000000
			// 12, 15, 16, 31

			BitArray bits = GetFlags();

			byte[] bytes = new byte[8];
			bits.CopyTo(bytes, 0);

			long dataValue = BitConverter.ToInt64(bytes, 0);
			//Log.Debug($"Bit-array datavalue: dec={dataValue} hex=0x{dataValue:x2}, bin={Convert.ToString((long) dataValue, 2)}b ");
			return dataValue;
		}

		public bool IsSneaking { get; set; }
		public bool IsRiding { get; set; }
		public bool IsSprinting { get; set; }
		public bool IsUsingItem { get; set; }
		public bool IsInvisible { get; set; }
		public bool IsTempted { get; set; }
		public bool IsInLove { get; set; }
		public bool IsSaddled { get; set; }
		public bool IsPowered { get; set; }
		public bool IsIgnited { get; set; }
		public bool IsBaby { get; set; }
		public bool IsConverting { get; set; }
		public bool IsCritical { get; set; }
		public bool IsShowName => !HideNameTag;
		public bool IsAlwaysShowName { get; set; }
		public bool IsNoAi => NoAi;
		public bool HaveAi => !NoAi;
		public bool IsSilent { get; set; }
		public bool IsWallClimbing { get; set; }
		public bool CanClimb { get; set; }
		public bool IsResting { get; set; }
		public bool IsSitting { get; set; }
		public bool IsAngry { get; set; }
		public bool IsInterested { get; set; }
		public bool IsCharged { get; set; }
		public bool IsTamed { get; set; }
		public bool IsLeashed { get; set; }
		public bool IsSheared { get; set; }
		public bool IsGliding { get; set; }
		public bool IsElder { get; set; }
		public bool IsMoving { get; set; }
		public bool IsBreathing => !IsInWater;
		public bool IsChested { get; set; }
		public bool IsStackable { get; set; }

		public enum DataFlags
		{
			OnFire = 0,
			Sneaking,
			Riding,
			Sprinting,
			UsingItem,
			Invisible,
			Tempted,
			InLove,

			Saddled,
			Powered,
			Ignited,
			Baby,
			Converting,
			Critcal,
			ShowName,
			AlwaysShowName,

			NoAi,
			Silent,
			WallClimbing,

			CanClimb,
			CanSwim,
			CanFly,

			Resting,
			Sitting,
			Angry,
			Interested,
			Charged,

			Tamed,
			Leashed,
			Sheared,
			FlagAllFlying,
			Elder,
			Moving,
			Breathing,
			Chested,

			Stackable,
		}

		protected virtual BitArray GetFlags()
		{
			BitArray bits = new BitArray(64);
			bits[(int) DataFlags.OnFire] = HealthManager.IsOnFire;
			bits[(int) DataFlags.Sneaking] = IsSneaking;
			bits[(int) DataFlags.Riding] = IsRiding;
			bits[(int) DataFlags.Sprinting] = IsSprinting;
			bits[(int) DataFlags.UsingItem] = IsUsingItem;
			bits[(int) DataFlags.Invisible] = IsInvisible;
			bits[(int) DataFlags.Tempted] = IsTempted;
			bits[(int) DataFlags.InLove] = IsInLove;
			bits[(int) DataFlags.Saddled] = IsSaddled;
			bits[(int) DataFlags.Powered] = IsPowered;
			bits[(int) DataFlags.Ignited] = IsIgnited;
			bits[(int) DataFlags.Baby] = IsBaby;
			bits[(int) DataFlags.Converting] = IsConverting;
			bits[(int) DataFlags.Critcal] = IsCritical;
			bits[(int) DataFlags.ShowName] = IsShowName;
			bits[(int) DataFlags.AlwaysShowName] = IsAlwaysShowName;
			bits[(int) DataFlags.NoAi] = IsNoAi;
			bits[(int) DataFlags.Silent] = IsSilent;
			bits[(int) DataFlags.WallClimbing] = IsWallClimbing;
			bits[(int) DataFlags.CanClimb] = CanClimb;
			bits[(int) DataFlags.Resting] = IsResting;
			bits[(int) DataFlags.Sitting] = IsSitting;
			bits[(int) DataFlags.Angry] = IsAngry;
			bits[(int) DataFlags.Interested] = IsInterested;
			bits[(int) DataFlags.Charged] = IsCharged;
			bits[(int) DataFlags.Tamed] = IsTamed;
			bits[(int) DataFlags.Leashed] = IsLeashed;
			bits[(int) DataFlags.Sheared] = IsSheared;
			bits[(int) DataFlags.FlagAllFlying] = IsGliding;
			bits[(int) DataFlags.Elder] = IsElder;
			bits[(int) DataFlags.Moving] = IsMoving;
			bits[(int) DataFlags.Breathing] = IsBreathing;
			bits[(int) DataFlags.Chested] = IsChested;
			bits[(int) DataFlags.Stackable] = IsStackable;

			return bits;
		}

		protected virtual bool DetectInPortal()
		{
			if (Level.Dimension == Dimension.Overworld && Level.NetherLevel == null) return false;
			if (Level.Dimension == Dimension.Nether && Level.OverworldLevel == null) return false;

			return Level.GetBlock(KnownPosition + new Vector3(0, 0.3f, 0)) is Portal;
		}

		public virtual void OnTick()
		{
			Age++;

			HealthManager.OnTick();
		}

		private void CheckBlockCollisions()
		{
			// Check all blocks within entity BB
		}

		public virtual void SpawnEntity()
		{
			Level.AddEntity(this);

			IsSpawned = true;
		}

		public virtual void SpawnToPlayers(Player[] players)
		{
			var addEntity = McpeAddEntity.CreateObject();
			addEntity.entityType = (byte) EntityTypeId;
			addEntity.entityIdSelf = EntityId;
			addEntity.runtimeEntityId = EntityId;
			addEntity.x = KnownPosition.X;
			addEntity.y = KnownPosition.Y;
			addEntity.z = KnownPosition.Z;
			addEntity.yaw = KnownPosition.Yaw;
			addEntity.pitch = KnownPosition.Pitch;
			addEntity.metadata = GetMetadata();
			addEntity.speedX = (float) Velocity.X;
			addEntity.speedY = (float) Velocity.Y;
			addEntity.speedZ = (float) Velocity.Z;
			addEntity.attributes = GetEntityAttributes();

			Level.RelayBroadcast(players, addEntity);
		}

		public virtual EntityAttributes GetEntityAttributes()
		{
			var attributes = new EntityAttributes();
			attributes["minecraft:attack_damage"] = new EntityAttribute
			{
				Name = "minecraft:attack_damage",
				MinValue = 1,
				MaxValue = 1,
				Value = 1,
			};
			attributes["minecraft:absorption"] = new EntityAttribute
			{
				Name = "minecraft:absorption",
				MinValue = 0,
				MaxValue = float.MaxValue,
				Value = HealthManager.Absorption,
			};
			attributes["minecraft:health"] = new EntityAttribute
			{
				Name = "minecraft:health",
				MinValue = 0,
				MaxValue = 20,
				Value = HealthManager.Hearts,
			};
			attributes["minecraft:knockback_resistance"] = new EntityAttribute
			{
				Name = "minecraft:knockback_resistance",
				MinValue = 0,
				MaxValue = 1,
				Value = 0,
			};
			attributes["minecraft:luck"] = new EntityAttribute
			{
				Name = "minecraft:luck",
				MinValue = -1025,
				MaxValue = 1024,
				Value = 0,
			};
			attributes["minecraft:fall_damage"] = new EntityAttribute
			{
				Name = "minecraft:fall_damage",
				MinValue = 0,
				MaxValue = float.MaxValue,
				Value = 1,
			};
			attributes["minecraft:follow_range"] = new EntityAttribute
			{
				Name = "minecraft:follow_range",
				MinValue = 0,
				MaxValue = 2048,
				Value = 16,
			};

			return attributes;
		}


		public virtual void DespawnEntity()
		{
			Level.RemoveEntity(this);
			IsSpawned = false;
		}

		public virtual void DespawnFromPlayers(Player[] players)
		{
			McpeRemoveEntity mcpeRemoveEntity = McpeRemoveEntity.CreateObject();
			mcpeRemoveEntity.entityIdSelf = EntityId;
			Level.RelayBroadcast(players, mcpeRemoveEntity);
		}

		public virtual void BroadcastSetEntityData()
		{
			McpeSetEntityData mcpeSetEntityData = McpeSetEntityData.CreateObject();
			mcpeSetEntityData.runtimeEntityId = EntityId;
			mcpeSetEntityData.metadata = GetMetadata();
			Level?.RelayBroadcast(mcpeSetEntityData);
		}

		public virtual void BroadcastEntityEvent()
		{
			var entityEvent = McpeEntityEvent.CreateObject();
			entityEvent.runtimeEntityId = EntityId;
			entityEvent.eventId = (byte) (HealthManager.Health <= 0 ? 3 : 2);
			Level.RelayBroadcast(entityEvent);
		}

		public BoundingBox GetBoundingBox()
		{
			var pos = KnownPosition;
			double halfWidth = Width/2;

			return new BoundingBox(new Vector3((float) (pos.X - halfWidth), pos.Y, (float) (pos.Z - halfWidth)), new Vector3((float) (pos.X + halfWidth), (float) (pos.Y + Height), (float) (pos.Z + halfWidth)));
		}

		public double DistanceTo(Entity entity)
		{
			if (entity == null) return -1;
			return Vector3.Distance(KnownPosition, entity.KnownPosition);
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

		public virtual void Knockback(Vector3 velocity)
		{
			Velocity += velocity;
			BroadcastMotion(!NoAi);
		}

		public void BroadcastMotion(bool forceMove = false)
		{
			if (NoAi || forceMove)
			{
				McpeSetEntityMotion motions = McpeSetEntityMotion.CreateObject();
				motions.runtimeEntityId = EntityId;
				motions.velocity = Velocity;
				motions.Encode();
				Level.RelayBroadcast(motions);
			}
		}

		public void BroadcastMove(bool forceMove = false)
		{
			if (NoAi || forceMove)
			{
				McpeMoveEntity moveEntity = McpeMoveEntity.CreateObject();
				moveEntity.runtimeEntityId = EntityId;
				moveEntity.position = (PlayerLocation) KnownPosition.Clone();
				moveEntity.Encode();
				Level.RelayBroadcast(moveEntity);
			}
		}


		public virtual Item[] GetDrops()
		{
			return new Item[] { };
		}

		public virtual void DoInteraction(byte actionId, Player player)
		{
		}

		public virtual void DoMouseOverInteraction(byte actionId, Player player)
		{
		}
	}
}