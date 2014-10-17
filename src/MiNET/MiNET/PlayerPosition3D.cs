namespace MiNET
{
	public class PlayerPosition3D
	{
		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }

		public float Yaw { get; set; }
		public float Pitch { get; set; }
		public float BodyYaw { get; set; }

		public PlayerPosition3D()
		{
		}

		public PlayerPosition3D(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}
	}
}
