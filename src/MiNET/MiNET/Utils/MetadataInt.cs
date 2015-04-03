using System.IO;

namespace MiNET.Utils
{
	public class MetadataInt : MetadataEntry
	{
		public override byte Identifier
		{
			get { return 2; }
		}

		public override string FriendlyName
		{
			get { return "int"; }
		}

		public int Value { get; set; }

		public static implicit operator MetadataInt(int value)
		{
			return new MetadataInt(value);
		}

		public MetadataInt()
		{
		}

		public MetadataInt(int value)
		{
			Value = value;
		}

		public override void FromStream(BinaryReader stream)
		{
			Value = stream.ReadInt32();
		}

		public override void WriteTo(BinaryWriter stream, byte index)
		{
			stream.Write(GetKey(index));
			stream.Write(Value);
		}

		public override string ToString()
		{
			return string.Format("{0} {1} {2}", FriendlyName, Identifier, Value);
		}
	}

	public class MetadataIntCoordinates : MetadataEntry
	{
		public override byte Identifier
		{
			get { return 6; }
		}

		public override string FriendlyName
		{
			get { return "int coordinates"; }
		}

		public BlockCoordinates Value { get; set; }

		public MetadataIntCoordinates()
		{
		}

		public MetadataIntCoordinates(int x, int y, int z)
		{
			Value = new BlockCoordinates(x, y, z);
		}

		public override void FromStream(BinaryReader stream)
		{
			Value = new BlockCoordinates
			{
				X = stream.ReadInt32(),
				Y = stream.ReadInt32(),
				Z = stream.ReadInt32(),
			};
		}

		public override void WriteTo(BinaryWriter stream, byte index)
		{
			stream.Write(GetKey(index));
			stream.Write(Value.X);
			stream.Write(Value.Y);
			stream.Write(Value.Z);
		}

		public override string ToString()
		{
			return string.Format("{0} {1} {2}", FriendlyName, Identifier, Value);
		}
	}

	public class MetadataSlots : MetadataDictionary
	{
		public static MetadataSlots FromStream(BinaryReader stream)
		{
			var value = new MetadataSlots();
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