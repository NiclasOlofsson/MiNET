using System.IO;
using Craft.Net.Common;
using MiNET.Utils;

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

		public Coordinates3D GetCoordinates3D()
		{
			return new Coordinates3D((int)X,(int)Y,(int)Z);
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
	}
}
