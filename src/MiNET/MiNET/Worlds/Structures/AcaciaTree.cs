using MiNET.Blocks;
using MiNET.Utils;

namespace MiNET.Worlds.Structures
{
    class AcaciaTree : Structure
    {
        public override string Name
        {
            get { return "AcaciaTree"; }
        }

        public override int MaxHeight
        {
            get { return 8; }
        }

	    public override void Create(ChunkColumn chunk, int x, int y, int z)
	    {
		    if (x > 11 || z > 11) return;
		    if (x < 5 || z < 5) return;

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
                    new Block(162) {Coordinates = new BlockCoordinates(0, 0, 0)},
                    new Block(162) {Coordinates = new BlockCoordinates(0, 1, 0)},
                    new Block(162) {Coordinates = new BlockCoordinates(0, 2, 0)},
                    new Block(162) {Coordinates = new BlockCoordinates(0, 3, 0)},
                    new Block(162) {Coordinates = new BlockCoordinates(0, 4, 0)},
                    new Block(162) {Coordinates = new BlockCoordinates(0, 5, -1)},
                    new Block(162) {Coordinates = new BlockCoordinates(0, 6, -2)},
                    new Block(161) {Coordinates = new BlockCoordinates(-3, 6, 0)},
                    new Block(161) {Coordinates = new BlockCoordinates(-3, 6, -1)},
                    new Block(161) {Coordinates = new BlockCoordinates(-3, 6, -2)},
                    new Block(161) {Coordinates = new BlockCoordinates(-3, 6, -3)},
                    new Block(161) {Coordinates = new BlockCoordinates(-3, 6, -4)},
                    new Block(161) {Coordinates = new BlockCoordinates(-2, 6, 1)},
                    new Block(161) {Coordinates = new BlockCoordinates(-2, 6, 0)},
                    new Block(161) {Coordinates = new BlockCoordinates(-2, 6, -1)},
                    new Block(161) {Coordinates = new BlockCoordinates(-2, 6, -2)},
                    new Block(161) {Coordinates = new BlockCoordinates(-2, 6, -3)},
                    new Block(161) {Coordinates = new BlockCoordinates(-2, 6, -4)},
                    new Block(161) {Coordinates = new BlockCoordinates(-2, 6, -5)},
                    new Block(161) {Coordinates = new BlockCoordinates(-1, 6, 1)},
                    new Block(161) {Coordinates = new BlockCoordinates(-1, 6, 0)},
                    new Block(161) {Coordinates = new BlockCoordinates(-1, 6, -1)},
                    new Block(161) {Coordinates = new BlockCoordinates(-1, 6, -2)},
                    new Block(161) {Coordinates = new BlockCoordinates(-1, 6, -3)},
                    new Block(161) {Coordinates = new BlockCoordinates(-1, 6, -4)},
                    new Block(161) {Coordinates = new BlockCoordinates(-1, 6, -5)},
                    new Block(161) {Coordinates = new BlockCoordinates(0, 6, 1)},
                    new Block(161) {Coordinates = new BlockCoordinates(0, 6, 0)},
                    new Block(161) {Coordinates = new BlockCoordinates(0, 6, -1)},
                    new Block(161) {Coordinates = new BlockCoordinates(0, 6, -2)},
                    new Block(161) {Coordinates = new BlockCoordinates(0, 6, -3)},
                    new Block(161) {Coordinates = new BlockCoordinates(0, 6, -4)},
                    new Block(161) {Coordinates = new BlockCoordinates(0, 6, -5)},
                    new Block(161) {Coordinates = new BlockCoordinates(1, 6, 1)},
                    new Block(161) {Coordinates = new BlockCoordinates(1, 6, 0)},
                    new Block(161) {Coordinates = new BlockCoordinates(1, 6, -1)},
                    new Block(161) {Coordinates = new BlockCoordinates(1, 6, -2)},
                    new Block(161) {Coordinates = new BlockCoordinates(1, 6, -3)},
                    new Block(161) {Coordinates = new BlockCoordinates(1, 6, -4)},
                    new Block(161) {Coordinates = new BlockCoordinates(1, 6, -5)},
                    new Block(161) {Coordinates = new BlockCoordinates(2, 6, 1)},
                    new Block(161) {Coordinates = new BlockCoordinates(2, 6, 0)},
                    new Block(161) {Coordinates = new BlockCoordinates(2, 6, -1)},
                    new Block(161) {Coordinates = new BlockCoordinates(2, 6, -2)},
                    new Block(161) {Coordinates = new BlockCoordinates(2, 6, -3)},
                    new Block(161) {Coordinates = new BlockCoordinates(2, 6, -4)},
                    new Block(161) {Coordinates = new BlockCoordinates(2, 6, -5)},
                    new Block(161) {Coordinates = new BlockCoordinates(3, 6, 0)},
                    new Block(161) {Coordinates = new BlockCoordinates(3, 6, -1)},
                    new Block(161) {Coordinates = new BlockCoordinates(3, 6, -2)},
                    new Block(161) {Coordinates = new BlockCoordinates(3, 6, -3)},
                    new Block(161) {Coordinates = new BlockCoordinates(3, 6, -4)},
                    new Block(161) {Coordinates = new BlockCoordinates(0, 7, 0)},
                    new Block(161) {Coordinates = new BlockCoordinates(-1, 7, -1)},
                    new Block(161) {Coordinates = new BlockCoordinates(0, 7, -1)},
                    new Block(161) {Coordinates = new BlockCoordinates(1, 7, -1)},
                    new Block(161) {Coordinates = new BlockCoordinates(-2, 7, -2)},
                    new Block(161) {Coordinates = new BlockCoordinates(-1, 7, -2)},
                    new Block(161) {Coordinates = new BlockCoordinates(0, 7, -2)},
                    new Block(161) {Coordinates = new BlockCoordinates(1, 7, -2)},
                    new Block(161) {Coordinates = new BlockCoordinates(2, 7, -2)},
                    new Block(161) {Coordinates = new BlockCoordinates(-1, 7, -3)},
                    new Block(161) {Coordinates = new BlockCoordinates(0, 7, -3)},
                    new Block(161) {Coordinates = new BlockCoordinates(1, 7, -3)},
                    new Block(161) {Coordinates = new BlockCoordinates(0, 7, -4)},
                };
            }
        }
    }
}
