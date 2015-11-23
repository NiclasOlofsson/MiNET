using MiNET.Entities;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemSnowball : Item
	{
		public ItemSnowball(short metadata) : base(332, metadata)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			float force = 1.5f;

			Snowball snowBall = new Snowball(null, world);
			snowBall.KnownPosition = (PlayerLocation) player.KnownPosition.Clone();
			snowBall.KnownPosition.Y += 1.62f;
			snowBall.Velocity = snowBall.KnownPosition.GetDirection() * (force);
			snowBall.BroadcastMovement = false;
			snowBall.DespawnOnImpact = true;
			snowBall.SpawnEntity();
		}
	}
}