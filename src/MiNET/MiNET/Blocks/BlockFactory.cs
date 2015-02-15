namespace MiNET.Blocks
{
	public static class BlockFactory
	{
		public static Block GetBlockById(byte blockId)
		{
			Block block;

			if (blockId == 0) block = new Air();
			else if (blockId == 5) block = new WoodenPlanks();
			else if (blockId == 6) block = new Sapling();
			else if (blockId == 7) block = new Bedrock();
			else if (blockId == 17) block = new Wood();
			else if (blockId == 20) block = new Glass();
			else if (blockId == 46) block = new Tnt();
			else if (blockId == 47) block = new Bookshelf();
			else if (blockId == 51) block = new Fire();
			else if (blockId == 53) block = new OakWoodStairs();
			else if (blockId == 54) block = new Chest();
			else if (blockId == 58) block = new CraftingTable();
			else if (blockId == 61) block = new Furnace();
			else if (blockId == 62) block = new LitFurnace();
			else if (blockId == 63) block = new StandingSign();
			else if (blockId == 64) block = new WoodenDoor();
			else if (blockId == 67) block = new CobbleStoneStairs();
			else if (blockId == 67) block = new WallSign();
			else if (blockId == 85) block = new Fence();
			else if (blockId == 107) block = new FenceGate();
			else if (blockId == 108) block = new BrickStairs();
			else if (blockId == 109) block = new StoneBrickStairs();
			else if (blockId == 114) block = new NetherBrickStairs();
			else if (blockId == 128) block = new SandStoneStairs();
			else if (blockId == 134) block = new SpruceWoodStairsStairs();
			else if (blockId == 135) block = new BirchWoodStairs();
			else if (blockId == 136) block = new JungleWoodStairs();
			else if (blockId == 156) block = new QuartzStairs();
			else if (blockId == 163) block = new AcaciaWoodStairs();
			else if (blockId == 164) block = new DarkOakWoodStairs();
			else if (blockId == 173) block = new CoalBlock();
			else block = new Block(blockId);

			return block;
		}
	}

	public class Sapling : Block
	{
		internal Sapling() : base(6)
		{
			FuelEfficiency = 5;
		}
	}

	public class Bookshelf : Block
	{
		internal Bookshelf() : base(47)
		{
			FuelEfficiency = 15;
		}
	}

	public class FenceGate : Block
	{
		internal FenceGate() : base(107)
		{
			FuelEfficiency = 15;
		}
	}

	public class Fence : Block
	{
		internal Fence() : base(85)
		{
			FuelEfficiency = 15;
		}
	}

	public class Wood : Block
	{
		internal Wood() : base(17)
		{
			FuelEfficiency = 15;
		}
	}

	public class WoodenPlanks : Block
	{
		internal WoodenPlanks() : base(5)
		{
			FuelEfficiency = 15;
		}
	}

	public class CoalBlock : Block
	{
		internal CoalBlock() : base(173)
		{
			FuelEfficiency = 800;
		}
	}
}