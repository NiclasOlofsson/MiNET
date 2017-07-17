using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LibNoise;
using LibNoise.Filter;
using LibNoise.Primitive;
using LibNoise.Transformer;
using MiNET.Utils;
using MiNET.Utils.Noise;
using MiNET.Worlds.Decorators;
using SimplexPerlin = MiNET.Utils.Noise.SimplexPerlin;
using Voronoi = MiNET.Utils.Noise.Voronoi;

namespace MiNET.Worlds.Survival
{
	public class OverworldGenerator : IWorldGenerator
	{
		private IModule2D RainNoise { get; }
		private IModule2D TempNoise { get; }
		private IModule2D BiomeSelector { get; }

		private readonly IModule2D _mainNoise;
		private readonly ScalePoint _depthNoise;

		private int Seed { get; }

		public OverworldGenerator()
		{
			int seed = Config.GetProperty("seed", "gottaloveMiNET").GetHashCode();
			Seed = seed;

			var rainSimplex = new SimplexPerlin(seed);
		
			var rainNoise = new Voronoi();
			rainNoise.Primitive3D = rainSimplex;
			rainNoise.Primitive2D = rainSimplex;
			rainNoise.Distance = false;
			rainNoise.Frequency = RainFallFrequency;
			rainNoise.OctaveCount = 2;
			//rainNoise.Displacement = 0.5f;
			//rainNoise.Lacunarity = 3;

			RainNoise = rainNoise;

			var tempSimplex = new SimplexPerlin(seed + 100);
			var tempNoise = new Voronoi();
			tempNoise.Primitive3D = tempSimplex;
			tempNoise.Primitive2D = tempSimplex;
			tempNoise.Distance = false;
			tempNoise.Frequency = TemperatureFrequency;
			tempNoise.OctaveCount = 2;
			//	tempNoise.Displacement = 0.5f;
			//	tempNoise.Lacunarity = 4.2f;

			TempNoise = tempNoise;

			var selectorSimplex = new SimplexPerlin(seed + 150);
			var selectorNoise = new Voronoi();
			selectorNoise.Primitive2D = selectorSimplex;
			selectorNoise.Distance = true; 
			selectorNoise.Frequency = 1.5f;
			selectorNoise.OctaveCount = 2;

			BiomeSelector = selectorNoise;

			var mainLimitNoise = new SimplexPerlin(seed + 200);

			var mainLimitFractal = new SumFractal()
			{
				Primitive3D = mainLimitNoise,
				Primitive2D = mainLimitNoise,
				Frequency = 0.095f,
				//SpectralExponent = 0.3f,
				OctaveCount = 8,
				Lacunarity = 3.5f,
				Offset = 0.312f,
				//		Gain = 12
			};
			var mainScaler = new ScaleableNoise()
			{
				XScale = 1f / MainNoiseScaleX,
				YScale = 1f / MainNoiseScaleY,
				ZScale = 1f / MainNoiseScaleZ,
				Primitive3D = mainLimitFractal,
				Primitive2D = mainLimitFractal
			};
			_mainNoise = mainScaler;

			var mountainNoise = new ImprovedPerlin(seed + 300, NoiseQuality.Fast);
			var mountainTerrain = new SumFractal()
			{
				Primitive3D = mountainNoise,
				Frequency = 2.75f,
				OctaveCount = 2,
				Lacunarity = 6f,
				//SpectralExponent = DepthNoiseScaleExponent
			};

			ScalePoint scaling = new ScalePoint(mountainTerrain);
			scaling.YScale = 1f / HeightScale;
			scaling.XScale = 1f / DepthNoiseScaleX;
			scaling.ZScale = 1f / DepthNoiseScaleZ;

			_depthNoise = scaling;
		}

		public void Initialize()
		{
			
		}

		public ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates)
		{
		//	Stopwatch sw = Stopwatch.StartNew();
			ChunkColumn chunk = new ChunkColumn
			{
				x = chunkCoordinates.X,
				z = chunkCoordinates.Z
			};

			Decorators.ChunkDecorator[] chunkDecorators = new ChunkDecorator[]
			{
				new WaterDecorator(),
				new OreDecorator(),
				new FoliageDecorator(),
			};

			var biomes = CalculateBiomes(chunk.x, chunk.z);

			foreach (var i in chunkDecorators)
			{
				i.SetSeed(Seed);
			}

			var heightMap = GenerateHeightMap(biomes, chunk.x, chunk.z);
			var thresholdMap = GetThresholdMap(chunk.x, chunk.z);

			CreateTerrainShape(chunk, heightMap, thresholdMap, biomes);
			DecorateChunk(chunk, heightMap, thresholdMap, biomes, chunkDecorators);

			chunk.isDirty = true;
			chunk.NeedSave = true;

			for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					for (int y = 0; y < 256; y++)
					{
						chunk.SetSkyLight(x, y, z, 255);
					}
				}
			}
		//	sw.Stop();

		//	if (sw.ElapsedMilliseconds > previousTime)
			//{
			//	Debug.WriteLine("Chunk gen took " + sw.ElapsedMilliseconds + " ms");
				//previousTime = sw.ElapsedMilliseconds;
			//}

			return chunk;
		}

		private const float TemperatureFrequency = 0.0083f;

		private const float RainFallFrequency = 0.0083f;

		private const float BiomeNoiseScale = 6.5f;

		private const float MainNoiseScaleX = 80F;
		private const float MainNoiseScaleY = 160F;
		private const float MainNoiseScaleZ = 80F;

		private const float DepthNoiseScaleX = 200F;
		private const float DepthNoiseScaleZ = 200F;
		private const float DepthNoiseScaleExponent = 0.5F;

		private const float CoordinateScale = 684.412F;
		private const float HeightScale = 684.412F;

		public const int WaterLevel = 64;

		private Dictionary<Biome, double> GetBiome(int x, int z)
		{
			float temp = TempNoise.GetValue(x / BiomeNoiseScale, z / BiomeNoiseScale).Normalize(1f, -1f, 2f, -1f);

			float rain = RainNoise.GetValue(x / BiomeNoiseScale, z / BiomeNoiseScale) / 2f + 0.5f;

			var biomes = BiomeUtils.GetBiomes(temp, rain);
			return biomes;
		}

		private Dictionary<Biome, double>[] CalculateBiomes(int cx, int cz)
		{
			cx *= 16;
			cz *= 16;

			Dictionary<Biome, double>[] rb = new Dictionary<Biome, double>[16 * 16];

			for (int x = 0; x < 16; x++)
			{
				int rx = cx + x;
				for (int z = 0; z < 16; z++)
				{
					rb[(x << 4) + z] = GetBiome(rx, cz + z);
				}
			}

			return rb;
		}

		private float[] GenerateHeightMap(Dictionary<Biome, double>[] biomes, int cx, int cz)
		{
			cx *= 16;
			cz *= 16;

			/*var b0 = GetBiome(cx, cz).FirstOrDefault().Key;
			var b1 = GetBiome(cx, cz + 16).FirstOrDefault().Key;

			var b2 = GetBiome(cx + 16, cz).FirstOrDefault().Key;
			var b3 = GetBiome(cx + 16, cz + 16).FirstOrDefault().Key;

			float q11 = WaterLevel + (128f * b0.MaxHeight) * _mainNoise.GetValue(cx, cz);
			float q12 = WaterLevel + (128f * b1.MaxHeight) * _mainNoise.GetValue(cx, cz + 16);

			float q21 = WaterLevel + (128f * b2.MaxHeight) * _mainNoise.GetValue(cx + 16, cz);
			float q22 = WaterLevel + (128f * b3.MaxHeight) * _mainNoise.GetValue(cx + 16, cz + 16); 

			*/

			var b11 = biomes[0].First();

			var b12 = biomes[15].First();

			var b21 = biomes[240].First();

			var b22 = biomes[255].First();

			float q11 = WaterLevel + (128f * b11.Key.MaxHeight) * _mainNoise.GetValue(cx, cz);
			float q12 = WaterLevel + (128f * b12.Key.MaxHeight) * _mainNoise.GetValue(cx, cz + 16);

			float q21 = WaterLevel + (128f * b21.Key.MaxHeight) * _mainNoise.GetValue(cx + 16, cz);
			float q22 = WaterLevel + (128f * b22.Key.MaxHeight) * _mainNoise.GetValue(cx + 16, cz + 16);


			float[] heightMap = new float[16 * 16];

			for (int x = 0; x < 16; x++)
			{
				int rx = cx + x;

				for (int z = 0; z < 16; z++)
				{
					int rz = cz + z;

					var baseNoise = Interpolation.BilinearCubic(
						rx, rz,
						q11,
						q12,
						q21,
						q22,
						cx, cx + 16, cz, cz + 16);

					heightMap[(x << 4) + z] = baseNoise;
				}


			}
			return heightMap;
		}

		private float[] GetThresholdMap(int cx, int cz)
		{
			cx *= 16;
			cz *= 16;

			float[] thresholdMap = new float[16 * 16 * 256];

			for (int x = 0; x < 16; x++)
			{
				float rx = cx + x;
				for (int z = 0; z < 16; z++)
				{
					float rz = cz + z;
					for (int y = 255; y > 0; y--)
					{
						thresholdMap[x + 16 * (y + 256 * z)] = _depthNoise.GetValue(rx, y, rz);
					}
				}
			}
			return thresholdMap;
		}

		public const float Threshold = -0.1f;
		private const int Width = 16;
		private const int Depth = 16;
		private const int Height = 256;

		private void CreateTerrainShape(ChunkColumn chunk, float[] heightMap, float[] thresholdMap, Dictionary<Biome, double>[] biomes)
		{
			for (int x = 0; x < Width; x++)
			{
				for (int z = 0; z < Depth; z++)
				{
					Biome biome = biomes[(x << 4) + z].FirstOrDefault().Key;
					chunk.SetBiome(x, z, (byte)biome.Id);

					float stoneHeight = heightMap[(x << 4) + z];

					var maxY = 0;
					for (int y = 0; y < Height; y++)
					{
						float density = thresholdMap[x + 16 * (y + 256 * z)];

						if (y <= stoneHeight)
						{
							if (y < WaterLevel || (density > Threshold && y >= WaterLevel))
							{
								chunk.SetBlock(x, y, z, 1);
								maxY = y;
							}
						}
					}

					chunk.SetBlock(x, 0, z, 7); //Bedrock
					heightMap[(x << 4) + z] = maxY;
					chunk.SetHeight(x, z, (byte)maxY);
				}
			}
		}

		private void DecorateChunk(ChunkColumn chunk, float[] heightMap, float[] thresholdMap, Dictionary<Biome, double>[] biomes,
			ChunkDecorator[] decorators)
		{
			for (int x = 0; x < Width; x++)
			{
				for (int z = 0; z < Depth; z++)
				{
					var height = heightMap[(x << 4) + z];
					var options = biomes[(x << 4) + z];
					int idx = Libnoise.FastFloor(BiomeSelector.GetValue((chunk.x + x) / BiomeNoiseScale, (chunk.z + z) / BiomeNoiseScale) * 3);
					if (idx < 0) idx = -idx;
					if (idx >= options.Count) idx = options.Count -1;

					var biomeData = biomes[(x << 4) + z].ElementAt(idx);
					var biome = biomeData.Key;

					for (int y = 0; y < Height; y++)
					{
						bool isSurface = false;
						if (y <= height)
						{
							if (y < 255 && chunk.GetBlock(x, y, z) == 1 && chunk.GetBlock(x, y + 1, z) == 0)
							{
								isSurface = true;
							}

							if (isSurface)
							{
								if (y >= WaterLevel)
								{
									chunk.SetBlock(x, y, z, biome.SurfaceBlock);
									chunk.SetMetadata(x, y, z, biome.SurfaceMetadata);

									chunk.SetBlock(x, y - 1, z, biome.SoilBlock);
									chunk.SetMetadata(x, y - 1, z, biome.SoilMetadata);
								}
							}
						}

						for(int i = 0; i < decorators.Length; i++)
						{
							decorators[i].Decorate(chunk, biome, thresholdMap, x, y, z, isSurface, y < height - 1);
						}
					}
				}
			}
		}
	}
}
