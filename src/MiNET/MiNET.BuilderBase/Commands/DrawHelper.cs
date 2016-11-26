using System;
using System.Collections.Generic;
using System.Numerics;
using log4net;
using MiNET.Blocks;
using MiNET.Entities;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.BuilderBase.Commands
{
	public class DrawHelper
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (DrawHelper));

		private readonly Level _level;

		public DrawHelper(Level level)
		{
			_level = level;
		}

		public static BlockCoordinates GetDirectionVector(Player player, string direction)
		{
			SelectionDirection dir;
			if (!Enum.TryParse(direction, true, out dir))
			{
				throw new ArgumentOutOfRangeException();
			}

			if (dir == SelectionDirection.Me)
			{
				int playerDirection = Entity.DirectionByRotationFlat(player.KnownPosition.HeadYaw);
				switch (playerDirection)
				{
					case 1:
						dir = SelectionDirection.West;
						break;
					case 2:
						dir = SelectionDirection.North;
						break;
					case 3:
						dir = SelectionDirection.East;
						break;
					case 0:
						dir = SelectionDirection.South;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			BlockCoordinates levelDir;
			switch (dir)
			{
				case SelectionDirection.Up:
					levelDir = Level.Up;
					break;
				case SelectionDirection.Down:
					levelDir = Level.Down;
					break;
				case SelectionDirection.North:
					levelDir = Level.North;
					break;
				case SelectionDirection.West:
					levelDir = Level.West;
					break;
				case SelectionDirection.South:
					levelDir = Level.South;
					break;
				case SelectionDirection.East:
					levelDir = Level.East;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			return levelDir;
		}


		public int DrawLine(RegionSelector selector, Pattern pattern, BlockCoordinates pos1, BlockCoordinates pos2, double radius, bool filled)
		{
			var clipboard = selector.CreateSnapshot();

			HashSet<BlockCoordinates> vset = new HashSet<BlockCoordinates>();
			bool notdrawn = true;

			int x1 = pos1.X, y1 = pos1.Y, z1 = pos1.Z;
			int x2 = pos2.X, y2 = pos2.Y, z2 = pos2.Z;
			int tipx = x1, tipy = y1, tipz = z1;
			int dx = Math.Abs(x2 - x1), dy = Math.Abs(y2 - y1), dz = Math.Abs(z2 - z1);

			if (dx + dy + dz == 0)
			{
				vset.Add(new BlockCoordinates(tipx, tipy, tipz));
				notdrawn = false;
			}

			if (Math.Max(Math.Max(dx, dy), dz) == dx && notdrawn)
			{
				for (int domstep = 0; domstep <= dx; domstep++)
				{
					tipx = x1 + domstep*(x2 - x1 > 0 ? 1 : -1);
					tipy = (int) Math.Round(y1 + domstep*((double) dy)/((double) dx)*(y2 - y1 > 0 ? 1 : -1));
					tipz = (int) Math.Round(z1 + domstep*((double) dz)/((double) dx)*(z2 - z1 > 0 ? 1 : -1));

					vset.Add(new BlockCoordinates(tipx, tipy, tipz));
				}
				notdrawn = false;
			}

			if (Math.Max(Math.Max(dx, dy), dz) == dy && notdrawn)
			{
				for (int domstep = 0; domstep <= dy; domstep++)
				{
					tipy = y1 + domstep*(y2 - y1 > 0 ? 1 : -1);
					tipx = (int) Math.Round(x1 + domstep*((double) dx)/((double) dy)*(x2 - x1 > 0 ? 1 : -1));
					tipz = (int) Math.Round(z1 + domstep*((double) dz)/((double) dy)*(z2 - z1 > 0 ? 1 : -1));

					vset.Add(new BlockCoordinates(tipx, tipy, tipz));
				}
				notdrawn = false;
			}

			if (Math.Max(Math.Max(dx, dy), dz) == dz && notdrawn)
			{
				for (int domstep = 0; domstep <= dz; domstep++)
				{
					tipz = z1 + domstep*(z2 - z1 > 0 ? 1 : -1);
					tipy = (int) Math.Round(y1 + domstep*((double) dy)/((double) dz)*(y2 - y1 > 0 ? 1 : -1));
					tipx = (int) Math.Round(x1 + domstep*((double) dx)/((double) dz)*(x2 - x1 > 0 ? 1 : -1));

					vset.Add(new BlockCoordinates(tipx, tipy, tipz));
				}
				notdrawn = false;
			}

			vset = GetBallooned(vset, radius);
			if (!filled)
			{
				vset = GetHollowed(vset);
			}

			var drawLine = SetBlocks(vset, pattern);

			clipboard.Snapshot(false);
			return drawLine;
		}

		public void SetBlocks(RegionSelector selector, Pattern pattern)
		{
			var clipboard = selector.CreateSnapshot();
			SetBlocks(selector.GetSelectedBlocks(), pattern);
			clipboard.Snapshot(false);
		}

		private void SetBlocks(BlockCoordinates[] selected, Pattern pattern)
		{
			foreach (BlockCoordinates coordinates in selected)
			{
				Block block = pattern.Next(coordinates);
				_level.SetBlock(block);
			}
		}

		private int SetBlocks(HashSet<BlockCoordinates> vset, Pattern pattern)
		{
			int affected = 0;
			foreach (BlockCoordinates v in vset)
			{
				affected += SetBlock(v, pattern) ? 1 : 0;
			}
			return affected;
		}

		public bool SetBlock(BlockCoordinates position, Pattern pattern)
		{
			return SetBlock(pattern.Next(position));
		}

		public bool SetBlock(Block block)
		{
			_level.SetBlock(block);

			return true;
		}

		private static HashSet<BlockCoordinates> GetHollowed(HashSet<BlockCoordinates> vset)
		{
			HashSet<BlockCoordinates> returnset = new HashSet<BlockCoordinates>();
			foreach (BlockCoordinates v in vset)
			{
				int x = v.X, y = v.Y, z = v.Z;
				if (!(vset.Contains(new BlockCoordinates(x + 1, y, z)) &&
				      vset.Contains(new BlockCoordinates(x - 1, y, z)) &&
				      vset.Contains(new BlockCoordinates(x, y + 1, z)) &&
				      vset.Contains(new BlockCoordinates(x, y - 1, z)) &&
				      vset.Contains(new BlockCoordinates(x, y, z + 1)) &&
				      vset.Contains(new BlockCoordinates(x, y, z - 1))))
				{
					returnset.Add(v);
				}
			}
			return returnset;
		}

		private static HashSet<BlockCoordinates> GetBallooned(HashSet<BlockCoordinates> vset, double radius)
		{
			HashSet<BlockCoordinates> returnset = new HashSet<BlockCoordinates>();
			int ceilrad = (int) Math.Ceiling(radius);

			foreach (BlockCoordinates v in vset)
			{
				int tipx = v.X, tipy = v.Y, tipz = v.Z;

				for (int loopx = tipx - ceilrad; loopx <= tipx + ceilrad; loopx++)
				{
					for (int loopy = tipy - ceilrad; loopy <= tipy + ceilrad; loopy++)
					{
						for (int loopz = tipz - ceilrad; loopz <= tipz + ceilrad; loopz++)
						{
							if (Hypot(loopx - tipx, loopy - tipy, loopz - tipz) <= radius)
							{
								returnset.Add(new BlockCoordinates(loopx, loopy, loopz));
							}
						}
					}
				}
			}
			return returnset;
		}

		private static double Hypot(params double[] pars)
		{
			double sum = 0;
			foreach (double d in pars)
			{
				sum += Math.Pow(d, 2);
			}
			return Math.Sqrt(sum);
		}

		public void ReplaceBlocks(RegionSelector selector, AllBlocksMask mask, Pattern pattern)
		{
			var clipboard = selector.CreateSnapshot();

			var selected = selector.GetSelectedBlocks();
			foreach (BlockCoordinates coordinates in selected)
			{
				if (mask.Test(coordinates))
				{
					Block block = pattern.Next(coordinates);
					_level.SetBlock(block);
				}
			}

			clipboard.Snapshot(false);
		}

		public void Center(RegionSelector selector, Pattern pattern)
		{
			Vector3 centerVec = selector.GetCenter();
			BlockCoordinates center = new BlockCoordinates((int) centerVec.X, (int) centerVec.Y, (int) centerVec.Z);

			RegionSelector centerRegion = new RegionSelector(selector.Player);
			centerRegion.SelectPrimary(center);
			centerRegion.SelectSecondary(centerVec);

			var clipboard = selector.CreateSnapshot(center, centerVec);

			SetBlocks(centerRegion.GetSelectedBlocks(), pattern);

			clipboard.Snapshot(false);
		}

		public void Move(RegionSelector selector, int count, BlockCoordinates dir)
		{
			var clipboard = selector.CreateSnapshot(selector.GetMin(), selector.GetMax() + (dir*count));

			var selected = new List<BlockCoordinates>(selector.GetSelectedBlocks());

			// Save old blocks with new coordinates so we don't overwrite while traverse
			List<Block> movedBlocks = new List<Block>();
			foreach (var coord in selected.ToArray())
			{
				Block block = _level.GetBlock(coord);
				block.Coordinates += (dir*count);
				movedBlocks.Add(block);

				selected.Remove(block.Coordinates);
			}

			// Actually move them
			foreach (var block in movedBlocks)
			{
				_level.SetBlock(block);
			}

			// Set the left-overs (gap) to air
			foreach (var coord in selected)
			{
				_level.SetAir(coord);
			}

			// Move selection too
			selector.Select(selector.Position1 + (dir*count), selector.Position2 + (dir*count));

			clipboard.Snapshot(false);
		}

		public void Stack(RegionSelector selector, int count, BlockCoordinates dir)
		{
			BlockCoordinates size = selector.GetMax() - selector.GetMin() + BlockCoordinates.One;

			var selected = new List<BlockCoordinates>(selector.GetSelectedBlocks());

			// Save old blocks with new coordinates so we don't overwrite while traverse

			var clipboard = selector.CreateSnapshot(selector.GetMin(), selector.GetMax() + (dir*size*count));

			List<Block> movedBlocks = new List<Block>();
			for (int i = 1; i <= count; i++)
			{
				foreach (var coord in selected.ToArray())
				{
					Block block = _level.GetBlock(coord);
					block.Coordinates += (dir*size*i);
					movedBlocks.Add(block);
				}
			}

			// Actually stack them
			foreach (var block in movedBlocks)
			{
				_level.SetBlock(block);
			}

			// Move selection too last stack
			selector.Select(selector.Position1 + (dir*size*count), selector.Position2 + (dir*size*count));

			clipboard.Snapshot(false);
		}
	}
}