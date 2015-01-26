using System.IO;

namespace MiNET.Utils
{
	public class MetadataFloat : MetadataEntry
	{
		public override byte Identifier
		{
			get { return 3; }
		}

		public override string FriendlyName
		{
			get { return "float"; }
		}

		public float Value { get; set; }

		public static implicit operator MetadataFloat(float value)
		{
			return new MetadataFloat(value);
		}

		public MetadataFloat()
		{
		}

		public MetadataFloat(float value)
		{
			Value = value;
		}

		public override void FromStream(BinaryReader stream)
		{
			Value = stream.ReadSingle();
		}

		public override void WriteTo(BinaryWriter stream, byte index)
		{
			stream.Write(GetKey(index));
			stream.Write(Value);
		}
	}
}