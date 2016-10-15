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

		public HealthManager HealthManager { get; set; }

		public string NameTag { get; set; }

		public bool NoAi { get; set; }
		public bool HideNameTag { get; set; }
		public bool Silent { get; set; }

		public long Age { get; set; }
		public double Height { get; set; } = 1.80;
		public double Width { get; set; } = 0.6;
		public double Length { get; set; } = 0.6;
		public double Drag { get; set; }
		public double Gravity { get; set; }
		public int Data { get; set; }

		public Entity(int entityTypeId, Level level)
		{
			Height = 1;
			Width = 1;
			Length = 1;
			Gravity = 0.08;
			Drag = 0.02;

			EntityId = EntityManager.EntityIdUndefined;
			Level = level;
			EntityTypeId = entityTypeId;
			KnownPosition = new PlayerLocation();
			HealthManager = new HealthManager(this);
		}

		public virtual MetadataDictionary GetMetadata()
		{
			MetadataDictionary metadata = new MetadataDictionary();
			metadata[0] = new MetadataLong(GetDataValue());
			metadata[1] = new MetadataInt(1);
			metadata[2] = new MetadataInt(0);
			metadata[3] = new MetadataByte(!HideNameTag);
			metadata[4] = new MetadataString(NameTag ?? string.Empty);
			metadata[7] = new MetadataShort(400); // Potion Color
			//metadata[4] = new MetadataByte(Silent);
			//metadata[7] = new MetadataInt(0); // Potion Color
			//metadata[8] = new MetadataByte(0); // Potion Ambient
			//metadata[15] = new MetadataByte(NoAi);
			//metadata[16] = new MetadataByte(0); // Player flags
			////metadata[17] = new MetadataIntCoordinates(0, 0, 0);
			//metadata[23] = new MetadataLong(-1); // Leads EID (target or holder?)
			//metadata[23] = new MetadataLong(-1); // Leads EID (target or holder?)
			//metadata[24] = new MetadataByte(0); // Leads on/off
			//metadata[39] = new MetadataFloat(0.4f); // Scale
			//metadata[53] = new MetadataFloat(1.99f); // Collision box width
			//metadata[54] = new MetadataFloat(1.99f); // Collision box height
			return metadata;
		}

		public bool IsSneaking { get; set; }
		public bool IsRiding { get; set; }
		public bool IsSprinting { get; set; }
		public bool IsInAction { get; set; }
		public bool IsInvisible { get; set; }


		public enum DataFlags
		{
			IsOnFire = 0,
			IsSneaking = 1,
			IsRiding = 2,
			IsSprinting = 3,
			IsInAction = 4,
			IsInvisible = 5,
			IsInLove = 8,
			IsPowered = 9,
			IsBaby = 12,
			IsNameTagVisible = 15,
			IsSitting = 21,
			IsAngry = 22,
			IsInAttention = 23,
			IsTamed = 25,
			IsSheared = 26,
			IsElderGuardian = 29,
			IsChested = 30,
			IsOutOfWater = 31,
		}

		public virtual long GetDataValue()
		{
			//Player: 10000000000000011001000000000000
			// 12, 15, 16, 31

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
			bits[(int) DataFlags.IsNameTagVisible] = !HideNameTag;
			bits[(int) DataFlags.IsSitting] = true;
			bits[(int) DataFlags.IsAngry] = false;
			bits[(int) DataFlags.IsInAttention] = false;
			bits[(int) DataFlags.IsTamed] = true;
			bits[(int) DataFlags.IsSheared] = false;
			bits[(int) DataFlags.IsElderGuardian] = false;
			bits[(int) DataFlags.IsChested] = false;
			bits[(int) DataFlags.IsOutOfWater] = true;

			byte[] bytes = new byte[8];
			bits.CopyTo(bytes, 0);

			var dataValue = BitConverter.ToInt32(bytes, 0);
			Log.Debug($"Bit-array datavalue: dec={dataValue} hex=0x{dataValue:x2}, bin={Convert.ToString(dataValue, 2)}b ");
			return dataValue;
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
	}
}