using System;
using MiNET.Entities;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemSnowball : Item
	{
		internal ItemSnowball(short metadata) : base(332, metadata)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			float force = 1.5f;

			Snowball snowBall = new Snowball(player, world);
			snowBall.KnownPosition = (PlayerLocation) player.KnownPosition.Clone();
			snowBall.KnownPosition.Y += 1.62f;

			var vx = -Math.Sin(player.KnownPosition.Yaw/180f*Math.PI)*Math.Cos(player.KnownPosition.Pitch/180f*Math.PI);
			var vy = -Math.Sin(player.KnownPosition.Pitch/180f*Math.PI);
			var vz = Math.Cos(player.KnownPosition.Yaw/180f*Math.PI)*Math.Cos(player.KnownPosition.Pitch/180f*Math.PI);

			snowBall.Velocity = new Vector3(vx, vy, vz)*force;
			snowBall.Data = player.EntityId;

			snowBall.SpawnEntity();
		}
	}
}