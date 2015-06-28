using System;
using MiNET.Blocks;
using MiNET.Net;
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
					Velocity -= new Vector3(0, Gravity, 0);
				}
			}

			McpeMoveEntity moveEntity = McpeMoveEntity.CreateObject();
			moveEntity.entities = new EntityLocations {{EntityId, KnownPosition}};
			Level.RelayBroadcast(moveEntity);

			McpeSetEntityMotion motions = McpeSetEntityMotion.CreateObject();
			motions.entities = new EntityMotions {{EntityId, Velocity}};
			Level.RelayBroadcast(motions);
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