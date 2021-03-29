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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Plotter
{
	public class PlotWorldGenerator : IWorldGenerator
	{
		public const int PlotWidth = 130;
		public const int PlotDepth = 130;
		public const int PlotHeight = 64;
		public const int RoadWidth = 9;
		public const int PlotAreaWidth = PlotWidth + RoadWidth;
		public const int PlotAreaDepth = PlotDepth + RoadWidth;

		private static Pattern RoadPattern { get; set; }
		private static Pattern PlotPattern { get; set; }

		public PlotWorldGenerator()
		{
			RoadPattern = new Pattern();
			var gravel = new Gravel();
			var stone = new Stone();
			var andesite = new Stone() {StoneType = "andesite"};
			var dirt = new Dirt();
			var grass = new Grass();

			RoadPattern.BlockList.Add(new Pattern.BlockDataEntry()
			{
				Weight = 20,
				Block = gravel
			});
			RoadPattern.BlockList.Add(new Pattern.BlockDataEntry()
			{
				Weight = 10,
				Block = dirt
			});
			RoadPattern.BlockList.Add(new Pattern.BlockDataEntry()
			{
				Weight = 10,
				Block = andesite
			});
			RoadPattern.BlockList.Add(new Pattern.BlockDataEntry()
			{
				Weight = 20,
				Block = stone
			});
			RoadPattern.BlockList.Add(new Pattern.BlockDataEntry()
			{
				Weight = 40,
				Block = grass
			});
			RoadPattern.Order();

			PlotPattern = new Pattern();
			PlotPattern.BlockList.Add(new Pattern.BlockDataEntry()
			{
				Weight = 70,
				Block = new Air()
			});
			PlotPattern.BlockList.Add(new Pattern.BlockDataEntry()
			{
				Weight = 27,
				Block = new Tallgrass() {TallGrassType = "tall"}
			});
			PlotPattern.BlockList.Add(new Pattern.BlockDataEntry()
			{
				Weight = 1,
				Block = new YellowFlower()
			});
			PlotPattern.BlockList.Add(new Pattern.BlockDataEntry()
			{
				Weight = 1,
				Block = new RedFlower()
			});
			//PlotPattern.BlockList.Add(new Pattern.BlockDataEntry() { Weight = 5, Id = 38, Metadata = 1 });
			//PlotPattern.BlockList.Add(new Pattern.BlockDataEntry() { Weight = 5, Id = 38, Metadata = 2 });
			//PlotPattern.BlockList.Add(new Pattern.BlockDataEntry() { Weight = 5, Id = 38, Metadata = 3 });
			//PlotPattern.BlockList.Add(new Pattern.BlockDataEntry() { Weight = 5, Id = 38, Metadata = 4 });
			//PlotPattern.BlockList.Add(new Pattern.BlockDataEntry() { Weight = 5, Id = 38, Metadata = 5 });
			//PlotPattern.BlockList.Add(new Pattern.BlockDataEntry() { Weight = 5, Id = 38, Metadata = 6 });
			//PlotPattern.BlockList.Add(new Pattern.BlockDataEntry() { Weight = 5, Id = 38, Metadata = 7 });
			PlotPattern.BlockList.Add(new Pattern.BlockDataEntry()
			{
				Weight = 1,
				Block = new RedFlower() {FlowerType = "oxeye"}
			});
			PlotPattern.Order();
		}

		public void Initialize(IWorldProvider worldProvider)
		{
		}

		public static void ResetBlocks(Level level, BoundingBox bbox, bool repopulate = false)
		{
			bbox = bbox.GetAdjustedBoundingBox();
			ChunkColumn chunk = null;
			Dictionary<ChunkCoordinates, ChunkColumn> chunks = new Dictionary<ChunkCoordinates, ChunkColumn>();
			for (int x = (int) bbox.Min.X; x < (int) bbox.Max.X + 1; x++)
			{
				for (int z = (int) bbox.Min.Z; z < (int) bbox.Max.Z + 1; z++)
				{
					var blockCoord = new BlockCoordinates(x, 0, z);
					ChunkCoordinates chunkCoordinates = (ChunkCoordinates) blockCoord;
					if (chunk == null || chunk.X != chunkCoordinates.X || chunk.Z != chunkCoordinates.Z)
					{
						if (!chunks.TryGetValue(chunkCoordinates, out chunk))
						{
							chunk = level.GetChunk(chunkCoordinates, true);
							chunks[chunkCoordinates] = chunk;
						}
					}

					level.SetBiomeId(blockCoord, 1);
					int height = 255;
					if (chunk != null)
					{
						height = chunk.GetHeight(blockCoord.X & 0x0f, blockCoord.Z & 0x0f);
					}

					for (int y = 0; y < 256; y++)
					{
						if (y == 0)
						{
							var block = new Bedrock {Coordinates = new BlockCoordinates(x, y, z)};
							level.SetBlock(block, applyPhysics: false, calculateLight: false, possibleChunk: chunk); // Bedrock
						}
						else if (repopulate && y == PlotHeight)
						{
							var block = PlotPattern.Next(new BlockCoordinates(x, PlotHeight, z));
							level.SetBlock(block, applyPhysics: false, calculateLight: false, possibleChunk: chunk); // grass
						}
						else if (y >= PlotHeight)
						{
							if (y <= height || !level.IsAir(new BlockCoordinates(x, y, z)))
							{
								var block = new Air {Coordinates = new BlockCoordinates(x, y, z)};
								level.SetBlock(block, applyPhysics: false, calculateLight: false, possibleChunk: chunk); // air
							}
						}
						else if (y == PlotHeight - 1)
						{
							var block = new Grass {Coordinates = new BlockCoordinates(x, y, z)};
							level.SetBlock(block, applyPhysics: false, calculateLight: false, possibleChunk: chunk); // grass
						}
						else if (y > PlotHeight - 4)
						{
							var block = new Dirt {Coordinates = new BlockCoordinates(x, y, z)};
							level.SetBlock(block, applyPhysics: false, calculateLight: false, possibleChunk: chunk); // dirt
						}
						else
						{
							var block = new Stone {Coordinates = new BlockCoordinates(x, y, z)};
							level.SetBlock(block, applyPhysics: false, calculateLight: false, possibleChunk: chunk); // stone
						}
					}
				}
			}
		}

		public static void SetBiome(Level level, BoundingBox bbox, byte biomeId)
		{
			bbox = bbox.GetAdjustedBoundingBox();
			for (int x = (int) bbox.Min.X; x < (int) bbox.Max.X + 1; x++)
			{
				for (int z = (int) bbox.Min.Z; z < (int) bbox.Max.Z + 1; z++)
				{
					level.SetBiomeId(new BlockCoordinates(x, 0, z), biomeId);
				}
			}
		}


		public ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates)
		{
			ChunkColumn chunk = new ChunkColumn();
			chunk.X = chunkCoordinates.X;
			chunk.Z = chunkCoordinates.Z;

			int xOffset = chunk.X << 4;
			int zOffset = chunk.Z << 4;

			for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					for (int y = 0; y < PlotHeight + 1; y++)
					{
						if (y == 0) chunk.SetBlock(x, y, z, new Bedrock()); // Bedrock
						else if (y == PlotHeight - 1)
							chunk.SetBlock(x, y, z, new Grass()); // grass
						else if (y == PlotHeight)
						{
							if (!IsZRoad(z + zOffset, true) && !IsXRoad(x + xOffset, true))
							{
								var block = PlotPattern.Next(new BlockCoordinates(x, PlotHeight, z));
								chunk.SetBlock(x, y, z, block); // pattern
							}
						}
						else if (y > PlotHeight - 4)
							chunk.SetBlock(x, y, z, new Dirt()); // dirt
						else
							chunk.SetBlock(x, y, z, new Stone()); // stone
					}

					chunk.SetHeight(x, z, PlotHeight);
				}
			}


			var leaves = new Leaves();

			//if (xOffset < 0) xOffset -= PlotAreaWidth;
			//if (zOffset < 0) zOffset -= PlotAreaDepth;

			for (int x = xOffset; x < xOffset + 16; x++)
			{
				for (int z = zOffset; z < zOffset + 16; z++)
				{
					for (int i = 1; i < RoadWidth - 1; i++)
					{
						var block = RoadPattern.Next(new BlockCoordinates(x, PlotHeight, z));
						if ((x - i) % PlotAreaWidth == 0)
						{
							chunk.SetBlock(x - xOffset, PlotHeight - 1, z - zOffset, block);
						}

						if ((z - i) % PlotAreaDepth == 0)
						{
							chunk.SetBlock(x - xOffset, PlotHeight - 1, z - zOffset, block);
						}
					}

					if (x % PlotAreaWidth == 0 && !IsZRoad(z)) chunk.SetBlock(x - xOffset, PlotHeight, z - zOffset, leaves);
					if ((x - RoadWidth + 1) % PlotAreaWidth == 0 && !IsZRoad(z)) chunk.SetBlock(x - xOffset, PlotHeight, z - zOffset, leaves);

					if (z % PlotAreaDepth == 0 && !IsXRoad(x)) chunk.SetBlock(x - xOffset, PlotHeight, z - zOffset, leaves);
					if ((z - RoadWidth + 1) % PlotAreaDepth == 0 && !IsXRoad(x)) chunk.SetBlock(x - xOffset, PlotHeight, z - zOffset, leaves);

					//if (x%PlotAreaWidth == 0 && z%PlotAreaDepth == 0) chunk.SetBlock(x - xOffset, PlotHeight + 1, z - zOffset, new RedstoneBlock().Id);
					//if (x%PlotAreaWidth == PlotAreaWidth - 1 && z%PlotAreaDepth == PlotAreaDepth - 1) chunk.SetBlock(x - xOffset, PlotHeight + 1, z - zOffset, new LapisBlock().Id); // stone
				}
			}

			return chunk;
		}

		public static bool IsXRoad(int x, bool all = false)
		{
			bool result = false;
			for (int i = all ? 0 : 1; i < RoadWidth - (all ? 0 : 1); i++)
			{
				result |= (x - i) % PlotAreaWidth == 0;
			}

			return result;
		}

		public static bool IsZRoad(int z, bool all = false)
		{
			bool result = false;
			for (int i = (all ? 0 : 1); i < RoadWidth - (all ? 0 : 1); i++)
			{
				result |= (z - i) % PlotAreaDepth == 0;
			}

			return result;
		}

		public class Pattern
		{
			public class BlockDataEntry
			{
				public Block Block { get; set; }
				public int Weight { get; set; } = 100;
				public int Accumulated { get; set; } = 100;
			}

			private List<BlockDataEntry> _blockList = new List<BlockDataEntry>();
			private Random _random;

			public List<BlockDataEntry> BlockList => _blockList;

			// Used by command handler
			public Pattern()
			{
				_random = new Random();
			}

			public Pattern(Block block)
			{
				BlockList.Add(new BlockDataEntry {Block = block});
			}

			private BlockDataEntry GetRandomBlock(Random random, List<BlockDataEntry> blocksa)
			{
				var blocks = blocksa.OrderBy(entry => entry.Accumulated).ToList();

				if (blocks.Count == 1) return blocks[0];

				double value = random.Next(blocks.Last().Accumulated + 1);

				return blocks.First(entry => value <= entry.Accumulated);
			}

			public void Order()
			{
				int acc = 0;
				foreach (var entry in _blockList.OrderBy(entry => entry.Weight))
				{
					acc += entry.Weight;
					entry.Accumulated = acc;
				}

				_blockList = _blockList.OrderBy(entry => entry.Accumulated).ToList();
			}

			public Block Next(BlockCoordinates position)
			{
				BlockDataEntry blockEntry = GetRandomBlock(_random, BlockList);
				return blockEntry.Block;
			}
		}
	}
}