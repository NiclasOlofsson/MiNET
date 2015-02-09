using Craft.Net.Common;
using MiNET.Utils;
using MiNET.Worlds;
using ItemStack = MiNET.Utils.ItemStack;
using MetadataSlot = MiNET.Utils.MetadataSlot;

namespace MiNET
{
	public class InventoryMngr
	{
		private readonly Level _level;

		public InventoryMngr(Level level)
		{
			_level = level;
		}

		public Inventory GetInventory(Coordinates3D inventoryCoord)
		{
			var blockEntity = _level.GetBlockEntity(inventoryCoord);

			if (blockEntity == null) return null;

			// Convert NBT data to metadata format.

			var comp = blockEntity.GetCompound();

			var slots = new MetadataSlots();
			for (byte i = 0; i < 27; i++)
			{
				var item = comp["Items"][i];
				slots[i] = new MetadataSlot(new ItemStack(item["id"].ShortValue, item["Count"].ByteValue));
			}

			Inventory inventory = new Inventory
			{
				Id = 10,
				Type = 0,
				Size = 27,
				Slots = slots,
				Coordinates = inventoryCoord,
			};

			return inventory;
		}
	}
}