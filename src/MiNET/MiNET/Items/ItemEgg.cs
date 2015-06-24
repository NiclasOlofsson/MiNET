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

			Egg egg = new Egg(player, world)
			{
				KnownPosition = (PlayerLocation) player.KnownPosition.Clone(),
				Velocity = new Vector3(blockCoordinates.X, blockCoordinates.Y, blockCoordinates.Z).Normalize()*force
			};
			egg.KnownPosition.Y += 1.62f;

			egg.SpawnEntity();
		}
	}
}