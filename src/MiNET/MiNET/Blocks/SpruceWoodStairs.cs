using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	internal class SpruceWoodStairsStairs : Block
	{
		public SpruceWoodStairsStairs() : base(134)
		{
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face)
		{
			byte direction = player.GetDirection();

			switch (direction)
			{
				case 1:
					Metadata = 2;
					break;
				case 2:
					Metadata = 0x03 & 5;
					break;
				case 3:
					Metadata = 3;
					break;
			}

			world.SetBlock(this);
			return true;
		}
	}
}