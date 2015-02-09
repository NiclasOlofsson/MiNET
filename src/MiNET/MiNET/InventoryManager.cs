using Craft.Net.Common;
using fNbt;
using MiNET.Worlds;

namespace MiNET
{
	public class InventoryManager
	{
		private readonly Level _level;

		public InventoryManager(Level level)
		{
			_level = level;
		}

		public Inventory GetInventory(Coordinates3D inventoryCoord)
		{
			var blockEntity = _level.GetBlockEntity(inventoryCoord);

			if (blockEntity == null) return null;

			NbtCompound comp = blockEntity.GetCompound();

			Inventory inventory = new Inventory(blockEntity, (NbtList) comp["Items"])
			{
				Id = 10,
				Type = 0,
				Size = 27,
			};

			return inventory;
		}
	}
}