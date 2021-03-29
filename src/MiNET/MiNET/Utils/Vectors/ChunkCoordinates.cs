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

namespace MiNET.Utils.Vectors
{
	public struct ChunkCoordinates : IEquatable<ChunkCoordinates>
	{
		public int X, Z;

		public ChunkCoordinates(int value)
		{
			X = Z = value;
		}

		public ChunkCoordinates(int x, int z)
		{
			X = x;
			Z = z;
		}

		public ChunkCoordinates(ChunkCoordinates v)
		{
			X = v.X;
			Z = v.Z;
		}

		public ChunkCoordinates(BlockCoordinates coordinates)
		{
			X = coordinates.X >> 4;
			Z = coordinates.Z >> 4;
		}

		public ChunkCoordinates(PlayerLocation location)
		{
			X = ((int) Math.Floor(location.X)) >> 4;
			Z = ((int) Math.Floor(location.Z)) >> 4;
		}

		/// <summary>
		///     Converts this ChunkCoordinates to a string in the format &lt;x, z&gt;.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return $"X={X}, Z={Z}";
		}

		#region Math

		/// <summary>
		///     Calculates the distance between two ChunkCoordinates objects.
		/// </summary>
		public double DistanceTo(ChunkCoordinates other)
		{
			return Math.Sqrt(Square(other.X - X) +
							Square(other.Z - Z));
		}

		/// <summary>
		///     Calculates the square of a num.
		/// </summary>
		private int Square(int num)
		{
			return num * num;
		}

		/// <summary>
		///     Finds the distance of this ChunkCoordinates from ChunkCoordinates.Zero
		/// </summary>
		public double Distance
		{
			get { return DistanceTo(Zero); }
		}

		public static ChunkCoordinates Min(ChunkCoordinates value1, ChunkCoordinates value2)
		{
			return new ChunkCoordinates(
				Math.Min(value1.X, value2.X),
				Math.Min(value1.Z, value2.Z)
			);
		}

		public static ChunkCoordinates Max(ChunkCoordinates value1, ChunkCoordinates value2)
		{
			return new ChunkCoordinates(
				Math.Max(value1.X, value2.X),
				Math.Max(value1.Z, value2.Z)
			);
		}

		#endregion

		#region Operators

		public static bool operator !=(ChunkCoordinates a, ChunkCoordinates b)
		{
			return !a.Equals(b);
		}

		public static bool operator ==(ChunkCoordinates a, ChunkCoordinates b)
		{
			return a.Equals(b);
		}

		public static ChunkCoordinates operator +(ChunkCoordinates a, ChunkCoordinates b)
		{
			return new ChunkCoordinates(a.X + b.X, a.Z + b.Z);
		}

		public static ChunkCoordinates operator -(ChunkCoordinates a, ChunkCoordinates b)
		{
			return new ChunkCoordinates(a.X - b.X, a.Z - b.Z);
		}

		public static ChunkCoordinates operator -(ChunkCoordinates a)
		{
			return new ChunkCoordinates(
				-a.X,
				-a.Z);
		}

		public static ChunkCoordinates operator *(ChunkCoordinates a, ChunkCoordinates b)
		{
			return new ChunkCoordinates(a.X * b.X, a.Z * b.Z);
		}

		public static ChunkCoordinates operator /(ChunkCoordinates a, ChunkCoordinates b)
		{
			return new ChunkCoordinates(a.X / b.X, a.Z / b.Z);
		}

		public static ChunkCoordinates operator %(ChunkCoordinates a, ChunkCoordinates b)
		{
			return new ChunkCoordinates(a.X % b.X, a.Z % b.Z);
		}

		public static ChunkCoordinates operator +(ChunkCoordinates a, int b)
		{
			return new ChunkCoordinates(a.X + b, a.Z + b);
		}

		public static ChunkCoordinates operator -(ChunkCoordinates a, int b)
		{
			return new ChunkCoordinates(a.X - b, a.Z - b);
		}

		public static ChunkCoordinates operator *(ChunkCoordinates a, int b)
		{
			return new ChunkCoordinates(a.X * b, a.Z * b);
		}

		public static ChunkCoordinates operator /(ChunkCoordinates a, int b)
		{
			return new ChunkCoordinates(a.X / b, a.Z / b);
		}

		public static ChunkCoordinates operator %(ChunkCoordinates a, int b)
		{
			return new ChunkCoordinates(a.X % b, a.Z % b);
		}

		public static ChunkCoordinates operator +(int a, ChunkCoordinates b)
		{
			return new ChunkCoordinates(a + b.X, a + b.Z);
		}

		public static ChunkCoordinates operator -(int a, ChunkCoordinates b)
		{
			return new ChunkCoordinates(a - b.X, a - b.Z);
		}

		public static ChunkCoordinates operator *(int a, ChunkCoordinates b)
		{
			return new ChunkCoordinates(a * b.X, a * b.Z);
		}

		public static ChunkCoordinates operator /(int a, ChunkCoordinates b)
		{
			return new ChunkCoordinates(a / b.X, a / b.Z);
		}

		public static ChunkCoordinates operator %(int a, ChunkCoordinates b)
		{
			return new ChunkCoordinates(a % b.X, a % b.Z);
		}

		public static explicit operator ChunkCoordinates(BlockCoordinates b)
		{
			return new ChunkCoordinates(b.X >> 4, b.Z >> 4);
		}

		#endregion

		#region Constants

		public static readonly ChunkCoordinates None = new ChunkCoordinates(Int32.MinValue);

		public static readonly ChunkCoordinates Zero = new ChunkCoordinates(0);
		public static readonly ChunkCoordinates One = new ChunkCoordinates(1);

		public static readonly ChunkCoordinates Forward = new ChunkCoordinates(0, 1);
		public static readonly ChunkCoordinates Backward = new ChunkCoordinates(0, -1);
		public static readonly ChunkCoordinates Left = new ChunkCoordinates(-1, 0);
		public static readonly ChunkCoordinates Right = new ChunkCoordinates(1, 0);

		#endregion

		public bool Equals(ChunkCoordinates other)
		{
			return X == other.X && Z == other.Z;
		}

		public override bool Equals(object obj)
		{
			return obj is ChunkCoordinates other && Equals(other);
		}


		public override int GetHashCode()
		{
			return HashCode.Combine(X, Z);
		}
	}
}