using System;
using System.Collections.Generic;
using System.Text;

namespace MiNET.Utils.Noise.Filter
{
	/// <summary>
	/// Noise module that outputs 3-dimensional MultiFractal noise.
	///
	/// The multifractal algorithm differs from the Fractal brownian motion in that perturbations are combined 
	/// multiplicatively and introduces an offset parameter. The perturbation at each frequency is computed as 
	/// in the fBM algorithm, but offset is finally added to the value. 
	/// The role of offset is to emphasize the final perturbation value. 
	/// Multiplicative combination of perturbation, in turn, emphasizes the "mountain-like-aspect" of the landscape, 
	/// so that between mountains a sort of slopes are generated
	/// (From http://meshlab.sourceforge.net/wiki/index.php/Fractal_Creation )
	/// </summary>
	public class MultiFractal : FilterModule, IModule3D, IModule2D
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

			// Initialize value
			float value = 1.0f;

			// inner loop of spectral construction, where the fractal is built
			for (curOctave = 0; curOctave < _octaveCount; curOctave++)
			{
				// Get the coherent-noise value.
				float signal = _offset + (_source2D.GetValue(x, y) * _spectralWeights[curOctave]);

				// Add the signal to the output value.
				value *= signal;

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
			int curOctave;

			x *= _frequency;
			y *= _frequency;
			z *= _frequency;

			// Initialize value
			float value = 1.0f;

			// inner loop of spectral construction, where the fractal is built
			for (curOctave = 0; curOctave < _octaveCount; curOctave++)
			{
				// Get the coherent-noise value.
				float signal = _offset + (_source3D.GetValue(x, y, z) * _spectralWeights[curOctave]);

				// Add the signal to the output value.
				value *= signal;

				// Go to the next octave.
				x *= _lacunarity;
				y *= _lacunarity;
				z *= _lacunarity;
			}

			//take care of remainder in _octaveCount
			float remainder = _octaveCount - (int)_octaveCount;

			if (remainder > 0.0f)
				value += remainder * _source2D.GetValue(x, y) * _spectralWeights[curOctave];

			return value;
		}

		#endregion
	}
}
