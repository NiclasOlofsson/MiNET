using System;
using System.Collections.Generic;
using System.Numerics;
using log4net;
using MiNET.Blocks;
using MiNET.BuilderBase.Commands;
using MiNET.BuilderBase.Masks;
using MiNET.BuilderBase.Patterns;
using MiNET.Entities;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.BuilderBase
{
	public class EditHelper
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (EditHelper));

		private readonly Level _level;
		private readonly Mask _mask;

		public EditHelper(Level level, Mask mask = null)
		{
			_level = level;
			_mask = mask;
		}

		public Block GetBlockInLineOfSight(Level level, PlayerLocation knownPosition, int range = 300)
		{
			var origin = knownPosition;
			origin.Y += 1.62f;
			Vector3 velocity2 = knownPosition.GetHeadDirection();
			double distance = range;
			velocity2 = Vector3.Normalize(velocity2)/2;

			for (int i = 0; i < Math.Ceiling(distance)*2; i++)
			{
				PlayerLocation nextPos = (PlayerLocation) origin.Clone();
				nextPos.X += (float) velocity2.X*i;
				nextPos.Y += (float) velocity2.Y*i;
				nextPos.Z += (float) velocity2.Z*i;

				BlockCoordinates coord = new BlockCoordinates(nextPos);
				Block block = level.GetBlock(coord);
				bool collided = block.IsSolid && (block.GetBoundingBox()).Contains(nextPos.ToVector3());
				if (collided)
				{
					return block;
				}
			}

			return null;
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
			HistoryEntry history = selector.CreateSnapshot();

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

			history.Snapshot(false);
			return drawLine;
		}

		public void SetBlocks(RegionSelector selector, Pattern pattern)
		{
			HistoryEntry history = selector.CreateSnapshot();
			SetBlocks(selector.GetSelectedBlocks(), pattern);
			history.Snapshot(false);
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
			if(_mask != null)
			{
				if (!_mask.Test(position)) return false;
			}

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

		public void ReplaceBlocks(RegionSelector selector, Mask mask, Pattern pattern)
		{
			HistoryEntry history = selector.CreateSnapshot();

			var selected = selector.GetSelectedBlocks();
			foreach (BlockCoordinates coordinates in selected)
			{
				if (mask.Test(coordinates))
				{
					Block block = pattern.Next(coordinates);
					_level.SetBlock(block);
				}
			}

			history.Snapshot(false);
		}

		public void Center(RegionSelector selector, Pattern pattern)
		{
			Vector3 centerVec = selector.GetCenter();
			BlockCoordinates center = new BlockCoordinates((int) centerVec.X, (int) centerVec.Y, (int) centerVec.Z);

			RegionSelector centerRegion = new RegionSelector(selector.Player);
			centerRegion.SelectPrimary(center);
			centerRegion.SelectSecondary(centerVec);

			HistoryEntry history = selector.CreateSnapshot(center, centerVec);

			SetBlocks(centerRegion.GetSelectedBlocks(), pattern);

			history.Snapshot(false);
		}

		public void Move(RegionSelector selector, int count, BlockCoordinates dir)
		{
			HistoryEntry history = selector.CreateSnapshot(selector.GetMin(), selector.GetMax() + (dir*count));

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

			history.Snapshot(false);
		}

		public void Stack(RegionSelector selector, int count, BlockCoordinates dir, bool skipAir)
		{
			BlockCoordinates size = selector.GetMax() - selector.GetMin() + BlockCoordinates.One;

			var selected = new List<BlockCoordinates>(selector.GetSelectedBlocks());

			// Save old blocks with new coordinates so we don't overwrite while traverse

			HistoryEntry history = selector.CreateSnapshot(selector.GetMin(), selector.GetMax() + (dir*size*count));

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
				if (skipAir && block is Air) continue;
				_level.SetBlock(block);
			}

			// Move selection too last stack
			selector.Select(selector.Position1 + (dir*size*count), selector.Position2 + (dir*size*count));

			history.Snapshot(false);
		}

		///**
		//   * Makes a sphere or ellipsoid.
		//   *
		//   * @param pos Center of the sphere or ellipsoid
		//   * @param block The block pattern to use
		//   * @param radiusX The sphere/ellipsoid's largest north/south extent
		//   * @param radiusY The sphere/ellipsoid's largest up/down extent
		//   * @param radiusZ The sphere/ellipsoid's largest east/west extent
		//   * @param filled If false, only a shell will be generated.
		//   * @return number of blocks changed
		//   * @throws MaxChangedBlocksException thrown if too many blocks are changed
		//   */
		public int MakeSphere(Vector3 pos, Pattern block, double radiusX, double radiusY, double radiusZ, bool filled)
		{
			int affected = 0;

			radiusX += 0.5;
			radiusY += 0.5;
			radiusZ += 0.5;

			double invRadiusX = 1/radiusX;
			double invRadiusY = 1/radiusY;
			double invRadiusZ = 1/radiusZ;

			int ceilRadiusX = (int) Math.Ceiling(radiusX);
			int ceilRadiusY = (int) Math.Ceiling(radiusY);
			int ceilRadiusZ = (int) Math.Ceiling(radiusZ);

			bool breakX = false;

			double nextXn = 0;
			forX:
			for (int x = 0; x <= ceilRadiusX && !breakX; ++x)
			{
				double xn = nextXn;
				nextXn = (x + 1)*invRadiusX;
				double nextYn = 0;
				bool breakY = false;
				forY:
				for (int y = 0; y <= ceilRadiusY && !breakY; ++y)
				{
					double yn = nextYn;
					nextYn = (y + 1)*invRadiusY;
					double nextZn = 0;
					bool breakZ = false;
					forZ:
					for (int z = 0; z <= ceilRadiusZ && !breakZ; ++z)
					{
						double zn = nextZn;
						nextZn = (z + 1)*invRadiusZ;

						double distanceSq = LengthSq(xn, yn, zn);
						if (distanceSq > 1)
						{
							if (z == 0)
							{
								if (y == 0)
								{
									breakX = true;
									goto forX;
								}

								breakY = true;
								goto forY;
							}

							breakZ = true;
							goto forZ;
						}

						if (!filled)
						{
							if (LengthSq(nextXn, yn, zn) <= 1 && LengthSq(xn, nextYn, zn) <= 1 && LengthSq(xn, yn, nextZn) <= 1)
							{
								continue;
							}
						}

						if (SetBlock(pos + new Vector3(x, y, z), block))
						{
							++affected;
						}
						if (SetBlock(pos + new Vector3(-x, y, z), block))
						{
							++affected;
						}
						if (SetBlock(pos + new Vector3(x, -y, z), block))
						{
							++affected;
						}
						if (SetBlock(pos + new Vector3(x, y, -z), block))
						{
							++affected;
						}
						if (SetBlock(pos + new Vector3(-x, -y, z), block))
						{
							++affected;
						}
						if (SetBlock(pos + new Vector3(x, -y, -z), block))
						{
							++affected;
						}
						if (SetBlock(pos + new Vector3(-x, y, -z), block))
						{
							++affected;
						}
						if (SetBlock(pos + new Vector3(-x, -y, -z), block))
						{
							++affected;
						}
					}
				}
			}

			return affected;
		}

		private static double LengthSq(double x, double y, double z)
		{
			return (x*x) + (y*y) + (z*z);
		}
	}
}