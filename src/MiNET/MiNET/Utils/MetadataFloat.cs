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

		public MetadataFloat(double value)
		{
			Value = (float) value;
		}

		public override void FromStream(BinaryReader reader)
		{
			Value = reader.ReadSingle();
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