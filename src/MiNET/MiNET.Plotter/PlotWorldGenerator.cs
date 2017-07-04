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
// The Original Code is Niclas Olofsson.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Plotter
{
	public class PlotWorldGenerator : IWorldGenerator
	{
		public const int PlotWidth = 42;
		public const int PlotDepth = 42;
		public const int PlotHeight = 64;
		public const int RoadWidth = 7;
		public const int PlotAreaWidth = PlotWidth + RoadWidth;
		public const int PlotAreaDepth = PlotDepth + RoadWidth;

		private Pattern RoadPattern { get; set; }
		private Pattern PlotPattern { get; set; }

		public PlotWorldGenerator()
		{
			RoadPattern = new Pattern();
			var gravel = new Gravel();
			var stone = new Stone();
			var andesite = new Stone() {Metadata = 5};
			var dirt = new Dirt();
			var grass = new Grass();
			RoadPattern.BlockList.Add(new Pattern.BlockDataEntry() {Weight = 20, Id = gravel.Id, Metadata = gravel.Metadata});
			RoadPattern.BlockList.Add(new Pattern.BlockDataEntry() {Weight = 10, Id = dirt.Id, Metadata = dirt.Metadata});
			RoadPattern.BlockList.Add(new Pattern.BlockDataEntry() {Weight = 10, Id = andesite.Id, Metadata = andesite.Metadata});
			RoadPattern.BlockList.Add(new Pattern.BlockDataEntry() {Weight = 20, Id = stone.Id, Metadata = stone.Metadata});
			RoadPattern.BlockList.Add(new Pattern.BlockDataEntry() {Weight = 40, Id = grass.Id, Metadata = grass.Metadata});
			RoadPattern.Order();

			PlotPattern = new Pattern();
			PlotPattern.BlockList.Add(new Pattern.BlockDataEntry() {Weight = 70, Id = 0, Metadata = 0});
			PlotPattern.BlockList.Add(new Pattern.BlockDataEntry() {Weight = 27, Id = 31, Metadata = 1});
			PlotPattern.BlockList.Add(new Pattern.BlockDataEntry() {Weight = 1, Id = 37, Metadata = 0});
			PlotPattern.BlockList.Add(new Pattern.BlockDataEntry() {Weight = 1, Id = 38, Metadata = 0});
			//PlotPattern.BlockList.Add(new Pattern.BlockDataEntry() { Weight = 5, Id = 38, Metadata = 1 });
			//PlotPattern.BlockList.Add(new Pattern.BlockDataEntry() { Weight = 5, Id = 38, Metadata = 2 });
			//PlotPattern.BlockList.Add(new Pattern.BlockDataEntry() { Weight = 5, Id = 38, Metadata = 3 });
			//PlotPattern.BlockList.Add(new Pattern.BlockDataEntry() { Weight = 5, Id = 38, Metadata = 4 });
			//PlotPattern.BlockList.Add(new Pattern.BlockDataEntry() { Weight = 5, Id = 38, Metadata = 5 });
			//PlotPattern.BlockList.Add(new Pattern.BlockDataEntry() { Weight = 5, Id = 38, Metadata = 6 });
			//PlotPattern.BlockList.Add(new Pattern.BlockDataEntry() { Weight = 5, Id = 38, Metadata = 7 });
			PlotPattern.BlockList.Add(new Pattern.BlockDataEntry() {Weight = 1, Id = 38, Metadata = 8});
			PlotPattern.Order();
		}

		public void Initialize()
		{
		}

		public ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates)
		{
			ChunkColumn chunk = new ChunkColumn();
			chunk.x = chunkCoordinates.X;
			chunk.z = chunkCoordinates.Z;

			int xOffset = chunk.x << 4;
			int zOffset = chunk.z << 4;

			for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					for (int y = 0; y < PlotHeight + 1; y++)
					{
						if (y == 0) chunk.SetBlock(x, y, z, 7); // Bedrock
						else if (y == PlotHeight - 1) chunk.SetBlock(x, y, z, 2); // grass
						else if (y == PlotHeight)
						{
							if (!IsZRoad(z + zOffset, true) && !IsXRoad(x + xOffset, true))
							{
								var block = PlotPattern.Next(new BlockCoordinates(x, PlotHeight, z));
								chunk.SetBlock(x, y, z, block.Id); // grass
								if (block.Metadata != 0)
								{
									chunk.SetMetadata(x, y, z, block.Metadata); // grass
								}
							}
						}
						else if (y > PlotHeight - 4) chunk.SetBlock(x, y, z, 3); // dirt
						else chunk.SetBlock(x, y, z, 1); // stone
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
						if ((x - i)%PlotAreaWidth == 0)
						{
							chunk.SetBlock(x - xOffset, PlotHeight - 1, z - zOffset, block.Id);
							if (block.Metadata != 0)
								chunk.SetMetadata(x - xOffset, PlotHeight - 1, z - zOffset, block.Metadata);
						}
						if ((z - i)%PlotAreaDepth == 0)
						{
							chunk.SetBlock(x - xOffset, PlotHeight - 1, z - zOffset, block.Id);
							if (block.Metadata != 0)
								chunk.SetMetadata(x - xOffset, PlotHeight - 1, z - zOffset, block.Metadata);
						}
					}

					if (x%PlotAreaWidth == 0 && !IsZRoad(z)) chunk.SetBlock(x - xOffset, PlotHeight, z - zOffset, leaves.Id);
					if ((x - RoadWidth + 1)%PlotAreaWidth == 0 && !IsZRoad(z)) chunk.SetBlock(x - xOffset, PlotHeight, z - zOffset, leaves.Id);

					if (z%PlotAreaDepth == 0 && !IsXRoad(x)) chunk.SetBlock(x - xOffset, PlotHeight, z - zOffset, leaves.Id);
					if ((z - RoadWidth + 1)%PlotAreaDepth == 0 && !IsXRoad(x)) chunk.SetBlock(x - xOffset, PlotHeight, z - zOffset, leaves.Id);

					if (x%PlotAreaWidth == 0 && z%PlotAreaDepth == 0) chunk.SetBlock(x - xOffset, PlotHeight + 1, z - zOffset, new RedstoneBlock().Id);
					if (x%PlotAreaWidth == PlotAreaWidth - 1 && z%PlotAreaDepth == PlotAreaDepth - 1) chunk.SetBlock(x - xOffset, PlotHeight + 1, z - zOffset, new LapisBlock().Id); // stone
				}
			}

			return chunk;
		}

		public static bool IsXRoad(int x, bool all = false)
		{
			bool result = false;
			for (int i = all ? 0 : 1; i < RoadWidth - (all ? 0 : 1); i++)
			{
				result |= (x - i)%PlotAreaWidth == 0;
			}

			return result;
		}

		public static bool IsZRoad(int z, bool all = false)
		{
			bool result = false;
			for (int i = (all ? 0 : 1); i < RoadWidth - (all ? 0 : 1); i++)
			{
				result |= (z - i)%PlotAreaDepth == 0;
			}

			return result;
		}

		public class Pattern
		{
			public class BlockDataEntry
			{
				public byte Id { get; set; }
				public byte Metadata { get; set; }
				public int Weight { get; set; } = 100;
				public int Accumulated { get; set; } = 100;
			}

			private List<BlockDataEntry> _blockList = new List<BlockDataEntry>();
			private Random _random;
			public string OriginalPattern { get; private set; }

			public List<BlockDataEntry> BlockList => _blockList;

			// Used by command handler
			public Pattern()
			{
				_random = new Random((int) DateTime.UtcNow.Ticks);
			}

			public Pattern(int blockId, int metadata)
			{
				BlockList.Add(new BlockDataEntry() {Id = (byte) blockId, Metadata = (byte) metadata});
				OriginalPattern = $"{blockId}:{metadata}";
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
				var blockEntry = GetRandomBlock(_random, BlockList);

				Block block = BlockFactory.GetBlockById(blockEntry.Id);
				block.Metadata = blockEntry.Metadata;
				block.Coordinates = position;

				return block;
			}
		}
	}
}