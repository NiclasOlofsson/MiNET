using System;

namespace MiNET.Utils
{
	public static class MathHelpers
	{
		public static float Normalize(this float x, float oldMax, float oldMin, float newMax, float newMin)
		{
			if (newMax < newMin)
			{
				var min = newMin;
				var max = newMax;
				newMin = max;
				newMax = min;
			}

			if (oldMax < oldMin)
			{
				var oMin = oldMin;
				oldMin = oldMax;
				oldMax = oMin;
			}

			var scale = (oldMax - oldMin) / (newMax - newMin);
			return newMin + (x - oldMin) / scale;

			//x = (x - oldMin)*(newMax - newMin)/(oldMax - oldMin) + newMin;
			//return x;
		}

		public static float Normalize(this float x, float newMin, float newMax)
		{
			return (((x - newMin) / newMax) * newMax);
		}

		public static float Lerp(float a, float b, float f)
		{
			return a + f*(b - a);
		}

		public static float Saturate(float v)
		{
			return Math.Max(0, Math.Min(1, v));
		}

		public static float Smoothstep(float edge0, float edge1, float v)
		{
			float t = Saturate((v - edge0)/(edge1 - edge0));
			return t*t*(3.0f - (2.0f*t));
		}
	}
}
