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
}