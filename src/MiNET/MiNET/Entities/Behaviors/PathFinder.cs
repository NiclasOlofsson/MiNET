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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using AStarNavigator;
using AStarNavigator.Algorithms;
using AStarNavigator.Providers;
using log4net;
using MiNET.Blocks;
using MiNET.Particles;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities.Behaviors
{
	//public class TileNavigator : ITileNavigator
	//{
	//	private readonly IBlockedProvider blockedProvider;
	//	private readonly INeighborProvider neighborProvider;

	//	private readonly IDistanceAlgorithm distanceAlgorithm;
	//	private readonly IDistanceAlgorithm heuristicAlgorithm;

	//	public TileNavigator(
	//		IBlockedProvider blockedProvider,
	//		INeighborProvider neighborProvider,
	//		IDistanceAlgorithm distanceAlgorithm,
	//		IDistanceAlgorithm heuristicAlgorithm)
	//	{
	//		this.blockedProvider = blockedProvider;
	//		this.neighborProvider = neighborProvider;

	//		this.distanceAlgorithm = distanceAlgorithm;
	//		this.heuristicAlgorithm = heuristicAlgorithm;
	//	}

	//	public IEnumerable<Tile> Navigate(Tile from, Tile to, int maxAttempts = int.MaxValue)
	//	{
	//		var closed = new HashSet<Tile>();
	//		var open = new HashSet<Tile>() {from};
	//		var path = new Dictionary<Tile, Tile>();

	//		from.FScore = heuristicAlgorithm.Calculate(from, to);

	//		int noOfAttempts = 0;

	//		Tile highScore = from;
	//		Tile last = from;
	//		while (open.Count != 0)
	//		{
	//			var current = last;
	//			if (last != highScore)
	//			{
	//				current = open
	//					.OrderBy(c => c.FScore)
	//					.First();
	//			}

	//			last = null;

	//			if (++noOfAttempts > maxAttempts)
	//			{
	//				return ReconstructPath(path, highScore);
	//			}
	//			if (current.Equals(to))
	//			{
	//				return ReconstructPath(path, current);
	//			}

	//			open.Remove(current);
	//			closed.Add(current);

	//			foreach (Tile neighbor in neighborProvider.GetNeighbors(current))
	//			{
	//				if (closed.Contains(neighbor) || blockedProvider.IsBlocked(neighbor))
	//				{
	//					continue;
	//				}

	//				var tentativeG = current.GScore + distanceAlgorithm.Calculate(current, neighbor);

	//				if (!open.Add(neighbor) && tentativeG >= neighbor.GScore)
	//				{
	//					continue;
	//				}

	//				path[neighbor] = current;

	//				neighbor.GScore = tentativeG;
	//				neighbor.FScore = neighbor.GScore + heuristicAlgorithm.Calculate(neighbor, to);
	//				if (neighbor.FScore <= highScore.FScore)
	//				{
	//					highScore = neighbor;
	//					last = neighbor;
	//				}
	//			}
	//		}

	//		return null;
	//	}

	//	private IEnumerable<Tile> ReconstructPath(
	//		IDictionary<Tile, Tile> path,
	//		Tile current)
	//	{
	//		List<Tile> totalPath = new List<Tile>() {current};

	//		while (path.ContainsKey(current))
	//		{
	//			current = path[current];
	//			totalPath.Insert(0, current);
	//		}

	//		totalPath.RemoveAt(0);

	//		return totalPath;
	//	}
	//}

	public class Path
	{
		public List<Tile> Current { get; set; } = new List<Tile>();
		private Dictionary<Tile, Block> _blockCache;

		public Path(Dictionary<Tile, Block> blockCache = null)
		{
			_blockCache = blockCache;
		}

		public bool HavePath()
		{
			return Current != null && Current.Count > 0;
		}

		public bool NoPath()
		{
			return Current == null || Current.Count == 0;
		}

		public void Reset()
		{
			Current?.Clear();
		}

		public void PrintPath(Level level)
		{
			if (Config.GetProperty("Pathfinder.PrintPath", false))
			{
				foreach (var tile in Current)
				{
					Block block = GetBlock(tile);
					Color color = Color.FromArgb(Math.Max(0, 255 - Current.Count*10), 255, 255);
					var particle = new DustParticle(level, color);
					particle.Position = (Vector3) block.Coordinates + new Vector3(0.5f, 0.5f, 0.5f);
					particle.Spawn();
				}
			}
		}

		public void PrintTile(Level level, Tile tile)
		{
			if (Config.GetProperty("Pathfinder.PrintPath", false))
			{
				Block block = GetBlock(tile);
				var particle = new RedstoneParticle(level);
				particle.Position = (Vector3) block.Coordinates + new Vector3(0.5f, 0.5f, 0.5f);
				particle.Spawn();
			}
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

		public Tile First()
		{
			return Current.First();
		}

		public void Remove(Tile tile)
		{
			Current.Remove(tile);
		}

		public bool GetNextTile(Entity entity, out Tile next, bool compressPath = false)
		{
			next = null;
			if (NoPath()) return false;

			next = First();

			BlockCoordinates currPos = (BlockCoordinates) entity.KnownPosition;
			if ((int) next.X == currPos.X && (int) next.Y == currPos.Z)
			{
				Remove(next);

				if (!GetNextTile(entity, out next)) return false;
			}

			// Use this to compact path and remove unnecessary points. This
			// makes the mobs move more direct on target, and also more independently
			// from each other.

			if (compressPath)
			{
				foreach (var tile in Current.ToArray())
				{
					if (IsClearBetweenPoints(entity.Level, entity.KnownPosition, new Vector3(tile.X, currPos.Y, tile.Y)))
					{
						next = tile;
						Remove(tile);
						PrintTile(entity.Level, tile);
					}
					else
					{
						break;
					}
				}
			}

			PrintPath(entity.Level);

			return true;
		}

		private bool IsClearBetweenPoints(Level level, Vector3 from, Vector3 to)
		{
			Vector3 entityPos = from;
			Vector3 targetPos = to;
			float distance = Vector3.Distance(entityPos, targetPos);

			Vector3 rayPos = entityPos;
			var direction = Vector3.Normalize(targetPos - entityPos);

			if (distance < direction.Length())
			{
				return true;
			}

			do
			{
				if (level.GetBlock(rayPos).IsSolid)
				{
					return false;
				}
				rayPos += direction;
			} while (distance > Vector3.Distance(entityPos, rayPos));

			return true;
		}
	}

	public class Pathfinder
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Pathfinder));

		private Dictionary<Tile, Block> _blockCache = new Dictionary<Tile, Block>();

		public Path FindPath(Entity source, Entity target, double distance)
		{
			return FindPath(source, (BlockCoordinates) target.KnownPosition, distance);
		}

		public Path FindPath(Entity source, BlockCoordinates target, double distance)
		{
			try
			{
				var blockAccess = new CachedBlockAccess(source.Level);

				var entityCoords = new HashSet<BlockCoordinates>();
				foreach (var entry in source.Level.GetEntites())
				{
					entityCoords.Add((BlockCoordinates) entry.KnownPosition);
				}

				var navigator = new TileNavigator(
					new LevelNavigator(source, blockAccess, distance, _blockCache, entityCoords),
					new BlockDiagonalNeighborProvider(blockAccess, (int) Math.Truncate(source.KnownPosition.Y), _blockCache, source), // Instance of: INeighborProvider
					new BlockDistanceAlgorithm(_blockCache), // Instance of: IDistanceAlgorithm
					new ManhattanHeuristicAlgorithm() // Instance of: IDistanceAlgorithm
				);

				BlockCoordinates targetPos = target;
				BlockCoordinates sourcePos = (BlockCoordinates) source.KnownPosition;
				var from = new Tile(sourcePos.X, sourcePos.Z);
				var to = new Tile(targetPos.X, targetPos.Z);

				var path = navigator.Navigate(from, to, 200)?.ToList() ?? new List<Tile>();

				//Log.Debug($"{source.GetType()} finding path within {distance} blocks. Did {blockAccess.NumberOfBlockGet} block-gets");

				return new Path(_blockCache) {Current = path};
			}
			catch (Exception e)
			{
				Log.Error("Navigate", e);
			}

			return new Path();
		}
	}

	public class BlockDistanceAlgorithm : IDistanceAlgorithm
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (BlockDistanceAlgorithm));

		private readonly Dictionary<Tile, Block> _blockCache;

		public BlockDistanceAlgorithm(Dictionary<Tile, Block> blockCache)
		{
			_blockCache = blockCache;
		}

		public double Calculate(Tile from, Tile to)
		{
			Vector3 vFrom = GetBlock(from).Coordinates;
			Vector3 vTo = GetBlock(to).Coordinates;
			return Vector3.Distance(vFrom, vTo);
		}

		public Block GetBlock(Tile tile)
		{
			Block block;
			if (!_blockCache.TryGetValue(tile, out block))
			{
				// Do something?

				Log.Error("Expected block, had none");
				return null;
			}

			return block;
		}
	}


	public class BlockDiagonalNeighborProvider : INeighborProvider
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (BlockDiagonalNeighborProvider));

		private readonly CachedBlockAccess _level;
		private readonly int _startY;
		private readonly Entity _entity;
		private readonly Dictionary<Tile, Block> _blockCache;

		public BlockDiagonalNeighborProvider(CachedBlockAccess level, int startY, Dictionary<Tile, Block> blockCache, Entity entity)
		{
			_level = level;
			_startY = startY;
			_blockCache = blockCache;
			_entity = entity;
		}

		private static readonly int[,] Neighbors = new int[,]
		{
			{
				0,
				-1
			},
			{
				1,
				0
			},
			{
				0,
				1
			},
			{
				-1,
				0
			},
			{
				-1,
				-1
			},
			{
				1,
				-1
			},
			{
				1,
				1
			},
			{
				-1,
				1
			}
		};

		public IEnumerable<Tile> GetNeighbors(Tile tile)
		{
			Block block;
			if (!_blockCache.TryGetValue(tile, out block))
			{
				block = _level.GetBlock(new BlockCoordinates((int) tile.X, _startY, (int) tile.Y));
				_blockCache.Add(tile, block);
			}

			HashSet<Tile> list = new HashSet<Tile>();
			for (int index = 0; index < Neighbors.GetLength(0); ++index)
			{
				var item = new Tile(tile.X + Neighbors[index, 0], tile.Y + Neighbors[index, 1]);

				// Check for too high steps
				BlockCoordinates coord = new BlockCoordinates((int) item.X, block.Coordinates.Y, (int) item.Y);
				if (_level.GetBlock(coord).IsSolid)
				{
					Block blockUp = _level.GetBlock(coord + BlockCoordinates.Up);
					if (blockUp.IsSolid)
					{
						// Can't jump
						continue;
					}

					if (IsObstructed(blockUp.Coordinates)) continue;

					_blockCache[item] = blockUp;
				}
				else
				{
					var blockDown = _level.GetBlock(coord + BlockCoordinates.Down);
					if (!blockDown.IsSolid)
					{
						if (!_level.GetBlock(coord + BlockCoordinates.Down + BlockCoordinates.Down).IsSolid)
						{
							// Will fall
							continue;
						}

						if (IsObstructed(blockDown.Coordinates)) continue;

						_blockCache[item] = blockDown;
					}
					else
					{
						if (IsObstructed(coord)) continue;

						_blockCache[item] = _level.GetBlock(coord);
					}
				}

				list.Add(item);
			}

			CheckDiagonals(block, list);

			return list;
		}

		private bool IsObstructed(BlockCoordinates coord)
		{
			for (int i = 1; i < _entity.Height; i++)
			{
				if (IsBlocked(coord + (BlockCoordinates.Up*i))) return true;
			}

			return false;
		}

		private bool IsBlocked(BlockCoordinates coord)
		{
			var block = _level.GetBlock(coord);

			if (block == null || block.IsSolid)
			{
				return true;
			}
			return false;
		}

		private void CheckDiagonals(Block block, HashSet<Tile> list)
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
		private static readonly ILog Log = LogManager.GetLogger(typeof (LevelNavigator));

		private readonly Entity _entity;
		private readonly Vector3 _entityPos;
		private readonly IBlockAccess _level;
		private readonly double _distance;
		private readonly Dictionary<Tile, Block> _blockCache;
		private readonly HashSet<BlockCoordinates> _entityCoords;

		public LevelNavigator(Entity entity, IBlockAccess level, double distance, Dictionary<Tile, Block> blockCache, HashSet<BlockCoordinates> entityCoords)
		{
			_entity = entity;
			_entityPos = entity.KnownPosition;
			_level = level;
			_distance = distance;
			_blockCache = blockCache;
			_entityCoords = entityCoords;
		}

		public bool IsBlocked(Tile coord)
		{
			Block block;
			if (!_blockCache.TryGetValue(coord, out block))
			{
				return true;
			}

			if (block.IsSolid) return true;
			if (_entityCoords.Contains(block.Coordinates)) return true;

			//if (Math.Abs(_entityPos.Y - block.Coordinates.Y) > _entity.Height + 3) return true;

			Vector2 entityPos = new Vector2(_entityPos.X, _entityPos.Z);
			Vector2 tilePos = new Vector2((float) coord.X, (float) coord.Y);

			if (Vector2.Distance(entityPos, tilePos) > _distance) return true;

			BlockCoordinates blockCoordinates = block.Coordinates;

			if (IsObstructed(blockCoordinates)) return true;

			return false;
		}

		private bool IsObstructed(BlockCoordinates coord)
		{
			for (int i = 1; i < _entity.Height; i++)
			{
				if (IsBlocked(coord + (BlockCoordinates.Up*i))) return true;
			}

			return false;
		}

		private bool IsBlocked(BlockCoordinates coord)
		{
			var block = _level.GetBlock(coord);

			if (block == null || block.IsSolid)
			{
				return true;
			}
			return false;
		}
	}

	public class CachedBlockAccess : IBlockAccess
	{
		private Level _level;
		public int NumberOfBlockGet = 0;
		private IDictionary<BlockCoordinates, Block> _blockCache = new ConcurrentDictionary<BlockCoordinates, Block>();

		public CachedBlockAccess(Level level)
		{
			_level = level;
		}

		public ChunkColumn GetChunk(BlockCoordinates coordinates, bool cacheOnly = false)
		{
			return _level.GetChunk(coordinates, true);
		}

		public ChunkColumn GetChunk(ChunkCoordinates coordinates, bool cacheOnly = false)
		{
			return _level.GetChunk(coordinates, true);
		}

		public void SetSkyLight(BlockCoordinates coordinates, byte skyLight)
		{
			_blockCache.Remove(coordinates);
			_level.SetSkyLight(coordinates, skyLight);
		}

		public int GetHeight(BlockCoordinates coordinates)
		{
			return _level.GetHeight(coordinates);
		}

		public Block GetBlock(BlockCoordinates coord, ChunkColumn tryChunk = null)
		{
			NumberOfBlockGet++;
			Block block;
			if (!_blockCache.TryGetValue(coord, out block))
			{
				block = _level.GetBlock(coord);
				_blockCache[coord] = block;
			}

			return block;
		}

		public void SetBlock(Block block, bool broadcast = true, bool applyPhysics = true, bool calculateLight = true)
		{
			_blockCache.Remove(block.Coordinates);
			_level.SetBlock(block, broadcast, applyPhysics, calculateLight);
		}
	}
}