using System.Numerics;
using MiNET.Blocks;
using MiNET.Utils;

namespace MiNET.Worlds.Generators.Survival.Structures
{
	public class PineTree : TreeStructure
	{
		private readonly int _leafRadius = 2;

		public override string Name
		{
			get { return "PineTree"; }
		}

		public override int MaxHeight
		{
			get { return 10; }
		}

		public override void Create(ChunkColumn chunk, int x, int y, int z)
		{
			var block = chunk.GetBlock(x, y - 1, z);
			if (block != 2 && block != 3) return;

			var location = new Vector3(x, y, z);
			if (!ValidLocation(location, _leafRadius)) return;

			var height = Rnd.Next(7, 8);
			GenerateColumn(chunk, location, height, 17, 1);
			for (var Y = 1; Y < height; Y++)
			{
				if (Y % 2 == 0)
				{
					GenerateVanillaCircle(chunk, location + new Vector3(0, Y + 1, 0), _leafRadius - 1, 18, 1);
					continue;
				}
				GenerateVanillaCircle(chunk, location + new Vector3(0, Y + 1, 0), _leafRadius, 18, 1);
			}

			GenerateTopper(chunk, location + new Vector3(0, height, 0), 0x1);
		}

		protected void GenerateTopper(ChunkColumn chunk, Vector3 location, byte type = 0x0)
		{
			var sectionRadius = 1;
			GenerateCircle(chunk, location, sectionRadius, 18, 1);
			var top = location + new Vector3(0, 1, 0);
			var x = (int)location.X;
			var y = (int)location.Y + 1;
			var z = (int)location.Z;
			chunk.SetBlock(x, y, z, 18);
			chunk.SetMetadata(x, y, z, 1);
			if (type == 0x1 && y < 256)
				GenerateVanillaCircle(chunk, new Vector3(x, y, z), sectionRadius, 18, 1);
		}

		public override void Create(Level chunk, int x, int y, int z)
		{
			var block = chunk.GetBlock(x, y - 1, z).Id;
			if (block != 2 && block != 3) return;

			var location = new Vector3(x, y, z);

			var height = Rnd.Next(7, 8);
			GenerateColumn(chunk, location, height, 17, 1);
			for (var Y = 1; Y < height; Y++)
			{
				if (Y % 2 == 0)
				{
					GenerateVanillaCircle(chunk, location + new Vector3(0, Y + 1, 0), _leafRadius - 1, 18, 1);
					continue;
				}
				GenerateVanillaCircle(chunk, location + new Vector3(0, Y + 1, 0), _leafRadius, 18, 1);
			}

			GenerateTopper(chunk, location + new Vector3(0, height, 0), 0x1);
		}

		protected void GenerateTopper(Level chunk, Vector3 location, byte type = 0x0)
		{
			var sectionRadius = 1;
			GenerateCircle(chunk, location, sectionRadius, 18, 1);
			var top = location + new Vector3(0, 1, 0);
			var x = (int)location.X;
			var y = (int)location.Y + 1;
			var z = (int)location.Z;

			Block block = BlockFactory.GetBlockById(18);
			block.Metadata = 1;
			block.Coordinates = new BlockCoordinates(x,y,z);

			chunk.SetBlock(block);
			if (type == 0x1 && y < 256)
				GenerateVanillaCircle(chunk, new Vector3(x, y, z), sectionRadius, 18, 1);
		}
	}
}
