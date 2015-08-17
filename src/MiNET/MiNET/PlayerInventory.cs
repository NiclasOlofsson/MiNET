using System;
using System.IO;
using MiNET.Net;
using MiNET.Utils;

namespace MiNET
{
	public class PlayerInventory
	{
		public MetadataSlots Armor { get; private set; }
		public MetadataSlots Slots { get; private set; }
		public MetadataInts ItemHotbar { get; private set; }
		public MetadataSlot ItemInHand { get; set; }
		private Player _player;

		public PlayerInventory(Player player)
		{
			_player = player;
			Armor = new MetadataSlots();
			Slots = new MetadataSlots();
			ItemHotbar = new MetadataInts();
			ItemInHand = new MetadataSlot(new ItemStack());

			Armor[0] = new MetadataSlot(new ItemStack());
			Armor[1] = new MetadataSlot(new ItemStack());
			Armor[2] = new MetadataSlot(new ItemStack());
			Armor[3] = new MetadataSlot(new ItemStack());

			//Armor[0] = new MetadataSlot(new ItemStack(306));
			//Armor[1] = new MetadataSlot(new ItemStack(307));
			//Armor[2] = new MetadataSlot(new ItemStack(308));
			//Armor[3] = new MetadataSlot(new ItemStack(309));

			for (byte i = 0; i < 35; i++)
			{
				Slots[i] = new MetadataSlot(new ItemStack((short) -1, 0));
			}

			byte c = 0;
			//Slots[c++] = new MetadataSlot(new ItemStack(268, 1)); // Wooden Sword
			//Slots[c++] = new MetadataSlot(new ItemStack(283, 1)); // Golden Sword
			//Slots[c++] = new MetadataSlot(new ItemStack(272, 1)); // Stone Sword
			//Slots[c++] = new MetadataSlot(new ItemStack(267, 1)); // Iron Sword
			//Slots[c++] = new MetadataSlot(new ItemStack(276, 1)); // Diamond Sword

			//Slots[c++] = new MetadataSlot(new ItemStack(261, 1)); // Bow
			//Slots[c++] = new MetadataSlot(new ItemStack(262, 64)); // Arrows
			//Slots[c++] = new MetadataSlot(new ItemStack(344, 64)); // Eggs
			//Slots[c++] = new MetadataSlot(new ItemStack(332, 64)); // Snowballs

			for (byte i = 0; i < 6; i++)
			{
				ItemHotbar[i] = new MetadataInt(i + 9);
			}
		}

		/// <summary>
		///     Set a players slot to the specified item.
		/// </summary>
		/// <param name="slot">The slot to set</param>
		/// <param name="itemId">The item id</param>
		/// <param name="amount">Amount of items</param>
		/// <param name="metadata">Metadata for the item</param>
		public void SetInventorySlot(byte slot, short itemId, byte amount = 1, short metadata = 0)
		{
			if (slot > 35) throw new IndexOutOfRangeException("slot");
			Slots[slot] = new MetadataSlot(new ItemStack(itemId, amount, metadata));

			_player.SendPackage(new McpeContainerSetContent
			{
				windowId = 0,
				slotData = Slots,
				hotbarData = ItemHotbar
			});
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
            order.selectedSlot = (byte)slot;
            _player.SendPackage(order);

            McpePlayerEquipment broadcast = McpePlayerEquipment.CreateObject();
            broadcast.entityId = _player.EntityId;
            broadcast.selectedSlot = (byte)slot;
            _player.Level.RelayBroadcast(broadcast);
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

		public byte[] Export()
		{
			byte[] buffer;
			using (MemoryStream stream = new MemoryStream())
			{
				NbtBinaryWriter writer = new NbtBinaryWriter(stream, false);

				Armor.WriteTo(writer);

				Slots.WriteTo(writer);

				ItemHotbar.WriteTo(writer);

				writer.Flush();
				buffer = stream.GetBuffer();
			}
			return buffer;
		}

		public bool Import(byte[] data)
		{
			using (MemoryStream stream = new MemoryStream(data))
			{
				NbtBinaryReader reader = new NbtBinaryReader(stream, false);

				Armor = MetadataSlots.FromStream(reader);
				Slots = MetadataSlots.FromStream(reader);
				ItemHotbar = MetadataInts.FromStream(reader);
			}

			return true;
		}
	}
}