namespace MiNET.BlockEntities
{
	public static class BlockEntityFactory
	{
		public static BlockEntity GetBlockEntityById(string blockEntityId)
		{
			BlockEntity blockEntity = null;

			if (blockEntityId == "Sign") blockEntity = new Sign();
			else if (blockEntityId == "Chest") blockEntity = new ChestBlockEntity();

			return blockEntity;
		}
	}
}