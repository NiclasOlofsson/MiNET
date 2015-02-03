using System;
using MiNET.Utils;
using System.IO;
using MiNET.Net;

namespace MiNET
{
	public class InventoryManager
	{
		public MetadataSlots Armor { get; private set; }
		public MetadataSlots Slots { get; private set; }
		public MetadataInts ItemHotbar { get; private set; }
		public MetadataSlot ItemInHand { get; private set; }
		private Player Player { get; set; }
		public InventoryManager(Player player)
		{
			Player = player;
			Armor = new MetadataSlots();
			Slots = new MetadataSlots();
			ItemHotbar = new MetadataInts();
			ItemInHand = new MetadataSlot(new ItemStack(-1));

			Armor[0] = new MetadataSlot(new ItemStack(-1));
			Armor[1] = new MetadataSlot(new ItemStack(-1));
			Armor[2] = new MetadataSlot(new ItemStack(-1));
			Armor[3] = new MetadataSlot(new ItemStack(-1));

			for (byte i = 0; i < 35; i++)
			{
				Slots[i] = new MetadataSlot(new ItemStack(-1));
			}

			for (byte i = 0; i < 6; i++)
			{
				ItemHotbar[i] = new MetadataInt(-1);
			}

			ItemHotbar[0] = new MetadataInt(9);
			ItemHotbar[1] = new MetadataInt(10);
			ItemHotbar[2] = new MetadataInt(11);
			ItemHotbar[3] = new MetadataInt(12);
			ItemHotbar[4] = new MetadataInt(13);
			ItemHotbar[5] = new MetadataInt(14);
		}

		/// <summary>
		/// Set a players slot to the specified item.
		/// </summary>
		/// <param name="slot">The slot to set</param>
		/// <param name="itemID">The item id</param>
		/// <param name="amount">Amount of items</param>
		/// <param name="metadata">Metadata for the item</param>
		public void SetInventorySlot(byte slot, short itemID, sbyte amount = 1, short metadata = 0)
		{
			if (slot > 35) throw new IndexOutOfRangeException("slot");
			Slots[slot] = new MetadataSlot(new ItemStack(itemID, amount, metadata ));

			Player.SendPackage(new McpeContainerSetContent
			{
				windowId = 0,
				slotData = Slots,
				hotbarData = ItemHotbar
			});
		}

		/// <summary>
		/// Empty the specified slot
		/// </summary>
		/// <param name="slot">The slot to empty.</param>
		public void EmptyInventorySlot(byte slot)
		{
			SetInventorySlot(slot, -1);
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