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
// The Original Code is Niclas Olofsson.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System.Collections.Generic;
using System.Linq;
using log4net;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET
{
	public class PlayerInventory
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (PlayerInventory));

		public const int HotbarSize = 9;
		public const int InventorySize = HotbarSize + 36;
		public Player Player { get; private set; }

		public List<Item> Slots { get; private set; }
		public int[] ItemHotbar { get; private set; }
		public int InHandSlot { get; set; }

		public Item Cursor { get; set; } = new ItemAir();

		// Armour
		public Item Boots { get; set; } = new ItemAir();
		public Item Leggings { get; set; } = new ItemAir();
		public Item Chest { get; set; } = new ItemAir();
		public Item Helmet { get; set; } = new ItemAir();

		public PlayerInventory(Player player)
		{
			Player = player;

			int idx = 1;
			Slots = Enumerable.Repeat((Item) new ItemAir(), InventorySize).ToList();
			//Slots[idx++] = new ItemCompass(); // test with y=-1
			//Slots[idx++] = new ItemSpawnEgg(EntityType.Wither);
			//Slots[idx++] = new ItemSpawnEgg(EntityType.Wolf);
			//Slots[idx++] = new ItemSpawnEgg(EntityType.Pig);
			//Slots[idx++] = new ItemSpawnEgg(EntityType.Horse);
			//Slots[idx++] = new ItemSpawnEgg(EntityType.SkeletonHorse);
			//Slots[idx++] = new ItemSpawnEgg(EntityType.Npc);
			//Slots[idx++] = new ItemSpawnEgg(EntityType.Zombie);
			//Slots[idx++] = new ItemSpawnEgg(EntityType.IronGolem);
			//Slots[idx++] = new ItemSnowball();
			//Slots[idx++] = new ItemBow();
			//Slots[idx++] = new ItemArrow() {Count = 64};

			ItemHotbar = new int[HotbarSize];
			for (byte i = 0; i < ItemHotbar.Length; i++)
			{
				ItemHotbar[i] = i;
			}

			InHandSlot = 0;
		}

		public virtual Item GetItemInHand()
		{
			var index = ItemHotbar[InHandSlot];
			if (index == -1 || index >= Slots.Count) return new ItemAir();

			return Slots[index] ?? new ItemAir();
		}

		[Wired]
		public void SetInventorySlot(int slot, Item item)
		{
			Slots[slot] = item;

			SendSetSlot(slot);
		}

		public void UpdateInventorySlot(int slot, Item item)
		{
			var existing = Slots[slot];
			if (existing.Id != item.Id)
			{
				Slots[slot] = item;
				existing = item;
			}

			existing.Count = item.Count;
			existing.Metadata = item.Metadata;
			existing.ExtraData = item.ExtraData;
		}

		public MetadataInts GetHotbar()
		{
			MetadataInts metadata = new MetadataInts();
			for (byte i = 0; i < ItemHotbar.Length; i++)
			{
				if (ItemHotbar[i] == -1)
				{
					metadata[i] = new MetadataInt(-1);
				}
				else
				{
					metadata[i] = new MetadataInt(ItemHotbar[i] + HotbarSize);
				}
			}

			return metadata;
		}

		public ItemStacks GetSlots()
		{
			ItemStacks slotData = new ItemStacks();
			for (int i = 0; i < Slots.Count; i++)
			{
				if (Slots[i].Count == 0) Slots[i] = new ItemAir();
				slotData.Add(Slots[i]);
			}

			return slotData;
		}

		public ItemStacks GetArmor()
		{
			return new ItemStacks
			{
				Helmet ?? new ItemAir(),
				Chest ?? new ItemAir(),
				Leggings ?? new ItemAir(),
				Boots ?? new ItemAir(),
			};
		}

		public bool SetFirstEmptySlot(Item item, bool update, bool reverseOrder)
		{
			if (reverseOrder)
			{
				for (int si = Slots.Count; si > 0; si--)
				{
					if (FirstEmptySlot(item, update, si - 1)) return true;
				}
			}
			else
			{
				for (int si = 0; si < Slots.Count; si++)
				{
					if (FirstEmptySlot(item, update, si)) return true;
				}
			}

			return false;
		}

		private bool FirstEmptySlot(Item item, bool update, int si)
		{
			Item existingItem = Slots[si];

			if (existingItem.Id == item.Id && existingItem.Metadata == item.Metadata && existingItem.Count + item.Count <= item.MaxStackSize)
			{
				Slots[si].Count += item.Count;
				if (update) SendSetSlot(si);
				return true;
			}
			else if (existingItem is ItemAir || existingItem.Id == -1)
			{
				Slots[si] = item;
				if (update) SendSetSlot(si);
				return true;
			}

			return false;
		}

		public void SetHeldItemSlot(int selectedHotbarSlot, bool sendToPlayer = true)
		{
			InHandSlot = selectedHotbarSlot;

			if (sendToPlayer)
			{
				McpeMobEquipment order = McpeMobEquipment.CreateObject();
				order.runtimeEntityId = EntityManager.EntityIdSelf;
				order.item = GetItemInHand();
				order.selectedSlot = (byte) selectedHotbarSlot;
				order.slot = (byte) ItemHotbar[InHandSlot];
				Player.SendPackage(order);
			}

			McpeMobEquipment broadcast = McpeMobEquipment.CreateObject();
			broadcast.runtimeEntityId = Player.EntityId;
			broadcast.item = GetItemInHand();
			broadcast.selectedSlot = (byte) selectedHotbarSlot;
			broadcast.slot = (byte) ItemHotbar[InHandSlot];
			Player.Level?.RelayBroadcast(Player, broadcast);
		}

		/// <summary>
		///     Empty the specified slot
		/// </summary>
		/// <param name="slot">The slot to empty.</param>
		public void ClearInventorySlot(byte slot)
		{
			SetInventorySlot(slot, new ItemAir());
		}

		public bool HasItem(Item item)
		{
			for (byte i = 0; i < Slots.Count; i++)
			{
				if ((Slots[i]).Id == item.Id && (Slots[i]).Metadata == item.Metadata)
				{
					return true;
				}
			}
			return false;
		}

		public void RemoveItems(short id, byte count)
		{
			for (byte i = 0; i < Slots.Count; i++)
			{
				var slot = Slots[i];
				if (slot.Id == id)
				{
					slot.Count--;
					if (slot.Count == 0)
					{
						Slots[i] = new ItemAir();
					}

					SendSetSlot(i);
					return;
				}
			}
		}

		public void SendSetSlot(int slot)
		{
			McpeInventorySlot sendSlot = McpeInventorySlot.CreateObject();
			sendSlot.inventoryId = 0;
			sendSlot.slot = (uint) slot;
			sendSlot.item = Slots[slot];
			Player.SendPackage(sendSlot);
		}

		public void Clear()
		{
			for (int i = 0; i < Slots.Count; ++i)
			{
				if (Slots[i] == null || Slots[i].Id != 0) Slots[i] = new ItemAir();
			}

			if (Helmet.Id != 0) Helmet = new ItemAir();
			if (Chest.Id != 0) Chest = new ItemAir();
			if (Leggings.Id != 0) Leggings = new ItemAir();
			if (Boots.Id != 0) Boots = new ItemAir();

			Player.SendPlayerInventory();
		}
	}
}