using System;
using System.Threading.Tasks;
using MiNET.Entities;
using MiNET.Net;
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
			Snowball snowBall = new Snowball(world);
			snowBall.KnownPosition = (PlayerLocation) player.KnownPosition.Clone();
			snowBall.KnownPosition.Y += 1.62f;

			var vx = -Math.Sin(player.KnownPosition.Yaw/180f*Math.PI)*Math.Cos(player.KnownPosition.Pitch/180f*Math.PI);
			var vy = -Math.Sin(player.KnownPosition.Pitch/180f*Math.PI);
			var vz = Math.Cos(player.KnownPosition.Yaw/180f*Math.PI)*Math.Cos(player.KnownPosition.Pitch/180f*Math.PI);

			snowBall.Velocity = new Vector3(vx, vy, vz)*1.5f;

			snowBall.SpawnEntity();

			var entityMotion = McpeSetEntityMotion.CreateObject();
			entityMotion.entities = new EntityMotions {{snowBall.EntityId, snowBall.Velocity}};
			entityMotion.Encode();

			new Task(() => world.RelayBroadcast(entityMotion)).Start();
		}
	}
}