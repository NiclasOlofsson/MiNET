using System;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities
{
	public class Mob : Entity
	{
		public Mob(int entityTypeId, Level level) : base(entityTypeId, level)
		{
			Width = Length = 0.6;
			Height = 1.80;
		}

		public Mob(MobTypes mobTypes, Level level) : this((int) mobTypes, level)
		{
		}

		public override void OnTick()
		{
			base.OnTick();

			if (Velocity.Distance > 0)
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
					if (!onGround)
					{
						Velocity -= new Vector3(0, Gravity, 0);
					}
				}
				LastUpdatedTime = DateTime.UtcNow;
			}
			else if (Velocity != Vector3.Zero)
			{
				Velocity = Vector3.Zero;
				LastUpdatedTime = DateTime.UtcNow;
			}
		}

		private bool IsOnGround(PlayerLocation position)
		{
			PlayerLocation pos = (PlayerLocation) position.Clone();
			pos.Y -= 0.1f;
			Block block = Level.GetBlock(new BlockCoordinates(pos));

			return block.Id != 0; // Should probably test for solid
		}
	}

	public class Chicken : Mob
	{
		public Chicken(Level level) : base(MobTypes.Chicken, level)
		{
			Width = Height = 0.3;
			Height = 0.7;
		}
	}

	public class Cow : Mob
	{
		public Cow(Level level) : base(MobTypes.Cow, level)
		{
			Width = 0.3;
			Length = 0.9;
			Height = 1.3; // baby = 0.65
		}
	}

	public class Pig : Mob
	{
		public Pig(Level level) : base(MobTypes.Pig, level)
		{
			Width = 0.3;
			Length = 0.9;
			Height = 0.875; // baby = ?
		}
	}

	public class Sheep : Mob
	{
		public Sheep(Level level) : base(MobTypes.Sheep, level)
		{
			Width = 0.625;
			Length = 1.4375;
			Height = 1.25; // baby = ?
		}
	}

	public class Wolf : Mob
	{
		public Wolf(Level level) : base(MobTypes.Wolf, level)
		{
			Width = 0.3;
			Length = 0.9;
			Height = 0.85; // baby = 0.425
		}
	}

	public class IronGolem : Mob
	{
		public IronGolem(Level level) : base(MobTypes.IronGolem, level)
		{
		}
	}
}