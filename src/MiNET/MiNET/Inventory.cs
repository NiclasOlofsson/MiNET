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
		public event Action<Inventory> InventoryChange;

		public byte Id { get; set; }
		public byte Type { get; set; }
		public MetadataSlots Slots { get; set; }
		public short Size { get; set; }
		public Coordinates3D Coordinates { get; set; }
		public BlockEntity BlockEntity { get; set; }

		public Inventory(BlockEntity blockEntity, NbtList slots)
		{
			BlockEntity = blockEntity;
			Coordinates = BlockEntity.Coordinates;

			SetSlots(slots);
		}

		private void SetSlots(NbtList slots)
		{
			Slots = new MetadataSlots();
			for (byte i = 0; i < 27; i++)
			{
				NbtCompound item = (NbtCompound) slots[i];
				Slots[i] = new MetadataSlot(new ItemStack(item["id"].ShortValue, item["Count"].ByteValue));
			}

			OnInventoryChange();
		}

		public void SetSlot(byte slot, MetadataSlot metadataSlot)
		{
			Slots[slot] = metadataSlot;

			NbtCompound compound = BlockEntity.GetCompound();
			compound["Items"] = GetSlots();

			OnInventoryChange();
		}

		public NbtList GetSlots()
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

		protected virtual void OnInventoryChange()
		{
			Action<Inventory> handler = InventoryChange;
			if (handler != null) handler(this);
		}
	}
}