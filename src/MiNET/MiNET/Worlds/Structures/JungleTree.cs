using MiNET.Blocks;
using MiNET.Utils;

namespace MiNET.Worlds.Structures
{
    class JungleTree : Structure
    {
        public override string Name
        {
            get { return "JungleTree"; }
        }

        public override int MaxHeight
        {
            get { return 8; }
        }

        public override Block[] Blocks
        {
            get
            {
                return new Block[]
                {
                    new Block(17) {Coordinates = new BlockCoordinates(0, 0, 0), Metadata = 3},
                    new Block(17) {Coordinates = new BlockCoordinates(0, 1, 0), Metadata = 3},
                    new Block(17) {Coordinates = new BlockCoordinates(0, 2, 0), Metadata = 3},
                    new Block(17) {Coordinates = new BlockCoordinates(0, 3, 0), Metadata = 3},
                    new Block(17) {Coordinates = new BlockCoordinates(0, 4, 0), Metadata = 3},
                    new Block(17) {Coordinates = new BlockCoordinates(0, 5, 0), Metadata = 3},
                    new Block(17) {Coordinates = new BlockCoordinates(0, 6, 0), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(-2, 4, 2), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(-2, 4, 1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(-2, 4, 0), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(-2, 4, -1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(-2, 4, -2), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 4, 2), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 4, 1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 4, 0), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 4, -1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 4, -2), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 4, 2), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 4, 1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 4, -1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 4, -2), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 4, 2), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 4, 1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 4, 0), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 4, -1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 4, -2), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 4, 1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 4, 0), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 4, -1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 4, -2), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(-2, 5, 1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(-2, 5, 0), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(-2, 5, -1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(-2, 5, -2), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 5, 2), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 5, 1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 5, 0), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 5, -1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 5, -2), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 5, 2), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 5, 1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 5, -1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 5, -2), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 5, 2), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 5, 1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 5, 0), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 5, -1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 5, -2), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 5, 2), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 5, 1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 5, 0), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 5, -1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 5, -2), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 6, 1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 6, 0), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 6, -1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 6, 1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 6, -1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 6, 1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 6, 0), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 6, -1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 7, 0), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 7, 1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 7, 0), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 7, -1), Metadata = 3},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 7, 0), Metadata = 3},
                };
            }
        }
    }
}
