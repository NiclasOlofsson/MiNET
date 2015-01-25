using System.Collections.Generic;
using System.Linq;
using Craft.Net.Common;
using MiNET.Utils;

namespace MiNET.Worlds
{
	public class FlatlandWorldProvider : IWorldProvider
	{
		private readonly List<ChunkColumn> _chunkCache = new List<ChunkColumn>();
		private object _sync = new object();

		public bool IsCaching { get; private set; }

		public FlatlandWorldProvider()
		{
			IsCaching = true;
		}

		public void Initialize()
		{
		}

		public ChunkColumn GenerateChunkColumn(Coordinates2D chunkCoordinates)
		{
			lock (_chunkCache)
			{
				ChunkColumn cachedChunk = _chunkCache.FirstOrDefault(chunk2 => chunk2 != null && chunk2.x == chunkCoordinates.X && chunk2.z == chunkCoordinates.Z);

				if (cachedChunk != null)
				{
					return cachedChunk;
				}

				ChunkColumn chunk = new ChunkColumn();
				chunk.x = chunkCoordinates.X;
				chunk.z = chunkCoordinates.Z;
				PopulateChunk(chunk);

				chunk.SetBlock(0, 5, 0, 7);
				chunk.SetBlock(1, 5, 0, 41);
				chunk.SetBlock(2, 5, 0, 41);
				chunk.SetBlock(3, 5, 0, 41);
				chunk.SetBlock(3, 5, 0, 41);

				if (chunkCoordinates.X == 1 && chunkCoordinates.Z == 1)
				{
					for (int x = 0; x < 16; x++)
					{
						for (int z = 0; z < 16; z++)
						{
							for (int y = 2; y < 4; y++)
							{
								chunk.SetBlock(x, y, z, 8);
							}
						}
					}
				}

                if (chunkCoordinates.X == 3 && chunkCoordinates.Z == 1)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        for (int z = 0; z < 16; z++)
                        {
                            for (int y = 3; y < 4; y++)
                            {
                                chunk.SetBlock(x, y, z, 10);
                            }
                        }
                    }
                }

				_chunkCache.Add(chunk);

				return chunk;
			}
		}

		public Coordinates3D GetSpawnPoint()
		{
			return new Coordinates3D(50, 10, 50);
		}

		public void PopulateChunk(ChunkColumn chunk)
		{
			var random = new CryptoRandom();
			var stones = new byte[16*16*128];

			for (int i = 0; i < stones.Length; i = i + 128)
			{
				stones[i] = 7; // Bedrock
				switch (random.Next(0, 20))
				{
					case 0:
						stones[i + 1] = 3; // Dirt
						stones[i + 2] = 3;
						break;
					case 1:
						stones[i + 1] = 1; // Stone
						stones[i + 2] = 1; // Stone
						break;
					case 2:
						stones[i + 1] = 13; // Gravel
						stones[i + 2] = 13; // Gravel
						break;
					case 3:
						stones[i + 1] = 14; // Gold
						stones[i + 2] = 14; // Gold
						break;
					case 4:
						stones[i + 1] = 16; // Cole
						stones[i + 2] = 16; // Cole
						break;
					case 5:
						stones[i + 1] = 56; // Dimond
						stones[i + 2] = 56; // Dimond
						break;
					default:
						stones[i + 1] = 1; // Stone
						stones[i + 2] = 1; // Stone
						break;
				}
				stones[i + 3] = 2; // Grass
				//int rand = random.Next(0, 10);
				//for (int j = 0; j < rand; j++)
				//{
				//	stones[i + 3 + j] = 41; // Gold
				//}
			}

			chunk.blocks = stones;
			//chunk.biomeColor = ArrayOf<int>.Create(256, random.Next(6761930, 8761930));
//			for (int i = 0; i < chunk.biomeColor.Length; i++)
//			{
//				chunk.biomeColor[i] = random.Next(6761930, 8761930);
//			}
		}
	}
}