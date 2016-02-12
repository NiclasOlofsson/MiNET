using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class WallSign : Block
	{
		public WallSign() : base(68)
		{
		}

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face)
		{
			return true;
		}

		public override Item GetDrops()
		{
			return new Item(323, 0) {Count = 1}; // Drop sign item
		}
	}
}