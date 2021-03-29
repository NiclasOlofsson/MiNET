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
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Utils.Vectors;

namespace MiNET.Worlds
{
	internal class ExperimentalWorldProvider : IWorldProvider
	{
		public ExperimentalWorldProvider()
		{
			IsCaching = true;
		}

		private readonly ConcurrentDictionary<ChunkCoordinates, ChunkColumn> _chunkCache = new ConcurrentDictionary<ChunkCoordinates, ChunkColumn>();
		public bool IsCaching { get; private set; }

		public void Initialize()
		{
		}

		public ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates, bool cacheOnly = false)
		{
			ChunkColumn cachedChunk;
			if (_chunkCache.TryGetValue(chunkCoordinates, out cachedChunk)) return cachedChunk;

			ChunkColumn chunk = new ChunkColumn();
			chunk.X = chunkCoordinates.X;
			chunk.Z = chunkCoordinates.Z;

			PopulateChunk(chunk);

			_chunkCache[chunkCoordinates] = chunk;

			return chunk;
		}

		public Vector3 GetSpawnPoint()
		{
			return new Vector3(50, 10, 50);
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
			return "Experimental";
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

		private float stoneBaseHeight = 0;
		private float stoneBaseNoise = 0.05f;
		private float stoneBaseNoiseHeight = 4;

		private float stoneMountainHeight = 48;
		private float stoneMountainFrequency = 0.008f;
		private float stoneMinHeight = 0;

		private float dirtBaseHeight = 1;
		private float dirtNoise = 0.004f;
		private float dirtNoiseHeight = 3;
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
					int stoneHeight = (int) Math.Floor(stoneBaseHeight);
					stoneHeight += GetNoise(chunk.X * 16 + x, chunk.Z * 16 + z, stoneMountainFrequency, (int) Math.Floor(stoneMountainHeight));

					if (stoneHeight < stoneMinHeight)
						stoneHeight = (int) Math.Floor(stoneMinHeight);

					stoneHeight += GetNoise(chunk.X * 16 + x, chunk.Z * 16 + z, stoneBaseNoise, (int) Math.Floor(stoneBaseNoiseHeight));

					int dirtHeight = stoneHeight + (int) Math.Floor(dirtBaseHeight);
					dirtHeight += GetNoise(chunk.X * 16 + x, chunk.Z * 16 + z, dirtNoise, (int) Math.Floor(dirtNoiseHeight));

					for (int y = 0; y < 256; y++)
					{
						//float y2 = Get3DNoise(chunk.X*16, y, chunk.Z*16, stoneBaseNoise, (int) Math.Floor(stoneBaseNoiseHeight));
						if (y <= stoneHeight)
						{
							chunk.SetBlock(x, y, z, new Stone());

							//Diamond ore
							if (GetRandomNumber(0, 2500) < 5)
							{
								chunk.SetBlock(x, y, z, new DiamondOre());
							}

							//Coal Ore
							if (GetRandomNumber(0, 1500) < 50)
							{
								chunk.SetBlock(x, y, z, new CoalOre());
							}

							//Iron Ore
							if (GetRandomNumber(0, 2500) < 30)
							{
								chunk.SetBlock(x, y, z, new IronOre());
							}

							//Gold Ore
							if (GetRandomNumber(0, 2500) < 20)
							{
								chunk.SetBlock(x, y, z, new GoldOre());
							}
						}

						if (y < waterLevel) //FlowingWater :)
						{
							if (chunk.GetBlockId(x, y, z) == 2 || chunk.GetBlockId(x, y, z) == 3) //Grass or Dirt?
							{
								if (GetRandomNumber(1, 40) == 5 && y < waterLevel - 4)
									chunk.SetBlock(x, y, z, new Clay()); //Clay
								else
									chunk.SetBlock(x, y, z, new Sand()); //Sand
							}
							if (y < waterLevel - 3)
								chunk.SetBlock(x, y + 1, z, new FlowingWater()); //FlowingWater
						}

						if (y <= dirtHeight && y >= stoneHeight)
						{
							chunk.SetBlock(x, y, z, new Dirt()); //Dirt
							chunk.SetBlock(x, y + 1, z, new Grass()); //Grass Block
							if (y > waterLevel)
							{
								//Grass
								if (GetRandomNumber(0, 5) == 2)
								{
									chunk.SetBlock(x, y + 2, z, new Tallgrass(){TallGrassType = "tall"});
								}

								//flower
								if (GetRandomNumber(0, 65) == 8)
								{
									int meta = GetRandomNumber(0, 8);
									//chunk.SetBlock(x, y + 2, z, 38, (byte) meta);
									chunk.SetBlock(x, y + 2, z, new RedFlower());
								}

								for (int pos = 0; pos < trees; pos++)
								{
									if (treeBasePositions[pos, 0] < 14 && treeBasePositions[pos, 0] > 4 && treeBasePositions[pos, 1] < 14 &&
										treeBasePositions[pos, 1] > 4)
									{
										if (y < waterLevel + 2)
											break;
										if (chunk.GetBlockId(treeBasePositions[pos, 0], y + 1, treeBasePositions[pos, 1]) == 2)
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
							chunk.SetBlock(x, y, z, new Bedrock());
						}
					}
				}
			}
		}

		private void GenerateTree(ChunkColumn chunk, int x, int treebase, int z)
		{
			int treeheight = GetRandomNumber(4, 5);

			chunk.SetBlock(x, treebase + treeheight + 2, z, new Leaves()); //Top leave

			chunk.SetBlock(x, treebase + treeheight + 1, z + 1, new Leaves());
			chunk.SetBlock(x, treebase + treeheight + 1, z - 1, new Leaves());
			chunk.SetBlock(x + 1, treebase + treeheight + 1, z, new Leaves());
			chunk.SetBlock(x - 1, treebase + treeheight + 1, z, new Leaves());

			chunk.SetBlock(x, treebase + treeheight, z + 1, new Leaves());
			chunk.SetBlock(x, treebase + treeheight, z - 1, new Leaves());
			chunk.SetBlock(x + 1, treebase + treeheight, z, new Leaves());
			chunk.SetBlock(x - 1, treebase + treeheight, z, new Leaves());

			chunk.SetBlock(x + 1, treebase + treeheight, z + 1, new Leaves());
			chunk.SetBlock(x - 1, treebase + treeheight, z - 1, new Leaves());
			chunk.SetBlock(x + 1, treebase + treeheight, z - 1, new Leaves());
			chunk.SetBlock(x - 1, treebase + treeheight, z + 1, new Leaves());

			for (int i = 0; i <= treeheight; i++)
			{
				chunk.SetBlock(x, treebase + i, z, new Log());
			}
		}

		private static readonly Random getrandom = new Random();
		private static readonly object syncLock = new object();

		private static int GetRandomNumber(int min, int max)
		{
			lock (syncLock)
			{
				// synchronize
				return getrandom.Next(min, max);
			}
		}

		private static readonly OpenSimplexNoise OpenNoise = new OpenSimplexNoise("a-seed".GetHashCode());

		public static int GetNoise(int x, int z, float scale, int max)
		{
			return (int) Math.Floor((OpenNoise.Evaluate(x * scale, z * scale) + 1f) * (max / 2f));
		}
	}
}