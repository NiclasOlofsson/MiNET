using System.IO;
using System.Numerics;

namespace MiNET.Utils
{
	public class MetadataVector3 : MetadataEntry
	{
		public override byte Identifier
		{
			get { return 8; }
		}

		public override string FriendlyName
		{
			get { return "vector3"; }
		}

		public Vector3 Value { get; set; }

		public MetadataVector3()
		{
		}

		public MetadataVector3(float x, float y, float z)
		{
			Value = new Vector3(x, y, z);
		}

		public override void FromStream(BinaryReader reader)
		{
			Value = new Vector3
			{
				X = reader.ReadSingle(),
				Y = reader.ReadSingle(),
				Z = reader.ReadSingle(),
			};
		}

		public override void WriteTo(BinaryWriter reader)
		{
			reader.Write(Value.X);
			reader.Write(Value.Y);
			reader.Write(Value.Z);
		}

		public override string ToString()
		{
			return string.Format("({0}) {2}", FriendlyName, Identifier, Value);
		}
	}
}