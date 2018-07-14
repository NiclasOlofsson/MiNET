using System;
using System.Numerics;
using MiNET.Blocks;
using MiNET.Utils;

namespace MiNET.Worlds.Generators.Survival.Structures
{
	//Partly taken from https://github.com/Ted80-Minecraft-Mods/Realistic-World-Gen
	//And from https://github.com/SirCmpwn/TrueCraft
	public class TreeStructure : Structure
	{
		protected void GenerateVanillaLeaves(Level chunk, Vector3 location, int radius, byte id, byte meta)
		{
			var radiusOffset = radius;
			for (var yOffset = -radius; yOffset <= radius; yOffset = (yOffset + 1))
			{
				var y = location.Y + yOffset;
				if (y > 256)
					continue;
				GenerateVanillaCircle(chunk, new Vector3(location.X, y, location.Z), radiusOffset, id, meta);
				if (yOffset != -radius && yOffset % 2 == 0)
					radiusOffset--;
			}
		}

		protected void GenerateVanillaLeaves(ChunkColumn chunk, Vector3 location, int radius, byte id, byte meta)
		{
			if (location.X > 16 || location.X < 0 || location.Z > 16 || location.Z < 0) return;
			var radiusOffset = radius;
			for (var yOffset = -radius; yOffset <= radius; yOffset = (yOffset + 1))
			{
				var y = location.Y + yOffset;
				if (y > 255)
					continue;
				GenerateVanillaCircle(chunk, new Vector3(location.X, y, location.Z), radiusOffset, id, meta);
				if (yOffset != -radius && yOffset % 2 == 0)
					radiusOffset--;
			}
		}
		protected void GenerateVanillaCircle(Level chunk, Vector3 location, int radius, byte id, byte meta,
	double corner = 0)
		{
			for (var I = -radius; I <= radius; I = (I + 1))
			{
				for (var j = -radius; j <= radius; j = (j + 1))
				{
					var max = (int)Math.Sqrt((I * I) + (j * j));
					if (max <= radius)
					{
						if (I.Equals(-radius) && j.Equals(-radius) || I.Equals(-radius) && j.Equals(radius) ||
							I.Equals(radius) && j.Equals(-radius) || I.Equals(radius) && j.Equals(radius))
						{
							if (corner + radius * 0.2 < 0.4 || corner + radius * 0.2 > 0.7 || corner.Equals(0))
								continue;
						}
						var x = location.X + I;
						var z = location.Z + j;
						if (chunk.IsAir(new BlockCoordinates((int)x, (int)location.Y, (int)z)))
						{
							var block = BlockFactory.GetBlockById(id);
							block.Metadata = meta;
							block.Coordinates = new BlockCoordinates((int)x, (int)location.Y, (int)z);

							chunk.SetBlock(block);
							//chunk.SetBlock((int)x, (int)location.Y, (int)z, id);
							//chunk.SetMetadata((int)x, (int)location.Y, (int)z, meta);
						}
					}
				}
			}
		}

		protected void GenerateVanillaCircle(ChunkColumn chunk, Vector3 location, int radius, byte id, byte meta,
			double corner = 0)
		{
			for (var I = -radius; I <= radius; I = (I + 1))
			{
				for (var j = -radius; j <= radius; j = (j + 1))
				{
					var max = (int)Math.Sqrt((I * I) + (j * j));
					if (max <= radius)
					{
						if (I.Equals(-radius) && j.Equals(-radius) || I.Equals(-radius) && j.Equals(radius) ||
							I.Equals(radius) && j.Equals(-radius) || I.Equals(radius) && j.Equals(radius))
						{
							if (corner + radius * 0.2 < 0.4 || corner + radius * 0.2 > 0.7 || corner.Equals(0))
								continue;
						}
						var x = location.X + I;
						var z = location.Z + j;
						if (x < 0 || z > 16) continue;
						if (z < 0 || z > 16) continue;

						if (chunk.GetBlock((int)x, (int)location.Y, (int)z) == 0)
						{
							chunk.SetBlock((int)x, (int)location.Y, (int)z, id);
							chunk.SetMetadata((int)x, (int)location.Y, (int)z, meta);
						}
					}
				}
			}
		}

		public static void GenerateColumn(ChunkColumn chunk, Vector3 location, int height, byte id, byte meta)
		{
			for (var o = 0; o < height; o++)
			{
				var x = (int)location.X;
				var y = (int)location.Y + o;
				var z = (int)location.Z;
				chunk.SetBlock(x, y, z, id);
				chunk.SetMetadata(x, y, z, meta);
			}
		}

		public static void GenerateColumn(Level chunk, Vector3 location, int height, byte id, byte meta)
		{
			for (var o = 0; o < height; o++)
			{
				var block = BlockFactory.GetBlockById(id);
				block.Metadata = meta;
				block.Coordinates = new BlockCoordinates((int)location.X, (int)location.Y + o, (int) location.Z);
				
				chunk.SetBlock(block);
			}
		}

		protected void GenerateCircle(ChunkColumn chunk, Vector3 location, int radius, byte id, byte meta)
		{
			for (var I = -radius; I <= radius; I = (I + 1))
			{
				for (var j = -radius; j <= radius; j = (j + 1))
				{
					var max = (int)Math.Sqrt((I * I) + (j * j));
					if (max <= radius)
					{
						var X = location.X + I;
						var Z = location.Z + j;

						if (X < 0 || X >= 16 || Z < 0 || Z >= 256)
							continue;

						var x = (int)X;
						var y = (int)location.Y;
						var z = (int)Z;
						if (chunk.GetBlock(x, y, z).Equals(0))
						{
							chunk.SetBlock(x, y, z, id);
							chunk.SetMetadata(x, y, z, meta);
						}
					}
				}
			}
		}

		protected void GenerateCircle(Level chunk, Vector3 location, int radius, byte id, byte meta)
		{
			for (var I = -radius; I <= radius; I = (I + 1))
			{
				for (var j = -radius; j <= radius; j = (j + 1))
				{
					var max = (int)Math.Sqrt((I * I) + (j * j));
					if (max <= radius)
					{
						var X = location.X + I;
						var Z = location.Z + j;

						if (X < 0 || X >= 16 || Z < 0 || Z >= 256)
							continue;

						var x = (int)X;
						var y = (int)location.Y;
						var z = (int)Z;

						if (chunk.IsAir(new BlockCoordinates(x,y,z)))
						{
							Block block = BlockFactory.GetBlockById(id);
							block.Metadata = meta;
							block.Coordinates = new BlockCoordinates(x,y,z);

							chunk.SetBlock(block);
						}
					}
				}
			}
		}

		protected bool CanGenerateBranch(float x, float y, float z, float horDir, float verDir, float branchLength, float speed, float size, float width)
		{
			if (verDir < 0f)
			{
				verDir = -verDir;
			}

			float c = 0f;
			float velY = 1f - verDir;

			if (verDir > 1f)
			{
				verDir = 1f - (verDir - 1f);
			}

			float velX = (float)Math.Cos(horDir * Math.PI / 180D) * verDir;
			float velZ = (float)Math.Sin(horDir * Math.PI / 180D) * verDir;

			while (c < branchLength)
			{
				x += velX;
				y += velY;
				z += velZ;

				c += speed;

				if (x < 0 || x >= 16 || z < 0 || z >= 16) return false;
			}

			int i, j, k, s = (int)(size - 1f), w = (int)((size - 1f) * width);
			for (i = -w; i <= w; i++)
			{
				for (j = -s; j <= s; j++)
				{
					for (k = -w; k <= w; k++)
					{
						if (x + i < 0 || x + i >= 16 || z + k < 0 || z + k >= 16) return false;
					}
				}
			}

			return true;
		}

		protected void GenerateBranch(ChunkColumn world, float x, float y, float z, double horDir, float verDir, float length, float speed, byte blockId, byte meta)
		{
			if (verDir < 0f)
			{
				verDir = -verDir;
			}

			float c = 0f;
			float velY = 1f - verDir;

			if (verDir > 1f)
			{
				verDir = 1f - (verDir - 1f);
			}

			float velX = (float)Math.Cos(horDir * Math.PI / 180D) * verDir;
			float velZ = (float)Math.Sin(horDir * Math.PI / 180D) * verDir;

			while (c < length)
			{
				world.SetBlock((int)x, (int)y, (int)z, blockId);
				world.SetMetadata((int)x, (int)y, (int)z, meta);

				x += velX;
				y += velY;
				z += velZ;

				c += speed;
			}
		}

		protected void GenerateBranch(Level world, float x, float y, float z, double horDir, float verDir, float length, float speed, byte blockId, byte meta)
		{
			if (verDir < 0f)
			{
				verDir = -verDir;
			}

			float c = 0f;
			float velY = 1f - verDir;

			if (verDir > 1f)
			{
				verDir = 1f - (verDir - 1f);
			}

			float velX = (float)Math.Cos(horDir * Math.PI / 180D) * verDir;
			float velZ = (float)Math.Sin(horDir * Math.PI / 180D) * verDir;

			while (c < length)
			{
				Block block = BlockFactory.GetBlockById(blockId);
				block.Metadata = meta;
				block.Coordinates = new BlockCoordinates((int)x, (int) y, (int) z);

				world.SetBlock(block);

				x += velX;
				y += velY;
				z += velZ;

				c += speed;
			}
		}

		protected bool CanGenerateLeaves(int x, int y, int z, float size, float width)
		{
			float dist;
			int i, j, k, s = (int)(size - 1f), w = (int)((size - 1f) * width);
			for (i = -w; i <= w; i++)
			{
				for (j = -s; j <= s; j++)
				{
					for (k = -w; k <= w; k++)
					{
						dist = Math.Abs((float)i / width) + (float)Math.Abs(j) + Math.Abs((float)k / width);
						if (dist <= size - 0.5f || (dist <= size && Rnd.NextDouble() < 0.5))
						{
							var mx = x + i;
							var mz = z + k;

							if (mx < 0 || mx >= 16 || mz < 0 || mz >= 16) return false;
						}
					}
				}
			}
			return true;
		}

		protected void GenerateLeaves(ChunkColumn world, int x, int y, int z, float size, float width, byte leafBlockId, byte leafMeta, byte woodId, byte woodMeta)
		{
			float dist;
			int i, j, k, s = (int)(size - 1f), w = (int)((size - 1f) * width);
			for (i = -w; i <= w; i++)
			{
				for (j = -s; j <= s; j++)
				{
					for (k = -w; k <= w; k++)
					{
						dist = Math.Abs((float)i / width) + (float)Math.Abs(j) + Math.Abs((float)k / width);
						if (dist <= size - 0.5f || (dist <= size && Rnd.NextDouble() < 0.5))
						{ 
							if (dist < 0.6f)
							{
								world.SetBlock(x + i, y + j, z + k, woodId);
								world.SetMetadata(x + i, y + j, z + k, woodMeta);
							}
							if (world.GetBlock(x + i, y + j, z + k) == 0)
							{
								world.SetBlock(x + i, y + j, z + k, leafBlockId);
								world.SetMetadata(x + i, y + j, z + k, leafMeta);
							}
						}
					}
				}
			}
		}

		protected void GenerateLeaves(Level world, int x, int y, int z, float size, float width, byte leafBlockId, byte leafMeta, byte woodId, byte woodMeta)
		{
			float dist;
			int i, j, k, s = (int)(size - 1f), w = (int)((size - 1f) * width);
			for (i = -w; i <= w; i++)
			{
				for (j = -s; j <= s; j++)
				{
					for (k = -w; k <= w; k++)
					{
						dist = Math.Abs((float)i / width) + (float)Math.Abs(j) + Math.Abs((float)k / width);
						if (dist <= size - 0.5f || (dist <= size && Rnd.NextDouble() < 0.5))
						{
							if (dist < 0.6f)
							{
								Block block = BlockFactory.GetBlockById(woodId);
								block.Metadata = woodMeta;
								block.Coordinates = new BlockCoordinates(x + i, y + j, z + k);

								world.SetBlock(block);
							}
							if (world.IsAir(new BlockCoordinates(x + i, y + j, z + k)))
							{
								Block block = BlockFactory.GetBlockById(leafBlockId);
								block.Metadata = leafMeta;
								block.Coordinates = new BlockCoordinates(x + i, y + j, z + k);

								world.SetBlock(block);
							}
						}
					}
				}
			}
		}

		public bool ValidLocation(Vector3 location, int leafRadius)
		{
			return !(location.X - leafRadius < 0) && !(location.X + leafRadius >= 16) && !(location.Z - leafRadius < 0) &&
				   !(location.Z + leafRadius >= 16);
		}
	}
}
