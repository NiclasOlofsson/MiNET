using MiNET.Blocks;
using MiNET.Utils;

namespace MiNET.Worlds.Generators.Survival.Structures
{
    public class SpruceTree : TreeStructure
    {
        public override string Name
        {
            get { return "SpruceTree"; }
        }

        public override int MaxHeight
        {
            get { return 8; }
        }

	    public override void Create(ChunkColumn chunk, int x, int y, int z)
	    {
			var block = chunk.GetBlock(x, y - 1, z);
			if (block != 2 && block != 3) return;

			base.Create(chunk, x, y, z);
	    }

	    public override Block[] Blocks
        {
            get
            {
                return new Block[]
                {
                    new Block(17) {Coordinates = new BlockCoordinates(0, 0, 0), Metadata = 1},
                    new Block(17) {Coordinates = new BlockCoordinates(0, 1, 0), Metadata = 1},
                    new Block(17) {Coordinates = new BlockCoordinates(0, 2, 0), Metadata = 1},
                    new Block(17) {Coordinates = new BlockCoordinates(0, 3, 0), Metadata = 1},
                    new Block(17) {Coordinates = new BlockCoordinates(0, 4, 0), Metadata = 1},
                    new Block(17) {Coordinates = new BlockCoordinates(0, 5, 0), Metadata = 1},
                    new Block(17) {Coordinates = new BlockCoordinates(0, 6, 0), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 2, 0), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 2, 1), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 2, -1), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 2, 0), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(-2, 3, 1), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(-2, 3, 0), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(-2, 3, -1), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 3, 2), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 3, 1), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 3, 0), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 3, -1), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 3, -2), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 3, 2), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 3, 1), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 3, 0), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 3, -1), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 3, -2), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 3, 2), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 3, 1), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 3, 0), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 3, -1), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 3, -2), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 3, 1), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 3, 0), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 3, -1), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 4, 0), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 4, 1), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 4, -1), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 4, 0), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 6, 0), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 6, 1), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 6, -1), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 6, 0), Metadata = 1},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 7, 0), Metadata = 1},
                };
            }
        }
    }
}

