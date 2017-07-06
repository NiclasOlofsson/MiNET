using System;
using MiNET.Utils;

namespace MiNET.Worlds.Decorators
{
	public class CaveDecorator : ChunkDecorator
	{
		private OpenSimplexNoise CaveNoise;
		private FastNoise BetterCaveNoise1;
		protected override void InitSeed(int seed)
		{
			CaveNoise = new OpenSimplexNoise(seed * 6);

			BetterCaveNoise1 = new FastNoise(seed);
			BetterCaveNoise1.SetCellularJitter(0.3f);
			BetterCaveNoise1.SetCellularReturnType(FastNoise.CellularReturnType.Distance);
			BetterCaveNoise1.SetFrequency(1.005f);
			BetterCaveNoise1.SetCellularDistanceFunction(FastNoise.CellularDistanceFunction.Euclidean);
		}

		private float GetCaveNoise(float x, float y, float z)
		{
			const float PI_2 = 1.57079633f;
			float oct1 = (BetterCaveNoise1.GetCellular(x * 0.1f, y * 0.1f, z * 0.1f)) * 4;

			oct1 = oct1 * oct1 * oct1;
			if (oct1 < 0f)
			{
				oct1 = PI_2;
			}

			if (oct1 > PI_2)
			{
				oct1 = PI_2;
			}

			return oct1;
		}

		private int GetIndex(int x, int y, int z)
		{
			return Math.Max(0, Math.Min(x + 16 * ((y + 1) + 256 * z), 65535));
		}

		public override void Decorate(ChunkColumn column, Biome biome, float[] thresholdMap, int x, int y, int z, bool surface, bool highestStoneLevel)
		{
			if (surface) return;
			if (thresholdMap[GetIndex(x - 1, y, z)] < SurvivalWorldProvider.Threshold) return;
			if (thresholdMap[GetIndex(x + 1, y, z)] < SurvivalWorldProvider.Threshold) return;
			if (thresholdMap[GetIndex(x, y, z - 1)] < SurvivalWorldProvider.Threshold) return;
			if (thresholdMap[GetIndex(x, y, z + 1)] < SurvivalWorldProvider.Threshold) return;

			int rx = column.x * 16 + x;
			int rz = column.z * 16 + z;

		//	float n0 = BetterCaveNoise1.GetCellular(rx, y, rz);

		//	float n1 = BetterCaveNoise2.GetCellular(rx, y, rz);

			if (column.GetBlock(x,y,z) == 1 && highestStoneLevel && Math.Cos(GetCaveNoise(rx, y, rz))*Math.Abs(Math.Cos(y*0.2f + 2)*0.75f) > 0.0055f /*BetterCaveNoise1.GetCellular(rx * 0.1f, y, rz * 0.1f) > 0.5f*/)
			{
				column.SetBlock(x,y,z, 0);
			}
		}
	}
}
