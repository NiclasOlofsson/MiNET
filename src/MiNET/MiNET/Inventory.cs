using System;
using fNbt;
using MiNET.BlockEntities;
using MiNET.Utils;

namespace MiNET
{
	public class Inventory
	{
		public event Action<Inventory, byte, ItemStack> InventoryChange;

		public byte Id { get; set; }
		public byte Type { get; set; }
		public MetadataSlots Slots { get; set; }
		public short Size { get; set; }
		public BlockCoordinates Coordinates { get; set; }
		private BlockEntity BlockEntity { get; set; }
		public short InventorySize { get; set; }

		public Inventory(BlockEntity blockEntity, short inventorySize, NbtList slots)
		{
			BlockEntity = blockEntity;
			InventorySize = inventorySize;
			Coordinates = BlockEntity.Coordinates;

			Slots = new MetadataSlots();
			for (byte i = 0; i < InventorySize; i++)
			{
				NbtCompound item = (NbtCompound) slots[i];
				Slots[i] = new MetadataSlot(new ItemStack(item["id"].ShortValue, item["Count"].ByteValue));
			}
		}

		public void SetSlot(byte slot, ItemStack itemStack)
		{
			Slots[slot] = new MetadataSlot(itemStack);

			NbtCompound compound = BlockEntity.GetCompound();
			compound["Items"] = GetSlots();

			OnInventoryChange(slot, itemStack);
		}

		public bool DecreasteSlot(byte slot, byte numberOfItems)
		{
			bool isEmpty = false;

			MetadataSlot slotData = (MetadataSlot) Slots[slot];
			if (slotData.Value.Id == 0 || slotData.Value.Count <= 1)
			{
				slotData.Value = new ItemStack(0, 0, 0);
				isEmpty = true;
			}
			else
			{
				slotData.Value.Count -= numberOfItems;
			}

			OnInventoryChange(slot, slotData.Value);

			return isEmpty;
		}

		private NbtList GetSlots()
		{
			NbtList slots = new NbtList("Items");
			for (byte i = 0; i < InventorySize; i++)
			{
				MetadataSlot slot = (MetadataSlot) Slots[i];
				slots.Add(new NbtCompound("")
				{
					new NbtByte("Count", slot.Value.Count),
					new NbtByte("Slot", i),
					new NbtShort("id", slot.Value.Id),
					new NbtByte("Damage", (byte) slot.Value.Metadata),
				});
			}

			return slots;
		}

		protected virtual void OnInventoryChange(byte slot, ItemStack itemStack)
		{
			Action<Inventory, byte, ItemStack> handler = InventoryChange;
			if (handler != null) handler(this, slot, itemStack);
		}
	}
}