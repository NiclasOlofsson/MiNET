#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE. 
// The License is based on the Mozilla Public License Version 1.1, but Sections 14 
// and 15 have been added to cover use of software over a computer network and 
// provide for limited attribution for the Original Developer. In addition, Exhibit A has 
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Numerics;

namespace MiNET.Utils.Vectors
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

		public PlayerLocation(float x, float y, float z, float headYaw = 0f, float yaw = 0f, float pitch = 0f)
		{
			X = x;
			Y = y;
			Z = z;
			HeadYaw = headYaw;
			Yaw = yaw;
			Pitch = pitch;
		}

		public PlayerLocation(double x, double y, double z, float headYaw = 0f, float yaw = 0f, float pitch = 0f) : this((float) x, (float) y, (float) z, headYaw, yaw, pitch)
		{
		}

		public PlayerLocation(Vector3 vector, float headYaw = 0f, float yaw = 0f, float pitch = 0f) : this(vector.X, vector.Y, vector.Z, headYaw, yaw, pitch)
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
			return num * num;
		}

		public Vector3 ToVector3()
		{
			return new Vector3(X, Y, Z);
		}

		//public Vector3 GetDirection()
		//{
		//	double pitch = Pitch.ToRadians();
		//	double yaw = Yaw.ToRadians();
		//	double y = -Math.Sin(pitch);
		//	double xz = Math.Cos(pitch);
		//	double x = -xz * Math.Sin(yaw);
		//	double z = xz * Math.Cos(yaw);

		//	return new Vector3((float)x, (float)y, (float)z);
		//}

		public Vector3 GetDirection()
		{
			Vector3 vector = new Vector3();
			double pitch = Pitch.ToRadians();
			double yaw = Yaw.ToRadians();
			vector.X = (float) (-Math.Sin(yaw) * Math.Cos(pitch));
			vector.Y = (float) -Math.Sin(pitch);
			vector.Z = (float) (Math.Cos(yaw) * Math.Cos(pitch));
			return vector;
		}

		public Vector3 GetHeadDirection()
		{
			Vector3 vector = new Vector3();

			double pitch = Pitch.ToRadians();
			double yaw = HeadYaw.ToRadians();
			vector.X = (float) (-Math.Sin(yaw) * Math.Cos(pitch));
			vector.Y = (float) -Math.Sin(pitch);
			vector.Z = (float) (Math.Cos(yaw) * Math.Cos(pitch));

			return vector;
		}

		public static PlayerLocation operator +(PlayerLocation b, Vector3 a)
		{
			return new PlayerLocation(
				a.X + b.X,
				a.Y + b.Y,
				a.Z + b.Z,
				b.HeadYaw,
				b.Yaw,
				b.Pitch);
		}

		public static implicit operator Vector2(PlayerLocation a)
		{
			return new Vector2(a.X, a.Z);
		}

		public static implicit operator Vector3(PlayerLocation a)
		{
			return new Vector3(a.X, a.Y, a.Z);
		}

		public static implicit operator PlayerLocation(BlockCoordinates v)
		{
			return new PlayerLocation(v.X, v.Y, v.Z);
		}

		public object Clone()
		{
			return MemberwiseClone();
		}

		public override string ToString()
		{
			return $"X={X}, Y={Y}, Z={Z}, HeadYaw={HeadYaw}, Yaw={Yaw}, Pich={Pitch}";
		}
	}
}