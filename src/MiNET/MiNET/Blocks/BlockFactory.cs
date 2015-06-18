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

			if (block != null) return block;

			if (blockId == 0) block = new Air();
			else if (blockId == 1) block = new Stone();
			else if (blockId == 2) block = new Grass();
			else if (blockId == 3) block = new Dirt();
			else if (blockId == 4) block = new Cobblestone();
			else if (blockId == 5) block = new WoodenPlanks();
			else if (blockId == 6) block = new Sapling();
			else if (blockId == 7) block = new Bedrock();
			else if (blockId == 8) block = new FlowingWater();
			else if (blockId == 9) block = new StationaryWater();
			else if (blockId == 10) block = new FlowingLava();
			else if (blockId == 11) block = new StationaryLava();
			else if (blockId == 12) block = new Sand();
			else if (blockId == 13) block = new Gravel();
			else if (blockId == 14) block = new GoldOre();
			else if (blockId == 15) block = new IronOre();
			else if (blockId == 16) block = new CoalOre();
			else if (blockId == 17) block = new Wood();
			else if (blockId == 20) block = new Glass();
			else if (blockId == 21) block = new LapsisOre();
			else if (blockId == 22) block = new LapsisBlock();
			else if (blockId == 24) block = new Sandstone();
			else if (blockId == 24) block = new Bed();
			else if (blockId == 31) block = new TallGrass();
			else if (blockId == 41) block = new GoldBlock();
			else if (blockId == 43) block = new DoubleStoneSlab();
			else if (blockId == 44) block = new StoneSlab();
			else if (blockId == 46) block = new Tnt();
			else if (blockId == 47) block = new Bookshelf();
			else if (blockId == 49) block = new Obsidian();
			else if (blockId == 51) block = new Fire();
			else if (blockId == 53) block = new OakWoodStairs();
			else if (blockId == 54) block = new Chest();
			else if (blockId == 56) block = new DiamondOre();
			else if (blockId == 58) block = new CraftingTable();
			else if (blockId == 61) block = new Furnace();
			else if (blockId == 62) block = new LitFurnace();
			else if (blockId == 63) block = new StandingSign();
			else if (blockId == 64) block = new WoodenDoor();
			else if (blockId == 67) block = new CobblestoneStairs();
			else if (blockId == 68) block = new WallSign();
			else if (blockId == 79) block = new Ice();
			else if (blockId == 80) block = new Snow();
			else if (blockId == 85) block = new Fence();
			else if (blockId == 98) block = new StoneBrick();
			else if (blockId == 107) block = new FenceGate();
			else if (blockId == 108) block = new BrickStairs();
			else if (blockId == 109) block = new StoneBrickStairs();
			else if (blockId == 114) block = new NetherBrickStairs();
			else if (blockId == 128) block = new SandStoneStairs();
			else if (blockId == 134) block = new SpruceWoodStairsStairs();
			else if (blockId == 135) block = new BirchWoodStairs();
			else if (blockId == 136) block = new JungleWoodStairs();
			else if (blockId == 156) block = new QuartzStairs();
			else if (blockId == 157) block = new DoubleWoodSlab();
			else if (blockId == 158) block = new WoodSlab();
			else if (blockId == 163) block = new AcaciaWoodStairs();
			else if (blockId == 164) block = new DarkOakWoodStairs();
			else if (blockId == 173) block = new CoalBlock();
			else
			{
				Log.InfoFormat(@"
	// Add this missing block to the BlockFactory
	else if (blockId == {1}) block = new {0}();
	
	public class {0} : Block
	{{
		internal {0}() : base({1})
		{{
		}}
	}}
", "Missing", blockId);
				block = new Block(blockId);
			}

			return block;
		}
	}
}