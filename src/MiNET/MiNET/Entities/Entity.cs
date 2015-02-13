using System;
using Craft.Net.Common;
using log4net;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;
using BoundingBox = MiNET.Utils.BoundingBox;
using MetadataByte = MiNET.Utils.MetadataByte;
using MetadataDictionary = MiNET.Utils.MetadataDictionary;
using MetadataShort = MiNET.Utils.MetadataShort;

namespace MiNET.Entities
{
	public class Entity
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Entity));


		public Level Level { get; set; }

		public int EntityTypeId { get; private set; }
		public int EntityId { get; set; }

		public DateTime LastUpdatedTime { get; set; }
		public PlayerLocation KnownPosition { get; set; }

		public HealthManager HealthManager { get; private set; }

		public double Height { get; set; }
		public double Width { get; set; }
		public double Length { get; set; }
		public double Drag { get; set; }
		public double Gravity { get; set; }

		public Entity(int entityTypeId, Level level)
		{
			Height = 1;
			Width = 1;
			Length = 1;
			Drag = 0;
			Gravity = 0;

			EntityId = EntityManager.EntityIdUndefined;
			Level = level;
			EntityTypeId = entityTypeId;
			KnownPosition = new PlayerLocation();
			HealthManager = new HealthManager(this);
		}

		public virtual MetadataDictionary GetMetadata()
		{
			MetadataDictionary metadata = new MetadataDictionary();
			metadata[0] = new MetadataByte((byte) (HealthManager.IsOnFire ? 1 : 0));
			metadata[1] = new MetadataShort(HealthManager.Air);
			metadata[16] = new MetadataByte(0);

			return metadata;
		}

		public virtual void OnTick()
		{
			// If dead despawn

			// Check collisions
			CheckBlockCollisions();

			// Fire ticks
			// damage ticks
			HealthManager.OnTick();
		}

		private void CheckBlockCollisions()
		{
			// Check all blocks within entity BB
		}

		public virtual void SpawnEntity()
		{
			Level.AddEntity(this);
		}

		public virtual void DespawnEntity()
		{
			Level.RemoveEntity(this);
		}

		public virtual void BroadcastSetEntityData()
		{
			Level.RelayBroadcast(this, new McpeSetEntityData
			{
				entityId = EntityId,
				namedtag = GetMetadata().GetBytes()
			});
		}

		public BoundingBox GetBoundingBox()
		{
			var pos = KnownPosition;
			double height = Height;
			double width = Width;
			double length = Length;
			double yaw = pos.Yaw;

			double s = Math.Sin(yaw);
			double c = Math.Cos(yaw);
			if (s < 0) s = -s;
			if (c < 0) c = -c;
			double wn = (width*s + length*c)/2; // width of AABB
			double hn = (width*c + length*s)/2; // height of AABB

			return new BoundingBox(new Vector3(pos.X - wn, pos.Y, pos.Z - hn), new Vector3(pos.X + wn, pos.Y + height, pos.Z + hn));
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
	}
}