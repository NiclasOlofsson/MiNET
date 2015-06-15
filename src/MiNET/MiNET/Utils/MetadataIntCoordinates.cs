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

		public override void FromStream(BinaryReader stream)
		{
			Value = new BlockCoordinates
			{
				X = stream.ReadInt32(),
				Y = stream.ReadInt32(),
				Z = stream.ReadInt32(),
			};
		}

		public override void WriteTo(BinaryWriter stream, byte index)
		{
			stream.Write(GetKey(index));
			stream.Write(Value.X);
			stream.Write(Value.Y);
			stream.Write(Value.Z);
		}

		public override string ToString()
		{
			return string.Format("{0} {1} {2}", FriendlyName, Identifier, Value);
		}
	}
}