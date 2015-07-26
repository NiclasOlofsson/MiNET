using System.Runtime.Serialization;
using MiNET.Utils;

namespace TestPlugin.Teams.Model
{
	[DataContract]
	public struct SpawnCoordinates
	{
		[DataMember]
		public int TeamId { get; set; }

		[DataMember]
		public int X { get; set; }

		[DataMember]
		public int Y { get; set; }

		[DataMember]
		public int Z { get; set; }

		public SpawnCoordinates(int teamId, int value) : this()
		{
			TeamId = teamId;
			X = value;
			Y = value;
			Z = value;
		}

		public SpawnCoordinates(int teamId, int x, int y, int z) : this()
		{
			TeamId = teamId;
			X = x;
			Y = y;
			Z = z;
		}

		public SpawnCoordinates(int teamId, BlockCoordinates v) : this()
		{
			TeamId = teamId;
			X = v.X;
			Y = v.Y;
			Z = v.Z;
		}
	}
}