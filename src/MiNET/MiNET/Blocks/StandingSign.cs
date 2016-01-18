using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class StandingSign : Block
	{
		public StandingSign() : base(63)
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