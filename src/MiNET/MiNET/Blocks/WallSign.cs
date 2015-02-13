using MiNET.Utils;
using MiNET.Worlds;
using ItemStack = MiNET.Utils.ItemStack;

namespace MiNET.Blocks
{
	public class WallSign : Block
	{
		internal WallSign() : base(68)
		{
		}

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face)
		{
			return true;
		}

		public override ItemStack GetDrops()
		{
			return new ItemStack(323, 1); // Drop sign item
		}
	}
}