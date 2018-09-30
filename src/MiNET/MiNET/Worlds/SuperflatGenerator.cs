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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Numerics;
using MiNET.Blocks;
using MiNET.Config;
using MiNET.Config.Contracts;
using MiNET.Utils;

namespace MiNET.Worlds
{
	public class SuperflatGenerator : IWorldGenerator
	{
		private static readonly IWorldConfiguration WorldConfig = ConfigurationProvider.MiNetConfiguration.World;
		public string Seed { get; set; }
		public List<Block> BlockLayers { get; set; }
		public Dimension Dimension { get; set; }

		public SuperflatGenerator(Dimension dimension)
		{
			Dimension = dimension;
			switch (dimension)
			{
				case Dimension.Overworld:
					Seed = WorldConfig.SuperflatOverworldSeed;
					break;
				case Dimension.Nether:
					Seed = WorldConfig.SuperflatNetherSeed;
					break;
				case Dimension.TheEnd:
					Seed = WorldConfig.SuperflatTheEndSeed;
					break;
			}
		}

		public void Initialize()
		{
			BlockLayers = ParseSeed(Seed);
		}

		public ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates)
		{
			ChunkColumn chunk = new ChunkColumn();
			chunk.x = chunkCoordinates.X;
			chunk.z = chunkCoordinates.Z;

			PopulateChunk(chunk);

			Random random = new Random((chunk.x*397) ^ chunk.z);
			if (random.NextDouble() > 0.99)
			{
				GenerateLake(random, chunk, Dimension == Dimension.Overworld ? new Water() : Dimension == Dimension.Nether ? (Block) new Lava() : new Air());
			}
			else if (random.NextDouble() > 0.97)
			{
				GenerateGlowStone(random, chunk);
			}

			return chunk;
		}

		private void GenerateGlowStone(Random random, ChunkColumn chunk)
		{
			if (Dimension != Dimension.Nether) return;

			int h = FindGroundLevel();

			if (h < 0) return;

			Vector2 center = new Vector2(7, 8);

			for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					Vector2 v = new Vector2(x, z);
					if (random.Next((int) Vector2.DistanceSquared(center, v)) < 1)
					{
						chunk.SetBlock(x, BlockLayers.Count - 2, z, new Glowstone().Id);
						if(random.NextDouble() > 0.85)
						{
							chunk.SetBlock(x, BlockLayers.Count - 3, z, new Glowstone().Id);
							if (random.NextDouble() > 0.50)
							{
								chunk.SetBlock(x, BlockLayers.Count - 4, z, new Glowstone().Id);
							}
						}
					}
				}
			}
		}

		private void GenerateLake(Random random, ChunkColumn chunk, Block block)
		{
			int h = FindGroundLevel();

			if (h < 0) return;

			Vector2 center = new Vector2(7, 8);

			for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					Vector2 v = new Vector2(x, z);
					if (random.Next((int) Vector2.DistanceSquared(center, v)) < 4)
					{
						if (Dimension == Dimension.Overworld)
						{
							chunk.SetBlock(x, h, z, block.Id);
						}
						else if (Dimension == Dimension.Nether)
						{
							chunk.SetBlock(x, h, z, block.Id);

							if (random.Next(30) == 0)
							{
								for (int i = h; i < BlockLayers.Count - 1; i++)
								{
									chunk.SetBlock(x, i, z, block.Id);
								}
							}
						}
						else if (Dimension == Dimension.TheEnd)
						{
							for (int i = 0; i < BlockLayers.Count; i++)
							{
								chunk.SetBlock(x, i, z, 0);
							}
						}
					}
					else if (Dimension == Dimension.TheEnd && random.Next((int) Vector2.DistanceSquared(center, v)) < 15)
					{
						chunk.SetBlock(x, h, z, 0);
					}
				}
			}
		}

		private int FindGroundLevel()
		{
			int h = 0;
			bool foundSolid = false;
			foreach (var block in BlockLayers)
			{
				if (foundSolid && block is Air) return h - 1;

				if (block.IsSolid) foundSolid = true;

				h++;
			}

			return foundSolid ? h - 1 : -1;
		}

		public void PopulateChunk(ChunkColumn chunk)
		{
			var layers = BlockLayers;

			for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					int h = 0;

					foreach (var layer in layers)
					{
						chunk.SetBlock(x, h, z, layer.Id);
						chunk.SetMetadata(x, h, z, layer.Metadata);
						h++;
					}

					chunk.SetHeight(x, z, (short) h);
					for (int i = h + Dimension == Dimension.Overworld ? 1 : 0; i >= 0; i--)
					{
						chunk.SetSkyLight(x, i, z, 0);
					}

					// need to take care of skylight for non overworld to make it 0.

					chunk.SetBiome(x, z, 1); // use pattern for this
				}
			}
		}

		public static List<Block> ParseSeed(string inputSeed)
		{
			if (string.IsNullOrEmpty(inputSeed)) return new List<Block>();

			var blocks = new List<Block>();

			var components = inputSeed.Split(';');

			var blockPattern = components[1].Split(',');
			foreach (var pattern in blockPattern)
			{
				var countAndBlock = pattern.Replace("minecraft:", "").Split('*');

				var blockAndMeta = countAndBlock[0].Split(':');
				int count = 1;
				if (countAndBlock.Length > 1)
				{
					count = int.Parse(countAndBlock[0]);
					blockAndMeta = countAndBlock[1].Split(':');
				}

				if (blockAndMeta.Length == 0) continue;

				Block block;

				if (byte.TryParse(blockAndMeta[0], out byte id))
				{
					block = BlockFactory.GetBlockById(id);
				}
				else
				{
					block = BlockFactory.GetBlockByName(blockAndMeta[0]);
				}

				if (blockAndMeta.Length > 1 && byte.TryParse(blockAndMeta[1], out byte meta))
				{
					block.Metadata = meta;
				}

				if (block != null)
				{
					for (int i = 0; i < count; i++)
					{
						blocks.Add(block);
					}
				}
				else
				{
					throw new Exception($"Expected block, but didn't fine one for pattern {pattern}, {string.Join("^", blockAndMeta)} ");
				}
			}

			return blocks;
		}
	}
}