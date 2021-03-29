using System;
using System.Collections.Generic;
using System.Numerics;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.BuilderBase
{
	public class HistoryEntry
	{
		public Level Level { get; set; }
		public BlockCoordinates Position1 { get; set; }
		public BlockCoordinates Position2 { get; set; }
		public List<Block> Presnapshot { get; set; }
		public List<Block> Postsnapshot { get; set; }

		public HistoryEntry(Level level, List<Block> presnapshot, List<Block> postsnapshot)
		{
			Level = level;
			Presnapshot = presnapshot;
			Postsnapshot = postsnapshot;
		}

		public HistoryEntry(Level level, BlockCoordinates position1, BlockCoordinates position2)
		{
			Level = level;
			Position1 = position1;
			Position2 = position2;
		}

		public void Snapshot(bool pre = true)
		{
			long time = DateTime.UtcNow.Ticks;

			BoundingBox box = new BoundingBox(Position1, Position2);

			var minX = Math.Min(box.Min.X, box.Max.X);
			var maxX = Math.Max(box.Min.X, box.Max.X);

			var minY = Math.Min(box.Min.Y, box.Max.Y);
			var maxY = Math.Max(box.Min.Y, box.Max.Y);

			var minZ = Math.Min(box.Min.Z, box.Max.Z);
			var maxZ = Math.Max(box.Min.Z, box.Max.Z);

			List<BlockCoordinates> coords = new List<BlockCoordinates>();

			// x/y
			for (float x = minX; x <= maxX; x++)
			{
				for (float y = minY; y <= maxY; y++)
				{
					for (float z = minZ; z <= maxZ; z++)
					{
						coords.Add(new Vector3(x, y, z));
					}
				}
			}

			List<Block> snapshot = new List<Block>();
			foreach (var coord in coords.ToArray())
			{
				Block block = Level.GetBlock(coord);
				snapshot.Add(block);
			}

			if (pre)
			{
				Presnapshot = snapshot;
			}
			else
			{
				Postsnapshot = snapshot;
			}
		}
	}
}