using System;
using System.Collections.Generic;
using System.Text;

namespace MiNET.Worlds.Generators.Survival.Decorators
{
    public class WaterDecorator : ChunkDecorator
	{
		protected override void InitSeed(int seed)
		{
			
		}

		public override void Decorate(ChunkColumn column, Biome biome, float[] thresholdMap, int x, int y, int z, bool surface, bool isBelowMaxHeight)
		{
			if (y > OverworldGenerator.WaterLevel && y > 32) return;

			var humidity = biome.Downfall;
			var currentTemperature = biome.Temperature;
			if (y > OverworldGenerator.WaterLevel)
			{
				int distanceToSeaLevel = y - OverworldGenerator.WaterLevel;
				currentTemperature = biome.Temperature - (0.00166667f * distanceToSeaLevel);
			}

			var block = column.GetBlock(x, y, z);
			if (surface)
			{
				if (humidity > 0.5f && currentTemperature <= 0.5f) //If its a wet environment & it's not to warm, we use dirt. 
				{
					column.SetBlock(x, y, z, 3); //Dirt
				}
				else
				{
					column.SetBlock(x, y, z, 12); //Sand
				}
			}
			else if (y <= OverworldGenerator.WaterLevel && block == 0)
			{
				if (currentTemperature <= 0f && y == OverworldGenerator.WaterLevel)
				{
					column.SetBlock(x, y, z, 79); //Ice
				}
				else
				{
					column.SetBlock(x, y, z, 8); //Water
				}
			}

		}
	}
}
