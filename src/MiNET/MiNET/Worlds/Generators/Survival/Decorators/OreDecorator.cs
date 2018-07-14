using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiNET.Utils.Noise;
using MiNET.Utils.Noise.Filter;

namespace MiNET.Worlds.Generators.Survival.Decorators
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

			public OreData(byte id, int minY, int maxY, int veins, int abundance, float rarity)
			{
				ID = id;
				MinY = minY;
				MaxY = maxY;
				Veins = veins;
				Abundance = abundance;
				Rarity = rarity;
			}
		}

		private OreData[] Ores = new OreData[]
		{
			new OreData(16, 10, 120, 25, 25, 3f), //Coal
			new OreData(15, 1, 64, 15, 5, 1.9f), //Iron
			new OreData(21, 10, 25, 7, 4, 1.1f), //Lapis
			new OreData(14, 1, 32, 6, 4, 1.04f), //Gold
			new OreData(56, 1, 15, 6, 3, 0.7f), //Diamond
			new OreData(73, 1, 16, 4, 6, 1.13f) //Redstone
		};

		public OreDecorator()
		{
			RunPerBlock = false;
		}

		private IModule3D _simplex;
		private Random _random;
		protected override void InitSeed(int seed)
		{
			_simplex = new SumFractal()
			{
				Primitive3D = new SimplexPerlin(seed + 666),
				Frequency = 1f,
				OctaveCount = 2,
				Lacunarity = 0.65f
			};

			_random = new Random();
		}

		private static readonly int[] LowWeightOffset = new int[2] { 2, 3 };
		private static readonly int[] HighWeightOffset = new int[2] { 2, 2 };

		public override void Decorate(ChunkColumn column, Biome biome, float[] thresholdMap, int x, int y, int z, bool surface,
			bool isBelowMaxHeight)
		{
			if (surface || column.GetBlock(x, y, z) != 1) return;

			if (isBelowMaxHeight)
			{
				int rx = column.x * 16 + x;
				int rz = column.z * 16 + z;

				var noise = _simplex.GetValue(rx, y, rz);

				foreach (var ore in Ores.Where(o => o.MinY < y && o.MaxY > y))
				{
					var weightOffsets = (ore.MaxY > 30) ? HighWeightOffset : LowWeightOffset;

					if (MathF.Abs(noise) * 3f < ore.Rarity)
					{
						double weight = 0;
						for (int i = 0; i < 4; i++)
						{
							weight += _random.NextDouble();
						}

						weight /= ore.Rarity;
						weight = weightOffsets[0] - MathF.Abs((float)weight - weightOffsets[1]);

						if (noise > weight)
						{
							int xOffset = 0;
							int zOffset = 0;
							int yOffset = 0;
							for (int i = 0; i < _random.Next(0, ore.Abundance); i++)
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
}
