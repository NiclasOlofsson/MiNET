using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Flower : Block
	{
		public Flower() : base(38)
		{
			IsSolid = false;
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

		public override Item GetSmelt()
		{
			byte meta = 0;
			switch (Metadata)
			{
				case 0: //Poppy
				case 4: //Red Tulip
					meta = 1;
					break;
				case 1: //Blue Orchid
					meta = 12;
					break;
				case 2: //Allium
					meta = 13;
					break;
				case 5: //Orange Tulip
					meta = 14;
					break;
				case 7: //Pink Tulip
					meta = 9;
					break;

				case 3: //Azure Bluet
				case 6: //White Tulip
				case 8: //Oxeye Daisy
					meta = 7;
					break;
			}
			return ItemFactory.GetItem(351, meta); //Smelt to proper dye
		}
	}
}