using MiNET.Blocks;
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

			Snowball snowBall = new Snowball(player, world)
			{
				KnownPosition = (PlayerLocation) player.KnownPosition.Clone(),
				Velocity = new Vector3(blockCoordinates.X, blockCoordinates.Y, blockCoordinates.Z).Normalize()*force
			};
			snowBall.KnownPosition.Y += 1.62f;
			snowBall.SpawnEntity();
		}
	}
}