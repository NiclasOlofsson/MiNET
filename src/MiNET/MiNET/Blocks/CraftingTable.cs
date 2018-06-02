using System.Numerics;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class CraftingTable : Block
	{
		public CraftingTable() : base(58)
		{
			FuelEfficiency = 15;
			BlastResistance = 12.5f;
			Hardness = 2.5f;
            IsConductive = true;
            //IsFlammable = true; // Only from lava.
        }

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			player.UsingCraftingTable = true;
			return true;
		}
	}
}