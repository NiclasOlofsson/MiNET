using System.IO;

namespace MiNET.Utils
{
	public class MetadataIntCoordinates : MetadataEntry
	{
		public override byte Identifier
		{
			get { return 6; }
		}

		public override string FriendlyName
		{
			get { return "int coordinates"; }
		}

		public BlockCoordinates Value { get; set; }

		public MetadataIntCoordinates()
		{
		}

		public MetadataIntCoordinates(int x, int y, int z)
		{
			Value = new BlockCoordinates(x, y, z);
		}

		public override void FromStream(BinaryReader reader)
		{
			Stream stream = reader.BaseStream;
			Value = new BlockCoordinates
			{
				X = VarInt.ReadInt32(stream),
				Y = VarInt.ReadInt32(stream),
				Z = VarInt.ReadInt32(stream),
			};
		}

		public override void WriteTo(BinaryWriter reader)
		{
			Stream stream = reader.BaseStream;
			VarInt.WriteInt32(stream, Value.X);
			VarInt.WriteInt32(stream, Value.Y);
			VarInt.WriteInt32(stream, Value.Z);
		}

		public override string ToString()
		{
			return string.Format("({0}) {2}", FriendlyName, Identifier, Value);
		}
	}
}