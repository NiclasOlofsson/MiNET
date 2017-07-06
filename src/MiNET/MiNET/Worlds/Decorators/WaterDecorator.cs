using System;

namespace MiNET.Worlds.Decorators
{
	public class WaterDecorator : ChunkDecorator
	{
		protected override void InitSeed(int seed)
		{
			
		}

		public override void Decorate(ChunkColumn column, Biome biome, float[] thresholdMap, int x, int y, int z, bool surface, bool highestStoneLevel)
		{
			if (y > SurvivalWorldProvider.WaterLevel && y > 32) return;

			var density = thresholdMap[x + 16*(y + 256*z)];
			var bid = column.GetBlock(x, y, z);
			if (bid == 0 /* || density < 0*/) //If this block is supposed to be air.
			{
				column.SetBlock(x, y, z, 8);
			}
			else if (surface)
			{
				column.SetBlock(x,y,z, 12);
			}

			/*if (y >= SurvivalWorldProvider.WaterLevel) return;

			var block = column.GetBlock(x, y, z);
			if (surface)
			{
				column.SetBlock(x, y, z, 12); //Sand
			}
			else if (y < SurvivalWorldProvider.WaterLevel - 2 && block == 0)
			{
				if (biome.Temperature <= 0f && y == SurvivalWorldProvider.WaterLevel - 3)
				{
					column.SetBlock(x, y, z, 79); //Ice
				}
				else
				{
					column.SetBlock(x, y, z, 8); //Water
				}
			}*/

		}
	}
}
