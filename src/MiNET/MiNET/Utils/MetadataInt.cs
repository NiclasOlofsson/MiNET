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

		public override void FromStream(BinaryReader reader)
		{
			Value = VarInt.ReadSInt32(reader.BaseStream);
		}

		public override void WriteTo(BinaryWriter stream)
		{
			VarInt.WriteSInt32(stream.BaseStream, Value);
		}

		public override string ToString()
		{
			return string.Format("({0}) {2}", FriendlyName, Identifier, Value);
		}
	}
}