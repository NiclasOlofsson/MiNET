using System;
using MiNET.Blocks;
using MiNET.Utils;

namespace MiNET.Worlds.Generators.Survival.Structures
{
    class SmallJungleTree : TreeStructure
    {
        public override string Name
        {
            get { return "JungleTree"; }
        }

        public override int MaxHeight
        {
            get { return 6; }
        }

		private int BaseSize = 5;
		private int Roots = 0;
		private float BranchLength = 5f;

		private int Branches = 2;
		private float VerticalStart = 0.32f;
		private float VerticalRand = 0.14f;
		public override void Create(ChunkColumn chunk, int x, int y, int z)
	    {
			var block = chunk.GetBlock(x, y - 1, z);
			if (block != 2 && block != 3) return;

		    BaseSize = 3 + Rnd.Next(2);
			if (Roots > 0f)
			{
				for (int k = 0; k < 3; k++)
				{
					GenerateBranch(chunk, x, y + Roots, z, (120 * k) - 40 + Rnd.Next(80), 1.6f + (float)Rnd.NextDouble() * 0.1f, Roots * 1.7f, 1f, 17, 3);
				}
			}

			for (int i = y + Roots; i < y + BaseSize; i++)
			{
				chunk.SetBlock(x, i, z, 17);
				chunk.SetMetadata(x, i, z, 3);
			}

			float horDir, verDir;
			int eX, eY, eZ;
			for (int j = 0; j < Branches; j++)
			{
				horDir = (120*j) - 60 + Rnd.Next(120);
				verDir = VerticalStart + (float)Rnd.NextDouble() * VerticalRand;

				eX = x + (int)(Math.Cos(horDir * Math.PI / 180D) * verDir * BranchLength);
				eZ = z + (int)(Math.Sin(horDir * Math.PI / 180D) * verDir * BranchLength);
				eY = y + BaseSize + (int)((1f - verDir) * BranchLength);

				if (CanGenerateBranch(x, y + BaseSize, z, horDir, verDir, BranchLength, 1f, 4f, 1.5f) && CanGenerateLeaves(eX, eY, eZ, 4f, 1.5f))
				{
					GenerateBranch(chunk, x, y + BaseSize, z, horDir, verDir, BranchLength, 1f, 17, 3);

					for (int m = 0; m < 1; m++)
					{
						GenerateLeaves(chunk, eX, eY, eZ, 4f, 1.5f, 18, 3, 17, 3);
					}
				}
			}

			/*var location = new Vector3(x, y, z);
			if (!ValidLocation(location, 2)) return;

			int height = Math.Max(4, Rnd.Next(MaxHeight));

			GenerateColumn(chunk, location, height, 17, 3);
			Vector3 leafLocation = location + new Vector3(0, height, 0);
			GenerateVanillaLeaves(chunk, leafLocation, 2, 18, 3);
			*/
			//	base.Create(chunk, x, y, z);
		}

	    public override void Create(Level level, int x, int y, int z)
	    {
		    BaseSize = 3 + Rnd.Next(2);
		    if (Roots > 0f)
		    {
			    for (int k = 0; k < 3; k++)
			    {
				    GenerateBranch(level, x, y + Roots, z, (120*k) - 40 + Rnd.Next(80), 1.6f + (float) Rnd.NextDouble()*0.1f,
					    Roots*1.7f, 1f, 17, 3);
			    }
		    }

		    for (int i = y + Roots; i < y + BaseSize; i++)
		    {
			    Block b = BlockFactory.GetBlockById(17);
			    b.Metadata = 3;
			    b.Coordinates = new BlockCoordinates(x, i, z);

			    level.SetBlock(b);
		    }

		    float horDir, verDir;
		    int eX, eY, eZ;
		    for (int j = 0; j < Branches; j++)
		    {
			    horDir = (120*j) - 60 + Rnd.Next(120);
			    verDir = VerticalStart + (float) Rnd.NextDouble()*VerticalRand;

			    eX = x + (int) (Math.Cos(horDir*Math.PI/180D)*verDir*BranchLength);
			    eZ = z + (int) (Math.Sin(horDir*Math.PI/180D)*verDir*BranchLength);
			    eY = y + BaseSize + (int) ((1f - verDir)*BranchLength);


			    GenerateBranch(level, x, y + BaseSize, z, horDir, verDir, BranchLength, 1f, 17, 3);

			    for (int m = 0; m < 1; m++)
			    {
				    GenerateLeaves(level, eX, eY, eZ, 4f, 1.5f, 18, 3, 17, 3);
			    }
		    }
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
