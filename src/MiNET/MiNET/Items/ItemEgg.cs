using System;
using MiNET.Entities;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemEgg : Item
	{
		internal ItemEgg(short metadata) : base(344, metadata)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			float force = 1.5f;

			Egg egg = new Egg(player, world);
			egg.KnownPosition = (PlayerLocation) player.KnownPosition.Clone();
			egg.KnownPosition.Y += 1.62f;

			var vx = -Math.Sin(player.KnownPosition.Yaw/180f*Math.PI)*Math.Cos(player.KnownPosition.Pitch/180f*Math.PI);
			var vy = -Math.Sin(player.KnownPosition.Pitch/180f*Math.PI);
			var vz = Math.Cos(player.KnownPosition.Yaw/180f*Math.PI)*Math.Cos(player.KnownPosition.Pitch/180f*Math.PI);

			egg.Velocity = new Vector3(vx, vy, vz)*force;
			egg.Data = player.EntityId;

			egg.SpawnEntity();
		}
	}
}