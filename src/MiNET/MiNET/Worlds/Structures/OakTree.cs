using MiNET.Blocks;
using MiNET.Utils;

namespace MiNET.Worlds.Structures
{
	class OakTree : Structure
	{
		public override string Name
		{
			get { return "OakTree"; }
		}

		public override int MaxHeight
		{
			get { return 7; }
		}

		public override Block[] Blocks
		{
			get
			{
                return new Block[]
                {
                    new Block(17) {Coordinates = new BlockCoordinates(0, 0, 0)},
                    new Block(17) {Coordinates = new BlockCoordinates(0, 1, 0)},
                    new Block(17) {Coordinates = new BlockCoordinates(0, 2, 0)},
                    new Block(17) {Coordinates = new BlockCoordinates(0, 3, 0)},
                    new Block(17) {Coordinates = new BlockCoordinates(0, 4, 0)},
                    new Block(17) {Coordinates = new BlockCoordinates(0, 5, 0)},
                    new Block(18) {Coordinates = new BlockCoordinates(-2, 3, 1)},
                    new Block(18) {Coordinates = new BlockCoordinates(-2, 3, 0)},
                    new Block(18) {Coordinates = new BlockCoordinates(-2, 3, -1)},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 3, 2)},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 3, 1)},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 3, 0)},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 3, -1)},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 3, -2)},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 3, 2)},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 3, 1)},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 3, -1)},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 3, -2)},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 3, 2)},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 3, 1)},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 3, 0)},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 3, -1)},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 3, -2)},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 3, 2)},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 3, 1)},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 3, 0)},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 3, -1)},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 3, -2)},
                    new Block(18) {Coordinates = new BlockCoordinates(-2, 4, 1)},
                    new Block(18) {Coordinates = new BlockCoordinates(-2, 4, 0)},
                    new Block(18) {Coordinates = new BlockCoordinates(-2, 4, -1)},
                    new Block(18) {Coordinates = new BlockCoordinates(-2, 4, -2)},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 4, 2)},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 4, 1)},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 4, 0)},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 4, -1)},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 4, -2)},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 4, 2)},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 4, 1)},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 4, -1)},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 4, -2)},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 4, 2)},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 4, 1)},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 4, 0)},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 4, -1)},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 4, -2)},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 4, 2)},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 4, 1)},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 4, 0)},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 4, -1)},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 5, 0)},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 5, 1)},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 5, -1)},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 5, 0)},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 5, -1)},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 6, 0)},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 6, 1)},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 6, 0)},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 6, -1)},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 6, 0)},
                };
			}
		}
	}
}
