using System;
using LibNoise;
using LibNoise.Filter;

namespace MiNET.Utils.Noise
{
	public class ImprovedVoronoi : Voronoi, IModule2D
	{
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
					var off = _source2D.GetValue(xCur, yCur);
					float xPos = xCur + off;//_source2D.GetValue(xCur, yCur);
					float yPos = yCur + off;//_source2D.GetValue(xCur, yCur);

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

			if (Distance)
			{
				// Determine the distance to the nearest seed point.
				float xDist = xCandidate - x;
				float yDist = yCandidate - y;
				value = (MathF.Sqrt(xDist * xDist + yDist * yDist)
					) * Libnoise.Sqrt3 - 1.0f;
			}
			else
				value = 0.0f;

			// Return the calculated distance with the displacement value applied.
			return value + (Displacement * _source2D.GetValue(
				Libnoise.FastFloor(xCandidate),
				Libnoise.FastFloor(yCandidate))
				);
		}

		#endregion
	}
}