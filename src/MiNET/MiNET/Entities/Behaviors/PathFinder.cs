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
using System.Linq;
using System.Numerics;
using AStarNavigator;
using AStarNavigator.Algorithms;
using AStarNavigator.Providers;
using log4net;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities.Behaviors
{
	public class PathFinder
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (PathFinder));
		private Dictionary<Tile, Block> _blockCache = new Dictionary<Tile, Block>();

		public List<Tile> FindPath(Entity source, Entity target, double distance)
		{
			return FindPath(source, (BlockCoordinates) target.KnownPosition, distance);
		}

		public List<Tile> FindPath(Entity source, BlockCoordinates target, double distance)
		{
			try
			{
				//new EmptyBlockedProvider(), // Instance of: IBockedProvider

				var navigator = new TileNavigator(
					new LevelNavigator(source, source.Level, distance, _blockCache),
					new BlockDiagonalNeighborProvider(source.Level, (int) source.KnownPosition.Y, _blockCache), // Instance of: INeighborProvider
					new BlockPythagorasAlgorithm(_blockCache), // Instance of: IDistanceAlgorithm
					new ManhattanHeuristicAlgorithm() // Instance of: IDistanceAlgorithm
				);

				BlockCoordinates targetPos = target;
				BlockCoordinates sourcePos = (BlockCoordinates) source.KnownPosition;
				var from = new Tile(sourcePos.X, sourcePos.Z);
				var to = new Tile(targetPos.X, targetPos.Z);

				return navigator.Navigate(from, to)?.ToList() ?? new List<Tile>();
			}
			catch (Exception e)
			{
				Log.Error("Navigate", e);
			}

			return new List<Tile>();
		}

		public Block GetBlock(Tile tile)
		{
			Block block;
			if (!_blockCache.TryGetValue(tile, out block))
			{
				// Do something?
				return null;
			}

			return block;
		}
	}

	public class BlockPythagorasAlgorithm : IDistanceAlgorithm
	{
		private readonly Dictionary<Tile, Block> _blockCache;

		public BlockPythagorasAlgorithm(Dictionary<Tile, Block> blockCache)
		{
			_blockCache = blockCache;
		}

		public double Calculate(Tile from, Tile to)
		{
			Vector3 vFrom = GetBlock(from).Coordinates;
			Vector3 vTo = GetBlock(to).Coordinates;
			return (vFrom - vTo).Length();
		}

		public Block GetBlock(Tile tile)
		{
			Block block;
			if (!_blockCache.TryGetValue(tile, out block))
			{
				// Do something?
				return null;
			}

			return block;
		}
	}


	public class BlockDiagonalNeighborProvider : INeighborProvider
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (BlockDiagonalNeighborProvider));

		private readonly Level _level;
		private readonly int _startY;
		private readonly Dictionary<Tile, Block> _blocks;

		public BlockDiagonalNeighborProvider(Level level, int startY, Dictionary<Tile, Block> blocks)
		{
			_level = level;
			_startY = startY;
			_blocks = blocks;
		}

		private static readonly double[,] Neighbors = new double[,]
		{
			{
				0.0,
				-1.0
			},
			{
				1.0,
				0.0
			},
			{
				0.0,
				1.0
			},
			{
				-1.0,
				0.0
			},
			{
				-1.0,
				-1.0
			},
			{
				1.0,
				-1.0
			},
			{
				1.0,
				1.0
			},
			{
				-1.0,
				1.0
			}
		};

		public IEnumerable<Tile> GetNeighbors(Tile tile)
		{
			Block block;
			if (!_blocks.TryGetValue(tile, out block))
			{
				block = _level.GetBlock((int) tile.X, _startY, (int) tile.Y);
				_blocks.Add(tile, block);
			}

			List<Tile> list = new List<Tile>();
			for (int index = 0; (long) index < Neighbors.GetLongLength(0); ++index)
			{
				var item = new Tile(tile.X + Neighbors[index, 0], tile.Y + Neighbors[index, 1]);

				// Check for too high steps
				BlockCoordinates coord = new BlockCoordinates((int) item.X, block.Coordinates.Y, (int) item.Y);
				if (_level.GetBlock(coord).IsSolid)
				{
					if (_level.GetBlock(coord + BlockCoordinates.Up).IsSolid)
					{
						// Can't jump
						continue;
					}

					_blocks[item] = _level.GetBlock(coord + BlockCoordinates.Up);
				}
				else
				{
					if (!_level.GetBlock(coord + BlockCoordinates.Down).IsSolid)
					{
						if (!_level.GetBlock(coord + BlockCoordinates.Down + BlockCoordinates.Down).IsSolid)
						{
							// Will fall
							continue;
						}
						_blocks[item] = _level.GetBlock(coord + BlockCoordinates.Down);
					}
					else
					{
						_blocks[item] = _level.GetBlock(coord);
					}
				}

				list.Add(item);
			}

			CheckDiagonals(block, list);

			return list;
		}

		private void CheckDiagonals(Block block, List<Tile> list)
		{
			// if no north, remove all north
			if (!list.Contains(TileFromBlock(block.Coordinates + BlockCoordinates.North)))
			{
				//Log.Debug("Removed north");
				list.Remove(TileFromBlock(block.Coordinates + BlockCoordinates.North + BlockCoordinates.East));
				list.Remove(TileFromBlock(block.Coordinates + BlockCoordinates.North + BlockCoordinates.West));
			}
			// if no south, remove all south
			if (!list.Contains(TileFromBlock(block.Coordinates + BlockCoordinates.South)))
			{
				//Log.Debug("Removed south");
				list.Remove(TileFromBlock(block.Coordinates + BlockCoordinates.South + BlockCoordinates.East));
				list.Remove(TileFromBlock(block.Coordinates + BlockCoordinates.South + BlockCoordinates.West));
			}
			// if no west, remove all west
			if (!list.Contains(TileFromBlock(block.Coordinates + BlockCoordinates.West)))
			{
				//Log.Debug("Removed west");
				list.Remove(TileFromBlock(block.Coordinates + BlockCoordinates.West + BlockCoordinates.North));
				list.Remove(TileFromBlock(block.Coordinates + BlockCoordinates.West + BlockCoordinates.South));
			}
			// if no east, remove all east
			if (!list.Contains(TileFromBlock(block.Coordinates + BlockCoordinates.East)))
			{
				//Log.Debug("Removed east");
				list.Remove(TileFromBlock(block.Coordinates + BlockCoordinates.East + BlockCoordinates.North));
				list.Remove(TileFromBlock(block.Coordinates + BlockCoordinates.East + BlockCoordinates.South));
			}
		}

		private Tile TileFromBlock(BlockCoordinates coord)
		{
			return new Tile(coord.X, coord.Z);
		}
	}


	public class LevelNavigator : IBlockedProvider
	{
		private readonly Entity _entity;
		private readonly Level _level;
		private readonly double _distance;
		private readonly Dictionary<Tile, Block> _blockCache;

		public LevelNavigator(Entity entity, Level level, double distance, Dictionary<Tile, Block> blockCache)
		{
			_entity = entity;
			_level = level;
			_distance = distance;
			_blockCache = blockCache;
		}

		public bool IsBlocked(Tile coord)
		{
			Block block;
			if (!_blockCache.TryGetValue(coord, out block))
			{
				return true;
			}

			if (Math.Abs(_entity.KnownPosition.Y - block.Coordinates.Y) > _entity.Height + 3) return true;

			Vector2 entityPos = new Vector2(_entity.KnownPosition.X, _entity.KnownPosition.Z);
			Vector2 tilePos = new Vector2((float) coord.X, (float) coord.Y);

			if ((entityPos - tilePos).Length() > _distance) return true;

			BlockCoordinates blockCoordinates = block.Coordinates;

			for (int i = 1; i < _entity.Height; i++)
			{
				if (IsBlockedUp(blockCoordinates + (BlockCoordinates.Up*i))) return true;
			}

			return false;
		}

		private bool IsBlockedUp(BlockCoordinates coordUp)
		{
			var tileUp = new Tile(coordUp.X, coordUp.Z);
			Block blockUp;
			if (!_blockCache.TryGetValue(tileUp, out blockUp))
			{
				blockUp = _level.GetBlock(coordUp);
				_blockCache.Add(tileUp, blockUp);
			}

			if (blockUp != null && blockUp.IsSolid)
			{
				//_level.SetBlock(new GoldBlock() {Coordinates = blockCoordinates + BlockCoordinates.Up});
				return true;
			}
			return false;
		}
	}
}