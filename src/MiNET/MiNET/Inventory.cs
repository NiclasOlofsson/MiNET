#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Concurrent;
using fNbt;
using log4net;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Utils.Vectors;

namespace MiNET
{
	public interface IInventory
	{
	}

	public class Inventory : IInventory
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Inventory));

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
				var nbtItem = (NbtCompound) slots[i];

				Item item = ItemFactory.GetItem(nbtItem["Name"].StringValue, nbtItem["Damage"].ShortValue, nbtItem["Count"].ByteValue);
				byte slotIdx = nbtItem["Slot"].ByteValue;
				Log.Debug($"Chest item {slotIdx}: {item}");
				Slots[slotIdx] = item;
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

		public void DecreaseSlot(byte slot)
		{
			var slotData = Slots[slot];
			if (slotData is ItemAir) return;

			slotData.Count--;

			if (slotData.Count <= 0)
			{
				slotData = new ItemAir();
			}

			SetSlot(null, slot, slotData);

			OnInventoryChange(null, slot, slotData);
		}

		public void IncreaseSlot(byte slot, short itemId, short metadata)
		{
			Item slotData = Slots[slot];
			if (slotData is ItemAir)
			{
				slotData = ItemFactory.GetItem(itemId, metadata, 1);
			}
			else
			{
				slotData.Count++;
			}

			SetSlot(null, slot, slotData);

			OnInventoryChange(null, slot, slotData);
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
					new NbtString("Name", slot.Name),
					new NbtShort("Damage", slot.Metadata),
				});
			}

			return slots;
		}

		protected virtual void OnInventoryChange(Player player, byte slot, Item itemStack)
		{
			InventoryChange?.Invoke(player, this, slot, itemStack);
		}


		// Below is a workaround making it possible to send
		// updates to only peopele that is looking at this inventory.
		// Is should be converted to some sort of event based version.

		public ConcurrentBag<Player> Observers { get; } = new ConcurrentBag<Player>();

		public void AddObserver(Player player)
		{
			Observers.Add(player);
		}

		public void RemoveObserver(Player player)
		{
			// Need to arrange for this to work when players get disconnected
			// from crash. It will leak players for sure.
			Observers.TryTake(out player);
		}
	}
}