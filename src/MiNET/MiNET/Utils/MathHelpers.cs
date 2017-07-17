using System;
using System.Runtime.InteropServices;

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
				get
				{
					return UnsignedBits >> 31;
				}
			}

			public int Exponent
			{
				get
				{
					return (SignedBits >> 23) & 0xFF;
				}
			}

			public uint Mantissa
			{
				get
				{
					return UnsignedBits & 0x007FFFFF;
				}
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
			return (long) (value * 32D);
		}

		private static double LongToDouble(long value)
		{
			return value / 32D;
		}

		private static long FloatToLong(float value)
		{
			return (long) (value * 32F);
		}

		private static float LongTofloat(long value)
		{
			return value / 32f;
		}
	}
}
