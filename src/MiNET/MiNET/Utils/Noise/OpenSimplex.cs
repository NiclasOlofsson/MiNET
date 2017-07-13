using System;
using LibNoise;

namespace MiNET.Utils.Noise
{
	public class OpenSimplex : IModule4D, IModule3D, IModule2D
	{
		private const float Stretch_2D = -0.211324865405187F;    //(1/Math.sqrt(2+1)-1)/2;
		private const float Stretch_3D = -1.0F / 6.0F;            //(1/Math.sqrt(3+1)-1)/3;
		private const float Stretch_4D = -0.138196601125011F;    //(1/Math.sqrt(4+1)-1)/4;
		private const float Squish_2D = 0.366025403784439F;      //(Math.sqrt(2+1)-1)/2;
		private const float Squish_3D = 1.0F / 3.0F;              //(Math.sqrt(3+1)-1)/3;
		private const float Squish_4D = 0.309016994374947F;      //(Math.sqrt(4+1)-1)/4;
		private const float Norm_2D = 47;
		private const float Norm_3D = 103;
		private const float Norm_4D = 30;

		private readonly byte[] _perm;
		private readonly byte[] _perm2D;
		private readonly byte[] _perm3D;
		private readonly byte[] _perm4D;

		//Gradients for 2D. They approximate the directions to the
		//vertices of an octagon from the center.
		private static readonly float[] Gradients2D = new float[]
		{
			 5,  2,    2,  5,
			-5,  2,   -2,  5,
			 5, -2,    2, -5,
			-5, -2,   -2, -5,
		};

		//Gradients for 3D. They approximate the directions to the
		//vertices of a rhombicuboctahedron from the center, skewed so
		//that the triangular and square facets can be inscribed inside
		//circles of the same radius.
		private static readonly float[] Gradients3D = new float[]
		{
			-11,  4,  4,     -4,  11,  4,    -4,  4,  11,
			 11,  4,  4,      4,  11,  4,     4,  4,  11,
			-11, -4,  4,     -4, -11,  4,    -4, -4,  11,
			 11, -4,  4,      4, -11,  4,     4, -4,  11,
			-11,  4, -4,     -4,  11, -4,    -4,  4, -11,
			 11,  4, -4,      4,  11, -4,     4,  4, -11,
			-11, -4, -4,     -4, -11, -4,    -4, -4, -11,
			 11, -4, -4,      4, -11, -4,     4, -4, -11,
		};

		//Gradients for 4D. They approximate the directions to the
		//vertices of a disprismatotesseractihexadecachoron from the center,
		//skewed so that the tetrahedral and cubic facets can be inscribed inside
		//spheres of the same radius.
		private static readonly float[] Gradients4D = new float[]
		{
			 3,  1,  1,  1,      1,  3,  1,  1,      1,  1,  3,  1,      1,  1,  1,  3,
			-3,  1,  1,  1,     -1,  3,  1,  1,     -1,  1,  3,  1,     -1,  1,  1,  3,
			 3, -1,  1,  1,      1, -3,  1,  1,      1, -1,  3,  1,      1, -1,  1,  3,
			-3, -1,  1,  1,     -1, -3,  1,  1,     -1, -1,  3,  1,     -1, -1,  1,  3,
			 3,  1, -1,  1,      1,  3, -1,  1,      1,  1, -3,  1,      1,  1, -1,  3,
			-3,  1, -1,  1,     -1,  3, -1,  1,     -1,  1, -3,  1,     -1,  1, -1,  3,
			 3, -1, -1,  1,      1, -3, -1,  1,      1, -1, -3,  1,      1, -1, -1,  3,
			-3, -1, -1,  1,     -1, -3, -1,  1,     -1, -1, -3,  1,     -1, -1, -1,  3,
			 3,  1,  1, -1,      1,  3,  1, -1,      1,  1,  3, -1,      1,  1,  1, -3,
			-3,  1,  1, -1,     -1,  3,  1, -1,     -1,  1,  3, -1,     -1,  1,  1, -3,
			 3, -1,  1, -1,      1, -3,  1, -1,      1, -1,  3, -1,      1, -1,  1, -3,
			-3, -1,  1, -1,     -1, -3,  1, -1,     -1, -1,  3, -1,     -1, -1,  1, -3,
			 3,  1, -1, -1,      1,  3, -1, -1,      1,  1, -3, -1,      1,  1, -1, -3,
			-3,  1, -1, -1,     -1,  3, -1, -1,     -1,  1, -3, -1,     -1,  1, -1, -3,
			 3, -1, -1, -1,      1, -3, -1, -1,      1, -1, -3, -1,      1, -1, -1, -3,
			-3, -1, -1, -1,     -1, -3, -1, -1,     -1, -1, -3, -1,     -1, -1, -1, -3,
		};

		private static readonly Contribution2[][] Contributions2D =
		{
			new Contribution2[] { new Contribution2(1, 1, 0), new Contribution2(1, 0, 1), new Contribution2(2, 1, 1) },
			new Contribution2[] { new Contribution2(1, 1, 0), new Contribution2(1, 0, 1), new Contribution2(0, 0, 0) }
		};

		private static readonly Contribution3[][] Contributions3D =
		{
			new Contribution3[] { new Contribution3(0, 0, 0, 0), new Contribution3(1, 1, 0, 0), new Contribution3(1, 0, 1, 0), new Contribution3(1, 0, 0, 1) },
			new Contribution3[] { new Contribution3(2, 1, 1, 0), new Contribution3(2, 1, 0, 1), new Contribution3(2, 0, 1, 1), new Contribution3(3, 1, 1, 1) },
			new Contribution3[] { new Contribution3(1, 1, 0, 0), new Contribution3(1, 0, 1, 0), new Contribution3(1, 0, 0, 1), new Contribution3(2, 1, 1, 0), new Contribution3(2, 1, 0, 1), new Contribution3(2, 0, 1, 1)}
		};

		private static readonly Contribution4[][] Contributions4D =
		{
			new Contribution4[] { new Contribution4(0, 0, 0, 0, 0), new Contribution4(1, 1, 0, 0, 0), new Contribution4(1, 0, 1, 0, 0), new Contribution4(1, 0, 0, 1, 0), new Contribution4(1, 0, 0, 0, 1) },
			new Contribution4[] { new Contribution4(3, 1, 1, 1, 0), new Contribution4(3, 1, 1, 0, 1), new Contribution4(3, 1, 0, 1, 1), new Contribution4(3, 0, 1, 1, 1), new Contribution4(4, 1, 1, 1, 1) },
			new Contribution4[] { new Contribution4(1, 1, 0, 0, 0), new Contribution4(1, 0, 1, 0, 0), new Contribution4(1, 0, 0, 1, 0), new Contribution4(1, 0, 0, 0, 1), new Contribution4(2, 1, 1, 0, 0), new Contribution4(2, 1, 0, 1, 0), new Contribution4(2, 1, 0, 0, 1), new Contribution4(2, 0, 1, 1, 0), new Contribution4(2, 0, 1, 0, 1), new Contribution4(2, 0, 0, 1, 1) },
			new Contribution4[] { new Contribution4(3, 1, 1, 1, 0), new Contribution4(3, 1, 1, 0, 1), new Contribution4(3, 1, 0, 1, 1), new Contribution4(3, 0, 1, 1, 1), new Contribution4(2, 1, 1, 0, 0), new Contribution4(2, 1, 0, 1, 0), new Contribution4(2, 1, 0, 0, 1), new Contribution4(2, 0, 1, 1, 0), new Contribution4(2, 0, 1, 0, 1), new Contribution4(2, 0, 0, 1, 1) }
		};

		public OpenSimplex()
			: this(DateTime.Now.Ticks)
		{
		}

		//Initializes the class using a permutation array generated from a 64-bit seed.
		//Generates a proper permutation (i.e. doesn't merely perform N successive pair swaps on a base array)
		//Uses a simple 64-bit LCG.
		public OpenSimplex(long seed)
		{
			_perm = new byte[256];
			_perm2D = new byte[256];
			_perm3D = new byte[256];
			_perm4D = new byte[256];
			var source = new byte[256];
			for (int i = 0; i < 256; i++)
			{
				source[i] = (byte)i;
			}
			seed = seed * 6364136223846793005L + 1442695040888963407L;
			seed = seed * 6364136223846793005L + 1442695040888963407L;
			seed = seed * 6364136223846793005L + 1442695040888963407L;
			for (int i = 255; i >= 0; i--)
			{
				seed = seed * 6364136223846793005L + 1442695040888963407L;
				int r = (int)((seed + 31) % (i + 1));
				if (r < 0)
				{
					r += (i + 1);
				}
				_perm[i] = source[r];
				_perm2D[i] = (byte)((_perm[i] % 8) * 2);
				_perm3D[i] = (byte)((_perm[i] % 12) * 3);
				_perm4D[i] = (byte)((_perm[i] % 64) * 4);
				source[r] = source[i];
			}
		}

		public float GetValue(float x, float y)
		{
			//Place input coordinates onto grid.
			var stretchOffset = (x + y) * Stretch_2D;
			var xs = x + stretchOffset;
			var ys = y + stretchOffset;

			//Floor to get grid coordinates of rhombus (stretched square) super-cell origin.
			var xsb = xs > 0 ? (int)xs : (int)xs - 1;
			var ysb = ys > 0 ? (int)ys : (int)ys - 1;

			//Skew out to get actual coordinates of rhombus origin. We'll need these later.
			var squishOffset = (xsb + ysb) * Squish_2D;
			var xb = xsb + squishOffset;
			var yb = ysb + squishOffset;

			//Positions relative to origin point.
			var dx0 = x - xb;
			var dy0 = y - yb;

			//Compute grid coordinates relative to rhombus origin.
			var xins = xs - xsb;
			var yins = ys - ysb;

			//Sum those together to get a value that determines which region we're in.
			var inSum = xins + yins;

			Contribution2[] contributions;
			if (inSum > 1) //We're inside the triangle (2-Simplex) at (0,0)
			{
				contributions = Contributions2D[0];
			}
			else           //We're inside the triangle (2-Simplex) at (1,1)
			{
				contributions = Contributions2D[1];
			}

			float value = 0;
			foreach (var c in contributions)
			{
				var dx = dx0 + c.Dx;
				var dy = dy0 + c.Dy;
				var attn = 2f - dx * dx - dy * dy;
				if (attn > 0f)
				{
					var px = xsb + c.Xsb;
					var py = ysb + c.Ysb;

					var i = _perm2D[(_perm[px & 0xFF] + py) & 0xFF];
					var valuePart = Gradients2D[i] * dx + Gradients2D[i + 1] * dy;

					attn *= attn;
					value += attn * attn * valuePart;
				}
			}

			return value / Norm_2D;
		}

		public float GetValue(float x, float y, float z)
		{
			//Place input coordinates on simplectic honeycomb.
			var stretchOffset = (x + y + z) * Stretch_3D;
			var xs = x + stretchOffset;
			var ys = y + stretchOffset;
			var zs = z + stretchOffset;

			//Floor to get simplectic honeycomb coordinates of rhombohedron (stretched cube) super-cell origin.
			var xsb = xs > 0f ? (int)xs : (int)xs - 1;
			var ysb = ys > 0f ? (int)ys : (int)ys - 1;
			var zsb = zs > 0f ? (int)zs : (int)zs - 1;

			//Skew out to get actual coordinates of rhombohedron origin. We'll need these later.
			var squishOffset = (xsb + ysb + zsb) * Squish_3D;
			var xb = xsb + squishOffset;
			var yb = ysb + squishOffset;
			var zb = zsb + squishOffset;

			//Positions relative to origin point.
			var dx0 = x - xb;
			var dy0 = y - yb;
			var dz0 = z - zb;

			//Compute simplectic honeycomb coordinates relative to rhombohedral origin.
			var xins = xs - xsb;
			var yins = ys - ysb;
			var zins = zs - zsb;

			//Sum those together to get a value that determines which region we're in.
			var inSum = xins + yins + zins;

			Contribution3[] contributions;
			if (inSum <= 1f)      //We're inside the tetrahedron (3-Simplex) at (0,0,0)
			{
				contributions = Contributions3D[0];
			}
			else if (inSum >= 2f) //We're inside the tetrahedron (3-Simplex) at (1,1,1)
			{
				contributions = Contributions3D[1];
			}
			else                 //We're inside the octahedron (Rectified 3-Simplex) in between.
			{
				contributions = Contributions3D[2];
			}

			float value = 0;
			foreach (var c in contributions)
			{
				var dx = dx0 + c.Dx;
				var dy = dy0 + c.Dy;
				var dz = dz0 + c.Dz;
				var attn = 2f - dx * dx - dy * dy - dz * dz;
				if (attn > 0f)
				{
					var px = xsb + c.Xsb;
					var py = ysb + c.Ysb;
					var pz = zsb + c.Zsb;

					var i = _perm3D[(_perm[(_perm[px & 0xFF] + py) & 0xFF] + pz) & 0xFF];
					var valuePart = Gradients3D[i] * dx + Gradients3D[i + 1] * dy + Gradients3D[i + 2] * dz;

					attn *= attn;
					value += attn * attn * valuePart;
				}
			}

			return value / Norm_3D;
		}

		public float GetValue(float x, float y, float z, float w)
		{
			//Place input coordinates on simplectic honeycomb.
			var stretchOffset = (x + y + z + w) * Stretch_4D;
			var xs = x + stretchOffset;
			var ys = y + stretchOffset;
			var zs = z + stretchOffset;
			var ws = w + stretchOffset;

			//Floor to get simplectic honeycomb coordinates of rhombo-hypercube super-cell origin.
			var xsb = xs > 0 ? (int)xs : (int)xs - 1;
			var ysb = ys > 0 ? (int)ys : (int)ys - 1;
			var zsb = zs > 0 ? (int)zs : (int)zs - 1;
			var wsb = ws > 0 ? (int)ws : (int)ws - 1;

			//Skew out to get actual coordinates of stretched rhombo-hypercube origin. We'll need these later.
			var squishOffset = (xsb + ysb + zsb + wsb) * Squish_4D;
			var xb = xsb + squishOffset;
			var yb = ysb + squishOffset;
			var zb = zsb + squishOffset;
			var wb = wsb + squishOffset;

			//Positions relative to origin point.
			var dx0 = x - xb;
			var dy0 = y - yb;
			var dz0 = z - zb;
			var dw0 = w - wb;

			//Compute simplectic honeycomb coordinates relative to rhombo-hypercube origin.
			var xins = xs - xsb;
			var yins = ys - ysb;
			var zins = zs - zsb;
			var wins = ws - wsb;

			//Sum those together to get a value that determines which region we're in.
			var inSum = xins + yins + zins + wins;

			Contribution4[] contributions;
			if (inSum <= 1)      // We're inside the pentachoron (4-Simplex) at (0,0,0,0)
			{
				contributions = Contributions4D[0];
			}
			else if (inSum >= 3) //We're inside the pentachoron (4-Simplex) at (1,1,1,1)
			{
				contributions = Contributions4D[1];
			}
			else if (inSum <= 2) // We're inside the first dispentachoron (Rectified 4-Simplex)
			{
				contributions = Contributions4D[2];
			}
			else                 // We're inside the second dispentachoron (Rectified 4-Simplex)
			{
				contributions = Contributions4D[3];
			}

			float value = 0;
			foreach (var c in contributions)
			{
				var dx = dx0 + c.Dx;
				var dy = dy0 + c.Dy;
				var dz = dz0 + c.Dz;
				var dw = dw0 + c.Dw;
				var attn = 2f - dx * dx - dy * dy - dz * dz - dw * dw;
				if (attn > 0f)
				{
					var px = xsb + c.Xsb;
					var py = ysb + c.Ysb;
					var pz = zsb + c.Zsb;
					var pw = wsb + c.Wsb;

					var i = _perm4D[(_perm[(_perm[(_perm[px & 0xFF] + py) & 0xFF] + pz) & 0xFF] + pw) & 0xFF];
					var valuePart = Gradients4D[i] * dx + Gradients4D[i + 1] * dy + Gradients4D[i + 2] * dz + Gradients4D[i + 3] * dw;

					attn *= attn;
					value += attn * attn * valuePart;
				}
			}

			return value / Norm_4D;
		}

		private class Contribution2
		{
			public readonly float Dx, Dy;
			public readonly int Xsb, Ysb;

			public Contribution2(float multiplier, int xsb, int ysb)
			{
				Dx = -xsb - multiplier * Squish_2D;
				Dy = -ysb - multiplier * Squish_2D;
				this.Xsb = xsb;
				this.Ysb = ysb;
			}
		}

		private class Contribution3
		{
			public readonly float Dx, Dy, Dz;
			public readonly int Xsb, Ysb, Zsb;

			public Contribution3(float multiplier, int xsb, int ysb, int zsb)
			{
				Dx = -xsb - multiplier * Squish_3D;
				Dy = -ysb - multiplier * Squish_3D;
				Dz = -zsb - multiplier * Squish_3D;
				this.Xsb = xsb;
				this.Ysb = ysb;
				this.Zsb = zsb;
			}
		}

		private class Contribution4
		{
			public readonly float Dx, Dy, Dz, Dw;
			public readonly int Xsb, Ysb, Zsb, Wsb;

			public Contribution4(float multiplier, int xsb, int ysb, int zsb, int wsb)
			{
				Dx = -xsb - multiplier * Squish_4D;
				Dy = -ysb - multiplier * Squish_4D;
				Dz = -zsb - multiplier * Squish_4D;
				Dw = -wsb - multiplier * Squish_4D;
				this.Xsb = xsb;
				this.Ysb = ysb;
				this.Zsb = zsb;
				this.Wsb = wsb;
			}
		}
	}
}
