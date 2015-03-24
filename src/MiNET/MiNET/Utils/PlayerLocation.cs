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
		public float BodyYaw { get; set; }

		public PlayerLocation()
		{
		}

		public PlayerLocation(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
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

		public byte[] Export()
		{
			using (MemoryStream stream = new MemoryStream())
			{
				NbtBinaryWriter writer = new NbtBinaryWriter(stream, false);
				writer.Write(X);
				writer.Write(Y);
				writer.Write(Z);
				writer.Write(Yaw);
				writer.Write(Pitch);
				writer.Write(BodyYaw);
				writer.Flush();
				return stream.GetBuffer();
			}
		}

		public void Import(byte[] data)
		{
			using (MemoryStream stream = new MemoryStream(data))
			{
				NbtBinaryReader reader = new NbtBinaryReader(stream, false);
				X = reader.ReadSingle();
				Y = reader.ReadSingle();
				Z = reader.ReadSingle();
				Yaw = reader.ReadSingle();
				Pitch = reader.ReadSingle();
				BodyYaw = reader.ReadSingle();
			}
		}

		public object Clone()
		{
			return MemberwiseClone();
		}
	}
}