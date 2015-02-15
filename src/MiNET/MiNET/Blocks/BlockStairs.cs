using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	internal abstract class BlockStairs : Block
	{
		protected BlockStairs(byte id) : base(id)
		{
			FuelEfficiency = 15;
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