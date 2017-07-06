using System;
using LibNoise;

namespace MiNET.Utils
{
	public class ModifyNoise : IModule, IModule3D, IModule2D
	{
		public IModule3D PrimaryNoise3D { get; set; }
		public IModule3D SecondaryNoise3D { get; set; }

		public IModule2D PrimaryNoise2D { get; set; }
		public IModule2D SecondaryNoise2D { get; set; }

		public NoiseModifier Modifier { get; set; }

		public ModifyNoise(NoiseModifier modifier)
		{
			Modifier = modifier;
		}

		public float GetValue(float x, float y, float z)
		{
			switch (Modifier)
			{
				case NoiseModifier.Add:
					return PrimaryNoise3D.GetValue(x, y, z) + SecondaryNoise3D.GetValue(x, y, z);
				case NoiseModifier.Multiply:
					return PrimaryNoise3D.GetValue(x, y, z) * SecondaryNoise3D.GetValue(x, y, z);
				case NoiseModifier.Power:
					return (float) Math.Pow(PrimaryNoise3D.GetValue(x, y, z), SecondaryNoise3D.GetValue(x, y, z));
				case NoiseModifier.Subtract:
					return PrimaryNoise3D.GetValue(x, y, z) - SecondaryNoise3D.GetValue(x, y, z);
				default:
					//This is unreachable.
					return PrimaryNoise3D.GetValue(x, y, z) + SecondaryNoise3D.GetValue(x, y, z);
			}
		}

		public float GetValue(float x, float y)
		{
			switch (Modifier)
			{
				case NoiseModifier.Add:
					return PrimaryNoise2D.GetValue(x, y) + SecondaryNoise2D.GetValue(x, y);
				case NoiseModifier.Multiply:
					return PrimaryNoise2D.GetValue(x, y) * SecondaryNoise2D.GetValue(x, y);
				case NoiseModifier.Power:
					return (float) Math.Pow(PrimaryNoise2D.GetValue(x, y), SecondaryNoise2D.GetValue(x, y));
				case NoiseModifier.Subtract:
					return PrimaryNoise2D.GetValue(x, y) - SecondaryNoise2D.GetValue(x, y);
				default:
					//This is unreachable.
					return PrimaryNoise2D.GetValue(x, y) + SecondaryNoise2D.GetValue(x, y);
			}
		}
	}

	public enum NoiseModifier
	{
		Add,
		Subtract,
		Multiply,
		Power
	}
}
