using System;
using System.IO;
using System.Text;
using log4net;

namespace MiNET.Utils
{
	public class MetadataString : MetadataEntry
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (MetadataString));

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

		public override void FromStream(BinaryReader reader)
		{
			try
			{
				var len = VarInt.ReadInt32(reader.BaseStream);

				byte[] bytes = new byte[len];
				reader.BaseStream.Read(bytes, 0, len);
				Value = Encoding.UTF8.GetString(bytes);
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		public override void WriteTo(BinaryWriter stream)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(Value);
			VarInt.WriteInt32(stream.BaseStream, bytes.Length);

			stream.Write(bytes);
		}

		public override string ToString()
		{
			return string.Format("({0}) '{2}'", FriendlyName, Identifier, Value);
		}
	}
}