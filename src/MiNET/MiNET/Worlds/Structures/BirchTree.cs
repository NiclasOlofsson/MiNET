using System;
using System.Numerics;
using MiNET.Blocks;
using MiNET.Utils;

namespace MiNET.Worlds.Structures
{
    class BirchTree : Structure
    {
        public override string Name
        {
            get { return "BirchTree"; }
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
                    new Block(17) {Coordinates = new BlockCoordinates(0, 0, 0), Metadata = 2},
                    new Block(17) {Coordinates = new BlockCoordinates(0, 1, 0), Metadata = 2},
                    new Block(17) {Coordinates = new BlockCoordinates(0, 2, 0), Metadata = 2},
                    new Block(17) {Coordinates = new BlockCoordinates(0, 3, 0), Metadata = 2},
                    new Block(17) {Coordinates = new BlockCoordinates(0, 4, 0), Metadata = 2},
                    new Block(17) {Coordinates = new BlockCoordinates(0, 5, 0), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(-2, 3, 1), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(-2, 3, 0), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(-2, 3, -1), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 3, 2), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 3, 1), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 3, 0), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 3, -1), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 3, -2), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 3, 2), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 3, 1), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 3, -1), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 3, -2), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 3, 2), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 3, 1), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 3, 0), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 3, -1), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 3, -2), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 3, 2), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 3, 1), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 3, 0), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 3, -1), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 3, -2), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(-2, 4, 1), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(-2, 4, 0), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(-2, 4, -1), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(-2, 4, -2), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 4, 2), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 4, 1), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 4, 0), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 4, -1), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 4, -2), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 4, 2), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 4, 1), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 4, -1), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 4, -2), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 4, 2), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 4, 1), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 4, 0), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 4, -1), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 4, -2), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 4, 2), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 4, 1), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 4, 0), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(2, 4, -1), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 5, 0), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 5, 1), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 5, -1), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 5, 0), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 5, -1), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(-1, 6, 0), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 6, 1), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 6, 0), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(0, 6, -1), Metadata = 2},
                    new Block(18) {Coordinates = new BlockCoordinates(1, 6, 0), Metadata = 2},
                };
            }
        }

		private const int LeafRadius = 2;
		public override void Create(ChunkColumn chunk, int x, int y, int z)
	    {
			var block = chunk.GetBlock(x, y - 1, z);
			if (block != 2 && block != 3) return;

			var location = new Vector3(x, y, z);
			//if (!ValidLocation(new Vector3(x, y, z))) return;

			Random R = new Random();
			int height = R.Next(4, 5);
			GenerateColumn(chunk, location, height, 17, 2);
			Vector3 leafLocation = location + new Vector3(0, height, 0);
			GenerateVanillaLeaves(chunk, leafLocation, LeafRadius, 18, 2);
		}
    }
}
