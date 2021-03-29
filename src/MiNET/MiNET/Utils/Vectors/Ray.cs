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
	public struct Ray : IEquatable<Ray>
	{
		#region Public Fields

		public readonly Vector3 Direction;
		public readonly Vector3 Position;

		#endregion

		#region Public Constructors

		public Ray(Vector3 position, Vector3 direction)
		{
			this.Position = position;
			this.Direction = direction;
		}

		#endregion

		#region Public Methods

		public override bool Equals(object obj)
		{
			return (obj is Ray) && Equals((Ray) obj);
		}


		public bool Equals(Ray other)
		{
			return Position.Equals(other.Position) && Direction.Equals(other.Direction);
		}


		public override int GetHashCode()
		{
			return Position.GetHashCode() ^ Direction.GetHashCode();
		}

		public double? Intersects(BoundingBox box)
		{
			//first test if start in box
			if (Position.X >= box.Min.X
				&& Position.X <= box.Max.X
				&& Position.Y >= box.Min.Y
				&& Position.Y <= box.Max.Y
				&& Position.Z >= box.Min.Z
				&& Position.Z <= box.Max.Z)
				return 0.0f; // here we concidere cube is full and origine is in cube so intersect at origine

			//Second we check each face
			Vector3 maxT = new Vector3(-1.0f);
			//Vector3 minT = new Vector3(-1.0f);
			//calcul intersection with each faces
			if (Position.X < box.Min.X && Direction.X != 0.0f)
				maxT.X = (box.Min.X - Position.X) / Direction.X;
			else if (Position.X > box.Max.X && Direction.X != 0.0f)
				maxT.X = (box.Max.X - Position.X) / Direction.X;
			if (Position.Y < box.Min.Y && Direction.Y != 0.0f)
				maxT.Y = (box.Min.Y - Position.Y) / Direction.Y;
			else if (Position.Y > box.Max.Y && Direction.Y != 0.0f)
				maxT.Y = (box.Max.Y - Position.Y) / Direction.Y;
			if (Position.Z < box.Min.Z && Direction.Z != 0.0f)
				maxT.Z = (box.Min.Z - Position.Z) / Direction.Z;
			else if (Position.Z > box.Max.Z && Direction.Z != 0.0f)
				maxT.Z = (box.Max.Z - Position.Z) / Direction.Z;

			//get the maximum maxT
			if (maxT.X > maxT.Y && maxT.X > maxT.Z)
			{
				if (maxT.X < 0.0f)
					return null; // ray go on opposite of face
				//coordonate of hit point of face of cube
				double coord = Position.Z + maxT.X * Direction.Z;
				// if hit point coord ( intersect face with ray) is out of other plane coord it miss
				if (coord < box.Min.Z || coord > box.Max.Z)
					return null;
				coord = Position.Y + maxT.X * Direction.Y;
				if (coord < box.Min.Y || coord > box.Max.Y)
					return null;
				return maxT.X;
			}
			if (maxT.Y > maxT.X && maxT.Y > maxT.Z)
			{
				if (maxT.Y < 0.0f)
					return null; // ray go on opposite of face
				//coordonate of hit point of face of cube
				double coord = Position.Z + maxT.Y * Direction.Z;
				// if hit point coord ( intersect face with ray) is out of other plane coord it miss
				if (coord < box.Min.Z || coord > box.Max.Z)
					return null;
				coord = Position.X + maxT.Y * Direction.X;
				if (coord < box.Min.X || coord > box.Max.X)
					return null;
				return maxT.Y;
			}
			else //Z
			{
				if (maxT.Z < 0.0f)
					return null; // ray go on opposite of face
				//coordonate of hit point of face of cube
				double coord = Position.X + maxT.Z * Direction.X;
				// if hit point coord ( intersect face with ray) is out of other plane coord it miss
				if (coord < box.Min.X || coord > box.Max.X)
					return null;
				coord = Position.Y + maxT.Z * Direction.Y;
				if (coord < box.Min.Y || coord > box.Max.Y)
					return null;
				return maxT.Z;
			}
		}

		public static bool operator !=(Ray a, Ray b)
		{
			return !a.Equals(b);
		}

		public static bool operator ==(Ray a, Ray b)
		{
			return a.Equals(b);
		}

		public override string ToString()
		{
			return string.Format("{{Position:{0} Direction:{1}}}", Position.ToString(), Direction.ToString());
		}

		#endregion
	}

	public class Ray2
	{
		/**
     * Epsilon
     */
		public static double EPSILON = 0.5;

		/**
     * Offset
     */
		public static double OFFSET = 0.0001;

		/**
     * Origin of the Ray
     */
		public Vector3 x;

		/**
     * Direction of the Ray
     */
		public Vector3 d;

		/**
     * Normal vector of intersection
     */
		public Vector3 n = new Vector3();

		/**
     * tNear variable
     */
		public double tNear;

		/**
     * t variable / parameter of ray
     */
		public double t;

		/**
     * Texture coordinate
     */
		public double u;

		/**
     * Texture coordinate
     */
		public double v;
	}
}