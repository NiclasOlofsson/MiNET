using System.Collections.Concurrent;
using MiNET.BlockEntities;
using MiNET.Utils;

namespace MiNET.Worlds
{
	public class FlatlandWorldProvider : IWorldProvider
	{
		private readonly ConcurrentDictionary<ChunkCoordinates, ChunkColumn> _chunkCache = new ConcurrentDictionary<ChunkCoordinates, ChunkColumn>();
		private bool _loadFromFile;
		private bool _saveToFile;

		public bool IsCaching { get; private set; }

		public FlatlandWorldProvider()
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

		public void Initialize()
		{
		}

		public ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates)
		{
			//lock (_chunkCache)
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
					int h = PopulateChunk(chunk);

					chunk.SetBlock(0, h + 1, 0, 7);
					chunk.SetBlock(1, h + 1, 0, 41);
					chunk.SetBlock(2, h + 1, 0, 41);
					chunk.SetBlock(3, h + 1, 0, 41);
					chunk.SetBlock(3, h + 1, 0, 41);

					//chunk.SetBlock(6, h + 1, 6, 57);

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
				}

				_chunkCache[chunkCoordinates] = chunk;

				return chunk;
			}
		}

		public Vector3 GetSpawnPoint()
		{
			return new Vector3(0, 10, 0);
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
			//chunk.biomeColor = ArrayOf<int>.Create(256, random.Next(6761930, 8761930));
//			for (int i = 0; i < chunk.biomeColor.Length; i++)
//			{
//				chunk.biomeColor[i] = random.Next(6761930, 8761930);
//			}
			return h;
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