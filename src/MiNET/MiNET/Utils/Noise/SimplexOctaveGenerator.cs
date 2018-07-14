using System;
using System.Collections.Generic;
using System.Text;

namespace MiNET.Utils.Noise
{
	public class SimplexOctaveGenerator
	{
		private readonly long _seed;
		private readonly int _octaves;
		private SimplexPerlin[] _generators;

		public SimplexOctaveGenerator(int seed, int octaves)
		{
			_seed = seed;
			_octaves = octaves;

			_generators = new SimplexPerlin[octaves];
			for (int i = 0; i < _generators.Length; i++)
			{
				_generators[i] = new SimplexPerlin(seed);
			}
		}


		public double Noise(double x, double y, double frequency, double amplitude, bool normalized)
		{
			double result = 0;
			double amp = 1;
			double freq = 1;
			double max = 0;

			x *= XScale;
			y *= YScale;

			foreach (var octave in _generators)
			{
				result += octave.GetValue((float)(x * freq), (float)(y * freq)) * amp;
				max += amp;
				freq *= frequency;
				amp *= amplitude;
			}

			if (normalized)
			{
				result /= max;
			}

			return result;
		}

		public double Noise(double x, double y, double z, double frequency, double amplitude, bool normalized)
		{
			double result = 0;
			double amp = 1;
			double freq = 1;
			double max = 0;

			x *= XScale;
			y *= YScale;
			z *= ZScale;

			foreach (var octave in _generators)
			{
				result += octave.GetValue((float)(x * freq), (float)(y * freq), (float)(z * freq)) * amp;
				max += amp;
				freq *= frequency;
				amp *= amplitude;
			}

			if (normalized)
			{
				result /= max;
			}

			return result;
		}

		public double Noise(double x, double y, double z, double w, double frequency, double amplitude)
		{
			return Noise(x, y, z, w, frequency, amplitude, false);
		}

		public double Noise(double x, double y, double z, double w, double frequency, double amplitude, bool normalized)
		{
			double result = 0;
			double amp = 1;
			double freq = 1;
			double max = 0;

			x *= XScale;
			y *= YScale;
			z *= ZScale;
			w *= WScale;

			foreach (var octave in _generators)
			{
				result += octave.GetValue((float)(x * freq), (float)(y * freq), (float)(z * freq), (float)(w * freq)) * amp;
				max += amp;
				freq *= frequency;
				amp *= amplitude;
			}

			if (normalized)
			{
				result /= max;
			}

			return result;
		}

		public double XScale { get; set; }
		public double YScale { get; set; }
		public double ZScale { get; set; }
		public double WScale { get; set; }

		public void SetScale(double scale)
		{
			XScale = scale;
			YScale = scale;
			ZScale = scale;
			WScale = scale;
		}
	}
}
