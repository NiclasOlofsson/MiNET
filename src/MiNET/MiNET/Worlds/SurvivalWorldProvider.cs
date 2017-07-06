using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Principal;
using LibNoise;
using LibNoise.Combiner;
using LibNoise.Filter;
using LibNoise.Modifier;
using LibNoise.Primitive;
using LibNoise.Transformer;
using MiNET.Utils;
using MiNET.Worlds.Decorators;

namespace MiNET.Worlds
{
	internal sealed class ScaleableNoise : IModule, IModule2D, IModule3D
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
			return Primitive3D.GetValue(x*XScale, y*YScale, z*ZScale);
		}
	}

	public class SurvivalWorldProvider : IWorldProvider, ICachingWorldProvider
	{
		private IModule3D RainNoise { get; }
		private IModule3D TempNoise { get; }

		//private readonly FastNoise _mainNoise;
		private readonly IModule2D _mainNoise;
		private readonly ScalePoint _depthNoise;

		private int Seed { get; }
		public SurvivalWorldProvider()
		{
			IsCaching = true;
			int seed = Config.GetProperty("seed", "gottaloveMiNET").GetHashCode();
			Seed = seed;

			var rainNoise = new Voronoi();
			rainNoise.Primitive3D = new SimplexPerlin(seed, NoiseQuality.Fast);
			rainNoise.Distance = false;
			rainNoise.Frequency = RainFallFrequency;
			rainNoise.OctaveCount = 2;
			//rainNoise.Displacement = 0.5f;
			//rainNoise.Lacunarity = 3;

			RainNoise = rainNoise;

			var tempNoise = new Voronoi();
			tempNoise.Primitive3D = new SimplexPerlin(-seed, NoiseQuality.Fast);
			tempNoise.Distance = false;
			tempNoise.Frequency = TemperatureFrequency;
			tempNoise.OctaveCount = 2;
		//	tempNoise.Displacement = 0.5f;
		//	tempNoise.Lacunarity = 4.2f;

			TempNoise = tempNoise;

			//CLSimplexPerlin.Init();
			var mainLimitNoise = new SimplexPerlin(seed / 8, NoiseQuality.Fast);
			//var mainLimitNoise = new CLSimplexPerlin();
			var mainLimitFractal = new SumFractal()
			{
			//	Primitive4D = mainLimitNoise,
				Primitive3D = mainLimitNoise,
				Primitive2D = mainLimitNoise,
				//Primitive1D = mainLimitNoise,
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

			var groundGradient = new ImprovedPerlin(seed * 2, NoiseQuality.Fast);
			
			var mountainTerrain = new SumFractal()
			{
				Primitive3D = groundGradient,
				Frequency = 2.75f,
				OctaveCount = 2,
				Lacunarity = 6f,
				//SpectralExponent = DepthNoiseScaleExponent
			};

			/*Turbulence groundTurbulence = new Turbulence(groundGradient);
			groundTurbulence.YDistortModule = mountainTerrain;
			groundTurbulence.XDistortModule = mountainTerrain;
			groundTurbulence.ZDistortModule = mountainTerrain;
			groundTurbulence.Power = 0.056f; */

			ScalePoint scaling = new ScalePoint(mountainTerrain);
			scaling.YScale = 1f / HeightScale;
			scaling.XScale = 1f/DepthNoiseScaleX;
			scaling.ZScale = 1f/DepthNoiseScaleZ;

			_depthNoise = scaling;
			//groundShape.Primitive2D = groundGradient;

			//RidgedMultiFractal mountainTerrain = new RidgedMultiFractal();
		}

		private readonly ConcurrentDictionary<ChunkCoordinates, ChunkColumn> _chunkCache =
			new ConcurrentDictionary<ChunkCoordinates, ChunkColumn>();

		public bool IsCaching { get; private set; }

		public void Initialize()
		{
		}

		public ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates)
		{
			ChunkColumn cachedChunk;
			if (_chunkCache.TryGetValue(chunkCoordinates, out cachedChunk)) return cachedChunk;

			ChunkColumn chunk = new ChunkColumn
			{
				x = chunkCoordinates.X,
				z = chunkCoordinates.Z
			};

			List<ChunkDecorator> chunkDecorators = new List<ChunkDecorator>();
			chunkDecorators.Add(new WaterDecorator());
			//chunkDecorators.Add(new CaveDecorator());
			chunkDecorators.Add(new OreDecorator());
			chunkDecorators.Add(new FoliageDecorator());

			var biomes = CalculateBiomes(chunk.x, chunk.z);

			foreach (var i in chunkDecorators)
			{
				i.SetSeed(Seed);
			}

			var heightMap = GenerateHeightMap(biomes, chunk.x, chunk.z);
			var thresholdMap = GetThresholdMap(chunk.x, chunk.z);
			PopulateChunk(chunk, heightMap, thresholdMap, biomes, chunkDecorators.ToArray());
			chunk.isDirty = true;

			for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					for (int y = 0; y < 256; y++)
						chunk.SetSkyLight(x, y, z, 255);
				}
			}
			_chunkCache[chunkCoordinates] = chunk;

			return chunk;
		}

		public Vector3 GetSpawnPoint()
		{
			return new Vector3(50, 256f, 50);
		}

		public long GetTime()
		{
			return 0;
		}

		public string GetName()
		{
			return "Survival";
		}

		public int SaveChunks()
		{
			return 0;
		}

		private const float TemperatureFrequency = 0.0083f;
		private const float TemperatureAmplitude = 0.03f;

		private const float RainFallFrequency = 0.0083f;
		private const float RainFallAmplitude = 0.03f;

		private const float BiomeNoiseScale = 6.5f;

		private const float UpperLimitScale = 512F;
		private const float LowerLimitScale = 512F;

		private const float MainNoiseScaleX = 80F;
		private const float MainNoiseScaleY = 160F;
		private const float MainNoiseScaleZ = 80F;

		private const float DepthNoiseScaleX = 200F;
		private const float DepthNoiseScaleZ = 200F;
		private const float DepthNoiseScaleExponent = 0.5F;

		private const float CoordinateScale = 684.412F;
		private const float HeightScale = 684.412F;

		private const float HeightStretch = 12f;

		private const int DirtBaseHeight = 1;
		public const int WaterLevel = 64;

		private Biome GetBiome(int x, int z)
		{
			float temp = TempNoise.GetValue(x/BiomeNoiseScale, 0, z/BiomeNoiseScale).Normalize(1, -1f, 2f, -1f);

			float rain = RainNoise.GetValue(x/BiomeNoiseScale, 0, z/BiomeNoiseScale) /2f + 0.5f;

			var biomes = BiomeUtils.GetBiomes(temp, rain);
			return biomes[0];
		}

		private Biome[] CalculateBiomes(int cx, int cz)
		{
			cx *= 16;
			cz *= 16;

			Biome[] rb = new Biome[16*16];

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

		//private 
		private float[] GenerateHeightMap(Biome[] biomes, int cx, int cz)
		{
			cx *= 16;
			cz *= 16;
/*
				float q11 = _mainNoise.GetValue(cx, cz).Normalize(-1f, 1f, biomes[0].MinHeight, biomes[0].MaxHeight);
				float q12 = _mainNoise.GetValue(cx, cz + 16).Normalize(-1f, 1f, biomes[15].MinHeight, biomes[15].MaxHeight);

				float q21 = _mainNoise.GetValue(cx + 16, cz).Normalize(-1f, 1f, biomes[240].MinHeight, biomes[240].MaxHeight);
				float q22 = _mainNoise.GetValue(cx + 16, cz + 16).Normalize(-1f, 1f, biomes[255].MinHeight, biomes[255].MaxHeight);*/
			

				float q11 = WaterLevel + (128f * biomes[0].MaxHeight) *  _mainNoise.GetValue(cx, cz) ;
				float q12 = WaterLevel + (128f * biomes[15].MaxHeight) * _mainNoise.GetValue(cx, cz + 16);

				float q21 = WaterLevel + (128f * biomes[240].MaxHeight) * _mainNoise.GetValue(cx + 16, cz);
				float q22 = WaterLevel + (128f * biomes[255].MaxHeight) * _mainNoise.GetValue(cx + 16, cz + 16); 

			float[] heightMap = new float[16*16];

			for (int x = 0; x < 16; x++)
			{
				int rx = cx + x;

				for (int z = 0; z < 16; z++)
				{
					int rz = cz + z;

					var baseNoise = Interpolation.Interpolate(
						InterpolationMethod.Cubic, 
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
						thresholdMap[x + 16*(y + 256*z)] = _depthNoise.GetValue(rx, y, rz);
					}
				}
			}
			return thresholdMap;
		}

		public const float Threshold = 0f;
		private void PopulateChunk(ChunkColumn chunk, float[] heightMap, float[] thresholdMap, Biome[] biomes,
			ChunkDecorator[] decorators)
		{
			for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					Biome biome = biomes[(x << 4) + z];
					chunk.SetBiome(x, z, (byte) biome.Id);

					float stoneHeight =  heightMap[(x << 4) + z];
					//var modifier = Math.Abs(biome.MaxHeight - biome.MinHeight);

					var maxY = 0;
					for (int y = 0; y < 256; y++)
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
						/*else if (y > stoneHeight/* && y <= stoneHeight + (modifier * 32f))
						{
							if (density > y * 0.002f)
							{
								chunk.SetBlock(x,y,z,1);
								maxY = y;
							}
						}*/
					}

					chunk.SetBlock(x, 0, z, 7); //Bedrock
					heightMap[(x << 4) + z] = maxY;
					chunk.SetHeight(x,z, (byte) maxY);
				}
			}

			for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					var height = heightMap[(x << 4) + z];
					var biome = biomes[(x << 4) + z];

					for (int y = 0; y < 256; y++)
					{
					//	float density = thresholdMap[x + 16 * (y + 256 * z)];

						bool isSurface = false;
						if (y <= height)
						{
							if (/*density > 0 &&*/ y < 255 && chunk.GetBlock(x,y,z) == 1 && chunk.GetBlock(x, y + 1, z) == 0)
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

						foreach (var dec in decorators)
						{
							dec.Decorate(chunk, biome, thresholdMap, x, y, z, isSurface, y < height - 1);
						}
					}
				}
			}
		}
		public ChunkColumn[] GetCachedChunks()
		{
			return _chunkCache.Values.ToArray();
		}

		public void ClearCachedChunks()
		{
			_chunkCache.Clear();
		}
	}
}
