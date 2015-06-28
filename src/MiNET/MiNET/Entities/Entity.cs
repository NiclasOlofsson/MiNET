using System;
using log4net;
using MiNET.Blocks;
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

		public DateTime LastUpdatedTime { get; set; }
		public PlayerLocation KnownPosition { get; set; }
		public Vector3 Velocity { get; set; }

		public HealthManager HealthManager { get; private set; }

		public double Height { get; set; }
		public double Width { get; set; }
		public double Length { get; set; }
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
			metadata[0] = new MetadataByte((byte) (HealthManager.IsOnFire ? 1 : 0));
			metadata[1] = new MetadataShort(HealthManager.Air);
			metadata[2] = new MetadataString("");
			metadata[3] = new MetadataByte(1);
			metadata[4] = new MetadataByte(0);
			metadata[15] = new MetadataByte(0);
			//metadata[16] = new MetadataByte(0);

			return metadata;
		}

		public virtual void OnTick()
		{
			// Fire ticks
			// damage ticks
			HealthManager.OnTick();

			if (Velocity.Distance > 0)
			{
				if (!(this is Player) && !(this is Projectile))
				{
					PlayerLocation oldPosition = (PlayerLocation) KnownPosition.Clone();
					var onGroundBefore = IsOnGround(KnownPosition);

					KnownPosition.X += (float) Velocity.X;
					KnownPosition.Y += (float) Velocity.Y;
					KnownPosition.Z += (float) Velocity.Z;

					var onGround = IsOnGround(KnownPosition);
					if (!onGroundBefore && onGround)
					{
						KnownPosition.Y = (float) Math.Floor(oldPosition.Y);
						Velocity = Vector3.Zero;
					}
					else
					{
						Velocity *= (1.0 - Drag);
						Velocity -= new Vector3(0, Gravity, 0);
					}
				}
			}

			if (!(this is Player) && !(this is Projectile))
			{
				McpeMoveEntity moveEntity = McpeMoveEntity.CreateObject();
				moveEntity.entities = new EntityLocations {{EntityId, KnownPosition}};
				Level.RelayBroadcast(moveEntity);

				McpeSetEntityMotion motions = McpeSetEntityMotion.CreateObject();
				motions.entities = new EntityMotions {{EntityId, Velocity}};
				Level.RelayBroadcast(motions);
			}
		}

		private bool IsOnGround(PlayerLocation position)
		{
			PlayerLocation pos = (PlayerLocation) position.Clone();
			pos.Y -= 0.1f;
			Block block = Level.GetBlock(new BlockCoordinates(pos));

			return block.Id != 0; // Should probably test for solid
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
				metadata = GetMetadata()
			});
		}

		public BoundingBox GetBoundingBox()
		{
			var pos = KnownPosition;
			double halfWidth = Width/2;

			return new BoundingBox(new Vector3(pos.X - halfWidth, pos.Y, pos.Z - halfWidth), new Vector3(pos.X + halfWidth, pos.Y + Height, pos.Z + halfWidth));
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
	}
}