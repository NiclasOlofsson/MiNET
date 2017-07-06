using System;
using System.Numerics;
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

		private readonly int _leafRadius = 2;
		public override void Create(ChunkColumn chunk, int x, int y, int z)
		{
			var block = chunk.GetBlock(x, y - 1, z);
			if (block != 2 && block != 3) return;

			var location = new Vector3(x, y, z);
		//	if (!ValidLocation(new Vector3(x, y, z))) return;

			Random r = new Random();
			int height = r.Next(4, 5);
			GenerateColumn(chunk, location, height, 17, 0);
			Vector3 leafLocation = location + new Vector3(0, height, 0);
			GenerateVanillaLeaves(chunk, leafLocation, _leafRadius, 18, 0);
		}
	}
}
