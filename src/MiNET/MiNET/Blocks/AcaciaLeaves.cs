using System;
using System.Collections.Generic;
using log4net;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class AcaciaLeaves : Block
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (AcaciaLeaves));

		public AcaciaLeaves() : base(161)
		{
			IsTransparent = true;
			BlastResistance = 1;
			Hardness = 0.2f;
			IsFlammable = true;
		}

		public override void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
		{
			// No decay
			if ((Metadata & 0x04) == 0x04)
				return;

			if ((Metadata & 0x08) == 0x08)
			{
				return;
			}

			Metadata = (byte) (Metadata | 0x08);
			level.SetBlock(this, false, false, false);
		}

		public override void OnTick(Level level, bool isRandom)
		{
			if ((Metadata & 0x04) == 0x04)
				return;

			if ((Metadata & 0x08) != 0x08)
			{
				return;
			}

			//Log.Debug("Checking decay");

			if (FindLog(level, Coordinates, new List<BlockCoordinates>(), 0))
			{
				Metadata = (byte) (Metadata & 0x07);
				level.SetBlock(this, false, false, false);
				return;
			}

			Log.Debug("Decay leaf");

			var drops = GetDrops(null);
			BreakBlock(level, drops.Length == 0);
			foreach (var drop in drops)
			{
				level.DropItem(Coordinates, drop);
			}
		}

		public override Item[] GetDrops(Item tool)
		{
			var rnd = new Random((int) DateTime.UtcNow.Ticks);
			if ((Metadata & 0x03) == 1) // Oak and dark oak drops apple
			{
				if (rnd.Next(200) == 0)
				{
					return new Item[] {ItemFactory.GetItem(260, 0, 1)};
				}
			}
			if (rnd.Next(20) == 0)
			{
				// Sapling
				return new Item[] {ItemFactory.GetItem(39, (short) (Metadata + 5), 1)};
			}
			return new Item[0];
		}

		private bool FindLog(Level level, BlockCoordinates coord, List<BlockCoordinates> visited, int distance)
		{
			if (visited.Contains(coord)) return false;

			var block = level.GetBlock(coord);
			if (block is AcaciaLog) return true;

			visited.Add(coord);

			if (distance >= 4) return false;

			if (!(block is AcaciaLeaves)) return false;
			if ((block.Metadata & 0x07) != (Metadata & 0x07)) return false;

			// check down
			if (FindLog(level, coord + BlockCoordinates.Down, visited, distance + 1)) return true;
			// check west
			if (FindLog(level, coord + BlockCoordinates.West, visited, distance + 1)) return true;
			// check east
			if (FindLog(level, coord + BlockCoordinates.East, visited, distance + 1)) return true;
			// check south
			if (FindLog(level, coord + BlockCoordinates.South, visited, distance + 1)) return true;
			// check north
			if (FindLog(level, coord + BlockCoordinates.North, visited, distance + 1)) return true;
			// check up
			if (FindLog(level, coord + BlockCoordinates.Up, visited, distance + 1)) return true;

			return false;
		}
	}
}