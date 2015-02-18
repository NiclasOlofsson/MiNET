using System;
using LibNoise;
using LibNoise.Primitive;
using MiNET.Utils;

namespace MiNET.Worlds
{
	internal class SimplexOctaveGenerator
	{
		private readonly long _seed;
		private readonly int _octaves;
		private SimplexPerlin[] _generators;

		public SimplexOctaveGenerator(int seed, int octaves)
		{
			_seed = seed;
			_octaves = octaves;

			_generators = new SimplexPerlin[octaves];
			for (int i = 0; i < _generators.Length; i++)
			{
				_generators[i] = new SimplexPerlin(seed, NoiseQuality.Best);
			}
		}


		public double Noise(double x, double y, double frequency, double amplitude)
		{
			return Noise(x, y, 0, 0, frequency, amplitude, false);
		}

		public double Noise(double x, double y, double z, double frequency, double amplitude)
		{
			return Noise(x, y, z, 0, frequency, amplitude, false);
		}

		public double Noise(double x, double y, double z, double w, double frequency, double amplitude)
		{
			return Noise(x, y, z, w, frequency, amplitude, false);
		}

		public double Noise(double x, double y, double z, double w, double frequency, double amplitude, bool normalized)
		{
			double result = 0;
			double amp = 1;
			double freq = 1;
			double max = 0;

			x *= XScale;
			y *= YScale;
			z *= ZScale;
			w *= WScale;

			foreach (var octave in _generators)
			{
				result += octave.GetValue((float) (x*freq), (float) (y*freq), (float) (z*freq), (float) (w*freq))*amp;
				max += amp;
				freq *= frequency;
				amp *= amplitude;
			}

			if (normalized)
			{
				result /= max;
			}

			return result;
		}

		public double XScale { get; set; }
		public double YScale { get; set; }
		public double ZScale { get; set; }
		public double WScale { get; set; }

		public void SetScale(double scale)
		{
			XScale = scale;
			YScale = scale;
			ZScale = scale;
			WScale = scale;
		}
	}

	public class CoolWorldProvider : IWorldProvider
	{
		public bool IsCaching { get; private set; }

		public void Initialize()
		{
			IsCaching = false;
		}

		public ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates)
		{
			ChunkColumn chunk = new ChunkColumn
			{
				x = chunkCoordinates.X,
				z = chunkCoordinates.Z
			};

			PopulateChunk(chunk);

			return chunk;
		}

		public Vector3 GetSpawnPoint()
		{
			return new Vector3(0, 100, 0);
		}

		public void SaveChunks()
		{
		}

		private void PopulateChunk(ChunkColumn chunk)
		{
			var bottom = new SimplexOctaveGenerator("noise1".GetHashCode(), 8);
			var overhang = new SimplexOctaveGenerator("noise2".GetHashCode(), 8);
			overhang.SetScale(1/64.0);
			bottom.SetScale(1/128.0);

			double overhangsMagnitude = 16;
			double bottomsMagnitude = 32;

			for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					float ox = x + chunk.x*16;
					float oz = z + chunk.z*16;


					int bottomHeight = (int) ((bottom.Noise(ox, oz, 0.5, 0.5)*bottomsMagnitude) + 64.0);
					int maxHeight = (int) ((overhang.Noise(ox, oz, 0.5, 0.5)*overhangsMagnitude) + bottomHeight + 32.0);

					double threshold = 0.0;

					maxHeight = Math.Max(1, maxHeight);

					for (int y = 0; y < maxHeight && y < 128; y++)
					{
						if (y <= 1)
						{
							chunk.SetBlock(x, y, z, 7);
							continue;
						}

						if (y > bottomHeight)
						{
							//part where we do the overhangs
							double density = overhang.Noise(ox, y, oz, 0.5, 0.5);

							if (density > threshold) chunk.SetBlock(x, y, z, (byte) Material.Stone);
						}
						else
						{
							chunk.SetBlock(x, y, z, (byte) Material.Stone);
						}
					}

					//turn the tops into grass
					chunk.SetBlock(x, bottomHeight, z, (byte) Material.Grass); //the top of the base hills
					chunk.SetBlock(x, bottomHeight - 1, z, (byte) Material.Dirt);
					chunk.SetBlock(x, bottomHeight - 2, z, (byte) Material.Dirt);

					for (int y = bottomHeight + 1; y > bottomHeight && y < maxHeight && y < 127; y++)
					{
						//the overhang
						byte thisblock = chunk.GetBlock(x, y, z);
						byte blockabove = chunk.GetBlock(x, y + 1, z);

						if (thisblock != (decimal) Material.Air && blockabove == (decimal) Material.Air)
						{
							chunk.SetBlock(x, y, z, (byte) Material.Grass);
							if (chunk.GetBlock(x, y - 1, z) != (decimal) Material.Air)
								chunk.SetBlock(x, y - 1, z, (byte) Material.Dirt);
							if (chunk.GetBlock(x, y - 2, z) != (decimal) Material.Air)
								chunk.SetBlock(x, y - 2, z, (byte) Material.Dirt);
						}
					}
				}
			}
		}
	}

	// 7 rock
	// 8 water
	// 3 dirt
	// 2 grass
	// 1 stone

	internal enum Material : byte
	{
		Air = 0,
		Stone = 1,
		Grass = 2,
		Dirt = 3,
		Bedrock = 7,
		Gold = 41,
	}
}