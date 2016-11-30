using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using log4net;
using MiNET.Blocks;
using MiNET.BuilderBase.Masks;
using MiNET.BuilderBase.Patterns;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.BuilderBase
{
	public class BrushTool : ItemIronShovel
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (BrushTool));

		public int Radius { get; set; } = 4;
		public int Height { get; set; } = 1;
		public int Range { get; set; } = 300;
		public Pattern Pattern { get; set; } = new Pattern(1, 0);
		public Mask Mask { get; set; } = new AnyBlockMask();
		public bool Filled { get; set; } = true;

		public int BrushType { get; set; } = 0;

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			var target = new EditHelper(world).GetBlockInLineOfSight(player.Level, player.KnownPosition, returnLastAir: true);
			if (target == null)
			{
				player.SendMessage("No block in range");
				return;
			}

			switch (BrushType)
			{
				case 0:
					new EditHelper(player.Level, Mask, true).MakeSphere(target.Coordinates, Pattern, Radius, Radius, Radius, Filled);
					break;
				case 1:
					new EditHelper(player.Level, Mask, true).MakeCylinder(target.Coordinates, Pattern, Radius, Radius, Height, Filled);
					break;
				case 2:

					// public ErosionParameters(final int erosionFaces, final int erosionRecursion, final int fillFaces, final int fillRecursion) {
					//MELT(new ErosionParameters(2, 1, 5, 1)),
					//FILL(new ErosionParameters(5, 1, 2, 1)),
					//SMOOTH(new ErosionParameters(3, 1, 3, 1)),
					//LIFT(new ErosionParameters(6, 0, 1, 1)),
					//FLOATCLEAN(new ErosionParameters(6, 1, 6, 1));

					Erosion(Radius, 2, 1, 5, 1, world, target.Coordinates); // melt
					break;
				case 3:

					Erosion(Radius, 5, 1, 2, 1, world, target.Coordinates); // fill
					break;
			}
		}

		private class BlockBuffer : Dictionary<BlockCoordinates, Block>
		{
		}

		List<BlockCoordinates> FACES_TO_CHECK = new List<BlockCoordinates> {Level.West, Level.East, Level.Up, Level.Down, Level.South, Level.North};

		protected void Erosion(int brushSize, int erodeFaces, int erodeRec, int fillFaces, int fillRec, Level level, BlockCoordinates targetBlock)
		{
			double brushSizeSquared = brushSize*brushSize;

			int tx = targetBlock.X;
			int ty = targetBlock.Y;
			int tz = targetBlock.Z;

			BlockBuffer buffer1 = new BlockBuffer();
			BlockBuffer buffer2 = new BlockBuffer();

			for (int x = -brushSize - 1; x <= brushSize + 1; x++)
			{
				int x0 = x + tx;
				for (int y = -brushSize - 1; y <= brushSize + 1; y++)
				{
					int y0 = y + ty;
					for (int z = -brushSize - 1; z <= brushSize + 1; z++)
					{
						int z0 = z + tz;
						buffer1[new BlockCoordinates(x, y, z)] = level.GetBlock(new BlockCoordinates(x0, y0, z0));
						buffer1[new BlockCoordinates(x, y, z)].Coordinates = BlockCoordinates.Zero;
						buffer2[new BlockCoordinates(x, y, z)] = level.GetBlock(new BlockCoordinates(x0, y0, z0));
						buffer2[new BlockCoordinates(x, y, z)].Coordinates = BlockCoordinates.Zero;
					}
				}
			}

			int inLen = buffer1.Count;

			int swap = 0;
			for (int i = 0; i < erodeRec; ++i)
			{
				ErosionIteration(brushSize, erodeFaces, swap%2 == 0 ? buffer1 : buffer2, swap%2 == 1 ? buffer1 : buffer2);
				swap++;
			}

			for (int i = 0; i < fillRec; ++i)
			{
				FillIteration(brushSize, fillFaces, swap%2 == 0 ? buffer1 : buffer2, swap%2 == 1 ? buffer1 : buffer2);
				swap++;
			}

			BlockBuffer finalBuffer = swap%2 == 0 ? buffer1 : buffer2;
			//this.undo = new Undo(finalBuffer.getBlockCount());

			// apply the buffer to the world
			for (int x = -brushSize; x <= brushSize; x++)
			{
				int x0 = x + tx;
				for (int y = -brushSize; y <= brushSize; y++)
				{
					int y0 = y + ty;
					for (int z = -brushSize; z <= brushSize; z++)
					{
						int z0 = z + tz;
						var coord = new BlockCoordinates(x, y, z);
						if (x*x + y*y + z*z <= brushSizeSquared && finalBuffer.ContainsKey(coord))
						{
							var block = finalBuffer[coord];
							block.Coordinates = new BlockCoordinates(x0, y0, z0);
							Block old = level.GetBlock(block.Coordinates);

							level.SetBlock(block);
							Log.Debug($"Set block {block.Coordinates}, Old Block={old}, New Block={block} Type={block.GetType().Name}");
							if (block.Id != old.Id) Log.Warn("YEAH!");
						}
					}
				}
			}

			if (finalBuffer.Count != inLen) Log.Warn("Not same lenght");

			//v.owner().storeUndo(this.undo);
			//this.undo = null;
		}


		private void FillIteration(int brushSize, int fillFaces, BlockBuffer current, BlockBuffer target)
		{
			double brushSizeSquared = brushSize*brushSize;
			brushSize = brushSize + 1;
			Dictionary<Block, int> frequency = new Dictionary<Block, int>();

			for (int x = -brushSize; x <= brushSize; x++)
			{
				for (int y = -brushSize; y <= brushSize; y++)
				{
					for (int z = -brushSize; z <= brushSize; z++)
					{
						BlockCoordinates coord = new BlockCoordinates(x, y, z);
						target[coord] = (Block) current[coord].Clone();
						if (x*x + y*y + z*z >= brushSizeSquared)
						{
							continue;
						}

						Block state = current[coord];
						if (!(state is Air))
						{
							continue;
						}

						int total = 0;
						int highest = 1;
						Block highestState = state;
						frequency.Clear();
						foreach (var offs in FACES_TO_CHECK)
						{
							Block next = current[coord + offs];
							if (next is Air)
							{
								continue;
							}

							total++;

							int count;
							var inf = frequency.Keys.FirstOrDefault(pair => pair.Id == next.Id && pair.Metadata == next.Metadata);
							if (inf == null)
							{
								inf = next;
								count = 1;
							}
							else
							{
								count = frequency[inf];
								count++;
							}
							if (count > highest)
							{
								Log.Warn("Set something");
								highest = count;
								highestState = next;
							}
							frequency[inf] = count;
						}
						if (total > fillFaces)
						{
							target[coord] = (Block) highestState.Clone();
						}
					}
				}
			}
		}

		private void ErosionIteration(int brushSize, int erodeFaces, BlockBuffer current, BlockBuffer target)
		{
			double brushSizeSquared = brushSize*brushSize;
			brushSize = brushSize + 1;
			Dictionary<Type, int> frequency = new Dictionary<Type, int>();

			for (int x = -brushSize; x <= brushSize; x++)
			{
				for (int y = -brushSize; y <= brushSize; y++)
				{
					for (int z = -brushSize; z <= brushSize; z++)
					{
						BlockCoordinates coord = new BlockCoordinates(x, y, z);
						target[coord] = (Block) current[coord].Clone();

						if (x*x + y*y + z*z >= brushSizeSquared)
						{
							continue;
						}
						Block state = current[coord];
						if (state is Air)
						{
							continue;
						}
						int total = 0;
						int highest = 1;
						Type highestState = state.GetType();
						frequency.Clear();
						foreach (var offs in FACES_TO_CHECK)
						{
							Block next = current[coord + offs];
							if (!(next is Air))
							{
								continue;
							}
							total++;

							int count;
							if (!frequency.ContainsKey(next.GetType()))
							{
								count = 1;
							}
							else
							{
								count = frequency[next.GetType()];
								count++;
							}

							if (count > highest)
							{
								highest = count;
								highestState = next.GetType();
							}
							frequency[next.GetType()] = count;
						}
						if (total > erodeFaces)
						{
							target[coord] = Default(highestState);
						}
					}
				}
			}
		}

		private Block Default(Type t)
		{
			var ctor = t.GetConstructor(Type.EmptyTypes);
			return (Block) ctor.Invoke(null);
		}
	}
}