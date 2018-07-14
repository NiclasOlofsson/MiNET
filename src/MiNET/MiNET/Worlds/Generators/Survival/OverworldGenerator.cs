using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using log4net;
using LibNoise;
using LibNoise.Filter;
using LibNoise.Primitive;
using MiNET.Utils;
using MiNET.Utils.Noise;
using MiNET.Worlds.Generators.Survival.Decorators;

namespace MiNET.Worlds.Generators.Survival
{
	public class OverworldGenerator : IWorldGenerator
	{
		private IModule2D RainNoise { get; }
		private IModule2D TempNoise { get; }

		private IModule2D BiomeHeightModifier { get; }
		private IModule2D BiomeModifierX { get; }
		private IModule2D BiomeModifierZ { get; }

		private readonly IModule2D _mainNoise;
		private readonly IModule3D _depthNoise;
		private readonly IModule2D _detailNoise;

		private int Seed { get; }

		public OverworldGenerator()
		{
			int seed = (int)(CalculateHash(Config.GetProperty("seed", "YoHoMotherducker!")) % int.MaxValue);
			Seed = seed;

			BiomeModifierX = new SimplexPerlin(seed + 370, NoiseQuality.Fast);
			BiomeModifierZ = new SimplexPerlin(seed + 5000, NoiseQuality.Fast);
			_detailNoise = new SimplexPerlin(seed + 231, NoiseQuality.Fast);

			var b = new SimplexPerlin(seed, NoiseQuality.Fast);
			BiomeHeightModifier = new Pipe()
			{
				Primitive2D = b,
				Frequency = 0.25f,
				
			};

			var rainSimplex = new OpenSimplexNoise(seed);

			var rainNoise = new ImprovedVoronoi();
			rainNoise.Primitive3D = rainSimplex;
			rainNoise.Primitive2D = rainSimplex;
			rainNoise.Distance = false;
			rainNoise.Frequency = RainFallFrequency;
			rainNoise.OctaveCount = 2;
			rainNoise.Gain = 0.128f;
			rainNoise.Displacement = 9;//.412f;
			RainNoise = rainNoise;

			var tempSimplex = new OpenSimplexNoise(seed + 100);
			var tempNoise = new ImprovedVoronoi();
			tempNoise.Primitive3D = tempSimplex;
			tempNoise.Primitive2D = tempSimplex;
			tempNoise.Distance = false;
			tempNoise.Frequency = TemperatureFrequency;
			tempNoise.Gain = 0.256f;
			tempNoise.OctaveCount = 2;
			//tempNoise.SpectralExponent = -1.1f;
			//	tempNoise.Gain = 2.5f;
			TempNoise = tempNoise;

			var mainLimitNoise = new SimplexPerlin(seed + 20, NoiseQuality.Fast);

			var mainLimitFractal = new SumFractal()
			{
				Primitive3D = mainLimitNoise,
				Primitive2D = mainLimitNoise,
				Frequency = MainNoiseFrequency,
				OctaveCount = 2,
				Lacunarity = MainNoiseLacunarity,
				Gain = MainNoiseGain,
				SpectralExponent = MainNoiseSpectralExponent,
				Offset = MainNoiseOffset,
			};
			var mainScaler = new ScaleableNoise()
			{
				XScale = 1f / MainNoiseScaleX,
				YScale = 1f / MainNoiseScaleY,
				ZScale = 1f / MainNoiseScaleZ,
				Primitive3D = mainLimitFractal,
				Primitive2D = mainLimitFractal
			};
			//ModTurbulence turbulence = new ModTurbulence(mainLimitFractal, new ImprovedPerlin(seed - 350, NoiseQuality.Fast), new ImprovedPerlin(seed + 350, NoiseQuality.Fast), null, 0.0125F);
			_mainNoise = mainScaler; //turbulence;

			var mountainNoise = new SimplexPerlin(seed + 300, NoiseQuality.Fast);
			var mountainTerrain = new HybridMultiFractal()
			{
				Primitive3D = mountainNoise,
				Primitive2D = mountainNoise,
				Frequency = DepthFrequency,
				OctaveCount = 4,
				Lacunarity = DepthLacunarity,
				SpectralExponent = DepthNoiseScaleExponent,
				//Offset = 0.7f,
				Gain = DepthNoiseGain
			};

			ScaleableNoise scaling = new ScaleableNoise();
			scaling.Primitive2D = mountainTerrain;
			scaling.Primitive3D = mountainTerrain;
			scaling.YScale = 1f / HeightScale;
			scaling.XScale = 1f / DepthNoiseScaleX;
			scaling.ZScale = 1f / DepthNoiseScaleZ;

			_depthNoise = scaling;

			BiomeUtils.FixMinMaxHeight();
		}

		private const float TemperatureFrequency = 0.1268f;

		private const float RainFallFrequency = 0.0368f;

		private const float MainNoiseScaleX = 80F;
		private const float MainNoiseScaleY = 160F;
		private const float MainNoiseScaleZ = 80F;
		private const float MainNoiseFrequency = 0.195f;
		private const float MainNoiseLacunarity = 2.127f;
		private const float MainNoiseGain = 2f;
		private const float MainNoiseSpectralExponent = 1f;
		private const float MainNoiseOffset = 1f;

		private const float DepthNoiseScaleX = 200F;
		private const float DepthNoiseScaleZ = 200F;
		private const float DepthFrequency = 0.362f;
		private const float DepthLacunarity = 2.375f;
		private const float DepthNoiseGain = 2f;
		private const float DepthNoiseScaleExponent = 1f;

		private const float CoordinateScale = 684.412F;
		private const float HeightScale = 684.412F;
		private const float BiomeScale = 135.412f;

		public const int WaterLevel = 64;

		public void Initialize()
		{

		}

		public static UInt64 CalculateHash(string read)
		{
			UInt64 hashedValue = 3074457345618258791ul;
			for (int i = 0; i < read.Length; i++)
			{
				hashedValue += read[i];
				hashedValue *= 3074457345618258799ul;
			}
			return hashedValue;
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
			var thresholdMap = GetThresholdMap(chunk.x, chunk.z, biomes);

			CreateTerrainShape(chunk, heightMap, thresholdMap, biomes);
			DecorateChunk(chunk, heightMap, thresholdMap, biomes, chunkDecorators);

			//chunk.isDirty = true;
			//chunk.NeedSave = true;

			/*for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					for (int y = 0; y < 256; y++)
					{
						chunk.SetSkyLight(x, y, z, 255);
					}
				}
			}*/
			//	sw.Stop();

			//	if (sw.ElapsedMilliseconds > previousTime)
			//{
			//	Debug.WriteLine("Chunk gen took " + sw.ElapsedMilliseconds + " ms");
			//previousTime = sw.ElapsedMilliseconds;
			//}

			return chunk;
		}

		private static readonly ILog Log = LogManager.GetLogger(typeof(OverworldGenerator));
		private Biome GetBiome(float x, float z)
		{
			x /= BiomeScale;
			z /= BiomeScale;

			var mX = x + MathF.Abs(BiomeModifierX.GetValue(x, z));
			var mZ = z + MathF.Abs(BiomeModifierZ.GetValue(x, z));

			var temp = TempNoise.GetValue(mX, mZ) * 3.024F;
			var rain = RainNoise.GetValue(mX, mZ);
			var height = BiomeHeightModifier.GetValue(mX, mZ) * 3.1923489f;

			if (temp < -2f) temp = -(temp % 1);
			if (rain < 0) rain = -rain;

			return BiomeUtils.GetBiomes(temp, rain).OrderBy(bx => Math.Abs((bx.Key.MaxHeight/bx.Key.MinHeight) - height)).FirstOrDefault().Key;

			return BiomeUtils.GetBiome(temp, rain);
		}

		private Biome[] CalculateBiomes(int chunkX, int chunkZ)
		{
			//cx *= 16;
			//cz *= 16;

			int minX = (chunkX * 16) - 1;
			int minZ = (chunkZ * 16) - 1;
			var maxX = ((chunkX + 1) << 4) - 1;
			var maxZ = ((chunkZ + 1) << 4) - 1;

			Biome[] rb = new Biome[16 * 16];

			for (int x = 0; x < 16; x++)
			{
				float rx = MathHelpers.Lerp(minX, maxX, (1f / 15f) * x);
				for (int z = 0; z < 16; z++)
				{
					rb[(x << 4) + z] = GetBiome(rx, MathHelpers.Lerp(minZ, maxZ, (1f / 15f) * z));
				}
			}

			return rb;
		}

		private float[] GenerateHeightMap(Biome[] biomes, int chunkX, int chunkZ)
		{
			int minX = (chunkX * 16) - 1;
			int minZ = (chunkZ * 16) - 1;
			var maxX = ((chunkX + 1) << 4) - 1;
			var maxZ = ((chunkZ + 1) << 4) - 1;

			int cx = (chunkX * 16);
			int cz = (chunkZ * 16);

			float q11 = MathHelpers.Abs(WaterLevel + (128f * biomes[0].MaxHeight) * _mainNoise.GetValue(minX, minZ));
			float q12 = MathHelpers.Abs(WaterLevel + (128f * biomes[15].MaxHeight) * _mainNoise.GetValue(minX, maxZ));

			float q21 = MathHelpers.Abs(WaterLevel + (128f * biomes[240].MaxHeight) * _mainNoise.GetValue(maxX, minZ));
			float q22 = MathHelpers.Abs(WaterLevel + (128f * biomes[255].MaxHeight) * _mainNoise.GetValue(maxX, maxZ));

			float highestCorner = Math.Max(q11, Math.Max(q12, Math.Max(q21, q22)));

			float[] heightMap = new float[16 * 16];

			for (int x = 0; x < 16; x++)
			{
				float rx = cx + x;

				for (int z = 0; z < 16; z++)
				{
					float rz = cz + z;

					var baseNoise = MathHelpers.BilinearCmr(
						rx, rz,
						q11,
						q12,
						q21,
						q22,
						minX, maxX, minZ, maxZ);

					if (baseNoise > highestCorner)
					{
						baseNoise = highestCorner;
					}

					heightMap[(x << 4) + z] = baseNoise; //WaterLevel + ((128f * baseNoise));
				}


			}
			return heightMap;
		}

		private float[] GetThresholdMap(int cx, int cz, Biome[] biomes)
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
						thresholdMap[x + ((y + (z << 8)) << 4)] = _depthNoise.GetValue(rx, y, rz);
					}
				}
			}
			return thresholdMap;
		}

		public const float Threshold = -0.1f;
		private const int Width = 16;
		private const int Depth = 16;
		private const int Height = 256;

		private void CreateTerrainShape(ChunkColumn chunk, float[] heightMap, float[] thresholdMap, Biome[] biomes)
		{
			for (int x = 0; x < Width; x++)
			{
				for (int z = 0; z < Depth; z++)
				{
					var idx = (x << 4) + z;
					Biome biome = biomes[idx];
					chunk.biomeId[idx] = (byte)biome.Id;// SetBiome(x, z, (byte)biome.Id);
					float stoneHeight = heightMap[idx];
					/*	if (stoneHeight > 200 || stoneHeight < 0)
						{
							Debug.WriteLine("MaxHeight: " + stoneHeight);
						}*/

					var maxY = 0;
					for (int y = 0; y < stoneHeight && y < 255; y++)
					{
						float density = thresholdMap[x + ((y + (z << 8)) << 4)];

						if (y < WaterLevel || (density > Threshold && y >= WaterLevel))
						{
							chunk.SetBlock(x, y, z, 1);
							maxY = y;
						}
					}

					chunk.SetBlock(x, 0, z, 7); //Bedrock
					heightMap[idx] = maxY;
					chunk.height[idx] = (short)maxY;
					//chunk.SetHeight(x, z, (byte)maxY);
				}
			}
		}

		private void DecorateChunk(ChunkColumn chunk, float[] heightMap, float[] thresholdMap, Biome[] biomes,
			ChunkDecorator[] decorators)
		{
			int cx = (chunk.x * 16);
			int cz = (chunk.z * 16);

			for (int x = 0; x < Width; x++)
			{
				var rx = cx + x;
				for (int z = 0; z < Depth; z++)
				{
					var rz = cz + z;

					var height = chunk.height[(x << 4) + z];
					var biome = biomes[(x << 4) + z];

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
									if (y < 170)
									{
										chunk.SetBlock(x, y, z, biome.SurfaceBlock);
										chunk.SetMetadata(x, y, z, biome.SurfaceMetadata);

										chunk.SetBlock(x, y - 1, z, biome.SoilBlock);
										chunk.SetMetadata(x, y - 1, z, biome.SoilMetadata);
									}
									else if (y < 185 && _detailNoise.GetValue(rx, rz) > 0.25f)
									{
										chunk.SetBlock(x, y, z, biome.SoilBlock);
									}
								}
							}
						}

						for (int i = 0; i < decorators.Length; i++)
						{
							decorators[i].Decorate(chunk, biome, thresholdMap, x, y, z, isSurface, y < height - 5);
						}
					}
				}
			}
		}
	}
}
