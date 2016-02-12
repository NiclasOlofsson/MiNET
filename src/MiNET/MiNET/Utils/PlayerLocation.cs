using System;

namespace MiNET.Utils
{
	public class PlayerLocation : ICloneable
	{
		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }

		public float Yaw { get; set; }
		public float Pitch { get; set; }
		public float HeadYaw { get; set; }

		public PlayerLocation()
		{
		}

		public PlayerLocation(double x, double y, double z) : this((float) x, (float) y, (float) z)
		{
		}

		public PlayerLocation(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public PlayerLocation(Vector3 vector) : this(vector.X, vector.Y, vector.Z)
		{
		}

		public BlockCoordinates GetCoordinates3D()
		{
			return new BlockCoordinates((int) X, (int) Y, (int) Z);
		}

		public double DistanceTo(PlayerLocation other)
		{
			return Math.Sqrt(Square(other.X - X) +
			                 Square(other.Y - Y) +
			                 Square(other.Z - Z));
		}

		private double Square(double num)
		{
			return num*num;
		}

		public Vector3 ToVector3()
		{
			return new Vector3(X, Y, Z);
		}

		public Vector3 GetDirection()
		{
			Vector3 vector = new Vector3();

			double pitch = Vector3.ToRadians(Pitch);
			double yaw = Vector3.ToRadians(Yaw);
			vector.X = -Math.Sin(yaw)*Math.Cos(pitch);
			vector.Y = -Math.Sin(pitch);
			vector.Z = Math.Cos(yaw)*Math.Cos(pitch);

			return vector;
		}

		public Vector3 GetHeadDirection()
		{
			Vector3 vector = new Vector3();

			double pitch = Vector3.ToRadians(Pitch);
			double yaw = Vector3.ToRadians(HeadYaw);
			vector.X = -Math.Sin(yaw)*Math.Cos(pitch);
			vector.Y = -Math.Sin(pitch);
			vector.Z = Math.Cos(yaw)*Math.Cos(pitch);

			return vector;
		}

		public object Clone()
		{
			return MemberwiseClone();
		}

		public override string ToString()
		{
			return $"X={X}, Y={Y}, Z={Z}, HeadYaw={HeadYaw}, Yaw={Yaw}, Pich={Pitch}, ";
		}

	}
}