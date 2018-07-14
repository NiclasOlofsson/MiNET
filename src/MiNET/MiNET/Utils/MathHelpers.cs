using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

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

		public static float Lerp(float a, float b, float f)
		{
			//f = f*f;
			return a + f * (b - a);
		}

		public static float Saturate(float v)
		{
			return Math.Max(0, Math.Min(1, v));
		}

		public static float Smoothstep(float edge0, float edge1, float v)
		{
			float t = Saturate((v - edge0) / (edge1 - edge0));
			return t * t * (3.0f - (2.0f * t));
		}

		public const float PI = 3.1415926535f;
		public const float HALF_PI = 0.5f * PI;
		public const float DOUBLE_PI = 2.0f * PI;
		public const float TWO_PI_INV = 1.0f / DOUBLE_PI;

		private static readonly float[] MantissaLogs = new float[(int)Math.Pow(2, 23)];
		private const float Base10 = 3.321928F;
		private const float BaseE = 1.442695F;

		[StructLayout(LayoutKind.Explicit)]
		private struct Ieee754
		{
			[FieldOffset(0)] public float Single;
			[FieldOffset(0)] public uint UnsignedBits;
			[FieldOffset(0)] public int SignedBits;

			public uint Sign
			{
				get { return UnsignedBits >> 31; }
			}

			public int Exponent
			{
				get { return (SignedBits >> 23) & 0xFF; }
			}

			public uint Mantissa
			{
				get { return UnsignedBits & 0x007FFFFF; }
			}
		}

		static MathHelpers()
		{
			for (uint i = 0; i < MantissaLogs.Length; i++)
			{
				var n = new Ieee754 { UnsignedBits = i | 0x3F800000 }; //added the implicit 1 leading bit
				MantissaLogs[i] = (float)Math.Log(n.Single, 2);
			}
		}

		public static float Log2(float value)
		{
			if (value == 0F)
				return float.NegativeInfinity;

			var number = new Ieee754 { Single = value };

			if (number.UnsignedBits >> 31 == 1) //NOTE: didn't call Sign property for higher performance
				return float.NaN;

			return (((number.SignedBits >> 23) & 0xFF) - 127) + MantissaLogs[number.UnsignedBits & 0x007FFFFF];
			//NOTE: didn't call Exponent and Mantissa properties for higher performance
		}

		public static float Log10(float value)
		{
			return Log2(value) / Base10;
		}

		public static float Ln(float value)
		{
			return Log2(value) / BaseE;
		}

		public static float Log(float value, float valueBase)
		{
			return Log2(value) / Log2(valueBase);
		}

		public static float Min(float a, float b)
		{
			return a < b ? a : b;
		}

		public static float Max(float a, float b)
		{
			return a > b ? a : b;
		}

		public static float Exp(float x, int e)
		{
			int a = e;
			float b = x;
			float result = 1;
			while (a > 1)
			{
				if ((a & 1) == 0)
				{
					result = result * b;
				}
				b = b * b;
				a >>= 1;
			}

			if (a > 0)
			{
				result = result * b;
			}
			return result;
		}

		public static float Abs(float d)
		{
			return d < 0 ? -d : d;
		}

		public static float Floor(float x)
		{
			if (x > 0) return x;
			return x - 0.9999999999999999f;
		}

		public static float Ceiling(float x)
		{
			if (x < 0) return (int)x;
			return (int)x + 1;
		}

		public static float Sqrt(float x)
		{
			float n = x / 2.0f;
			float lstX = 0.0f;
			while (n != lstX)
			{
				lstX = n;
				n = (n + x / n) / 2.0f;
			}
			return n;
		}

		public static float Fmod(float a, float b)
		{
			return (int)((((a / b) - ((int)(a / b))) * b) + 0.5f);
		}

		public static float RandomPow(float input, float exponent)
		{
			float num, original;
			original = input;
			num = input;

			for (int x = 1; x < exponent; x++) num = num * original;
			return num;
		}

		public static int Pow(int a, int b)
		{
			int x = a >> 32;
			int y = b * (x - 1072632447) + 1072632447;
			return y << 32;
		}

		public static double Pow(double a, double b)
		{
			long tmp = (DoubleToLong(a) >> 32);
			double tmp2 = (b * (tmp - 1072632447) + 1072632447);
			return LongToDouble((long)tmp2 << 32);
		}

		public static float Pow(float a, float b)
		{
			long tmp = (FloatToLong(a) >> 32);
			float tmp2 = (b * (tmp - 1072632447) + 1072632447);
			return LongTofloat((long)tmp2 << 32);
		}

		private static long DoubleToLong(double value)
		{
			return (long)(value * 32D);
		}

		private static double LongToDouble(long value)
		{
			return value / 32D;
		}

		private static long FloatToLong(float value)
		{
			return (long)(value * 32F);
		}

		private static float LongTofloat(long value)
		{
			return value / 32f;
		}

		public static float Sin(float x)
		{
			if (x < -PI)
			{
				x += DOUBLE_PI;
			}
			else if (x > PI)
			{
				x -= DOUBLE_PI;
			}

			float sinus;

			if (x < 0f)
				sinus = 1.27323954f * x + .405284735f * x * x;
			else
				sinus = 1.27323954f * x - 0.405284735f * x * x;

			return sinus;
		}

		public static float Cos(float x)
		{
			if (x < -PI)
			{
				x += DOUBLE_PI;
			}
			else if (x > PI)
			{
				x -= DOUBLE_PI;
			}

			x += HALF_PI;

			float cosinus;

			if (x < 0f)
				cosinus = 1.27323954f * x + 0.405284735f * x * x;
			else
				cosinus = 1.27323954f * x - 0.405284735f * x * x;

			return cosinus;
		}

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

		public static float BilinearLerp(float x, float y, float q11, float q12, float q21, float q22, float x1, float x2,
			float y1, float y2)
		{
			float r1 = Lerp(x, x1, x2, q11, q21);
			float r2 = Lerp(x, x1, x2, q12, q22);

			return Lerp(y, y1, y2, r1, r2);
		}

		public static float TrilinearLerp(float x, float y, float z, float q000, float q001, float q010, float q011,
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

		public static float CubicInterpolate(float p00, float p01, float p02, float p03, float x)
		{
			return p01 + 0.5f * x * (p02 - p00 + x * (2.0f * p00 - 5.0f * p01 + 4.0f * p02 - p03 + x * (3.0f * (p01 - p02) + p03 - p00)));
		}

		public static float BiCubicInterpolate(float p00, float p01, float p02, float p03, float p10, float p11, float p12,
			float p13, float p20, float p21, float p22, float p23, float p30, float p31, float p32, float p33, float x, float y)
		{
			float r0 = CubicInterpolate(p00, p01, p02, p03, y);
			float r1 = CubicInterpolate(p10, p11, p12, p13, y);
			float r2 = CubicInterpolate(p20, p21, p22, p23, y);
			float r3 = CubicInterpolate(p30, p31, p32, p33, y);
			return CubicInterpolate(r0, r1, r2, r3, x);
		}

		public static float BilinearCubic(float x, float y, float q11, float q12, float q21, float q22, float x1, float x2,
			float y1, float y2)
		{
			var xAlpha = (x2 - x) / (x2 - x1);
			var yAlpha = (y2 - y) / (y2 - y1);
			float r1 = CubicInterpolate(q11, q21, q11, q21, xAlpha);
			float r2 = CubicInterpolate(q12, q22, q12, q22, xAlpha);

			return CubicInterpolate(r1, r2, r1, r2, yAlpha);
		}

		public static float TrilinearCubic(float x, float y, float z, float q000, float q001, float q010, float q011,
			float q100, float q101, float q110, float q111, float x1, float x2, float y1, float y2, float z1, float z2)
		{
			var xAlpha = (x2 - x) / (x2 - x1);
			var yAlpha = (y2 - y) / (y2 - y1);
			var zAlpha = (z2 - z) / (z2 - z1);

			float x00 = CubicInterpolate(q000, q100, q000, q100, xAlpha);
			float x10 = CubicInterpolate(q010, q110, q010, q110, xAlpha);
			float x01 = CubicInterpolate(q001, q101, q001, q101, xAlpha);
			float x11 = CubicInterpolate(q011, q111, q011, q111, xAlpha);
			float r0 = CubicInterpolate(x00, x01, x00, x01, yAlpha);
			float r1 = CubicInterpolate(x10, x11, x10, x11, yAlpha);
			return CubicInterpolate(z1, z2, r0, r1, zAlpha);
		}

		///Catmull-rom
		public static float Cmr(float p0, float p1, float p2, float p3, float t)
		{
			float a = 2f * p1;
			float b = p2 - p0;
			float c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
			float d = -p0 + 3f * p1 - 3f * p2 + p3;

			//The cubic polynomial: a + b * t + c * t^2 + d * t^3
			float pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

			return pos;
		}

		public static float BilinearCmr(float x, float y, float q11, float q12, float q21, float q22, float x1,
			float x2, float y1, float y2)
		{
			var xAlpha = (x2 - x) / (x2 - x1);
			var yAlpha = (y2 - y) / (y2 - y1);
			float r1 = Cmr(q11, q21, q11, q21, xAlpha);
			float r2 = Cmr(q12, q22, q12, q22, xAlpha);

			return Cmr(r1, r2, r1, r2, yAlpha);
		}

		public static float TrilinearCmr(float x, float y, float z, float q000, float q001, float q010, float q011,
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
