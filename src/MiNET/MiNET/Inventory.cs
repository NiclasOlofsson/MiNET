using System;
using Craft.Net.Common;
using fNbt;
using MiNET.BlockEntities;
using MiNET.Utils;
using ItemStack = MiNET.Utils.ItemStack;
using MetadataSlot = MiNET.Utils.MetadataSlot;

namespace MiNET
{
	public class Inventory
	{
		public event Action<Inventory, byte, ItemStack> InventoryChange;

		public byte Id { get; set; }
		public byte Type { get; set; }
		public MetadataSlots Slots { get; set; }
		public short Size { get; set; }
		public Coordinates3D Coordinates { get; set; }
		private BlockEntity Chest { get; set; }

		public Inventory(ChestBlockEntity chest, NbtList slots)
		{
			Chest = chest;
			Coordinates = Chest.Coordinates;

			Slots = new MetadataSlots();
			for (byte i = 0; i < 27; i++)
			{
				NbtCompound item = (NbtCompound) slots[i];
				Slots[i] = new MetadataSlot(new ItemStack(item["id"].ShortValue, item["Count"].ByteValue));
			}
		}

		public void SetSlot(byte slot, ItemStack itemStack)
		{
			Slots[slot] = new MetadataSlot(itemStack);

			NbtCompound compound = Chest.GetCompound();
			compound["Items"] = GetSlots();

			OnInventoryChange(slot, itemStack);
		}

		private NbtList GetSlots()
		{
			NbtList slots = new NbtList("Items");
			for (byte i = 0; i < 27; i++)
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