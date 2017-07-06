using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibNoise;

namespace MiNET.Utils
{
	public sealed class ScaleableNoise : IModule, IModule2D, IModule3D
	{
		public IModule2D Primitive2D { get; set; }
		public IModule3D Primitive3D { get; set; }

		public float XScale { get; set; } = 1f;
		public float YScale { get; set; } = 1f;
		public float ZScale { get; set; } = 1f;

		public ScaleableNoise()
		{

		}

		public float GetValue(float x, float y)
		{
			return Primitive2D.GetValue(x * XScale, y * ZScale);
		}

		public float GetValue(float x, float y, float z)
		{
			return Primitive3D.GetValue(x * XScale, y * YScale, z * ZScale);
		}
	}
}
