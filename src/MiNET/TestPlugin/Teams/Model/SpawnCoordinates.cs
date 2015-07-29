using System.Runtime.Serialization;
using MiNET.Utils;

namespace TestPlugin.Teams.Model
{
	[DataContract]
	public struct SpawnCoordinates
	{
		[DataMember]
		public int X { get; set; }

		[DataMember]
		public int Y { get; set; }

		[DataMember]
		public int Z { get; set; }

		public SpawnCoordinates(int value) : this()
		{
			X = value;
			Y = value;
			Z = value;
		}

		public SpawnCoordinates(int x, int y, int z) : this()
		{
			X = x;
			Y = y;
			Z = z;
		}

		public SpawnCoordinates(BlockCoordinates v) : this()
		{
			X = v.X;
			Y = v.Y;
			Z = v.Z;
		}

		public static implicit operator BlockCoordinates(SpawnCoordinates a)
		{
			return new BlockCoordinates(a.X, a.Y, a.Z);
		}
	}
}