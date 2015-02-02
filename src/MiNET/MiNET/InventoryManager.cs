using MiNET.Utils;
using System.IO;

namespace MiNET
{
	public class InventoryManager
	{
		public MetadataSlots Armor { get; private set; }
		public MetadataSlots Slots { get; private set; }
		public MetadataInts ItemHotbar { get; private set; }
		public MetadataSlot ItemInHand { get; private set; }

		public InventoryManager()
		{
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