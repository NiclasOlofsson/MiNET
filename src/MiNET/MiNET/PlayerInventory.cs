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
		public MetadataSlot ItemInHand { get; private set; }
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

			for (byte i = 0; i < 35; i++)
			{
				Slots[i] = new MetadataSlot(new ItemStack((short) (i + 1), 10));
			}
			Slots[0] = new MetadataSlot(new ItemStack(54, 10));
			//Slots[1] = new MetadataSlot(new ItemStack(272, 1));
			//Slots[2] = new MetadataSlot(new ItemStack(267, 1));
			//Slots[3] = new MetadataSlot(new ItemStack(276, 1));

			for (byte i = 0; i < 6; i++)
			{
				ItemHotbar[i] = new MetadataInt();
			}

			ItemHotbar[0] = new MetadataInt(9);
			ItemHotbar[1] = new MetadataInt(10);
			ItemHotbar[2] = new MetadataInt(11);
			ItemHotbar[3] = new MetadataInt(12);
			ItemHotbar[4] = new MetadataInt(13);
			ItemHotbar[5] = new MetadataInt(14);
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

		/// <summary>
		///     Empty the specified slot
		/// </summary>
		/// <param name="slot">The slot to empty.</param>
		public void EmptyInventorySlot(byte slot)
		{
			SetInventorySlot(slot, 0, 0);
		}

		public bool HasItem(MetadataSlot item)
		{
			for (byte i = 0; i < Slots.Count; i++)
			{
				if (((MetadataSlot) Slots[i]).Value.Id == item.Value.Id)
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