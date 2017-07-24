using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class DoublePlant : Block
	{
		public DoublePlant() : base(175)
		{
			BlastResistance = 3;
			Hardness = 0.6f;

			IsSolid = false;
			IsReplacible = true;
			IsTransparent = true;
		}

		protected override bool CanPlace(Level world, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
		{
			if (base.CanPlace(world, blockCoordinates, face))
			{
				Block under = world.GetBlock(Coordinates + BlockCoordinates.Down);
				return under is Grass || under is Dirt;
			}

			return false;
		}

		public override void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
		{
			if (Coordinates + BlockCoordinates.Down == blockCoordinates)
			{
				level.SetAir(Coordinates);
				UpdateBlocks(level);
			}
		}

		public override Item[] GetDrops(Item tool)
		{
			if ((Metadata & 0x08) == 0x08)
			{
				return new Item[] {new ItemBlock(this, (short) (Metadata & 0x07)) {Count = 1}};
			}

			return new Item[0];
		}
	}
}