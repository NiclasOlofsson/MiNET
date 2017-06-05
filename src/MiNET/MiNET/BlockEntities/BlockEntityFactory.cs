namespace MiNET.BlockEntities
{
	public static class BlockEntityFactory
	{
		public static BlockEntity GetBlockEntityById(string blockEntityId)
		{
			BlockEntity blockEntity = null;

			if (blockEntityId == "Sign") blockEntity = new Sign();
			else if (blockEntityId == "Chest") blockEntity = new ChestBlockEntity();
			else if (blockEntityId == "EnchantTable") blockEntity = new EnchantingTableBlockEntity();
			else if (blockEntityId == "Furnace") blockEntity = new FurnaceBlockEntity();
			else if (blockEntityId == "Skull") blockEntity = new SkullBlockEntity();
			else if (blockEntityId == "ItemFrame") blockEntity = new ItemFrameBlockEntity();
			else if (blockEntityId == "Bed") blockEntity = new BedBlockEntity();

			return blockEntity;
		}
	}
}