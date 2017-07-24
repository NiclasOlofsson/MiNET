using System.Numerics;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class UnlitRedstoneTorch : Block
	{
		public UnlitRedstoneTorch() : this(75)
		{

		}

		public UnlitRedstoneTorch(byte id) : base(id) {
			IsTransparent = true;
			IsSolid = false;
		}

		protected override bool CanPlace(Level world, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
		{
			Block block = world.GetBlock(blockCoordinates);
			if (block is Farmland
				|| block is Ice
				/*|| block is Glowstone || block is Leaves  */
				|| block is Tnt
				|| block is BlockStairs
				|| block is StoneSlab
				|| block is WoodSlab) return true;

			//TODO: More checks here, but PE blocks it pretty good right now
			if (block is Glass && face == BlockFace.Up) return true;

			return !block.IsTransparent;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			if (face == BlockFace.Down) return true;

			switch (face)
			{
				case BlockFace.Up:
					Metadata = 5;
					break;
				case BlockFace.East:
					Metadata = 4;
					break;
				case BlockFace.West:
					Metadata = 3;
					break;
				case BlockFace.North:
					Metadata = 2;
					break;
				case BlockFace.South:
					Metadata = 1;
					break;
			}

			world.SetBlock(this);
			return true;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new[] {new ItemBlock(new RedstoneTorch(), 0)};
		}
	}
}