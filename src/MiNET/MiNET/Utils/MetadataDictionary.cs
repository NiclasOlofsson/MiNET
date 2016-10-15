using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using log4net;

namespace MiNET.Utils
{
	/// <summary>
	///     Used to send metadata with entities
	/// </summary>
	public class MetadataDictionary
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (MetadataDictionary));

		public readonly Dictionary<int, MetadataEntry> _entries;

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

		public static MetadataDictionary FromStream(BinaryReader reader)
		{
			Stream stream = reader.BaseStream;
			MetadataDictionary metadata = new MetadataDictionary();

			{
				var count = VarInt.ReadInt32(stream);

				for (int i = 0; i < count; i++)
				{
					int index = VarInt.ReadInt32(stream);
					int type = VarInt.ReadInt32(stream);
					var entry = EntryTypes[type]();

					entry.FromStream(reader);
					entry.Index = (byte) index;

					metadata[index] = entry;
				}
			}

			return metadata;

			return null;
		}

		public void WriteTo(BinaryWriter writer)
		{
			Stream stream = writer.BaseStream;

			VarInt.WriteInt32(stream, _entries.Count);
			foreach (var entry in _entries)
			{
				VarInt.WriteInt32(stream, entry.Key);
				VarInt.WriteInt32(stream, entry.Value.Identifier);
				entry.Value.WriteTo(writer);
			}
		}

		public delegate MetadataEntry CreateEntryInstance();

		public static readonly CreateEntryInstance[] EntryTypes = new CreateEntryInstance[]
		{
			() => new MetadataByte(), // 0
			() => new MetadataShort(), // 1
			() => new MetadataInt(), // 2
			() => new MetadataFloat(), // 3
			() => new MetadataString(), // 4
			() => new MetadataSlot(), // 5 - Should be MetadataSlot() but I don't want it :-(
			() => new MetadataIntCoordinates(), // 6
			() => new MetadataLong(), // 7
			() => new MetadataVector3(), // 8
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

				sb.Append("[" + entry.Key + "]");
				sb.Append(entry.Value.ToString());
			}

			if (sb != null)
				return sb.ToString();

			return string.Empty;
		}

		public byte[] GetBytes()
		{
			var stream = MiNetServer.MemoryStreamManager.GetStream();
			var writer = new BinaryWriter(stream);
			WriteTo(writer);
			writer.Flush();
			return stream.ToArray();
		}
	}
}