using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MiNET.Utils
{
	/// <summary>
	///     Used to send metadata with entities
	/// </summary>
	public class MetadataDictionary
	{
		private readonly Dictionary<byte, MetadataEntry> entries;

		public MetadataDictionary()
		{
			entries = new Dictionary<byte, MetadataEntry>();
		}

		public int Count
		{
			get { return entries.Count; }
		}

		public MetadataEntry this[byte index]
		{
			get { return entries[index]; }
			set { entries[index] = value; }
		}

		public static MetadataDictionary FromStream(BinaryReader stream)
		{
			var value = new MetadataDictionary();
			while (true)
			{
				byte key = stream.ReadByte();
				if (key == 127) break;

				byte type = (byte) ((key & 0xE0) >> 5);
				byte index = (byte) (key & 0x1F);

				var entry = EntryTypes[type]();
				if (index == 17)
				{
					entry = new MetadataLong {id = type};
				}

				entry.FromStream(stream);
				entry.Index = index;

				value[index] = entry;
			}
			return value;
		}

		public void WriteTo(BinaryWriter stream)
		{
			foreach (var entry in entries)
			{
				entry.Value.WriteTo(stream, entry.Key);
			}
			stream.Write((byte) 0x7F);
		}

		public delegate MetadataEntry CreateEntryInstance();

		public static readonly CreateEntryInstance[] EntryTypes = new CreateEntryInstance[]
		{
			() => new MetadataByte(), // 0
			() => new MetadataShort(), // 1
			() => new MetadataInt(), // 2
			() => new MetadataFloat(), // 3
			() => new MetadataString(), // 4
			() => new MetadataSlot(), // 5
			() => new MetadataIntCoordinates(), // 6
			() => new MetadataLong(), // 7
			() => new MetadataLong(), // 8
		};

		public override string ToString()
		{
			StringBuilder sb = null;

			foreach (var entry in entries)
			{
				if (sb != null)
					sb.Append(", ");
				else
					sb = new StringBuilder();

				sb.Append("[" + entry.Key + "] ");
				sb.Append(entry.Value.ToString());
			}

			if (sb != null)
				return sb.ToString();

			return string.Empty;
		}

		public byte[] GetBytes()
		{
			var stream = new MemoryStream();
			var writer = new BinaryWriter(stream);
			WriteTo(writer);
			writer.Flush();
			return stream.ToArray();
		}
	}
}