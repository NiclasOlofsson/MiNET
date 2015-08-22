using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class CraftingTable : Block
	{
		public CraftingTable() : base(58)
		{
			FuelEfficiency = 15;
		}

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face)
		{
			return true;
		}
	}
}