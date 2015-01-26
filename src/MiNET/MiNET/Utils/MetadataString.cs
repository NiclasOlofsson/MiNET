using System;
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
			if (value.Length > 16)
				throw new ArgumentOutOfRangeException("value", "Maximum string length is 16 characters");
			while (value.Length < 16)
				value = value + "\0";
			Value = value;
		}

		public override void FromStream(BinaryReader stream)
		{
			Value = stream.ReadString();
		}

		public override void WriteTo(BinaryWriter stream, byte index)
		{
			stream.Write(GetKey(index));

			byte[] bytes = Encoding.UTF8.GetBytes(Value);
			stream.Write((short) bytes.Length);
			stream.Write(bytes);
		}
	}
}