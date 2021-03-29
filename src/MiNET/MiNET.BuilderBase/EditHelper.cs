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
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.BuilderBase
{
	public class EditHelper
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (EditHelper));

		private Level _level;
		private readonly Player _player;
		private Mask _mask;
		private bool _randomizeGeneration = false;
		private readonly UndoRecorder _undoRecorder;
		private Random _random = new Random();

		public EditHelper(Level level, Player player, Mask mask = null, bool randomizeGeneration = false, UndoRecorder undoRecorder = null)
		{
			_level = level;
			_player = player;
			_mask = mask;
			_randomizeGeneration = randomizeGeneration;
			_undoRecorder = undoRecorder;
		}

		public Block GetBlockInLineOfSight(Level level, PlayerLocation knownPosition, int range = 300, bool returnLastAir = false, bool limitHeight = true)
		{
			Vector3 origin = knownPosition + new Vector3(0, 1.62f, 0);

			Vector3 velocity2 = knownPosition.GetHeadDirection();
			double distance = range;
			velocity2 = Vector3.Normalize(velocity2)/2;

			Block lastAir = null;
			for (int i = 0; i < Math.Ceiling(distance)*2; i++)
			{
				Vector3 nextPos = origin + velocity2*i;

				BlockCoordinates coord = nextPos;
				Block block = level.GetBlock(coord);

				if (!limitHeight && coord.Y >= 255) block = new Air {Coordinates = block.Coordinates};

				bool lookingDownFromHeaven = (origin.Y > coord.Y && coord.Y > 255);
				bool lookingUpAtHeaven = (origin.Y < coord.Y);

				bool collided = !lookingDownFromHeaven && !(block is Air) && block.GetBoundingBox().Contains(nextPos);
				if (collided || lookingUpAtHeaven && coord.Y > 150)
				{
					return limitHeight ? lastAir : block;
				}

				if (block is Air) lastAir = block;
			}

			return returnLastAir ? lastAir : null;
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
					levelDir = Level.West;
					break;
				case SelectionDirection.West:
					levelDir = Level.South;
					break;
				case SelectionDirection.South:
					levelDir = Level.East;
					break;
				case SelectionDirection.East:
					levelDir = Level.North;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			return levelDir;
		}


		public int DrawLine(RegionSelector selector, Pattern pattern, BlockCoordinates pos1, BlockCoordinates pos2, double radius, bool filled)
		{
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

			return drawLine;
		}

		public void SetBlocks(RegionSelector selector, Pattern pattern)
		{
			SetBlocks(selector.GetSelectedBlocks(), pattern);
		}

		private void SetBlocks(BlockCoordinates[] selected, Pattern pattern)
		{
			foreach (BlockCoordinates coordinates in selected)
			{
				SetBlock(coordinates, pattern);
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
			if (_mask != null)
			{
				if (!_mask.Test(position)) return false;
			}

			return SetBlock(pattern.Next(position));
		}

		public bool SetBlock(Block block)
		{
			var existing = _level.GetBlock(block.Coordinates);
			_undoRecorder.RecordUndo(existing);
			try
			{
				if (_level.OnBlockPlace(new BlockPlaceEventArgs(_player, _level, block, existing)))
				{
					//if (block.PlaceBlock(_level, _player, block.Coordinates, BlockFace.Up, Vector3.Zero)) return true;

					_level.SetBlock(block);
				}
			}
			finally
			{
				_undoRecorder.RecordRedo(block);
			}
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
			var selected = selector.GetSelectedBlocks();
			foreach (BlockCoordinates coordinates in selected)
			{
				if (mask.Test(coordinates))
				{
					SetBlock(coordinates, pattern);
				}
			}
		}

		public void Center(RegionSelector selector, Pattern pattern)
		{
			Vector3 centerVec = selector.GetCenter();
			BlockCoordinates center = new Vector3((float) Math.Truncate(centerVec.X), (float) Math.Truncate(centerVec.Y), (float) Math.Truncate(centerVec.Z));

			RegionSelector centerRegion = new RegionSelector(selector.Player);
			centerRegion.SelectPrimary(center);
			centerRegion.SelectSecondary(centerVec);
			SetBlocks(centerRegion.GetSelectedBlocks(), pattern);
		}

		public void Move(RegionSelector selector, int count, BlockCoordinates dir)
		{
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
				SetBlock(block);
			}

			// Set the left-overs (gap) to air
			foreach (var coord in selected)
			{
				SetBlock(new Air {Coordinates = coord});
			}

			// Move selection too
			selector.Select(selector.Position1 + (dir*count), selector.Position2 + (dir*count));
		}

		public void Stack(RegionSelector selector, int count, BlockCoordinates dir, bool skipAir, bool moveSelection)
		{
			BlockCoordinates size = selector.GetMax() - selector.GetMin() + BlockCoordinates.One;

			var selected = new List<BlockCoordinates>(selector.GetSelectedBlocks());

			// Save old blocks with new coordinates so we don't overwrite while traverse

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
				SetBlock(block);
			}

			// Move selection too last stack
			if (moveSelection) selector.Select(selector.Position1 + (dir*size*count), selector.Position2 + (dir*size*count));
		}

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

		public int MakeCylinder(Vector3 pos, Pattern block, double radiusX, double radiusZ, int height, bool filled)
		{
			int affected = 0;

			radiusX += 0.5;
			radiusZ += 0.5;

			if (height == 0)
			{
				return 0;
			}
			else if (height < 0)
			{
				height = -height;
				pos = pos - new Vector3(0, height, 0);
			}

			if (pos.Y < 0)
			{
				pos = new Vector3(pos.X, 0, pos.Z);
			}
			else if (pos.Y + height - 1 > 255)
			{
				height = (int) (255 - pos.Y + 1);
			}

			double invRadiusX = 1/radiusX;
			double invRadiusZ = 1/radiusZ;

			int ceilRadiusX = (int) Math.Ceiling(radiusX);
			int ceilRadiusZ = (int) Math.Ceiling(radiusZ);

			double nextXn = 0;
			bool breakX = false;
			forX:
			for (int x = 0; x <= ceilRadiusX && !breakX; ++x)
			{
				double xn = nextXn;
				nextXn = (x + 1)*invRadiusX;
				double nextZn = 0;
				bool breakZ = false;
				forZ:
				for (int z = 0; z <= ceilRadiusZ && !breakZ; ++z)
				{
					double zn = nextZn;
					nextZn = (z + 1)*invRadiusZ;

					double distanceSq = LengthSq(xn, zn);
					if (distanceSq > 1)
					{
						if (z == 0)
						{
							breakX = true;
							goto forX;
						}

						breakZ = true;
						goto forZ;
					}

					if (!filled)
					{
						if (LengthSq(nextXn, zn) <= 1 && LengthSq(xn, nextZn) <= 1)
						{
							continue;
						}
					}

					for (int y = 0; y < height; ++y)
					{
						if (SetBlock(pos + new Vector3(x, y, z), block))
						{
							++affected;
						}
						if (SetBlock(pos + new Vector3(-x, y, z), block))
						{
							++affected;
						}
						if (SetBlock(pos + new Vector3(x, y, -z), block))
						{
							++affected;
						}
						if (SetBlock(pos + new Vector3(-x, y, -z), block))
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

		private static double LengthSq(double x, double z)
		{
			return (x*x) + (z*z);
		}


		public void Naturalize(Player player, RegionSelector selector)
		{
			var min = selector.GetMin();
			var max = selector.GetMax();

			var level = player.Level;
			for (int x = min.X; x <= max.X; x++)
			{
				for (int z = min.Z; z <= max.Z; z++)
				{
					int depth = 0;
					for (int y = Math.Min(255, max.Y); y >= min.Y; y--)
					{
						var coordinates = new BlockCoordinates(x, y, z);
						if (level.IsAir(coordinates) || !level.GetBlock(coordinates).IsSolid)
						{
							if (depth != 0) depth = 4;
							continue;
						}

						switch (depth++)
						{
							case 0:
								SetBlock(new Grass() {Coordinates = coordinates});
								break;
							case 1:
							case 2:
							case 3:
								SetBlock(new Dirt() {Coordinates = coordinates});
								break;
							default:
								SetBlock(new Stone() {Coordinates = coordinates});
								break;
						}
					}
				}
			}
		}

		public void Fill(BlockCoordinates origin, Pattern pattern, Mask mask, int radius, int depth, bool fillUp = false)
		{
			Queue<BlockCoordinates> visits = new Queue<BlockCoordinates>();

			visits.Enqueue(origin); // Kick it off with some good stuff

			while (visits.Count > 0)
			{
				var coordinates = visits.Dequeue();

				if (origin.DistanceTo(coordinates) > radius) continue;
				if (depth != -1 && coordinates.Y <= origin.Y - depth) continue;

				if (!mask.Test(coordinates)) continue;

				Visit(coordinates, pattern);

				visits.Enqueue(coordinates + Level.East);
				visits.Enqueue(coordinates + Level.West);
				visits.Enqueue(coordinates + Level.North);
				visits.Enqueue(coordinates + Level.South);
				visits.Enqueue(coordinates + Level.Down);
				if (fillUp) visits.Enqueue(coordinates + Level.Up);
			}
		}

		private void Visit(BlockCoordinates coordinates, Pattern pattern)
		{
			SetBlock(coordinates, pattern);
		}
	}
}