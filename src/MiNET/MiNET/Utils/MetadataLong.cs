using System.IO;

namespace MiNET.Utils
{
	public class MetadataLong : MetadataEntry
	{
		public byte id = 7;

		public override byte Identifier
		{
			get { return id; }
		}

		public override string FriendlyName
		{
			get { return "long"; }
		}

		public long Value { get; set; }

		public static implicit operator MetadataLong(long value)
		{
			return new MetadataLong(value);
		}

		public MetadataLong()
		{
		}

		public MetadataLong(long value)
		{
			Value = value;
		}

		public override void FromStream(BinaryReader reader)
		{
			Value = VarInt.ReadSInt64(reader.BaseStream);
		}

		public override void WriteTo(BinaryWriter stream)
		{
			VarInt.WriteSInt64(stream.BaseStream, Value);
		}

		public override string ToString()
		{
			return string.Format("({0}) {2}", FriendlyName, Identifier, Value);
		}
	}
}