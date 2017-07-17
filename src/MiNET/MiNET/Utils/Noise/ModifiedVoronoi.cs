// This file is part of libnoise-dotnet.
//
// libnoise-dotnet is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// libnoise-dotnet is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with libnoise-dotnet.  If not, see <http://www.gnu.org/licenses/>.
// 
// From the original Jason Bevins's Libnoise (http://libnoise.sourceforge.net)
// Modified for 2D voronoi by Kenny van Vulpen

using System;
using LibNoise;

namespace MiNET.Utils.Noise
{
	/// <summary>
	/// Noise module that outputs Voronoi cells.
	///
	///
	/// In mathematics, a <i>Voronoi cell</i> is a region containing all the
	/// points that are closer to a specific <i>seed point</i> than to any
	/// other seed point.  These cells mesh with one another, producing
	/// polygon-like formations.
	///
	/// By default, this noise module randomly places a seed point within
	/// each unit cube.  By modifying the <i>frequency</i> of the seed points,
	/// an application can change the distance between seed points.  The
	/// higher the frequency, the closer together this noise module places
	/// the seed points, which reduces the size of the cells.
	///
	/// This noise module assigns each Voronoi cell with a random constant
	/// value from a coherent-noise function.  The <i>displacement value</i>
	/// controls the range of random values to assign to each cell.  The
	/// range of random values is +/- the displacement value.
	/// The frequency determines the size of the Voronoi cells and the
	/// distance between these cells.
	/// 
	/// To modify the random positions of the seed points, call the SetSeed()
	/// method.
	///
	/// This noise module can optionally add the distance from the nearest
	/// seed to the output value.  To enable this feature, call the
	/// EnableDistance() method.  This causes the points in the Voronoi cells
	/// to increase in value the further away that point is from the nearest
	/// seed point.
	///
	/// Voronoi cells are often used to generate cracked-mud terrain
	/// formations or crystal-like textures
	/// </summary>
	public class Voronoi : FilterModule, IModule3D, IModule2D
	{
		#region Constants

		/// <summary>
		/// Default persistence value for the Voronoi noise module.
		/// </summary>
		public const float DefaultDisplacement = 1.0f;

		#endregion

		#region Fields

		private float _displacement = DefaultDisplacement;

		private bool _distance;

		#endregion

		#region Accessors

		/// <summary>
		/// This noise module assigns each Voronoi cell with a random constant
		/// value from a coherent-noise function.  The <i>displacement
		/// value</i> controls the range of random values to assign to each
		/// cell.  The range of random values is +/- the displacement value.
		/// </summary>
		public float Displacement
		{
			get { return _displacement; }
			set { _displacement = value; }
		}

		/// <summary>
		/// Applying the distance from the nearest seed point to the output
		/// value causes the points in the Voronoi cells to increase in value
		/// the further away that point is from the nearest seed point.
		/// </summary>
		public bool Distance
		{
			get { return _distance; }
			set { _distance = value; }
		}

		#endregion

		#region Ctor/Dtor

		#endregion

		#region IModule3D Members

		/// <summary>
		/// Generates an output value given the coordinates of the specified input value.
		/// </summary>
		/// <param name="x">The input coordinate on the x-axis.</param>
		/// <param name="y">The input coordinate on the y-axis.</param>
		/// <param name="z">The input coordinate on the z-axis.</param>
		/// <returns>The resulting output value.</returns>
		public float GetValue(float x, float y, float z)
		{
			//TODO This method could be more efficient by caching the seed values.
			x *= _frequency;
			y *= _frequency;
			z *= _frequency;

			int xInt = (x > 0.0f ? (int)x : (int)x - 1);
			int yInt = (y > 0.0f ? (int)y : (int)y - 1);
			int zInt = (z > 0.0f ? (int)z : (int)z - 1);

			float minDist = 2147483647.0f;
			float xCandidate = 0.0f;
			float yCandidate = 0.0f;
			float zCandidate = 0.0f;

			// Inside each unit cube, there is a seed point at a random position.  Go
			// through each of the nearby cubes until we find a cube with a seed point
			// that is closest to the specified position.
			for (int zCur = zInt - 2; zCur <= zInt + 2; zCur++)
			{
				for (int yCur = yInt - 2; yCur <= yInt + 2; yCur++)
				{
					for (int xCur = xInt - 2; xCur <= xInt + 2; xCur++)
					{
						// Calculate the position and distance to the seed point inside of
						// this unit cube.
						float xPos = xCur + _source3D.GetValue(xCur, yCur, zCur);
						float yPos = yCur + _source3D.GetValue(xCur, yCur, zCur);
						float zPos = zCur + _source3D.GetValue(xCur, yCur, zCur);

						float xDist = xPos - x;
						float yDist = yPos - y;
						float zDist = zPos - z;
						float dist = xDist * xDist + yDist * yDist + zDist * zDist;

						if (dist < minDist)
						{
							// This seed point is closer to any others found so far, so record
							// this seed point.
							minDist = dist;
							xCandidate = xPos;
							yCandidate = yPos;
							zCandidate = zPos;
						}
					}
				}
			}

			float value;

			if (_distance)
			{
				// Determine the distance to the nearest seed point.
				float xDist = xCandidate - x;
				float yDist = yCandidate - y;
				float zDist = zCandidate - z;
				value = (MathHelpers.Sqrt(xDist * xDist + yDist * yDist + zDist * zDist)
					) * Libnoise.Sqrt3 - 1.0f;
			}
			else
				value = 0.0f;

			// Return the calculated distance with the displacement value applied.
			return value + (_displacement * _source3D.GetValue(
				Libnoise.FastFloor(xCandidate),
				Libnoise.FastFloor(yCandidate),
				Libnoise.FastFloor(zCandidate))
				);
		}

		#endregion

		#region IModule2D Members - Added by Kenny van Vulpen

		public float GetValue(float x, float y)
		{
			x *= _frequency;
			y *= _frequency;

			int xInt = (x > 0.0f ? (int)x : (int)x - 1);
			int yInt = (y > 0.0f ? (int)y : (int)y - 1);

			float minDist = 2147483647.0f;
			float xCandidate = 0.0f;
			float yCandidate = 0.0f;

			// Inside each unit cube, there is a seed point at a random position.  Go
			// through each of the nearby cubes until we find a cube with a seed point
			// that is closest to the specified position.
			for (int yCur = yInt - 2; yCur <= yInt + 2; yCur++)
			{
				for (int xCur = xInt - 2; xCur <= xInt + 2; xCur++)
				{
					// Calculate the position and distance to the seed point inside of
					// this unit cube.
					float xPos = xCur + _source2D.GetValue(xCur, yCur);
					float yPos = yCur + _source2D.GetValue(xCur, yCur);

					float xDist = xPos - x;
					float yDist = yPos - y;
					float dist = xDist * xDist + yDist * yDist;

					if (dist < minDist)
					{
						// This seed point is closer to any others found so far, so record
						// this seed point.
						minDist = dist;
						xCandidate = xPos;
						yCandidate = yPos;
					}
				}
			}

			float value;

			if (_distance)
			{
				// Determine the distance to the nearest seed point.
				float xDist = xCandidate - x;
				float yDist = yCandidate - y;
				value = (MathHelpers.Sqrt(xDist * xDist + yDist * yDist)
					) * Libnoise.Sqrt3 - 1.0f;
			}
			else
				value = 0.0f;

			// Return the calculated distance with the displacement value applied.
			return value + (_displacement * _source2D.GetValue(
				Libnoise.FastFloor(xCandidate),
				Libnoise.FastFloor(yCandidate))
				);
		}

		#endregion
	}
}