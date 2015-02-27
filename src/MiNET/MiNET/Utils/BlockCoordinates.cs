using System;

namespace MiNET.Utils
{
	public struct BlockCoordinates : IEquatable<BlockCoordinates>
	{
		public int X, Y, Z;

		public BlockCoordinates(int value)
		{
			X = Y = Z = value;
		}

		public BlockCoordinates(int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public BlockCoordinates(BlockCoordinates v)
		{
			X = v.X;
			Y = v.Y;
			Z = v.Z;
		}

		/// <summary>
		/// Calculates the distance between two BlockCoordinates objects.
		/// </summary>
		public double DistanceTo(BlockCoordinates other)
		{
			return Math.Sqrt(Square(other.X - X) +
			                 Square(other.Y - Y) +
			                 Square(other.Z - Z));
		}

		/// <summary>
		/// Calculates the square of a num.
		/// </summary>
		private int Square(int num)
		{
			return num*num;
		}

		/// <summary>
		/// Finds the distance of this Coordinate3D from BlockCoordinates.Zero
		/// </summary>
		public double Distance
		{
			get { return DistanceTo(Zero); }
		}

		public static BlockCoordinates Min(BlockCoordinates value1, BlockCoordinates value2)
		{
			return new BlockCoordinates(
				Math.Min(value1.X, value2.X),
				Math.Min(value1.Y, value2.Y),
				Math.Min(value1.Z, value2.Z)
				);
		}

		public static BlockCoordinates Max(BlockCoordinates value1, BlockCoordinates value2)
		{
			return new BlockCoordinates(
				Math.Max(value1.X, value2.X),
				Math.Max(value1.Y, value2.Y),
				Math.Max(value1.Z, value2.Z)
				);
		}

		public static bool operator !=(BlockCoordinates a, BlockCoordinates b)
		{
			return !a.Equals(b);
		}

		public static bool operator ==(BlockCoordinates a, BlockCoordinates b)
		{
			return a.Equals(b);
		}

		public static BlockCoordinates operator +(BlockCoordinates a, BlockCoordinates b)
		{
			return new BlockCoordinates(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
		}

		public static BlockCoordinates operator -(BlockCoordinates a, BlockCoordinates b)
		{
			return new BlockCoordinates(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
		}

		public static BlockCoordinates operator -(BlockCoordinates a)
		{
			return new BlockCoordinates(-a.X, -a.Y, -a.Z);
		}

		public static BlockCoordinates operator *(BlockCoordinates a, BlockCoordinates b)
		{
			return new BlockCoordinates(a.X*b.X, a.Y*b.Y, a.Z*b.Z);
		}

		public static BlockCoordinates operator /(BlockCoordinates a, BlockCoordinates b)
		{
			return new BlockCoordinates(a.X/b.X, a.Y/b.Y, a.Z/b.Z);
		}

		public static BlockCoordinates operator %(BlockCoordinates a, BlockCoordinates b)
		{
			return new BlockCoordinates(a.X%b.X, a.Y%b.Y, a.Z%b.Z);
		}

		public static BlockCoordinates operator +(BlockCoordinates a, int b)
		{
			return new BlockCoordinates(a.X + b, a.Y + b, a.Z + b);
		}

		public static BlockCoordinates operator -(BlockCoordinates a, int b)
		{
			return new BlockCoordinates(a.X - b, a.Y - b, a.Z - b);
		}

		public static BlockCoordinates operator *(BlockCoordinates a, int b)
		{
			return new BlockCoordinates(a.X*b, a.Y*b, a.Z*b);
		}

		public static BlockCoordinates operator /(BlockCoordinates a, int b)
		{
			return new BlockCoordinates(a.X/b, a.Y/b, a.Z/b);
		}

		public static BlockCoordinates operator %(BlockCoordinates a, int b)
		{
			return new BlockCoordinates(a.X%b, a.Y%b, a.Z%b);
		}

		public static BlockCoordinates operator +(int a, BlockCoordinates b)
		{
			return new BlockCoordinates(a + b.X, a + b.Y, a + b.Z);
		}

		public static BlockCoordinates operator -(int a, BlockCoordinates b)
		{
			return new BlockCoordinates(a - b.X, a - b.Y, a - b.Z);
		}

		public static BlockCoordinates operator *(int a, BlockCoordinates b)
		{
			return new BlockCoordinates(a*b.X, a*b.Y, a*b.Z);
		}

		public static BlockCoordinates operator /(int a, BlockCoordinates b)
		{
			return new BlockCoordinates(a/b.X, a/b.Y, a/b.Z);
		}

		public static BlockCoordinates operator %(int a, BlockCoordinates b)
		{
			return new BlockCoordinates(a%b.X, a%b.Y, a%b.Z);
		}

		public static explicit operator BlockCoordinates(ChunkCoordinates a)
		{
			return new BlockCoordinates(a.X, 0, a.Z);
		}

		public static implicit operator BlockCoordinates(Vector3 a)
		{
			return new BlockCoordinates((int) a.X, (int) a.Y, (int) a.Z);
		}

		public static implicit operator Vector3(BlockCoordinates a)
		{
			return new Vector3(a.X, a.Y, a.Z);
		}

		public static readonly BlockCoordinates Zero = new BlockCoordinates(0);
		public static readonly BlockCoordinates One = new BlockCoordinates(1);

		public static readonly BlockCoordinates Up = new BlockCoordinates(0, 1, 0);
		public static readonly BlockCoordinates Down = new BlockCoordinates(0, -1, 0);
		public static readonly BlockCoordinates Left = new BlockCoordinates(-1, 0, 0);
		public static readonly BlockCoordinates Right = new BlockCoordinates(1, 0, 0);
		public static readonly BlockCoordinates Backwards = new BlockCoordinates(0, 0, -1);
		public static readonly BlockCoordinates Forwards = new BlockCoordinates(0, 0, 1);

		public static readonly BlockCoordinates East = new BlockCoordinates(1, 0, 0);
		public static readonly BlockCoordinates West = new BlockCoordinates(-1, 0, 0);
		public static readonly BlockCoordinates North = new BlockCoordinates(0, 0, -1);
		public static readonly BlockCoordinates South = new BlockCoordinates(0, 0, 1);

		public bool Equals(BlockCoordinates other)
		{
			return X == other.X && Y == other.Y && Z == other.Z;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is BlockCoordinates && Equals((BlockCoordinates) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = X;
				hashCode = (hashCode*397) ^ Y;
				hashCode = (hashCode*397) ^ Z;
				return hashCode;
			}
		}

		//public override int GetHashCode()
		//{
		//	unchecked
		//	{
		//		int result = X.GetHashCode();
		//		result = (result*397) ^ Y.GetHashCode();
		//		result = (result*397) ^ Z.GetHashCode();
		//		return result;
		//	}
		//}
	}
}