using System;
using System.Numerics;
using log4net;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities
{
	public class Mob : Entity
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Mob));

		public Mob(int entityTypeId, Level level) : base(entityTypeId, level)
		{
			Width = Length = 0.6;
			Height = 1.80;
		}

		public Mob(EntityType mobTypes, Level level) : this((int) mobTypes, level)
		{
		}

		public override void OnTick()
		{
			base.OnTick();

			if (Velocity.Length() > 0.01)
			{
				PlayerLocation oldPosition = (PlayerLocation) KnownPosition.Clone();
				bool onGroundBefore = IsOnGround(KnownPosition);

				KnownPosition.X += (float) Velocity.X;
				KnownPosition.Y += (float) Velocity.Y;
				KnownPosition.Z += (float) Velocity.Z;
				BroadcastMove();
				BroadcastMotion();

				bool onGround = IsOnGround(KnownPosition);
				if (!onGroundBefore && onGround)
				{
					while (!Level.IsAir(KnownPosition.GetCoordinates3D()))
					{
						KnownPosition.Y++;
					}
					KnownPosition.Y = (float) Math.Floor(KnownPosition.Y);
					Velocity = Vector3.Zero;
					BroadcastMove();
					BroadcastMotion();
				}
				else
				{
					if (!onGround)
					{
						Velocity -= new Vector3(0, 0.08f, 0);
					}

					Velocity *= new Vector3(0.86f, 1, 0.86f);
				}
			}
			else if (Velocity != Vector3.Zero)
			{
				KnownPosition.X += (float) Velocity.X;
				KnownPosition.Y += (float) Velocity.Y;
				KnownPosition.Z += (float) Velocity.Z;

				Velocity = Vector3.Zero;
				LastUpdatedTime = DateTime.UtcNow;
				BroadcastMove(true);
				BroadcastMotion(true);
			}
		}

		protected void CheckBlockAhead()
		{
			var length = Length/2;
			var direction = Vector3.Normalize(Velocity * 1.00000101f);
			var position = KnownPosition.ToVector3();
			int count = (int) (Math.Ceiling(Velocity.Length()/length) + 2);
			for (int i = 0; i < count; i++)
			{
				var distVec = direction*(float) length*i;
				BlockCoordinates blockPos = position + distVec;
				Block block = Level.GetBlock(blockPos);
				if (block.IsSolid)
				{
					var yaw = (Math.Atan2(direction.X, direction.Z) * 180.0D / Math.PI) + 180;
					//Log.Warn($"Will hit block {block} at angle of {yaw}");

					Ray ray = new Ray(position, direction);
					if (ray.Intersects(block.GetBoundingBox()).HasValue)
					{
						int face = IntersectSides(block.GetBoundingBox(), ray);

						//Log.Warn($"Hit block {block} at angle of {yaw} on face {face}");
						if (face == -1) continue;
						switch (face)
						{
							case 0:
								Velocity *= new Vector3(1, 1, 0);
								break;
							case 1:
								Velocity *= new Vector3(0, 1, 1);
								break;
							case 2:
								Velocity *= new Vector3(1, 1, 0);
								break;
							case 3:
								Velocity *= new Vector3(0, 1, 1);
								break;
							case 4: // Under
								Velocity *= new Vector3(1, 0, 1);
								break;
							//case 5:
							//	float ff = 0.6f * 0.98f;
							//	Velocity *= new Vector3(ff, 0.0f, ff);
							//	break;
						}
						return;
					}
					else
					{
						//Log.Warn($"Hit block {block} at angle of {yaw} had no intersection (strange)");
						Velocity *= new Vector3(0, 0, 0);
					}
				}
			}
		}

		public static int IntersectSides(BoundingBox box, Ray ray)
		{
			BoundingBox[] sides = new[]
			{
				// -Z 
				new BoundingBox(new Vector3(box.Min.X, box.Min.Y, box.Min.Z), new Vector3(box.Max.X, box.Max.Y, box.Min.Z)),

				// -X 
				new BoundingBox(new Vector3(box.Min.X, box.Min.Y, box.Min.Z), new Vector3(box.Min.X, box.Max.Y, box.Max.Z)),

				// +Z 
				new BoundingBox(new Vector3(box.Min.X, box.Min.Y, box.Max.Z), new Vector3(box.Max.X, box.Max.Y, box.Max.Z)),

				// +X 
				new BoundingBox(new Vector3(box.Max.X, box.Min.Y, box.Min.Z), new Vector3(box.Max.X, box.Max.Y, box.Max.Z)),


				// -Y
				new BoundingBox(new Vector3(box.Min.X, box.Min.Y, box.Min.Z), new Vector3(box.Max.X, box.Min.Y, box.Max.Z)),

				// +Y
				new BoundingBox(new Vector3(box.Min.X, box.Max.Y, box.Min.Z), new Vector3(box.Max.X, box.Max.Y, box.Max.Z)),
			};

			double? dist = null;
			int side = -1;
			for (int i = 0; i < sides.Length; i++)
			{
				var s = sides[i];
				var d = ray.Intersects(s);
				if (d.HasValue && (!dist.HasValue || d.Value < dist.Value))
				{
					dist = d;
					side = i;
				}
			}

			return side;
		}

		private static readonly int[] Layers = {-1, 0, +1};
		private static readonly int[] Arounds = {0, 1, -1};

		protected Block Intersects(Vector3 position)
		{
			BlockCoordinates pos = position;
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
							return block;
						}
					}
				}
			}

			return null;
		}

		protected bool IsOnGround(Vector3 pos)
		{
			Block block = Level.GetBlock((BlockCoordinates)(pos - new Vector3(0, 0.1f, 0)));

			return block.IsSolid;
			//return block.IsSolid && block.GetBoundingBox().Contains(GetBoundingBox().OffsetBy(new Vector3(0, -0.1f, 0))) == ContainmentType.Intersects;
		}

		protected bool IsInGround(Vector3 position)
		{
			Block block = Level.GetBlock(position);

			return block.IsSolid;
		}
	}
}