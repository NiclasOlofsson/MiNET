using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using log4net;
using MiNET.Blocks;
using MiNET.Entities.Behaviors;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities
{
	public class Mob : Entity
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Mob));

		public bool DespawnIfNotSeenPlayer { get; set; }
		public DateTime LastSeenPlayerTimer { get; set; }

		public List<IBehavior> Behaviors { get; } = new List<IBehavior>();
		private IBehavior _currentBehavior = null;


		public double Speed { get; set; } = 0.25f;

		public Mob(int entityTypeId, Level level) : base(entityTypeId, level)
		{
			Width = Length = 0.6;
			Height = 1.80;
		}

		public Mob(EntityType mobTypes, Level level) : this((int) mobTypes, level)
		{
		}

		public override void SpawnEntity()
		{
			LastSeenPlayerTimer = DateTime.UtcNow;
			base.SpawnEntity();
		}

		public override void OnTick()
		{
			base.OnTick();

			if (HealthManager.IsDead) return;

			if (Level.EnableChunkTicking && DespawnIfNotSeenPlayer && DateTime.UtcNow - LastSeenPlayerTimer > TimeSpan.FromSeconds(30))
			{
				if (Level.GetSpawnedPlayers().Count(e => Vector3.Distance(KnownPosition, e.KnownPosition) < 32) == 0)
				{
					if (Level.Random.Next(800) == 0)
					{
						if (Log.IsDebugEnabled)
							Log.Warn($"Despawn because didn't see any players within 32 blocks for 30s or longer. Last seen {LastSeenPlayerTimer}");

						DespawnEntity();
						return;
					}
				}
				else
				{
					LastSeenPlayerTimer = DateTime.UtcNow;
				}
			}

			_currentBehavior = GetBehavior();
			if (_currentBehavior != null)
			{
				if (_currentBehavior.OnTick(this))
				{
					_currentBehavior.OnEnd(this);
					_currentBehavior = null;
				}
			}

			// Execute move
			bool onGroundBefore = IsMobOnGround(KnownPosition);

			PlayerLocation prevPos = (PlayerLocation) KnownPosition.Clone();

			KnownPosition.X += (float) Velocity.X;
			KnownPosition.Y += (float) Velocity.Y;
			KnownPosition.Z += (float) Velocity.Z;

			// Fix potential fall through ground because of speed
			bool onGround = IsMobOnGround(KnownPosition);
			if (!onGroundBefore && onGround)
			{
				while (Level.GetBlock(KnownPosition).IsSolid)
				{
					KnownPosition.Y = (float) Math.Floor(KnownPosition.Y + 1);
				}
			}
			Vector3 currPos = KnownPosition;
			if ((prevPos - currPos).Length() > 0.01)
			{
				BroadcastMove();
				BroadcastMotion();
			}

			// Calculate velocity for next move
			if (_currentBehavior != null)
			{
				if (_currentBehavior.CalculateNextMove(this))
				{
					_currentBehavior.OnEnd(this);
					_currentBehavior = null;
				}
			}

			bool inWater = IsMobInFluid(KnownPosition);

			if (inWater && Level.Random.NextDouble() < 0.8)
			{
				Velocity += new Vector3(0, 0.039f, 0);
			}
			else if (onGround)
			{
				if (Velocity.Y < 0) Velocity *= new Vector3(1, 0, 1);
			}
			else
			{
				Velocity -= new Vector3(0, (float) Gravity, 0);
			}

			float drag = (float) (1 - Drag);
			if (inWater)
			{
				drag = 0.8F;
			}

			Velocity *= drag;
		}

		private IBehavior GetBehavior()
		{
			foreach (var behavior in Behaviors)
			{
				if (behavior == _currentBehavior) return behavior;

				if (behavior.ShouldStart(this))
				{
					if (_currentBehavior == null || Behaviors.IndexOf(_currentBehavior) > Behaviors.IndexOf(behavior))
					{
						_currentBehavior?.OnEnd(this);
						return behavior;
					}
				}
			}

			return null;
		}

		protected void CheckBlockAhead()
		{
			var length = Length/2;
			var direction = Vector3.Normalize(Velocity*1.00000101f);
			var position = KnownPosition.ToVector3();
			int count = (int) (Math.Ceiling(Velocity.Length()/length) + 2);
			for (int i = 0; i < count; i++)
			{
				var distVec = direction*(float) length*i;
				BlockCoordinates blockPos = position + distVec;
				Block block = Level.GetBlock(blockPos);
				if (block.IsSolid)
				{
					var yaw = (Math.Atan2(direction.X, direction.Z)*180.0D/Math.PI) + 180;
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

		private bool IsMobInFluid(Vector3 position)
		{
			float y = (float) (position.Y + Height*0.7);

			BlockCoordinates waterPos = new BlockCoordinates
			{
				X = (int) Math.Floor(position.X),
				Y = (int) Math.Floor(y),
				Z = (int) Math.Floor(position.Z)
			};

			var block = Level.GetBlock(waterPos);

			if (block == null || (block.Id != 8 && block.Id != 9)) return false;

			return y < Math.Floor(y) + 1 - ((1f/9f) - 0.1111111);
		}

		private bool IsMobStandingInFluid(Vector3 position)
		{
			Block block = Level.GetBlock(position);

			return block is FlowingWater || block is StationaryWater || block is FlowingLava || block is StationaryLava;
		}

		protected bool IsMobOnGround(Vector3 pos)
		{
			if (pos.Y - Math.Truncate(pos.Y) > 0.1)
				return IsMobInGround(pos);

			BlockCoordinates coord = pos;
			Block block = Level.GetBlock(coord + BlockCoordinates.Down);

			return block.IsSolid;
			//return block.IsSolid && block.GetBoundingBox().Contains(GetBoundingBox().OffsetBy(new Vector3(0, -0.1f, 0))) == ContainmentType.Intersects;
		}

		protected bool IsMobInGround(Vector3 position)
		{
			Block block = Level.GetBlock(position);

			return block.IsSolid;
		}
	}
}