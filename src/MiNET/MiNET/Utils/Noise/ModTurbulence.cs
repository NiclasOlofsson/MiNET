namespace MiNET.Utils.Noise
{
	/// <summary>
	/// Noise module that randomly displaces the input value before
	/// returning the output value from a source module.
	/// 
	/// Turbulence is the pseudo-random displacement of the input value.
	/// The GetValue() method randomly displaces the ( x, y, z )
	/// coordinates of the input value before retrieving the output value from
	/// the source module.
	/// 
	/// The power of the turbulence determines the scaling factor that is
	/// applied to the displacement amount.  To specify the power, use the
	/// Power property.
	/// 
	/// Use of this noise module may require some trial and error.  Assuming
	/// that you are using a generator module as the source module, you
	/// should first set the power to the reciprocal of the frequency.
	/// 
	/// 
	/// Displacing the input values result in more realistic terrain and
	/// textures.  If you are generating elevations for terrain height maps,
	/// you can use this noise module to produce more realistic mountain
	/// ranges or terrain features that look like flowing lava rock.  If you
	/// are generating values for textures, you can use this noise module to
	/// produce realistic marble-like or "oily" textures.
	/// 
	/// Internally, there are three noise modules
	/// that displace the input value; one for the x, one for the y,
	/// and one for the z coordinate.
	/// </summary>
	public class ModTurbulence : TransformerModule, IModule3D, IModule2D, IModule
	{
		/// <summary>The power (scale) of the displacement.</summary>
		protected float _power = 1f;
		/// <summary>Default power for the Turbulence noise module</summary>
		public const float DEFAULT_POWER = 1f;
		/// <summary>The source input module</summary>
		protected IModule _sourceModule;
		/// <summary>Noise module that displaces the x coordinate.</summary>
		protected IModule _xDistortModule;
		/// <summary>Noise module that displaces the y coordinate.</summary>
		protected IModule _yDistortModule;
		/// <summary>Noise module that displaces the z coordinate.</summary>
		protected IModule _zDistortModule;

		/// <summary>Gets or sets the source module</summary>
		public IModule SourceModule
		{
			get
			{
				return this._sourceModule;
			}
			set
			{
				this._sourceModule = value;
			}
		}

		/// <summary>
		/// Gets or sets the noise module that displaces the x coordinate.
		/// </summary>
		public IModule XDistortModule
		{
			get
			{
				return this._xDistortModule;
			}
			set
			{
				this._xDistortModule = value;
			}
		}

		/// <summary>
		/// Gets or sets the noise module that displaces the y coordinate.
		/// </summary>
		public IModule YDistortModule
		{
			get
			{
				return this._yDistortModule;
			}
			set
			{
				this._yDistortModule = value;
			}
		}

		/// <summary>
		/// Gets or sets the noise module that displaces the z coordinate.
		/// </summary>
		public IModule ZDistortModule
		{
			get
			{
				return this._zDistortModule;
			}
			set
			{
				this._zDistortModule = value;
			}
		}

		/// <summary>
		/// Returns the power of the turbulence.
		/// 
		/// The power of the turbulence determines the scaling factor that is
		/// applied to the displacement amount.
		/// </summary>
		public float Power
		{
			get
			{
				return this._power;
			}
			set
			{
				this._power = value;
			}
		}

		/// <summary>Create a new noise module with default values</summary>
		public ModTurbulence()
		{
			this._power = 1f;
		}

		/// <summary>Create a new noise module with the given values</summary>
		/// <param name="source">the source module</param>
		public ModTurbulence(IModule source)
		  : this()
		{
			this._sourceModule = source;
		}

		/// <summary>Create a new noise module with the given values.</summary>
		/// <param name="source">the source module</param>
		/// <param name="xDistortModule">the noise module that displaces the x coordinate</param>
		/// <param name="yDistortModule">the noise module that displaces the y coordinate</param>
		/// <param name="zDistortModule">the noise module that displaces the z coordinate</param>
		/// <param name="power">the power of the turbulence</param>
		public ModTurbulence(IModule source, IModule xDistortModule, IModule yDistortModule, IModule zDistortModule, float power)
		{
			this._sourceModule = source;
			this._xDistortModule = xDistortModule;
			this._yDistortModule = yDistortModule;
			this._zDistortModule = zDistortModule;
			this._power = power;
		}

		/// <summary>
		/// Generates an output value given the coordinates of the specified input value.
		/// </summary>
		/// <param name="x">The input coordinate on the x-axis.</param>
		/// <param name="y">The input coordinate on the y-axis.</param>
		/// <param name="z">The input coordinate on the z-axis.</param>
		/// <returns>The resulting output value.</returns>
		public float GetValue(float x, float y, float z)
		{
			float x1 = x + 0.1894226f;
			float y1 = y + 0.9937134f;
			float z1 = z + 0.4781647f;
			float x2 = x + 0.4046478f;
			float y2 = y + 0.2766113f;
			float z2 = z + 0.9230499f;
			float x3 = x + 0.821228f;
			float y3 = y + 0.1710968f;
			float z3 = z + 0.6842804f;
			return ((IModule3D)this._sourceModule).GetValue(x + ((IModule3D)this._xDistortModule).GetValue(x1, y1, z1) * this._power, y + ((IModule3D)this._yDistortModule).GetValue(x2, y2, z2) * this._power, z + ((IModule3D)this._zDistortModule).GetValue(x3, y3, z3) * this._power);
		}

		public float GetValue(float x, float y)
		{
			float x1 = x + 0.1894226f;
			float y1 = y + 0.9937134f;
			float x2 = x + 0.4046478f;
			float y2 = y + 0.2766113f;

			return ((IModule2D) this._sourceModule).GetValue(x + ((IModule2D) this._xDistortModule).GetValue(x1, y1)*this._power,
				y + ((IModule2D) this._yDistortModule).GetValue(x2, y2)*this._power);
		}
	}
}
