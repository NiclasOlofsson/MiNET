using System.Collections.Generic;
using System.Linq;
using Craft.Net.Common;

namespace MiNET.Network
{
	public class FlatlandWorldProvider : IWorldProvider
	{
		private List<ChunkColumn> _chunkCache = new List<ChunkColumn>();

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
			var firstOrDefault = _chunkCache.FirstOrDefault(chunk2 => chunk2 != null && chunk2.x == chunkCoordinates.X && chunk2.z == chunkCoordinates.Z);
			if (firstOrDefault != null)
			{
				return firstOrDefault;
			}

			FlatlandWorldProvider generator = new FlatlandWorldProvider();
			ChunkColumn chunk = new ChunkColumn();
			chunk.x = chunkCoordinates.X;
			chunk.z = chunkCoordinates.Z;
			generator.PopulateChunk(chunk);

			_chunkCache.Add(chunk);

			return chunk;
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
			}

			chunk.blocks = stones;

			//chunk.SetBlock(0, 0, 0, 9); // water
			//chunk.SetBlock(1, 1, 1, 10); // lava
			//chunk.SetBlock(2, 2, 2, 9); // lava
			//chunk.SetBlock(15, 127, 15, 9); // lava
		}
	}
}
