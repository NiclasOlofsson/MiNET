using System;
using System.Numerics;

namespace MiNET.Utils
{
	public static class VectorHelpers
	{
		public static double GetYaw(this Vector3 vector)
		{
			return ToDegrees(Math.Atan2(vector.X, vector.Z));
		}

		public static double GetPitch(this Vector3 vector)
		{
			var distance = Math.Sqrt((vector.X*vector.X) + (vector.Z*vector.Z));
			return ToDegrees(Math.Atan2(vector.Y, distance));
		}

		public static double ToRadians(this float angle)
		{
			return (Math.PI/180.0f)*angle;
		}

		public static double ToDegrees(this double angle)
		{
			return angle*(180.0f/Math.PI);
		}
	}

	//public struct Vector3 : IEquatable<Vector3>
	//{
	//	public double X;
	//	public double Y;
	//	public double Z;

	//	public Vector3(double value)
	//	{
	//		X = Y = Z = value;
	//	}

	//	public Vector3(double x, double y, double z)
	//	{
	//		X = x;
	//		Y = y;
	//		Z = z;
	//	}

	//	public Vector3(Vector3 v)
	//	{
	//		X = v.X;
	//		Y = v.Y;
	//		Z = v.Z;
	//	}

	//	public double GetYaw()
	//	{
	//		return ToDegrees(Math.Atan2(X, Z));
	//	}

	//	public double GetPitch()
	//	{
	//		var distance = Math.Sqrt((X*X) + (Z*Z));
	//		return ToDegrees(Math.Atan2(Y, distance));
	//	}

	//	public static double ToRadians(double angle)
	//	{
	//		return (Math.PI/180)*angle;
	//	}

	//	public static double ToDegrees(double angle)
	//	{
	//		return angle*(180.0/Math.PI);
	//	}

	//	/// <summary>
	//	/// Floors the decimal component of each part of this Vector3.
	//	/// </summary>
	//	public Vector3 Floor()
	//	{
	//		return new Vector3(Math.Floor(X), Math.Floor(Y), Math.Floor(Z));
	//	}

	//	/// <summary>
	//	/// Truncate the decimal component of each part of this Vector3.
	//	/// </summary>
	//	public Vector3 Truncate()
	//	{
	//		return new Vector3(Math.Truncate(X), Math.Truncate(Y), Math.Truncate(Z));
	//	}

	//	public Vector3 Normalize()
	//	{
	//		return new Vector3(X/Distance, Y/Distance, Z/Distance);
	//	}

	//	/// <summary>
	//	/// Calculates the distance between two Vector3 objects.
	//	/// </summary>
	//	public double DistanceTo(Vector3 other)
	//	{
	//		return Math.Sqrt(Square(other.X - X) +
	//		                 Square(other.Y - Y) +
	//		                 Square(other.Z - Z));
	//	}

	//	/// <summary>
	//	/// Calculates the square of a num.
	//	/// </summary>
	//	private double Square(double num)
	//	{
	//		return num*num;
	//	}

	//	/// <summary>
	//	/// Finds the distance of this vector from Vector3.Zero
	//	/// </summary>
	//	public double Distance
	//	{
	//		get { return DistanceTo(Zero); }
	//	}

	//	public static Vector3 Min(Vector3 value1, Vector3 value2)
	//	{
	//		return new Vector3(
	//			Math.Min(value1.X, value2.X),
	//			Math.Min(value1.Y, value2.Y),
	//			Math.Min(value1.Z, value2.Z)
	//			);
	//	}

	//	public static Vector3 Max(Vector3 value1, Vector3 value2)
	//	{
	//		return new Vector3(
	//			Math.Max(value1.X, value2.X),
	//			Math.Max(value1.Y, value2.Y),
	//			Math.Max(value1.Z, value2.Z)
	//			);
	//	}

	//	public static bool operator !=(Vector3 a, Vector3 b)
	//	{
	//		return !a.Equals(b);
	//	}

	//	public static bool operator ==(Vector3 a, Vector3 b)
	//	{
	//		return a.Equals(b);
	//	}

	//	public static Vector3 operator +(Vector3 a, Vector3 b)
	//	{
	//		return new Vector3(
	//			a.X + b.X,
	//			a.Y + b.Y,
	//			a.Z + b.Z);
	//	}

	//	public static Vector3 operator -(Vector3 a, Vector3 b)
	//	{
	//		return new Vector3(
	//			a.X - b.X,
	//			a.Y - b.Y,
	//			a.Z - b.Z);
	//	}

	//	public static Vector3 operator -(Vector3 a)
	//	{
	//		return new Vector3(
	//			-a.X,
	//			-a.Y,
	//			-a.Z);
	//	}

	//	public static Vector3 operator *(Vector3 a, Vector3 b)
	//	{
	//		return new Vector3(
	//			a.X*b.X,
	//			a.Y*b.Y,
	//			a.Z*b.Z);
	//	}

	//	public static Vector3 operator /(Vector3 a, Vector3 b)
	//	{
	//		return new Vector3(
	//			a.X/b.X,
	//			a.Y/b.Y,
	//			a.Z/b.Z);
	//	}

	//	public static Vector3 operator %(Vector3 a, Vector3 b)
	//	{
	//		return new Vector3(a.X%b.X, a.Y%b.Y, a.Z%b.Z);
	//	}

	//	public static Vector3 operator +(Vector3 a, double b)
	//	{
	//		return new Vector3(
	//			a.X + b,
	//			a.Y + b,
	//			a.Z + b);
	//	}

	//	public static Vector3 operator -(Vector3 a, double b)
	//	{
	//		return new Vector3(
	//			a.X - b,
	//			a.Y - b,
	//			a.Z - b);
	//	}

	//	public static Vector3 operator *(Vector3 a, double b)
	//	{
	//		return new Vector3(
	//			a.X*b,
	//			a.Y*b,
	//			a.Z*b);
	//	}

	//	public static Vector3 operator /(Vector3 a, double b)
	//	{
	//		return new Vector3(
	//			a.X/b,
	//			a.Y/b,
	//			a.Z/b);
	//	}

	//	public static Vector3 operator %(Vector3 a, double b)
	//	{
	//		return new Vector3(a.X%b, a.Y%b, a.Y%b);
	//	}

	//	public static Vector3 operator +(double a, Vector3 b)
	//	{
	//		return new Vector3(
	//			a + b.X,
	//			a + b.Y,
	//			a + b.Z);
	//	}

	//	public static Vector3 operator -(double a, Vector3 b)
	//	{
	//		return new Vector3(
	//			a - b.X,
	//			a - b.Y,
	//			a - b.Z);
	//	}

	//	public static Vector3 operator *(double a, Vector3 b)
	//	{
	//		return new Vector3(
	//			a*b.X,
	//			a*b.Y,
	//			a*b.Z);
	//	}

	//	public static Vector3 operator /(double a, Vector3 b)
	//	{
	//		return new Vector3(
	//			a/b.X,
	//			a/b.Y,
	//			a/b.Z);
	//	}

	//	public static Vector3 operator %(double a, Vector3 b)
	//	{
	//		return new Vector3(a%b.X, a%b.Y, a%b.Y);
	//	}

	//	#region Constants

	//	public static readonly Vector3 Zero = new Vector3(0);
	//	public static readonly Vector3 One = new Vector3(1);

	//	public static readonly Vector3 Up = new Vector3(0, 1, 0);
	//	public static readonly Vector3 Down = new Vector3(0, -1, 0);
	//	public static readonly Vector3 Left = new Vector3(-1, 0, 0);
	//	public static readonly Vector3 Right = new Vector3(1, 0, 0);
	//	public static readonly Vector3 Backwards = new Vector3(0, 0, -1);
	//	public static readonly Vector3 Forwards = new Vector3(0, 0, 1);

	//	public static readonly Vector3 East = new Vector3(1, 0, 0);
	//	public static readonly Vector3 West = new Vector3(-1, 0, 0);
	//	public static readonly Vector3 North = new Vector3(0, 0, -1);
	//	public static readonly Vector3 South = new Vector3(0, 0, 1);

	//	#endregion

	//	public bool Equals(Vector3 other)
	//	{
	//		return other.X.Equals(X) && other.Y.Equals(Y) && other.Z.Equals(Z);
	//	}

	//	public override bool Equals(object obj)
	//	{
	//		if (ReferenceEquals(null, obj)) return false;
	//		if (obj.GetType() != typeof (Vector3)) return false;
	//		return Equals((Vector3) obj);
	//	}

	//	public override int GetHashCode()
	//	{
	//		unchecked
	//		{
	//			int result = X.GetHashCode();
	//			result = (result*397) ^ Y.GetHashCode();
	//			result = (result*397) ^ Z.GetHashCode();
	//			return result;
	//		}
	//	}

	//	public override string ToString()
	//	{
	//		return $"X={X}, Y={Y}, Z={Z}";
	//	}
	//}
}