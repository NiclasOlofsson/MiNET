using log4net;

namespace MiNET.Blocks
{
	public interface ICustomBlockFactory
	{
		Block GetBlockById(byte blockId);
	}

	public static class BlockFactory
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (BlockFactory));

		public static ICustomBlockFactory CustomBlockFactory { get; set; }

		public static Block GetBlockById(byte blockId)
		{
			Block block = null;

			if (CustomBlockFactory != null)
			{
				block = CustomBlockFactory.GetBlockById(blockId);
			}

			if (block != null)
			{ 
				return block;
			}

			switch (blockId)
			{
				case 0:
					block = new Air();
					break;
				case 1: 
					block = new Stone();
					break;
				case 2:
					block = new Grass();
					break;
				case 3:
					block = new Dirt();
					break;
				case 4:
					block = new Cobblestone();
					break;
				case 5:
					block = new WoodenPlanks();
					break;
				case 6:
					block = new Sapling();
					break;
				case 7:
					block = new Bedrock();
					break;
				case 8:
					block = new FlowingWater();
					break;
				case 9:
					block = new StationaryWater();
					break;
				case 10:
					block = new FlowingLava();
					break;
				case 11:
					block = new StationaryLava();
					break;
				case 12:
					block = new Sand();
					break;
				case 13:
					block = new Gravel();
					break;
				case 14:
					block = new GoldOre();
					break;
				case 15:
					block = new IronOre();
					break;
				case 16:
					block = new CoalOre();
					break;
				case 17:
					block = new Wood();
					break;
				case 20:
					block = new Glass();
					break;
				case 21:
					block = new LapsisOre();
					break;
				case 22:
					block = new LapsisBlock();
					break;
				case 24:
					block = new Sandstone();
					break;
				case 26:
					block = new Bed();
					break;
				case 31:
					block = new TallGrass();
					break;
				case 41:
					block = new GoldBlock();
					break;
				case 43:
					block = new DoubleStoneSlab();
					break;
				case 44:
					block = new StoneSlab();
					break;
				case 46:
					block = new Tnt();
					break;
				case 47:
					block = new Bookshelf();
					break;
				case 49:
					block = new Obsidian();
					break;
				case 50:
					block = new Torch();
					break;
				case 51:
					block = new Fire();
					break;
				case 53:
					block = new OakWoodStairs();
					break;
				case 54:
					block = new Chest();
					break;
				case 56:
					block = new DiamondOre();
					break;
				case 58:
					block = new CraftingTable();
					break;
				case 60:
					block = new Farmland();
					break;
				case 61:
					block = new Furnace();
					break;
				case 62:
					block = new LitFurnace();
					break;
				case 63:
					block = new StandingSign();
					break;
				case 64:
					block = new WoodenDoor();
					break;
				case 67:
					block = new CobblestoneStairs();
					break;
				case 68:
					block = new WallSign();
					break;
				case 79:
					block = new Ice();
					break;
				case 80:
					block = new Snow();
					break;
				case 85:
					block = new Fence();
					break;
				case 98:
					block = new StoneBrick();
					break;
				case 107:
					block = new FenceGate();
					break;
				case 108:
					block = new BrickStairs();
					break;
				case 109:
					block = new StoneBrickStairs();
					break;
				case 114:
					block = new NetherBrickStairs();
					break;
				case 128:
					block = new SandStoneStairs();
					break;
				case 134:
					block = new SpruceWoodStairsStairs();
					break;
				case 135:
					block = new BirchWoodStairs();
					break;
				case 136:
					block = new JungleWoodStairs();
					break;
				case 156:
					block = new QuartzStairs();
					break;
				case 157:
					block = new DoubleWoodSlab();
					break;
				case 158:
					block = new WoodSlab();
					break;
				case 163:
					block = new AcaciaWoodStairs();
					break;
				case 164:
					block = new DarkOakWoodStairs();
					break;
				case 173:
					block = new CoalBlock();
					break;
				case 198:
					block = new GrassPath();
					break;
				default:
					block = new Block(blockId);
					break;
			}
			return block;
		}
	}
}
