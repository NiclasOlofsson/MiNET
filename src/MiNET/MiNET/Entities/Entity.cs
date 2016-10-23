using System;
using System.Collections;
using System.Numerics;
using log4net;
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
		public bool AlwaysSHowNameTag { get; set; }
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
			MaxAir = 44,
			CollisionBoxHeight = 53,
			CollisionBoxWidth = 54,
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
			metadata[(int) MetadataFlags.MaybeAge] = new MetadataInt(0); // Scale
			metadata[(int) MetadataFlags.Scale] = new MetadataFloat(Scale); // Scale
			metadata[(int) MetadataFlags.MaxAir] = new MetadataShort(HealthManager.MaxAir);
			metadata[(int) MetadataFlags.CollisionBoxHeight] = new MetadataFloat(Height); // Collision box width
			metadata[(int) MetadataFlags.CollisionBoxWidth] = new MetadataFloat(Width); // Collision box height
			return metadata;
		}

		public bool IsSneaking { get; set; }
		public bool IsRiding { get; set; }
		public bool IsSprinting { get; set; }
		public bool IsInAction { get; set; }
		public bool IsInvisible { get; set; }
		public bool IsCritical { get; set; }
		public bool IsTamed { get; set; }
		public bool IsChestedHorse { get; set; }


		public enum DataFlags
		{
			IsOnFire = 1,
			IsSneaking = 2,
			IsRiding = 3,
			IsSprinting = 4,
			IsInAction = 5,
			IsInvisible = 6,
			IsInLove = 8,
			IsPowered = 9,
			IsBaby = 12,
			IsCritcal = 14,
			IsNameTagVisible = 15,
			IsAlwaysShowNameTag = 16,
			IsWithoutAi = 17,
			IsSitting = 21,
			IsAngry = 22,
			IsInAttention = 23,
			IsTamed = 25,
			IsSheared = 26,
			IsElderGuardian = 29,
			IsOutOfWater = 31,
			IsChestedHorse = 32,
		}

		public virtual long GetDataValue()
		{
			//Player: 10000000000000011001000000000000
			// 12, 15, 16, 31

			var bits = GetFlags();

			byte[] bytes = new byte[8];
			bits.CopyTo(bytes, 0);

			long dataValue = BitConverter.ToInt64(bytes, 0);
			Log.Debug($"Bit-array datavalue: dec={dataValue} hex=0x{dataValue:x2}, bin={Convert.ToString(dataValue, 2)}b ");
			return dataValue;
		}

		protected virtual BitArray GetFlags()
		{
			BitArray bits = new BitArray(64);
			bits[(int) DataFlags.IsOnFire] = HealthManager.IsOnFire;
			bits[(int) DataFlags.IsSneaking] = IsSneaking;
			bits[(int) DataFlags.IsRiding] = IsRiding;
			bits[(int) DataFlags.IsSprinting] = IsSprinting;
			bits[(int) DataFlags.IsInAction] = IsInAction;
			bits[(int) DataFlags.IsInvisible] = IsInvisible;
			bits[(int) DataFlags.IsInLove] = false;
			bits[(int) DataFlags.IsPowered] = false;
			bits[(int) DataFlags.IsBaby] = false;
			bits[(int) DataFlags.IsCritcal] = IsCritical;
			bits[(int) DataFlags.IsNameTagVisible] = !HideNameTag;
			bits[(int) DataFlags.IsAlwaysShowNameTag] = AlwaysSHowNameTag;
			bits[(int) DataFlags.IsWithoutAi] = NoAi;
			bits[(int) DataFlags.IsSitting] = false;
			bits[(int) DataFlags.IsAngry] = false;
			bits[(int) DataFlags.IsInAttention] = false;
			bits[(int) DataFlags.IsTamed] = IsTamed;
			bits[(int) DataFlags.IsSheared] = false;
			bits[(int) DataFlags.IsElderGuardian] = false;
			bits[(int) DataFlags.IsOutOfWater] = !IsInWater;
			bits[(int) DataFlags.IsChestedHorse] = IsChestedHorse;
			return bits;
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
			addEntity.entityId = EntityId;
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
			Level.RelayBroadcast(players, addEntity);

			var msg = addEntity;
			Log.DebugFormat("McpeAddEntity Entity ID: {0}", msg.entityId);
			Log.DebugFormat("McpeAddEntity Runtime Entity ID: {0}", msg.runtimeEntityId);
			Log.DebugFormat("Entity Type: {0}", msg.entityType);
			Log.DebugFormat("X: {0}", msg.x);
			Log.DebugFormat("Y: {0}", msg.y);
			Log.DebugFormat("Z: {0}", msg.z);
			Log.DebugFormat("Yaw: {0}", msg.yaw);
			Log.DebugFormat("Pitch: {0}", msg.pitch);
			Log.DebugFormat("Velocity X: {0}", msg.speedX);
			Log.DebugFormat("Velocity Y: {0}", msg.speedY);
			Log.DebugFormat("Velocity Z: {0}", msg.speedZ);
			Log.DebugFormat("Metadata: {0}", MetadataDictionary.MetadataToCode(msg.metadata));
		}

		public virtual void DespawnEntity()
		{
			Level.RemoveEntity(this);
		}

		public virtual void DespawnFromPlayers(Player[] players)
		{
			McpeRemoveEntity mcpeRemoveEntity = McpeRemoveEntity.CreateObject();
			mcpeRemoveEntity.entityId = EntityId;
			Level.RelayBroadcast(players, mcpeRemoveEntity);
		}

		public virtual void BroadcastSetEntityData()
		{
			McpeSetEntityData mcpeSetEntityData = McpeSetEntityData.CreateObject();
			mcpeSetEntityData.entityId = EntityId;
			mcpeSetEntityData.metadata = GetMetadata();
			Level?.RelayBroadcast(mcpeSetEntityData);
		}

		public BoundingBox GetBoundingBox()
		{
			var pos = KnownPosition;
			double halfWidth = Width/2;

			return new BoundingBox(new Vector3((float) (pos.X - halfWidth), pos.Y, (float) (pos.Z - halfWidth)), new Vector3((float) (pos.X + halfWidth), (float) (pos.Y + Height), (float) (pos.Z + halfWidth)));
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
		}


		public virtual Item[] GetDrops()
		{
			return new Item[] {};
		}

		public virtual void DoInteraction(byte actionId, Player player)
		{
		}

		public virtual void DoMouseOverInteraction(byte actionId, Player player)
		{
		}
	}
}
