using System;
using System.Collections.Concurrent;
using MiNET.Utils;

namespace MiNET.Worlds
{
	class ExperimentalWorldProvider : IWorldProvider
	{
		public ExperimentalWorldProvider()
		{
			IsCaching = true;
#if DEBUG
			_loadFromFile = ConfigParser.GetProperty("load_pe", false);
			_saveToFile = ConfigParser.GetProperty("save_pe", false);
#else
			_loadFromFile = ConfigParser.GetProperty("load_pe", true);
			_saveToFile = ConfigParser.GetProperty("save_pe", true);
#endif
		}

		private readonly ConcurrentDictionary<ChunkCoordinates, ChunkColumn> _chunkCache = new ConcurrentDictionary<ChunkCoordinates, ChunkColumn>();
		public bool IsCaching
		{
			get;
			private set;
		}
		private bool _loadFromFile;
		private bool _saveToFile;


		public void Initialize()
		{
			
		}

		public ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates)
		{
			ChunkColumn cachedChunk;
			if (_chunkCache.TryGetValue(chunkCoordinates, out cachedChunk)) return cachedChunk;

			ChunkColumn chunk = new ChunkColumn();
			chunk.x = chunkCoordinates.X;
			chunk.z = chunkCoordinates.Z;

			bool loaded = false;
			if (_loadFromFile)
			{
				loaded = chunk.TryLoadFromFile();
			}

			if (!loaded)
			{
				PopulateChunk(chunk);
			}

			_chunkCache[chunkCoordinates] = chunk;

			return chunk;
		}

		public Vector3 GetSpawnPoint()
		{
			return new Vector3(50, 10, 50);
		}

		public void SaveChunks()
		{
			if (_saveToFile)
			{
				foreach (ChunkColumn chunkColumn in _chunkCache.Values)
				{
					chunkColumn.SaveChunk();
				}
			}
		}

		float stoneBaseHeight = 0;
		float stoneBaseNoise = 0.05f;
		float stoneBaseNoiseHeight = 4;

		float stoneMountainHeight = 48;
		float stoneMountainFrequency = 0.008f;
		float stoneMinHeight = 0;

		float dirtBaseHeight = 1;
		float dirtNoise = 0.004f;
		float dirtNoiseHeight = 3;
		private int waterLevel = 25;
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

			for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					int stoneHeight = (int)Math.Floor(stoneBaseHeight);
					stoneHeight += GetNoise(chunk.x * 16 + x, chunk.z * 16 + z, stoneMountainFrequency, (int)Math.Floor(stoneMountainHeight));

					if (stoneHeight < stoneMinHeight)
						stoneHeight = (int)Math.Floor(stoneMinHeight);

					stoneHeight += GetNoise(chunk.x * 16 + x, chunk.z * 16 + z, stoneBaseNoise, (int)Math.Floor(stoneBaseNoiseHeight));

					int dirtHeight = stoneHeight + (int)Math.Floor(dirtBaseHeight);
					dirtHeight += GetNoise(chunk.x * 16 + x, chunk.z * 16 + z, dirtNoise, (int)Math.Floor(dirtNoiseHeight));

					for (int y = 0; y < 256; y++)
					{
						//float y2 = Get3DNoise(chunk.X*16, y, chunk.Z*16, stoneBaseNoise, (int) Math.Floor(stoneBaseNoiseHeight));
						if (y <= stoneHeight)
						{
							chunk.SetBlock(x, y, z, 1);

							//Diamond ore
							if (GetRandomNumber(0, 2500) < 5)
							{
								chunk.SetBlock(x, y, z, 56);
							}

							//Coal Ore
							if (GetRandomNumber(0, 1500) < 50)
							{
								chunk.SetBlock(x, y, z, 16);
							}

							//Iron Ore
							if (GetRandomNumber(0, 2500) < 30)
							{
								chunk.SetBlock(x, y, z, 15);
							}

							//Gold Ore
							if (GetRandomNumber(0, 2500) < 20)
							{
								chunk.SetBlock(x, y, z, 14);
							}
						}

						if (y < waterLevel) //FlowingWater :)
						{
							if (chunk.GetBlock(x, y, z) == 2 || chunk.GetBlock(x, y, z) == 3) //Grass or Dirt?
							{
								if (GetRandomNumber(1, 40) == 5 && y < waterLevel - 4)
									chunk.SetBlock(x, y, z, 82); //Clay
								else
									chunk.SetBlock(x, y, z, 12); //Sand
							}
							if (y < waterLevel - 3)
								chunk.SetBlock(x, y + 1, z, 8); //FlowingWater
						}

						if (y <= dirtHeight && y >= stoneHeight)
						{
							chunk.SetBlock(x, y, z, 3); //Dirt
							chunk.SetBlock(x, y + 1, z, 2); //Grass Block
							if (y > waterLevel)
							{
								//Grass
								if (GetRandomNumber(0, 5) == 2)
								{
									chunk.SetBlock(x, y + 2, z, 31);
									chunk.SetMetadata(x,y +2, z, 1);
								}

								//flower
								if (GetRandomNumber(0, 65) == 8)
								{
									int meta = GetRandomNumber(0, 8);
									chunk.SetBlock(x, y + 2, z, 38);
									chunk.SetMetadata(x, y + 2, z, (byte)meta);
								}

								for (int pos = 0; pos < trees; pos++)
								{
									if (treeBasePositions[pos, 0] < 14 && treeBasePositions[pos, 0] > 4 && treeBasePositions[pos, 1] < 14 &&
										treeBasePositions[pos, 1] > 4)
									{
										if (y < waterLevel + 2)
											break;
										if (chunk.GetBlock(treeBasePositions[pos, 0], y + 1, treeBasePositions[pos, 1]) == 2)
										{
											if (y == dirtHeight)
												GenerateTree(chunk, treeBasePositions[pos, 0], y + 1, treeBasePositions[pos, 1]);
										}
									}
								}
							}
						}

						if (y == 0)
						{
							chunk.SetBlock(x, y, z, 7);
						}

					}
				}
			}
		}

		private void GenerateTree(ChunkColumn chunk, int x, int treebase, int z)
		{
			int treeheight = GetRandomNumber(4, 5);

			chunk.SetBlock(x, treebase + treeheight + 2, z, 18); //Top leave

			chunk.SetBlock(x, treebase + treeheight + 1, z + 1, 18);
			chunk.SetBlock(x, treebase + treeheight + 1, z - 1, 18);
			chunk.SetBlock(x + 1, treebase + treeheight + 1, z, 18);
			chunk.SetBlock(x - 1, treebase + treeheight + 1, z, 18);

			chunk.SetBlock(x, treebase + treeheight, z + 1, 18);
			chunk.SetBlock(x, treebase + treeheight, z - 1, 18);
			chunk.SetBlock(x + 1, treebase + treeheight, z, 18);
			chunk.SetBlock(x - 1, treebase + treeheight, z, 18);

			chunk.SetBlock(x + 1, treebase + treeheight, z + 1, 18);
			chunk.SetBlock(x - 1, treebase + treeheight, z - 1, 18);
			chunk.SetBlock(x + 1, treebase + treeheight, z - 1, 18);
			chunk.SetBlock(x - 1, treebase + treeheight, z + 1, 18);

			for (int i = 0; i <= treeheight; i++)
			{
				chunk.SetBlock(x, treebase + i, z, 17);
			}
		}

		private static readonly Random getrandom = new Random();
		private static readonly object syncLock = new object();
		private static int GetRandomNumber(int min, int max)
		{
			lock (syncLock)
			{ // synchronize
				return getrandom.Next(min, max);
			}
		}

		private static readonly OpenSimplexNoise OpenNoise = new OpenSimplexNoise("a-seed".GetHashCode());

		public static int GetNoise(int x, int z, float scale, int max)
		{
			return (int)Math.Floor((OpenNoise.Evaluate(x * scale, z * scale) + 1f) * (max / 2f));
		}
	}
}
