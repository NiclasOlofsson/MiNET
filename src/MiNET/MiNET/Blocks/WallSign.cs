using System.Numerics;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class WallSign : Block
	{
		public WallSign() : base(68)
		{
			IsTransparent = true;
			IsSolid = false;
			BlastResistance = 5;
			Hardness = 1;

			IsFlammable = true; // Only in PE!!
		}

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			return true;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new[] {ItemFactory.GetItem(323, 0, 1)}; // Drop sign item
		}
	}
}