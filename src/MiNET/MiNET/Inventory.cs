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
		public BlockEntity BlockEntity { get; set; }

		public Inventory(byte id, BlockEntity blockEntity, short inventorySize, NbtList slots)
		{
			Id = id;
			BlockEntity = blockEntity;
			Size = inventorySize;
			Coordinates = BlockEntity.Coordinates;

			Slots = new MetadataSlots();
			for (byte i = 0; i < Size; i++)
			{
				if (i < slots.Count)
				{
					NbtCompound item = (NbtCompound)slots[i];
					Slots[i] = new MetadataSlot(new ItemStack(item["id"].ShortValue, item["Count"].ByteValue));
				}
				else
				{
					Slots[i] = new MetadataSlot(new ItemStack());
				}
			}
		}

		public void SetSlot(byte slot, ItemStack itemStack)
		{
			Slots[slot] = new MetadataSlot(itemStack);

			NbtCompound compound = BlockEntity.GetCompound();
			compound["Items"] = GetSlots();

			OnInventoryChange(slot, itemStack);
		}

        public ItemStack GetSlot(byte slot)
        {
            MetadataSlot slotData = (MetadataSlot)Slots[slot];
            return slotData.Value;
        }

		public bool DecreaseSlot(byte slot)
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
				slotData.Value.Count--;
			}

			OnInventoryChange(slot, slotData.Value);

			return isEmpty;
		}

		public void IncreaseSlot(byte slot, int itemId, short metadata)
		{
			MetadataSlot slotData = (MetadataSlot) Slots[slot];
			if (slotData.Value.Id == 0)
			{
				slotData.Value = new ItemStack((short) itemId, 1, metadata);
			}
			else
			{
				slotData.Value.Count++;
			}

			OnInventoryChange(slot, slotData.Value);
		}


		private NbtList GetSlots()
		{
			NbtList slots = new NbtList("Items");
			for (byte i = 0; i < Size; i++)
			{
				MetadataSlot slot = (MetadataSlot) Slots[i];
				slots.Add(new NbtCompound
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