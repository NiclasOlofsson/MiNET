using System;
using System.Collections.Generic;
using System.Linq;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;

namespace MiNET
{
	public class PlayerInventory
	{
		public Player Player { get; private set; }

		public List<ItemStack> Slots { get; private set; }
		public int[] ItemHotbar { get; private set; }
		public Item ItemInHand { get; set; }


		// Armour
		public Item Boots { get; set; }
		public Item Leggings { get; set; }
		public Item Chest { get; set; }
		public Item Helmet { get; set; }

		public PlayerInventory(Player player)
		{
			Player = player;
			Slots = Enumerable.Repeat(new ItemStack(), 35).ToList();
			ItemHotbar = new int[6];
			ItemInHand = new Item(-1, 0);

			Boots = new ItemDiamondBoots(0);
			Leggings = new ItemDiamondLeggings(0);
			Chest = new ItemDiamondChestplate(0);
			Helmet = new ItemDiamondHelmet(0);

			for (byte i = 0; i < 6; i++)
			{
				ItemHotbar[i] = i + 9;
			}
		}

		[Wired]
		public void SetInventorySlot(byte slot, short itemId, byte amount = 1, short metadata = 0)
		{
			if (slot > 35) throw new IndexOutOfRangeException("slot");
			Slots[slot] = new ItemStack(itemId, amount, metadata);

			Player.SendPackage(new McpeContainerSetContent
			{
				windowId = 0,
				slotData = GetSlots(),
				hotbarData = GetHotbar()
			});
		}

		public MetadataInts GetHotbar()
		{
			MetadataInts metadata = new MetadataInts();
			for (byte i = 0; i < ItemHotbar.Length; i++)
			{
				metadata[i] = new MetadataInt(ItemHotbar[i]);
			}

			return metadata;
		}

		public MetadataSlots GetSlots()
		{
			var slotData = new MetadataSlots();
			for (byte i = 0; i < Slots.Count; i++)
			{
				slotData[i] = new MetadataSlot(Slots[i]);
			}

			return slotData;
		}

		public MetadataSlots GetArmor()
		{
			var slotData = new MetadataSlots();
			slotData[0] = new MetadataSlot(new ItemStack((short) Helmet.Id, 1, Helmet.Metadata));
			slotData[1] = new MetadataSlot(new ItemStack((short) Chest.Id, 1, Helmet.Metadata));
			slotData[2] = new MetadataSlot(new ItemStack((short) Leggings.Id, 1, Helmet.Metadata));
			slotData[3] = new MetadataSlot(new ItemStack((short) Boots.Id, 1, Helmet.Metadata));
			return slotData;
		}

		public void SetFirstEmptySlot(short itemId, byte amount = 1, short metadata = 0)
		{
			for (byte s = 0; s < Slots.Count; s++)
			{
				var b = (MetadataSlot) Slots[s];
				if (b.Value.Id == itemId && b.Value.Metadata == metadata && b.Value.Count < 64)
				{
					SetInventorySlot(s, itemId, (byte) (b.Value.Count + amount), metadata);
					break;
				}
				else if (b.Value == null || b.Value.Id == 0 || b.Value.Id == -1)
				{
					SetInventorySlot(s, itemId, amount, metadata);
					break;
				}
			}
		}

		public void SetHeldItemSlot(int slot)
		{
			McpePlayerEquipment order = McpePlayerEquipment.CreateObject();
			order.entityId = 0;
			order.selectedSlot = (byte) slot;
			Player.SendPackage(order);

			McpePlayerEquipment broadcast = McpePlayerEquipment.CreateObject();
			broadcast.entityId = Player.EntityId;
			broadcast.selectedSlot = (byte) slot;
			Player.Level.RelayBroadcast(broadcast);
		}

		/// <summary>
		///     Empty the specified slot
		/// </summary>
		/// <param name="slot">The slot to empty.</param>
		public void ClearInventorySlot(byte slot)
		{
			SetInventorySlot(slot, 0, 0);
		}

		public bool HasItem(MetadataSlot item)
		{
			for (byte i = 0; i < Slots.Count; i++)
			{
				if (((MetadataSlot) Slots[i]).Value.Id == item.Value.Id && ((MetadataSlot) Slots[i]).Value.Metadata == item.Value.Metadata)
				{
					return true;
				}
			}
			return false;
		}
	}
}