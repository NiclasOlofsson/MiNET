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

		public override void FromStream(BinaryReader stream)
		{
			Value = stream.ReadInt16();
		}

		public override void WriteTo(BinaryWriter stream, byte index)
		{
			stream.Write(GetKey(index));
			stream.Write(Value);
		}
	}
}