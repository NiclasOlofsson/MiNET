using System;
using System.Collections.Generic;
using System.Numerics;

namespace MiNET.Utils
{
	public enum ContainmentType
	{
		Disjoint,
		Contains,
		Intersects
	}

	public struct BoundingBox : IEquatable<BoundingBox>
	{
		public Vector3 Min;
		public Vector3 Max;
		public const int CornerCount = 8;

		public BoundingBox(Vector3 min, Vector3 max)
		{
			Min = min;
			Max = max;
		}

		public BoundingBox(BoundingBox box)
		{
			Min = box.Min;
			Max = box.Max;
		}

		public BoundingBox GetAdjustedBoundingBox()
		{
			float xMin = Math.Min(Min.X, Max.X);
			float yMin = Math.Min(Min.Y, Max.Y);
			float zMin = Math.Min(Min.Z, Max.Z);

			float xMax = Math.Max(Min.X, Max.X);
			float yMax = Math.Max(Min.Y, Max.Y);
			float zMax = Math.Max(Min.Z, Max.Z);

			return new BoundingBox(new Vector3(xMin, yMin, zMin), new Vector3(xMax, yMax, zMax));
		}

		public ContainmentType Contains(BoundingBox box)
		{
			//test if all corner is in the same side of a face by just checking min and max
			if (box.Max.X < Min.X
			    || box.Min.X > Max.X
			    || box.Max.Y < Min.Y
			    || box.Min.Y > Max.Y
			    || box.Max.Z < Min.Z
			    || box.Min.Z > Max.Z)
				return ContainmentType.Disjoint;


			if (box.Min.X >= Min.X
			    && box.Max.X <= Max.X
			    && box.Min.Y >= Min.Y
			    && box.Max.Y <= Max.Y
			    && box.Min.Z >= Min.Z
			    && box.Max.Z <= Max.Z)
				return ContainmentType.Contains;

			return ContainmentType.Intersects;
		}

		public bool Contains(Vector3 vec)
		{
			return Min.X <= vec.X && vec.X <= Max.X &&
			       Min.Y <= vec.Y && vec.Y <= Max.Y &&
			       Min.Z <= vec.Z && vec.Z <= Max.Z;
		}

		public static BoundingBox CreateFromPoints(IEnumerable<Vector3> points)
		{
			if (points == null)
				throw new ArgumentNullException();

			bool empty = true;
			Vector3 vector2 = new Vector3(float.MaxValue);
			Vector3 vector1 = new Vector3(float.MinValue);
			foreach (Vector3 vector3 in points)
			{
				vector2 = Vector3.Min(vector2, vector3);
				vector1 = Vector3.Max(vector1, vector3);
				empty = false;
			}
			if (empty)
				throw new ArgumentException();

			return new BoundingBox(vector2, vector1);
		}

		public BoundingBox OffsetBy(Vector3 offset)
		{
			return new BoundingBox(Min + offset, Max + offset);
		}

		public Vector3[] GetCorners()
		{
			return new[]
			{
				new Vector3(Min.X, Max.Y, Max.Z),
				new Vector3(Max.X, Max.Y, Max.Z),
				new Vector3(Max.X, Min.Y, Max.Z),
				new Vector3(Min.X, Min.Y, Max.Z),
				new Vector3(Min.X, Max.Y, Min.Z),
				new Vector3(Max.X, Max.Y, Min.Z),
				new Vector3(Max.X, Min.Y, Min.Z),
				new Vector3(Min.X, Min.Y, Min.Z)
			};
		}

		public bool Equals(BoundingBox other)
		{
			return (Min == other.Min) && (Max == other.Max);
		}

		public override bool Equals(object obj)
		{
			return (obj is BoundingBox) && Equals((BoundingBox) obj);
		}

		public override int GetHashCode()
		{
			return Min.GetHashCode() + Max.GetHashCode();
		}

		public bool Intersects(BoundingBox box)
		{
			bool result;
			Intersects(ref box, out result);
			return result;
		}

		public void Intersects(ref BoundingBox box, out bool result)
		{
			if ((Max.X >= box.Min.X) && (Min.X <= box.Max.X))
			{
				if ((Max.Y < box.Min.Y) || (Min.Y > box.Max.Y))
				{
					result = false;
					return;
				}

				result = (Max.Z >= box.Min.Z) && (Min.Z <= box.Max.Z);
				return;
			}

			result = false;
		}

		public static BoundingBox operator +(BoundingBox a, float b)
		{
			return new BoundingBox(a.Min - new Vector3(b), a.Max + new Vector3(b));
		}

		public static BoundingBox operator -(BoundingBox a, float b)
		{
			return new BoundingBox(a.Min + new Vector3(b), a.Max - new Vector3(b));
		}

		public static bool operator ==(BoundingBox a, BoundingBox b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(BoundingBox a, BoundingBox b)
		{
			return !a.Equals(b);
		}

		public override string ToString()
		{
			return string.Format("{{Min:{0} Max:{1}}}", Min.ToString(), Max.ToString());
		}

		public double Height
		{
			get { return Max.Y - Min.Y; }
		}

		public double Width
		{
			get { return Max.X - Min.X; }
		}

		public double Depth
		{
			get { return Max.Z - Min.Z; }
		}
	}
}