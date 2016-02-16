using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MiNET.Utils
{
	/// <summary>
	///     Used to send metadata with entities
	/// </summary>
	public class MetadataDictionary
	{
		private readonly Dictionary<int, MetadataEntry> _entries;

		public MetadataDictionary()
		{
			_entries = new Dictionary<int, MetadataEntry>();
		}

		public int Count
		{
			get { return _entries.Count; }
		}

		public MetadataEntry this[int index]
		{
			get { return _entries[index]; }
			set { _entries[index] = value; }
		}

		public MetadataEntry[] GetValues()
		{
			return _entries.Values.ToArray();
		}

		public bool Contains(byte index)
		{
			return _entries.ContainsKey(index);
		}

		public static MetadataDictionary FromStream(BinaryReader stream)
		{
			MetadataDictionary metadata = new MetadataDictionary();
			while (true)
			{
				byte key = stream.ReadByte();
				if (key == 0x7F) break;

				byte type = (byte) ((key & 0xE0) >> 5);
				byte index = (byte) (key & 0x1F);

				var entry = EntryTypes[type]();
				if (index == 17 && type != 6)
				{
						entry = new MetadataLong { id = type };
				}

				entry.FromStream(stream);
				entry.Index = index;

				metadata[index] = entry;
			}
			return metadata;
		}

		public void WriteTo(BinaryWriter stream)
		{
			foreach (var entry in _entries)
			{
				entry.Value.WriteTo(stream, (byte)entry.Key);
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
			() => new MetadataString(), // 5 - Should be MetadataSlot() but I don't want it :-(
			() => new MetadataIntCoordinates(), // 6
			() => new MetadataLong(), // 7
		};

		public override string ToString()
		{
			StringBuilder sb = null;

			foreach (var entry in _entries)
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