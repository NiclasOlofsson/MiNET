using System;
using System.Collections.Generic;
using log4net;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Farmland : Block
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Farmland));

		public Farmland() : base(60)
		{
			IsTransparent = false; // Partial - blocks light.
			BlastResistance = 3;
			Hardness = 0.6f;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new[] {new ItemBlock(new Dirt(), 0) {Count = 1}}; // Drop dirt block
		}

		public override void OnTick(Level level, bool isRandom)
		{
			byte data = Metadata;
			if (FindWater(level, Coordinates, new List<BlockCoordinates>(), 0))
			{
				Metadata = 7;
			}
			else
			{
				Metadata = (byte) Math.Max(0, Metadata - 1);
			}
			if (data != Metadata)
			{
				level.SetBlock(this);
			}
		}

		public bool FindWater(Level level, BlockCoordinates coord, List<BlockCoordinates> visited, int distance)
		{
			if (visited.Contains(coord)) return false;

			var block = level.GetBlock(coord);
			if (block is StationaryWater || block is FlowingWater) return true;

			visited.Add(coord);

			if (distance >= 4) return false;

			// check down
			//if (FindWater(level, coord + BlockCoordinates.Down, visited, distance + 1)) return true;
			// check west
			if (FindWater(level, coord + BlockCoordinates.West, visited, distance + 1)) return true;
			// check east
			if (FindWater(level, coord + BlockCoordinates.East, visited, distance + 1)) return true;
			// check south
			if (FindWater(level, coord + BlockCoordinates.South, visited, distance + 1)) return true;
			// check north
			if (FindWater(level, coord + BlockCoordinates.North, visited, distance + 1)) return true;
			// check up
			//if (FindWater(level, coord + BlockCoordinates.Up, visited, distance + 1)) return true;

			return false;
		}
	}
}