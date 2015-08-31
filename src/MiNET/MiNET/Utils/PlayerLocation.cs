using System;
using System.IO;

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

		private byte[] Export()
		{
			using (MemoryStream stream = new MemoryStream())
			{
				NbtBinaryWriter writer = new NbtBinaryWriter(stream, false);
				writer.Write(X);
				writer.Write(Y);
				writer.Write(Z);
				writer.Write(Yaw);
				writer.Write(Pitch);
				writer.Write(HeadYaw);
				writer.Flush();
				return stream.GetBuffer();
			}
		}

		private void Import(byte[] data)
		{
			using (MemoryStream stream = new MemoryStream(data))
			{
				NbtBinaryReader reader = new NbtBinaryReader(stream, false);
				X = reader.ReadSingle();
				Y = reader.ReadSingle();
				Z = reader.ReadSingle();
				Yaw = reader.ReadSingle();
				Pitch = reader.ReadSingle();
				HeadYaw = reader.ReadSingle();
			}
		}

		public object Clone()
		{
			return MemberwiseClone();
		}
	}
}