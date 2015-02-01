namespace MiNET.Blocks
{
	public static class BlockFactory
	{
		public static Block GetBlockById(byte blockId)
		{
			Block block = new Block(blockId);

			if (blockId == 0) block = new Air();
			if (blockId == 7) block = new Bedrock();
			if (blockId == 20) block = new Glass();
			if (blockId == 46) block = new Tnt();
			if (blockId == 51) block = new Fire();
			if (blockId == 53) block = new OakWoodStairs();
			if (blockId == 54) block = new Chest();
			if (blockId == 64) block = new WoodenDoor();
			if (blockId == 67) block = new CobbleStoneStairs();
			if (blockId == 108) block = new BrickStairs();
			if (blockId == 109) block = new StoneBrickStairs();
			if (blockId == 114) block = new NetherBrickStairs();
			if (blockId == 128) block = new SandStoneStairs();
			if (blockId == 134) block = new SpruceWoodStairsStairs();
			if (blockId == 135) block = new BirchWoodStairs();
			if (blockId == 136) block = new JungleWoodStairs();
			if (blockId == 156) block = new QuartzStairs();
			if (blockId == 163) block = new AcaciaWoodStairs();
			if (blockId == 164) block = new DarkOakWoodStairs();

			return block;
		}
	}
}