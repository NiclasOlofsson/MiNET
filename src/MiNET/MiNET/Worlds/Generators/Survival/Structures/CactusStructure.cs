using MiNET.Blocks;
using MiNET.Utils;

namespace MiNET.Worlds.Generators.Survival.Structures
{
	public class CactusStructure : Structure
	{
		public CactusStructure() : this(3)
		{

		}

		private int _height;

		public CactusStructure(int height)
		{
			_height = height;
		}

		public override void Create(ChunkColumn chunk, int x, int y, int z)
		{
			if (chunk.GetBlock(x, y - 1, z) != 12) return; //Not sand, do not generate.

			var growth = Rnd.Next(0x1, 0x15);
			for (int modifiedY = y; modifiedY < y + _height; modifiedY++)
			{
				if (!CheckSafe(chunk, x, modifiedY, z)) break;

				chunk.SetBlock(x, modifiedY, z, 81); //Cactus block
				chunk.SetMetadata(x, modifiedY, z, (byte)growth);
			}
		}

		public override void Create(Level level, int x, int y, int z)
		{
			if (level.GetBlock(x, y - 1, z).Id != 12) return; //Not sand, do not generate.

			var growth = Rnd.Next(1, 15);
			for (int modifiedY = y; modifiedY < y + _height; modifiedY++)
			{
				if (!CheckSafe(level, x, modifiedY, z)) break;

				Block b = BlockFactory.GetBlockById(81);
				b.Metadata = (byte) growth;
				b.Coordinates = new BlockCoordinates(x, modifiedY, z);

				level.SetBlock(b);
			}
		}

		private bool CheckSafe(ChunkColumn chunk, int x, int y, int z)
		{
			if (chunk.GetBlock(x - 1, y , z) != 0) return false;
			if (chunk.GetBlock(x + 1, y, z) != 0) return false;
			if (chunk.GetBlock(x, y, z - 1) != 0) return false;
			if (chunk.GetBlock(x, y, z + 1) != 0) return false;

			return true;
		}

		private bool CheckSafe(Level level, int x, int y, int z)
		{
			if (level.IsAir(new BlockCoordinates(x - 1, y, z))) return false;
			if (level.IsAir(new BlockCoordinates(x + 1, y, z))) return false;
			if (level.IsAir(new BlockCoordinates(x, y, z - 1))) return false;
			if (level.IsAir(new BlockCoordinates(x, y, z + 1))) return false;

			return true;
		}
	}
}
