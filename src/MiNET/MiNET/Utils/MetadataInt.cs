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
	}

	public class MetadataSlots : MetadataDictionary
	{
	}
}