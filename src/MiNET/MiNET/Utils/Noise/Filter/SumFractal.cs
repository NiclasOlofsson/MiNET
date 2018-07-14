using System;
using System.Collections.Generic;
using System.Text;

namespace MiNET.Utils.Noise.Filter
{
	/// <summary>
	/// Noise module that outputs 3-dimensional Sum Fractal noise. This noise
	/// is also known as "Fractional BrownianMotion noise"
	///
	/// Sum Fractal noise is the sum of several coherent-noise functions of
	/// ever-increasing frequencies and ever-decreasing amplitudes.
	/// 
	/// This class implements the original noise::module::Perlin
	/// 
	/// </summary>
	public class SumFractal : FilterModule, IModule3D, IModule2D
	{
		#region Ctor/Dtor

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

			// Initialize value, fBM starts with 0
			float value = 0;

			// Inner loop of spectral construction, where the fractal is built

			for (curOctave = 0; curOctave < _octaveCount; curOctave++)
			{
				// Get the coherent-noise value.
				float signal = _source2D.GetValue(x, y) * _spectralWeights[curOctave];

				// Add the signal to the output value.
				value += signal;

				// Go to the next octave.
				x *= _lacunarity;
				y *= _lacunarity;
			}

			//take care of remainder in _octaveCount
			float remainder = _octaveCount - (int)_octaveCount;
			if (remainder > 0.0f)
				value += remainder * _source2D.GetValue(x, y) * _spectralWeights[curOctave];

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
			float signal;
			float value;
			int curOctave;

			x *= _frequency;
			y *= _frequency;
			z *= _frequency;

			// Initialize value, fBM starts with 0
			value = 0;

			// Inner loop of spectral construction, where the fractal is built
			for (curOctave = 0; curOctave < _octaveCount; curOctave++)
			{
				// Get the coherent-noise value.
				signal = _source3D.GetValue(x, y, z) * _spectralWeights[curOctave];

				// Add the signal to the output value.
				value += signal;

				// Go to the next octave.
				x *= _lacunarity;
				y *= _lacunarity;
				z *= _lacunarity;
			}

			//take care of remainder in _octaveCount
			float remainder = _octaveCount - (int)_octaveCount;
			if (remainder > 0.0f)
				value += remainder * _source3D.GetValue(x, y, z) * _spectralWeights[curOctave];

			return value;
		}

		#endregion
	}
}
