using System;
using System.Collections.Concurrent;
using System.Numerics;
using MiNET.BlockEntities;
using MiNET.Utils;

namespace MiNET.Worlds
{
	public class FlatlandWorldProvider : IWorldProvider
	{
		private readonly ConcurrentDictionary<ChunkCoordinates, ChunkColumn> _chunkCache = new ConcurrentDictionary<ChunkCoordinates, ChunkColumn>();

		public bool IsCaching { get; private set; }

		public FlatlandWorldProvider()
		{
			IsCaching = true;
		}

		public void Initialize()
		{
		}

		Random rand = new Random();
		public ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates)
		{
			lock (_chunkCache)
			{
				ChunkColumn cachedChunk;
				if (_chunkCache.TryGetValue(chunkCoordinates, out cachedChunk))
				{
					return cachedChunk;
				}

				ChunkColumn chunk = new ChunkColumn();
				chunk.x = chunkCoordinates.X;
				chunk.z = chunkCoordinates.Z;
				//chunk.biomeId = ArrayOf<byte>.Create(256, (byte) rand.Next(0, 37));

				int h = PopulateChunk(chunk);

				chunk.SetBlock(0, h + 1, 0, 7);
				chunk.SetBlock(1, h + 1, 0, 41);
				chunk.SetBlock(2, h + 1, 0, 41);
				chunk.SetBlock(3, h + 1, 0, 41);
				chunk.SetBlock(3, h + 1, 0, 41);

				//chunk.SetBlock(6, h + 1, 6, 57);

				chunk.SetBlock(0, h, 0, 31);
				chunk.SetBlock(0, h, 1, 161);
				chunk.SetBlock(0, h, 2, 18);

				chunk.SetBlock(0, h, 15, 31);
				chunk.SetBlock(0, h, 14, 161);
				chunk.SetBlock(0, h, 13, 18);

				chunk.SetBlock(6, h, 9, 63);
				chunk.SetMetadata(6, h, 9, 12);
				var blockEntity = GetBlockEntity((chunkCoordinates.X*16) + 6, h, (chunkCoordinates.Z*16) + 9);
				chunk.SetBlockEntity(blockEntity.Coordinates, blockEntity.GetCompound());

				if (chunkCoordinates.X == 1 && chunkCoordinates.Z == 1)
				{
					for (int x = 0; x < 10; x++)
					{
						for (int z = 0; z < 10; z++)
						{
							for (int y = h - 2; y < h; y++)
							{
								chunk.SetBlock(x, y, z, 8);
							}
						}
					}
				}

				if (chunkCoordinates.X == 3 && chunkCoordinates.Z == 1)
				{
					for (int x = 0; x < 10; x++)
					{
						for (int z = 0; z < 10; z++)
						{
							for (int y = h - 1; y < h; y++)
							{
								chunk.SetBlock(x, y, z, 10);
							}
						}
					}
				}

				for (int x = 0; x < 16; x++)
				{
					for (int z = 0; z < 16; z++)
					{
						for (int y = 127; y != 0; y--)
						{
							if (chunk.GetBlock(x, y, z) == 0x00)
							{
								chunk.SetSkylight(x, y, z, 0xff);
							}
							else
							{
								chunk.SetSkylight(x, y, z, 0x00);
							}
						}
					}
				}

				// Cache
				chunk.GetBatch();
				_chunkCache[chunkCoordinates] = chunk;

				return chunk;
			}
		}

		public Vector3 GetSpawnPoint()
		{
			return new Vector3(0, 12, 0);
		}

		public long GetTime()
		{
			return 0;
		}

		public int PopulateChunk(ChunkColumn chunk)
		{
			int h = 0;

			var random = new CryptoRandom();
			var stones = new byte[16*16*128];

			for (int i = 0; i < stones.Length; i = i + 128)
			{
				h = 1;

				stones[i] = 7; // Bedrock

				stones[i + h++] = 1; // Stone
				stones[i + h++] = 1; // Stone

				switch (random.Next(0, 20))
				{
					case 0:
						stones[i + h++] = 3; // Dirt
						stones[i + h++] = 3;
						break;
					case 1:
						stones[i + h++] = 1; // Stone
						stones[i + h++] = 1; // Stone
						break;
					case 2:
						stones[i + h++] = 13; // Gravel
						stones[i + h++] = 13; // Gravel
						break;
					case 3:
						stones[i + h++] = 14; // Gold
						stones[i + h++] = 14; // Gold
						break;
					case 4:
						stones[i + h++] = 16; // Cole
						stones[i + h++] = 16; // Cole
						break;
					case 5:
						stones[i + h++] = 56; // Dimond
						stones[i + h++] = 56; // Dimond
						break;
					default:
						stones[i + h++] = 1; // Stone
						stones[i + h++] = 1; // Stone
						break;
				}
				stones[i + h++] = 3; // Dirt
				stones[i + h++] = 2; // Grass
			}

			chunk.blocks = stones;

			return h;
		}

		public void SaveChunks()
		{
		}

		private Sign GetBlockEntity(int x, int y, int z)
		{
			var sign = new Sign
			{
				Text1 = "Test1",
				Coordinates = new BlockCoordinates(x, y, z)
			};

			return sign;
		}
	}
}