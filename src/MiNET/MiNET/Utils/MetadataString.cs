using System.IO;
using System.Text;

namespace MiNET.Utils
{
	public class MetadataString : MetadataEntry
	{
		public override byte Identifier
		{
			get { return 4; }
		}

		public override string FriendlyName
		{
			get { return "string"; }
		}

		public string Value { get; set; }

		public static implicit operator MetadataString(string value)
		{
			return new MetadataString(value);
		}

		public MetadataString()
		{
		}

		public MetadataString(string value)
		{
			Value = value;
		}

		public override void FromStream(BinaryReader stream)
		{
			short len = stream.ReadInt16();
			Value = Encoding.Default.GetString(stream.ReadBytes(len));
		}

		public override void WriteTo(BinaryWriter stream, byte index)
		{
			stream.Write(GetKey(index));

			byte[] bytes = Encoding.Default.GetBytes(Value);
			stream.Write((short) bytes.Length);
			stream.Write(bytes);
		}

		public override string ToString()
		{
			return string.Format("{0} {1} {2}", FriendlyName, Identifier, Value);
		}
	}
}