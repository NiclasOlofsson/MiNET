using System;
using fNbt;
using MiNET.BlockEntities;
using MiNET.Items;
using MiNET.Utils;

namespace MiNET
{
	public class Inventory
	{
		public event Action<Player, Inventory, byte, Item> InventoryChange;

		public int Id { get; set; }
		public byte Type { get; set; }
		public ItemStacks Slots { get; set; }
		public short Size { get; set; }
		public BlockCoordinates Coordinates { get; set; }
		public BlockEntity BlockEntity { get; set; }
		public byte WindowsId { get; set; }

		public Inventory(int id, BlockEntity blockEntity, short inventorySize, NbtList slots)
		{
			Id = id;
			BlockEntity = blockEntity;
			Size = inventorySize;
			Coordinates = BlockEntity.Coordinates;

			Slots = new ItemStacks();
			for (byte i = 0; i < Size; i++)
			{
				Slots.Add(new ItemAir());
			}

			for (byte i = 0; i < slots.Count; i++)
			{
				NbtCompound item = (NbtCompound) slots[i];

				Slots[item["Slot"].ByteValue] = new Item(item["id"].ShortValue, item["Damage"].ByteValue) {Count = item["Count"].ByteValue};
			}
		}

		public void SetSlot(Player player, byte slot, Item itemStack)
		{
			Slots[slot] = itemStack;

			NbtCompound compound = BlockEntity.GetCompound();
			compound["Items"] = GetSlots();

			OnInventoryChange(player, slot, itemStack);
		}

		public Item GetSlot(byte slot)
		{
			return Slots[slot];
		}

		public bool DecreaseSlot(byte slot)
		{
			return DecreaseSlot(null, slot);
		}

		public bool DecreaseSlot(Player player, byte slot)
		{
			bool isEmpty = false;

			var slotData = Slots[slot];
			if (slotData.Id == 0 || slotData.Count <= 1)
			{
				slotData = new ItemAir();
				isEmpty = true;
			}
			else
			{
				slotData.Count--;
			}

			OnInventoryChange(player, slot, slotData);

			return isEmpty;
		}

		public void IncreaseSlot(byte slot, short itemId, short metadata)
		{
			IncreaseSlot(null, slot, itemId, metadata);
		}

		public void IncreaseSlot(Player player, byte slot, short itemId, short metadata)
		{
			var slotData = Slots[slot];
			if (slotData.Id == 0)
			{
				slotData = new Item(itemId, metadata) {Count = 1};
			}
			else
			{
				slotData.Count++;
			}

			OnInventoryChange(player, slot, slotData);
		}

		public bool IsOpen()
		{
			return InventoryChange != null;
		}


		private NbtList GetSlots()
		{
			NbtList slots = new NbtList("Items");
			for (byte i = 0; i < Size; i++)
			{
				var slot = Slots[i];
				slots.Add(new NbtCompound
				{
					new NbtByte("Count", slot.Count),
					new NbtByte("Slot", i),
					new NbtShort("id", slot.Id),
					new NbtByte("Damage", (byte) slot.Metadata),
				});
			}

			return slots;
		}

		protected virtual void OnInventoryChange(Player player, byte slot, Item itemStack)
		{
			Action<Player, Inventory, byte, Item> handler = InventoryChange;
			if (handler != null) handler(player, this, slot, itemStack);
		}
	}
}