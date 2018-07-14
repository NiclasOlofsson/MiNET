using System;
using System.Collections.Generic;
using System.Text;
using MiNET.Blocks;
using MiNET.Utils;

namespace MiNET.Worlds.Generators.Survival.Structures
{
	public class Structure
	{
		protected readonly Random Rnd = new Random();
		public virtual string Name
		{
			get { return null; }
		}

		public virtual Block[] Blocks
		{
			get { return null; }
		}

		public virtual int MaxHeight { get { return 0; } }

		public virtual void Create(ChunkColumn chunk, int x, int y, int z)
		{
			if (chunk.GetBlock(x, y + MaxHeight, z) == (byte)0)
			{
				foreach (Block b in Blocks)
				{
					chunk.SetBlock(x + b.Coordinates.X, y + b.Coordinates.Y, z + b.Coordinates.Z, b.Id);
					chunk.SetMetadata(x + b.Coordinates.X, y + b.Coordinates.Y, z + b.Coordinates.Z, b.Metadata);
				}
			}
		}

		public virtual void Create(Level level, int x, int y, int z)
		{
			if (level.IsAir(new BlockCoordinates(x, y + MaxHeight, z)))
			{
				foreach (Block b in Blocks)
				{
					Block clone = BlockFactory.GetBlockById(b.Id);
					clone.Metadata = b.Metadata;
					clone.Coordinates = new BlockCoordinates(x + b.Coordinates.X, y + b.Coordinates.Y, z + b.Coordinates.Z);

					level.SetBlock(clone);
				}
			}
		}
	}
}
