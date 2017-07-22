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

		protected override bool CanPlace(Level world, BlockCoordinates blockCoordinates, BlockFace face)
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

		public override Item GetSmelt()
		{
			byte meta = 0;
			switch (Metadata & 0x7)
			{
				case 0: //Sunflower
					meta = 11;
					break;
				case 1: //Lilac
					meta = 13;
					break;

				case 2: //Double TallGrass
					return base.GetSmelt();
				case 3: //Large Fern
					return base.GetSmelt();
				case 4: //Rose Bush
					return base.GetSmelt();

				case 5: //Peony
					meta = 9;
					break;
			}

			return ItemFactory.GetItem(351, meta); //Smelt to proper dye
		}
	}
}