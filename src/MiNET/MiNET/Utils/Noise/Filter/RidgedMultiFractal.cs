using System;
using System.Collections.Generic;
using System.Text;

namespace MiNET.Utils.Noise.Filter
{
	public class RidgedMultiFractal : FilterModule, IModule3D, IModule2D
	{
		#region Ctor/Dtor

		/// <summary>
		/// 0-args constructor.
		/// </summary>
		public RidgedMultiFractal()
		{
			_gain = 2.0f;
			_offset = 1.0f;
			_spectralExponent = 0.9f;

			ComputeSpectralWeights();
		}

		#endregion

		#region IModule2D Members

		/// <summary>
		/// Generates an output value given the coordinates of the specified input value.
		/// </summary>
		/// <param name="x">The input coordinate on the x-axis.</param>
		/// <param name="y">The input coordinate on the y-axis.</param>
		/// <returns>The resulting output value.</returns>
		public float GetValue(float x, float y)
		{
			int curOctave;

			x *= _frequency;
			y *= _frequency;

			// Initialize value : 1st octave
			float signal = _source2D.GetValue(x, y);

			// get absolute value of signal (this creates the ridges)
			if (signal < 0.0f)
				signal = -signal;

			// invert and translate (note that "offset" should be ~= 1.0)
			signal = _offset - signal;

			// Square the signal to increase the sharpness of the ridges.
			signal *= signal;

			// Add the signal to the output value.
			float value = signal;

			float weight = 1.0f;

			for (curOctave = 1; weight > 0.001 && curOctave < _octaveCount; curOctave++)
			{
				x *= _lacunarity;
				y *= _lacunarity;

				// Weight successive contributions by the previous signal.
				weight = Libnoise.Clamp01(signal * _gain);

				// Get the coherent-noise value.
				signal = _source2D.GetValue(x, y);

				// Make the ridges.
				if (signal < 0.0)
					signal = -signal;

				signal = _offset - signal;

				// Square the signal to increase the sharpness of the ridges.
				signal *= signal;

				// The weighting from the previous octave is applied to the signal.
				// Larger values have higher weights, producing sharp points along the
				// ridges.
				signal *= weight;

				// Add the signal to the output value.
				value += (signal * _spectralWeights[curOctave]);
			}

			return value;
		}

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
			int curOctave;

			x *= _frequency;
			y *= _frequency;
			z *= _frequency;

			// Initialize value : 1st octave
			float signal = _source3D.GetValue(x, y, z);

			// get absolute value of signal (this creates the ridges)
			if (signal < 0.0)
				signal = -signal;

			// invert and translate (note that "offset" should be ~= 1.0)
			signal = _offset - signal;

			// Square the signal to increase the sharpness of the ridges.
			signal *= signal;

			// Add the signal to the output value.
			float value = signal;

			float weight = 1.0f;

			for (curOctave = 1; weight > 0.001 && curOctave < _octaveCount; curOctave++)
			{
				x *= _lacunarity;
				y *= _lacunarity;
				z *= _lacunarity;

				// Weight successive contributions by the previous signal.
				weight = Libnoise.Clamp01(signal * _gain);

				// Get the coherent-noise value.
				signal = _source3D.GetValue(x, y, z);

				// Make the ridges.
				if (signal < 0.0f)
					signal = -signal;

				signal = _offset - signal;

				// Square the signal to increase the sharpness of the ridges.
				signal *= signal;

				// The weighting from the previous octave is applied to the signal.
				// Larger values have higher weights, producing sharp points along the
				// ridges.
				signal *= weight;

				// Add the signal to the output value.
				value += (signal * _spectralWeights[curOctave]);
			}

			return value;
		}

		#endregion
	}
}
