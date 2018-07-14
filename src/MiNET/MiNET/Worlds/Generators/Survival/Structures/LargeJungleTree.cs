using System;
using MiNET.Blocks;
using MiNET.Utils;

namespace MiNET.Worlds.Generators.Survival.Structures
{
	public class LargeJungleTree : TreeStructure
	{
		public override string Name
		{
			get { return "JungleTree"; }
		}

		public override int MaxHeight
		{
			get { return 30; }
		}

		private int BaseSize = 9;
		private int Roots = 3;
		private float BranchLength = 13f;

		private int Branches = 3;
		private float VerticalStart = 0.32f;
		private float VerticalRand = 0.1f;
		public override void Create(ChunkColumn chunk, int x, int y, int z)
		{
			if (x < 6 || x > 10 || z < 6 || z > 10) return;

			var block = chunk.GetBlock(x, y - 1, z);
			if (block != 2 && block != 3) return;

			BaseSize = 9 + Rnd.Next(5);

			if (Roots > 0f)
			{
				for (int k = 0; k < 3; k++)
				{
					GenerateBranch(chunk, x, y + Roots, z, (120 * k) - 40 + Rnd.Next(80), 1.6f + (float)Rnd.NextDouble() * 0.1f, Roots * 1.7f, 1f, 
						17, 3); //17 = Wood, 3 = Jungle Type
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
				horDir = (120 * j) - 60 + Rnd.Next(120);
				verDir = VerticalStart + (float)Rnd.NextDouble() * VerticalRand;

				eX = x + (int)(Math.Cos(horDir * Math.PI / 180D) * verDir * BranchLength);
				eZ = z + (int)(Math.Sin(horDir * Math.PI / 180D) * verDir * BranchLength);
				eY = y + BaseSize + (int)((1f - verDir) * BranchLength);

				if (CanGenerateBranch(x, y + BaseSize, z, horDir, verDir, BranchLength, 1f, 4f, 1.5f) && CanGenerateLeaves(eX, eY, eZ, 4f, 1.5f))
				{
					GenerateBranch(chunk, x, y + BaseSize, z, horDir, verDir, BranchLength, 1f, 
						17, 3); //17 = Wood, 3 = Jungle Type

					for (int m = 0; m < 1; m++)
					{
						GenerateLeaves(chunk, eX, eY, eZ, 4f, 1.5f,
							18, 3, //18 = Leaves, 3 = Jungle Type
							17, 3); //17 = Wood, 3 = Jungle Type
					}
				}
			}
		}

		public override void Create(Level level, int x, int y, int z)
		{
			BaseSize = 9 + Rnd.Next(5);

			if (Roots > 0f)
			{
				for (int k = 0; k < 3; k++)
				{
					GenerateBranch(level, x, y + Roots, z, (120 * k) - 40 + Rnd.Next(80), 1.6f + (float)Rnd.NextDouble() * 0.1f, Roots * 1.7f, 1f,
						17, 3); //17 = Wood, 3 = Jungle Type
				}
			}

			for (int i = y + Roots; i < y + BaseSize; i++)
			{
				Block block = BlockFactory.GetBlockById(17);
				block.Metadata = 3;
				block.Coordinates = new BlockCoordinates(x, i, z);
				
				level.SetBlock(block);
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

				GenerateBranch(level, x, y + BaseSize, z, horDir, verDir, BranchLength, 1f,
					17, 3); //17 = Wood, 3 = Jungle Type

				for (int m = 0; m < 1; m++)
				{
					GenerateLeaves(level, eX, eY, eZ, 4f, 1.5f,
						18, 3, //18 = Leaves, 3 = Jungle Type
						17, 3); //17 = Wood, 3 = Jungle Type
				}
			}
		}
	}
}
