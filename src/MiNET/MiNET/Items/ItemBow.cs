using System;
using System.Threading.Tasks;
using MiNET.Entities;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemBow : Item
	{
		public ItemBow(short metadata) : base(261, metadata)
		{
		}

		public override void Relese(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			Arrow arrow = new Arrow(world);
			arrow.KnownPosition = (PlayerLocation) player.KnownPosition.Clone();
			arrow.KnownPosition.Y += 1.62f;

			float yaw = player.KnownPosition.Yaw;
			float pitch = player.KnownPosition.Pitch;

			var vx = -Math.Sin(yaw/180f*Math.PI)*Math.Cos(pitch/180f*Math.PI);
			var vy = -Math.Sin(pitch/180f*Math.PI);
			var vz = Math.Cos(yaw/180f*Math.PI)*Math.Cos(pitch/180f*Math.PI);

			arrow.Velocity = new Vector3(vx, vy, vz)*1.5f;

			arrow.SpawnEntity();

			var entityMotion = McpeSetEntityMotion.CreateObject();
			entityMotion.entities = new EntityMotions {{arrow.EntityId, arrow.Velocity}};
			entityMotion.Encode();

			new Task(() => world.RelayBroadcast(entityMotion)).Start();
		}
	}
}