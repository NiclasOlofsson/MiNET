using System;
using System.Collections.Generic;
using System.Linq;
using Craft.Net.Anvil;
using Craft.Net.Common;
using Craft.Net.TerrainGeneration;

namespace MiNET.Worlds
{
	public class CraftNetWorldProvider : IWorldProvider
	{
		private List<ChunkColumn> _chunkCache = new List<ChunkColumn>();
		private StandardGenerator _generator;

		public bool IsCaching { get; private set; }


		public CraftNetWorldProvider()
		{
			IsCaching = true;
			_generator = new StandardGenerator { Seed = 1000 };
			_generator.Initialize(null);
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


			Chunk anvilChunk = _generator.GenerateChunk(chunkCoordinates);

			ChunkColumn chunk = new ChunkColumn { x = chunkCoordinates.X, z = chunkCoordinates.Z };

			chunk.biomeId = anvilChunk.Biomes;
			for (int i = 0; i < chunk.biomeId.Length; i++)
			{
				if (chunk.biomeId[i] > 22) chunk.biomeId[i] = 0;
			}
			if (chunk.biomeId.Length > 256) throw new Exception();

			for (int xi = 0; xi < 16; xi++)
			{
				for (int zi = 0; zi < 16; zi++)
				{
					for (int yi = 0; yi < 128; yi++)
					{
						chunk.SetBlock(xi, yi, zi, (byte) anvilChunk.GetBlockId(new Coordinates3D(xi, yi + 45, zi)));
						chunk.SetBlocklight(xi, yi, zi, anvilChunk.GetBlockLight(new Coordinates3D(xi, yi + 45, zi)));
						chunk.SetMetadata(xi, yi, zi, anvilChunk.GetMetadata(new Coordinates3D(xi, yi + 45, zi)));
						chunk.SetSkylight(xi, yi, zi, anvilChunk.GetSkyLight(new Coordinates3D(xi, yi + 45, zi)));
					}
				}
			}

			for (int i = 0; i < chunk.skylight.Length; i++)
				chunk.skylight[i] = 0xff;

			for (int i = 0; i < chunk.biomeColor.Length; i++)
				chunk.biomeColor[i] = 8761930;

			_chunkCache.Add(chunk);

			return chunk;
		}

		public Coordinates3D GetSpawnPoint()
		{
			Vector3 spawnPoint = _generator.SpawnPoint;
			if (spawnPoint.Y > 127) spawnPoint.Y = 127;
			return spawnPoint;
		}

		public int GetBlockId(Coordinates3D blockCoordinates)
		{
			return 0;
		}
	}
}
