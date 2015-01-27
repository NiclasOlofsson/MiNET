namespace MiNET.Blocks
{
	public static class BlockFactory
	{
		public static Block GetBlockById(byte blockId)
		{
			Block block = new Block(blockId);

			if (blockId == 0) block = new BlockAir();

			if (blockId == 54) block = new BlockChest();
			if (blockId == 64) block = new BlockWoodenDoor();
            if (blockId == 7) block = new BlockBedrock();
			return block;
		}
	}
}