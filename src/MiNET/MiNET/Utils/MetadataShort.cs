using System.IO;

namespace MiNET.Utils
{
	public class MetadataShort : MetadataEntry
	{
		public override byte Identifier
		{
			get { return 1; }
		}

		public override string FriendlyName
		{
			get { return "short"; }
		}

		public short Value { get; set; }

		public static implicit operator MetadataShort(short value)
		{
			return new MetadataShort(value);
		}

		public MetadataShort()
		{
		}

		public MetadataShort(short value)
		{
			Value = value;
		}

		public override void FromStream(BinaryReader reader)
		{
			Value = reader.ReadInt16();
		}

		public override void WriteTo(BinaryWriter stream)
		{
			stream.Write(Value);
		}

		public override string ToString()
		{
			return string.Format("({0}) {2}", FriendlyName, Identifier, Value);
		}

	}
}