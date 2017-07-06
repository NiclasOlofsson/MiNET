using System;

namespace MiNET.Utils
{
	public static class Interpolation
	{
		/// <summary>
		/// Trilinear Interpolation
		/// </summary>
		/// <param name="method"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="q000"></param>
		/// <param name="q001"></param>
		/// <param name="q010"></param>
		/// <param name="q011"></param>
		/// <param name="q100"></param>
		/// <param name="q101"></param>
		/// <param name="q110"></param>
		/// <param name="q111"></param>
		/// <param name="x1"></param>
		/// <param name="x2"></param>
		/// <param name="y1"></param>
		/// <param name="y2"></param>
		/// <param name="z1"></param>
		/// <param name="z2"></param>
		/// <returns></returns>
		public static float Interpolate(InterpolationMethod method, float x, float y, float z, float q000, float q001,
			float q010, float q011, float q100, float q101, float q110, float q111, float x1, float x2, float y1, float y2,
			float z1, float z2)
		{
			switch (method)
			{
				case InterpolationMethod.Linear:
					return TrilinearLerp(x, y, z, q000, q001, q010, q011, q100, q101, q110, q111, x1, x2, y1, y2, z1, z2);
				case InterpolationMethod.Cubic:
					return TrilinearCubic(x, y, z, q000, q001, q010, q011, q100, q101, q110, q111, x1, x2, y1, y2, z1, z2);
				case InterpolationMethod.CatmullRom:
					return TrilinearCmr(x, y, z, q000, q001, q010, q011, q100, q101, q110, q111, x1, x2, y1, y2, z1, z2);
				default:
					throw new ArgumentOutOfRangeException(nameof(method), method, null);
			}
		}

		/// <summary>
		/// Bilinear Interpolation
		/// </summary>
		/// <param name="method"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="q11"></param>
		/// <param name="q12"></param>
		/// <param name="q21"></param>
		/// <param name="q22"></param>
		/// <param name="x1"></param>
		/// <param name="x2"></param>
		/// <param name="y1"></param>
		/// <param name="y2"></param>
		/// <returns></returns>
		public static float Interpolate(InterpolationMethod method, float x, float y, float q11, float q12, float q21,
			float q22, float x1, float x2,
			float y1, float y2)
		{
			switch (method)
			{
				case InterpolationMethod.Linear:
					return BilinearLerp(x, y, q11, q12, q21, q22, x1, x2, y1, y2);
				case InterpolationMethod.Cubic:
					return BilinearCubic(x, y, q11, q12, q21, q22, x1, x2, y1, y2);
				case InterpolationMethod.CatmullRom:
					return BilinearCmr(x, y, q11, q12, q21, q22, x1, x2, y1, y2);
				default:
					throw new ArgumentOutOfRangeException(nameof(method), method, null);
			}
		}

		private static float Lerp(float t, float x1, float x2, float q00, float q01)
		{
			return ((x2 - t) / (x2 - x1)) * q00 + ((t - x1) / (x2 - x1)) * q01;
		}

		private static float BilinearLerp(float x, float y, float q11, float q12, float q21, float q22, float x1, float x2,
		float y1, float y2)
		{
			float r1 = Lerp(x, x1, x2, q11, q21);
			float r2 = Lerp(x, x1, x2, q12, q22);

			return Lerp(y, y1, y2, r1, r2);
		}

		private static float TrilinearLerp(float x, float y, float z, float q000, float q001, float q010, float q011,
			float q100, float q101, float q110, float q111, float x1, float x2, float y1, float y2, float z1, float z2)
		{
			float x00 = Lerp(x, x1, x2, q000, q100);
			float x10 = Lerp(x, x1, x2, q010, q110);
			float x01 = Lerp(x, x1, x2, q001, q101);
			float x11 = Lerp(x, x1, x2, q011, q111);
			float r0 = Lerp(y, y1, y2, x00, x01);
			float r1 = Lerp(y, y1, y2, x10, x11);
			return Lerp(z, z1, z2, r0, r1);
		}

		private static float Cubic(float n0, float n1, float n2, float n3, float a)
		{
			float num1 = n3 - n2 - (n0 - n1);
			float num2 = n0 - n1 - num1;
			float num3 = n2 - n0;
			float num4 = n1;
			return (num1 * a * a * a + num2 * a * a + num3 * a) + num4;
		}

		private static float BilinearCubic(float x, float y, float q11, float q12, float q21, float q22, float x1, float x2,
	float y1, float y2)
		{
			var xAlpha = (x2 - x) / (x2 - x1);
			var yAlpha = (y2 - y) / (y2 - y1);
			float r1 = Cubic(q11, q21, q11, q21, xAlpha);
			float r2 = Cubic(q12, q22, q12, q22, xAlpha);

			return Cubic(r1, r2, r1, r2, yAlpha);
		}

		private static float TrilinearCubic(float x, float y, float z, float q000, float q001, float q010, float q011,
			float q100, float q101, float q110, float q111, float x1, float x2, float y1, float y2, float z1, float z2)
		{
			var xAlpha = (x2 - x) / (x2 - x1);
			var yAlpha = (y2 - y) / (y2 - y1);
			var zAlpha = (z2 - z) / (z2 - z1);

			float x00 = Cubic(q000, q100, q000, q100, xAlpha);
			float x10 = Cubic(q010, q110, q010, q110, xAlpha);
			float x01 = Cubic(q001, q101, q001, q101, xAlpha);
			float x11 = Cubic(q011, q111, q011, q111, xAlpha);
			float r0 = Cubic(x00, x01, x00, x01, yAlpha);
			float r1 = Cubic(x10, x11, x10, x11, yAlpha);
			return Cubic(z1, z2, r0, r1, zAlpha);
		}

		///Catmull-rom
		private static float Cmr(float p0, float p1, float p2, float p3, float t)
		{
			float a = 2f * p1;
			float b = p2 - p0;
			float c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
			float d = -p0 + 3f * p1 - 3f * p2 + p3;

			//The cubic polynomial: a + b * t + c * t^2 + d * t^3
			float pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

			return pos;
		}

		private static float BilinearCmr(float x, float y, float q11, float q12, float q21, float q22, float x1,
			float x2, float y1, float y2)
		{
			var xAlpha = (x2 - x) / (x2 - x1);
			var yAlpha = (y2 - y) / (y2 - y1);
			float r1 = Cmr(q11, q21, q11, q21, xAlpha);
			float r2 = Cmr(q12, q22, q12, q22, xAlpha);

			return Cmr(r1, r2, r1, r2, yAlpha);
		}

		private static float TrilinearCmr(float x, float y, float z, float q000, float q001, float q010, float q011,
	float q100, float q101, float q110, float q111, float x1, float x2, float y1, float y2, float z1, float z2)
		{
			var xAlpha = (x2 - x) / (x2 - x1);
			var yAlpha = (y2 - y) / (y2 - y1);
			var zAlpha = (z2 - z) / (z2 - z1);

			float x00 = Cmr(q000, q100, q000, q100, xAlpha);
			float x10 = Cmr(q010, q110, q010, q110, xAlpha);
			float x01 = Cmr(q001, q101, q001, q101, xAlpha);
			float x11 = Cmr(q011, q111, q011, q111, xAlpha);
			float r0 = Cmr(x00, x01, x00, x01, yAlpha);
			float r1 = Cmr(x10, x11, x10, x11, yAlpha);
			return Cmr(z1, z2, r0, r1, zAlpha);
		}

		/*private static float Interpolate(float p0, float p1, float p2, float p3, float t)
		{
			float p = (p3 - p2) - (p0 - p1);

			return t * t * t * p + t * t * ((p0 - p1) - p) + t * (p2 - p0) + p1;
		}

		private static float BilinearInterpolate(float x, float y, float q11, float q12, float q21, float q22, float x1,
	float x2, float y1, float y2)
		{
			var xAlpha = (x2 - x) / (x2 - x1);
			var yAlpha = (y2 - y) / (y2 - y1);
			float r1 = Interpolate(q11, q21, q11, q21, xAlpha);
			float r2 = Interpolate(q12, q22, q12, q22, xAlpha);

			return Interpolate(r1, r2, r1, r2, yAlpha);
		}*/
	}

	public enum InterpolationMethod
	{
		Linear,
		Cubic,
		CatmullRom
	}
}
