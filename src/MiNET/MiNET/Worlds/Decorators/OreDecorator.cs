using System;
using System.Linq;
using LibNoise;
using LibNoise.Primitive;

namespace MiNET.Worlds.Decorators
{
	public class OreDecorator : ChunkDecorator
	{
		private struct OreData
		{
			public byte ID;
			public int MinY;
			public int MaxY;
			public int Veins;
			public int Abundance;
			public float Rarity;

			public OreData(byte id, int minY, int maxY, int viens, int abundance, float rarity)
			{
				ID = id;
				MinY = minY;
				MaxY = maxY;
				Veins = viens;
				Abundance = abundance;
				Rarity = rarity;
			}
		}

		private OreData[] Ores = new OreData[]
		{
			new OreData(16, 10, 120, 25, 25, 3f), //Coal
			new OreData(15, 1, 64, 15, 5, 1.9f), //Iron
			new OreData(21, 10, 25, 7, 4, 1.4f), //Lapis
			new OreData(14, 1, 32, 6, 4, 1.04f), //Gold
			new OreData(56, 1, 15, 6, 3, 0.7f), //Diamond
			new OreData(73, 1, 16, 4, 6, 1.13f) //Redstone
		};

		private Random _random;
		protected override void InitSeed(int seed)
		{
			_simplex = new SimplexPerlin(seed / 10, NoiseQuality.Fast);
			_random = new Random();
		}

		public override void Decorate(ChunkColumn column, Biome biome, float[] thresholdMap, int x, int y, int z, bool surface,
			bool highestStoneLevel)
		{
			if (surface || column.GetBlock(x, y, z) != 1 && highestStoneLevel) return;

			if (highestStoneLevel)
			{
				int rx = column.x*16 + x;
				int rz = column.z*16 + z;

				foreach (var ore in Ores.Where(o => o.MinY < y && o.MaxY > y))
				{
					double weight = 0;
					for (int i = 0; i < 4; i++)
					{
						weight += _random.NextDouble();
					}
					weight /= ore.Rarity;

					if (Math.Abs(_simplex.GetValue(rx*ore.Abundance, y, rz*ore.Abundance))*3 < ore.Rarity &&
					    _simplex.GetValue(rx, y, rz) > weight)
					{
						int xOffset = 0;
						int zOffset = 0;
						int yOffset = 0;
						for (int i = 0; i < _random.Next(1, ore.Abundance); i++)
						{
							int offset = _random.Next(0, 3);
							double offset2 = _random.NextDouble();
							if (offset.Equals(0) && offset2 < 0.4)
								xOffset += 1;
							else if (offset.Equals(1) && offset2 >= 0.4 && offset2 < 0.65)
								yOffset += 1;
							else
								zOffset += 1;

							var mX = Math.Min(x + xOffset, x);
							var my = Math.Min(y + yOffset, ore.MaxY);
							var mz = Math.Min(z + zOffset, z);
							if (column.GetBlock(mX, my, mz) != 1) return;

							column.SetBlock(mX, my, mz, ore.ID);
						}
					}
				}
			}
		}
	}
}
