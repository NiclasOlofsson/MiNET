using System.IO;

namespace MiNET.Utils
{
	public class MetadataInts : MetadataDictionary
	{
		public static MetadataInts FromStream(BinaryReader stream)
		{
			var value = new MetadataInts();
			while (true)
			{
				byte key = stream.ReadByte();
				if (key == 127) break;

				byte type = (byte) ((key & 0xE0) >> 5);
				byte index = (byte) (key & 0x1F);

				var entry = EntryTypes[type]();
				entry.FromStream(stream);
				entry.Index = index;

				value[index] = entry;
			}
			return value;
		}
	}
}