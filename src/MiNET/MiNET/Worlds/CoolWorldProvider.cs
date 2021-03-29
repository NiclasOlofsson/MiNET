#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE. 
// The License is based on the Mozilla Public License Version 1.1, but Sections 14 
// and 15 have been added to cover use of software over a computer network and 
// provide for limited attribution for the Original Developer. In addition, Exhibit A has 
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Collections.Concurrent;
using System.Numerics;
using LibNoise;
using LibNoise.Primitive;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Utils.Vectors;

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
				result += octave.GetValue((float) (x * freq), (float) (y * freq), (float) (z * freq), (float) (w * freq)) * amp;
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
		private string _seed = Config.GetProperty("seed", "noise");
		private readonly ConcurrentDictionary<ChunkCoordinates, ChunkColumn> _chunkCache = new ConcurrentDictionary<ChunkCoordinates, ChunkColumn>();
		public bool IsCaching { get; private set; }

		public void Initialize()
		{
			IsCaching = true;
		}

		public ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates, bool cacheOnly = false)
		{
			ChunkColumn cachedChunk;
			if (_chunkCache.TryGetValue(chunkCoordinates, out cachedChunk)) return cachedChunk;

			ChunkColumn chunk = new ChunkColumn
			{
				X = chunkCoordinates.X,
				Z = chunkCoordinates.Z
			};

			PopulateChunk(chunk);
			_chunkCache[chunkCoordinates] = chunk;

			return chunk;
		}

		public Vector3 GetSpawnPoint()
		{
			return new Vector3(0, 100, 0);
		}

		public long GetTime()
		{
			return 0;
		}

		public long GetDayTime()
		{
			return 0;
		}

		public string GetName()
		{
			return "Cool world";
		}

		public int SaveChunks()
		{
			return 0;
		}

		public bool HaveNether()
		{
			return false;
		}

		public bool HaveTheEnd()
		{
			return false;
		}

		private const int WaterLevel = 50;

		private void PopulateChunk(ChunkColumn chunk)
		{
			int trees = new Random().Next(0, 10);
			int[,] treeBasePositions = new int[trees, 2];

			for (int t = 0; t < trees; t++)
			{
				int x = new Random().Next(1, 16);
				int z = new Random().Next(1, 16);
				treeBasePositions[t, 0] = x;
				treeBasePositions[t, 1] = z;
			}

			var bottom = new SimplexOctaveGenerator(_seed.GetHashCode(), 8);
			var overhang = new SimplexOctaveGenerator(_seed.GetHashCode(), 8);
			overhang.SetScale(1 / 64.0);
			bottom.SetScale(1 / 128.0);

			double overhangsMagnitude = 16;
			double bottomsMagnitude = 32;

			for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					float ox = x + chunk.X * 16;
					float oz = z + chunk.Z * 16;


					int bottomHeight = (int) ((bottom.Noise(ox, oz, 0.5, 0.5) * bottomsMagnitude) + 64.0);
					int maxHeight = (int) ((overhang.Noise(ox, oz, 0.5, 0.5) * overhangsMagnitude) + bottomHeight + 32.0);

					double threshold = 0.0;

					maxHeight = Math.Max(1, maxHeight);

					for (int y = 0; y < maxHeight && y < 255; y++)
					{
						if (y <= 1)
						{
							chunk.SetBlock(x, y, z, new Bedrock());
							continue;
						}

						if (y > bottomHeight)
						{
							//part where we do the overhangs
							double density = overhang.Noise(ox, y, oz, 0.5, 0.5);
							if (density > threshold) chunk.SetBlock(x, y, z, new Stone());
						}
						else
						{
							chunk.SetBlock(x, y, z, new Stone());
						}
					}

					//turn the tops into grass
					chunk.SetBlock(x, bottomHeight, z, new Grass()); //the top of the base hills
					chunk.SetBlock(x, bottomHeight - 1, z, new Dirt());
					chunk.SetBlock(x, bottomHeight - 2, z, new Dirt());

					for (int y = bottomHeight + 1; y > bottomHeight && y < maxHeight && y < 255; y++)
					{
						//the overhang
						int thisblock = chunk.GetBlockId(x, y, z);
						int blockabove = chunk.GetBlockId(x, y + 1, z);

						if (thisblock != (decimal) Material.Air && blockabove == (decimal) Material.Air)
						{
							if (chunk.GetBlockId(x, y, z) == (byte) Material.Dirt || chunk.GetBlockId(x, y, z) == (byte) Material.Air || chunk.GetBlockId(x, y, z) == (byte) Material.Stone) chunk.SetBlock(x, y, z, new Grass());
							if (chunk.GetBlockId(x, y - 1, z) != (decimal) Material.Air)
								chunk.SetBlock(x, y - 1, z, new Dirt());
							if (chunk.GetBlockId(x, y - 2, z) != (decimal) Material.Air)
								chunk.SetBlock(x, y - 2, z, new Dirt());
						}
					}

					for (int y = 0; y < WaterLevel; y++)
					{
						//Lake generation
						if (y < WaterLevel)
						{
							if (chunk.GetBlockId(x, y, z) == (decimal) Material.Grass || chunk.GetBlockId(x, y, z) == (decimal) Material.Dirt) //Grass or Dirt?
							{
								if (GetRandomNumber(1, 40) == 1 && y < WaterLevel - 4)
									chunk.SetBlock(x, y, z, new Clay()); //Clay
								else
									chunk.SetBlock(x, y, z, new Sand()); //Sand
							}
							if (chunk.GetBlockId(x, y + 1, z) == (decimal) Material.Air)
							{
								if (y < WaterLevel - 3)
									chunk.SetBlock(x, y + 1, z, new FlowingWater()); //FlowingWater
							}
						}
					}

					for (int y = 0; y < 255; y++)
					{
						int thisblock = chunk.GetBlockId(x, y, z);
						int blockabove = chunk.GetBlockId(x, y + 1, z);
						if (thisblock == (decimal) Material.Grass && blockabove == (decimal) Material.Air && y > WaterLevel)
						{
							//Grass
							if (GetRandomNumber(0, 5) == 1)
							{
								chunk.SetBlock(x, y + 1, z, new Tallgrass {TallGrassType = "tall"});
							}

							//Flowers
							if (GetRandomNumber(0, 65) == 1)
							{
								int meta = GetRandomNumber(0, 8);
								//chunk.SetBlock(x, y + 1, z, 38, (byte) meta);
								chunk.SetBlock(x, y + 1, z, new RedFlower());
							}

							//Trees
							for (int pos = 0; pos < trees; pos++)
							{
								if (treeBasePositions[pos, 0] < 14 && treeBasePositions[pos, 0] > 4 && treeBasePositions[pos, 1] < 14 &&
									treeBasePositions[pos, 1] > 4)
								{
									if (chunk.GetBlockId(treeBasePositions[pos, 0], y + 1, treeBasePositions[pos, 1]) == 2)
									{
										if (y >= bottomHeight)
											GenerateTree(chunk, treeBasePositions[pos, 0], y + 1, treeBasePositions[pos, 1], WoodType.Oak);
									}
								}
							}
						}
					}
				}
			}
		}

		private void GenerateTree(ChunkColumn chunk, int x, int treebase, int z, WoodType woodType)
		{
			//new OakTree().Create(chunk, x, treebase, z);
		}

		private static readonly Random Getrandom = new Random();
		private static readonly object SyncLock = new object();

		private static int GetRandomNumber(int min, int max)
		{
			lock (SyncLock)
			{
				// synchronize
				return Getrandom.Next(min, max);
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

	internal enum WoodType : byte
	{
		Oak = 0,
		Spruce = 1,
		Birch = 2,
		Jungle = 3
	}
}