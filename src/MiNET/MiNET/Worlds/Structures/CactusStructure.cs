namespace MiNET.Worlds.Structures
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

			for (int modifiedY = y; modifiedY < y + _height; modifiedY++)
			{
				if (!CheckSafe(chunk, x, modifiedY, z)) break;

				chunk.SetBlock(x, modifiedY, z, 81); //Cactus block
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
	}
}
