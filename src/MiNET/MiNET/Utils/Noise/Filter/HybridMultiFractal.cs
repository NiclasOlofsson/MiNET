using System;
using System.Collections.Generic;
using System.Text;

namespace MiNET.Utils.Noise.Filter
{
	/// <summary>
	/// Noise module that outputs 3-dimensional hybrid-multifractal noise.
	///
	/// Hybrid-multifractal noise the perturbations are combined additively, 
	/// but the single perturbation is computed by multiplying two quantities 
	/// called weight and signal. The signal quantity is the standard multifractal 
	/// perturbation, and the weight quantity is the multiplicative combination 
	/// of all the previous signal quantities.
	///
	/// Hybrid-multifractal attempts to control the amount of details according
	/// to the slope of the underlying overlays. Hybrid Multifractal  is 
	/// conventionally used to generate terrains with smooth valley areas and 
	/// rough peaked mountains. With high Lacunarity values, it tends to produce 
	/// embedded plateaus. 
	/// 
	/// Some good parameter values to start with:
	///		gain = 1.0;
	///		offset = 0.7;
	///		spectralExponent = 0.25;
	/// 
	/// </summary>
	public class HybridMultiFractal : FilterModule, IModule3D, IModule2D
	{
		#region Ctor/Dtor

		/// <summary>
		/// 0-args constructor
		/// </summary>
		public HybridMultiFractal()
		{
			_gain = 1.0f;
			_offset = 0.7f;
			_spectralExponent = 0.25f;

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
			float signal;
			int curOctave;

			x *= _frequency;
			y *= _frequency;

			// Initialize value : get first octave of function; later octaves are weighted
			float value = _source2D.GetValue(x, y) + _offset;
			float weight = _gain * value;

			x *= _lacunarity;
			y *= _lacunarity;

			// inner loop of spectral construction, where the fractal is built
			for (curOctave = 1; weight > 0.001 && curOctave < _octaveCount; curOctave++)
			{
				// prevent divergence
				if (weight > 1.0)
					weight = 1.0f;

				// get next higher frequency
				signal = (_offset + _source2D.GetValue(x, y)) * _spectralWeights[curOctave];

				// The weighting from the previous octave is applied to the signal.
				signal *= weight;

				// Add the signal to the output value.
				value += signal;

				// update the (monotonically decreasing) weighting value
				weight *= _gain * signal;

				// Go to the next octave.
				x *= _lacunarity;
				y *= _lacunarity;
			}

			//take care of remainder in _octaveCount
			float remainder = _octaveCount - (int)_octaveCount;

			if (remainder > 0.0f)
			{
				signal = _source2D.GetValue(x, y);
				signal *= _spectralWeights[curOctave];
				signal *= remainder;
				value += signal;
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
			float signal;
			int curOctave;

			x *= _frequency;
			y *= _frequency;
			z *= _frequency;

			// Initialize value : get first octave of function; later octaves are weighted
			float value = _source3D.GetValue(x, y, z) + _offset;
			float weight = _gain * value;

			x *= _lacunarity;
			y *= _lacunarity;
			z *= _lacunarity;

			// inner loop of spectral construction, where the fractal is built
			for (curOctave = 1; weight > 0.001 && curOctave < _octaveCount; curOctave++)
			{
				// prevent divergence
				if (weight > 1.0)
					weight = 1.0f;

				// get next higher frequency
				signal = (_offset + _source3D.GetValue(x, y, z)) * _spectralWeights[curOctave];

				// The weighting from the previous octave is applied to the signal.
				signal *= weight;

				// Add the signal to the output value.
				value += signal;

				// update the (monotonically decreasing) weighting value
				weight *= _gain * signal;

				// Go to the next octave.
				x *= _lacunarity;
				y *= _lacunarity;
				z *= _lacunarity;
			}

			//take care of remainder in _octaveCount
			float remainder = _octaveCount - (int)_octaveCount;

			if (remainder > 0.0f)
			{
				signal = _source3D.GetValue(x, y, z);
				signal *= _spectralWeights[curOctave];
				signal *= remainder;
				value += signal;
			}

			return value;
		}

		#endregion
	}
}
