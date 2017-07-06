using LibNoise;

namespace MiNET.Utils
{
	public class NoiseBlender : SelectorModule, IModule3D, IModule2D
	{
		#region Fields

		/// <summary>
		/// The control module
		/// </summary>
		protected IModule _mainNoise;

		/// <summary>
		/// The left input module
		/// </summary>
		protected IModule _lowerLimitNoise;

		/// <summary>
		/// The right input module
		/// </summary>
		protected IModule _upperLimitNoise;

		#endregion

		#region Accessors

		/// <summary>
		/// Gets or sets the left module
		/// </summary>
		public IModule LowerLimitNoise
		{
			get { return _lowerLimitNoise; }
			set { _lowerLimitNoise = value; }
		}

		/// <summary>
		/// Gets or sets the right module
		/// </summary>
		public IModule UpperLimitNoise
		{
			get { return _upperLimitNoise; }
			set { _upperLimitNoise = value; }
		}

		/// <summary>
		/// Gets or sets the control module
		/// </summary>
		public IModule MainNoise
		{
			get { return _mainNoise; }
			set { _mainNoise = value; }
		}

		#endregion

		#region Ctor/Dtor

		public NoiseBlender()
		{
		}


		public NoiseBlender(IModule mainNoise, IModule upperLimitNoise, IModule lowerLimitNoise)
		{
			_mainNoise = mainNoise;
			_lowerLimitNoise = lowerLimitNoise;
			_upperLimitNoise = upperLimitNoise;
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
			float v0 = ((IModule3D)_lowerLimitNoise).GetValue(x, y, z);
			float v1 = ((IModule3D)_upperLimitNoise).GetValue(x, y, z);
			float alpha = (((IModule3D)_mainNoise).GetValue(x, y, z) + 1.0f) / 2.0f;
			return MathHelpers.Smoothstep(v0, v1, alpha);
		}

		public float GetValue(float x, float y)
		{
			float lower = ((IModule2D)_lowerLimitNoise).GetValue(x, y);
			float upper = ((IModule2D)_upperLimitNoise).GetValue(x, y);
			float alpha = (((IModule2D)_mainNoise).GetValue(x, y) + 1.0f) / 2.0f;

			return MathHelpers.Smoothstep(lower, upper, alpha);
		}

		#endregion
	}
}
