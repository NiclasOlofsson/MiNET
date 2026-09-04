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
			var andesite = new Andesite();
			var dirt = new Dirt();
			var grass = new GrassBlock();

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
				Block = new ShortGrass()
			});
			PlotPattern.BlockList.Add(new Pattern.BlockDataEntry()
			{
				Weight = 1,
				Block = new Dandelion()
			});
			PlotPattern.BlockList.Add(new Pattern.BlockDataEntry()
			{
				Weight = 1,
				Block = new Poppy()
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
				Block = new OxeyeDaisy()
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

			// The runtime id each layer wants, resolved once. Every block already sitting on its
			// target is skipped, so a clear costs one packet per block that actually changes
			// instead of one per block in the plot.
			int bedrockId = BlockFactory.GetDefaultState("minecraft:bedrock").RuntimeId;
			int airId = BlockFactory.AirRuntimeId;
			int grassId = BlockFactory.GetDefaultState("minecraft:grass_block").RuntimeId;
			int dirtId = BlockFactory.GetDefaultState("minecraft:dirt").RuntimeId;
			int stoneId = BlockFactory.GetDefaultState("minecraft:stone").RuntimeId;

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

					for (int y = 0; y < 256; y++)
					{
						int current = chunk?.GetBlockRuntimeId(x & 0x0f, y, z & 0x0f) ?? -1;

						if (repopulate && y == PlotHeight)
						{
							Block pattern = PlotPattern.Next(new BlockCoordinates(x, PlotHeight, z));
							if (pattern.GetRuntimeId() == current) continue;

							level.SetBlock(pattern, applyPhysics: false, calculateLight: false, possibleChunk: chunk);
							continue;
						}

						int wanted =
							y == 0 ? bedrockId :
							y >= PlotHeight ? airId :
							y == PlotHeight - 1 ? grassId :
							y > PlotHeight - 4 ? dirtId :
							stoneId;

						if (wanted == current) continue;

						Block block = BlockFactory.GetBlockByRuntimeId(wanted);
						block.Coordinates = new BlockCoordinates(x, y, z);
						level.SetBlock(block, applyPhysics: false, calculateLight: false, possibleChunk: chunk);
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


		// Every column shares the same bedrock/stone/dirt/grass body; roads and decoration only
		// touch the top two Y levels. So the body is built once and cloned per chunk, and only
		// the surface cells are stamped per column. The per-block object path cost 6.8ms per
		// column, 87 CPU-seconds for a radius-64 disc; a clone plus a few hundred surface
		// writes is microseconds.
		private static readonly Lazy<ChunkColumn> BaseColumn = new Lazy<ChunkColumn>(() =>
		{
			int bedrock = new Bedrock().GetRuntimeId();
			int stone = new Stone().GetRuntimeId();
			int dirt = new Dirt().GetRuntimeId();
			int grass = new GrassBlock().GetRuntimeId();

			var column = new ChunkColumn();
			for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					column.SetBlockByRuntimeId(x, 0, z, bedrock);
					for (int y = 1; y <= PlotHeight - 4; y++) column.SetBlockByRuntimeId(x, y, z, stone);
					column.SetBlockByRuntimeId(x, PlotHeight - 3, z, dirt);
					column.SetBlockByRuntimeId(x, PlotHeight - 2, z, dirt);
					column.SetBlockByRuntimeId(x, PlotHeight - 1, z, grass);
					column.SetHeight(x, z, PlotHeight);
				}
			}
			return column;
		});

		public ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates)
		{
			ChunkColumn chunk = (ChunkColumn) BaseColumn.Value.Clone();
			chunk.X = chunkCoordinates.X;
			chunk.Z = chunkCoordinates.Z;

			int xOffset = chunk.X << 4;
			int zOffset = chunk.Z << 4;

			for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					// The random plot-surface pattern stays a live roll per column, so decoration
					// keeps its variety instead of repeating with the template.
					if (!IsZRoad(z + zOffset, true) && !IsXRoad(x + xOffset, true))
					{
						var block = PlotPattern.Next(new BlockCoordinates(x, PlotHeight, z));
						chunk.SetBlock(x, PlotHeight, z, block);
					}
				}
			}

			var leaves = new OakLeaves();

			for (int x = xOffset; x < xOffset + 16; x++)
			{
				for (int z = zOffset; z < zOffset + 16; z++)
				{
					for (int i = 1; i < RoadWidth - 1; i++)
					{
						if ((x - i) % PlotAreaWidth == 0 || (z - i) % PlotAreaDepth == 0)
						{
							var block = RoadPattern.Next(new BlockCoordinates(x, PlotHeight, z));
							chunk.SetBlock(x - xOffset, PlotHeight - 1, z - zOffset, block);
						}
					}

					if (x % PlotAreaWidth == 0 && !IsZRoad(z)) chunk.SetBlock(x - xOffset, PlotHeight, z - zOffset, leaves);
					if ((x - RoadWidth + 1) % PlotAreaWidth == 0 && !IsZRoad(z)) chunk.SetBlock(x - xOffset, PlotHeight, z - zOffset, leaves);

					if (z % PlotAreaDepth == 0 && !IsXRoad(x)) chunk.SetBlock(x - xOffset, PlotHeight, z - zOffset, leaves);
					if ((z - RoadWidth + 1) % PlotAreaDepth == 0 && !IsXRoad(x)) chunk.SetBlock(x - xOffset, PlotHeight, z - zOffset, leaves);
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