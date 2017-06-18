using System;
using System.Numerics;
using log4net;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public abstract class Flowing : Block
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Flowing));

		private int _adjacentSources;
		private int[] _flowCost = new int[4];
		private bool[] _optimalFlowDirections = new bool[4];

		protected Flowing(byte id) : base(id)
		{
			IsSolid = false;
			IsBuildable = false;
			IsReplacible = true;
			IsTransparent = true;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			CheckForHarden(world, blockCoordinates.X, blockCoordinates.Y, blockCoordinates.Z);
			world.ScheduleBlockTick(this, TickRate());
			return false;
		}

		public override void DoPhysics(Level level)
		{
			CheckForHarden(level, Coordinates.X, Coordinates.Y, Coordinates.Z);
			level.ScheduleBlockTick(this, TickRate());
		}

		public override void OnTick(Level world, bool isRandom)
		{
			if (isRandom) return;

			Random random = new Random();

			int x = Coordinates.X;
			int y = Coordinates.Y;
			int z = Coordinates.Z;

			int currentDecay = GetFlowDecay(world, x, y, z);
			byte multiplier = 1;

			if (this is FlowingLava)
			{
				multiplier = 2;
			}

			bool flag = true;
			int tickRate = TickRate();

			if (currentDecay > 0)
			{
				int smallestFlowDecay = -100;
				_adjacentSources = 0;
				smallestFlowDecay = GetSmallestFlowDecay(world, x - 1, y, z, smallestFlowDecay);
				smallestFlowDecay = GetSmallestFlowDecay(world, x + 1, y, z, smallestFlowDecay);
				smallestFlowDecay = GetSmallestFlowDecay(world, x, y, z - 1, smallestFlowDecay);
				smallestFlowDecay = GetSmallestFlowDecay(world, x, y, z + 1, smallestFlowDecay);
				int newDecay = smallestFlowDecay + multiplier;
				if (newDecay >= 8 || smallestFlowDecay < 0)
				{
					newDecay = -1;
				}

				if (GetFlowDecay(world, x, y + 1, z) >= 0)
				{
					int topFlowDecay = GetFlowDecay(world, x, y + 1, z);

					if (topFlowDecay >= 8)
					{
						newDecay = topFlowDecay;
					}
					else
					{
						newDecay = topFlowDecay + 8;
					}
				}

				if (_adjacentSources >= 2 && this is FlowingWater)
				{
					if (world.GetBlock(x, y - 1, z).IsSolid)
					{
						newDecay = 0;
					}
					else if (IsSameMaterial(world.GetBlock(x, y - 1, z)) && world.GetBlock(x, y - 1, z).Metadata == 0)
					{
						newDecay = 0;
					}
				}

				if (this is FlowingLava && currentDecay < 8 && newDecay < 8 && newDecay > currentDecay && random.Next(4) != 0)
				{
					//newDecay = currentDecay;
					//flag = false;
					tickRate *= 4;
				}

				if (newDecay == currentDecay)
				{
					if (flag)
					{
						SetToStill(world, x, y, z);
					}
				}
				else
				{
					currentDecay = newDecay;
					if (newDecay < 0)
					{
						world.SetAir(x, y, z);
					}
					else
					{
						world.SetData(x, y, z, (byte) newDecay);
						world.ApplyPhysics(x, y, z);
						world.ScheduleBlockTick(this, tickRate); // Schedule tick
					}
				}
			}
			else
			{
				SetToStill(world, x, y, z);
			}

			if (CanBeFlownInto(world, x, y - 1, z) /* || world.GetBlock(x, y - 1, z) is Flowing*/)
			{
				if (this is FlowingLava && (world.GetBlock(x, y - 1, z) is FlowingWater || world.GetBlock(x, y - 1, z) is StationaryWater))
				{
					world.SetBlock(new Cobblestone {Coordinates = new BlockCoordinates(x, y - 1, z)});
					return;
				}

				if (currentDecay >= 8)
				{
					Flow(world, x, y - 1, z, currentDecay);
				}
				else
				{
					Flow(world, x, y - 1, z, currentDecay + 8);
				}
			}
			else if (currentDecay >= 0 && (currentDecay == 0 || BlocksFluid(world, x, y - 1, z)))
			{
				bool[] optimalFlowDirections = GetOptimalFlowDirections(world, x, y, z);

				int newDecay = currentDecay + multiplier;
				if (currentDecay >= 8)
				{
					newDecay = 1;
				}

				if (newDecay >= 8)
				{
					return;
				}

				if (optimalFlowDirections[0])
				{
					Flow(world, x - 1, y, z, newDecay);
				}

				if (optimalFlowDirections[1])
				{
					Flow(world, x + 1, y, z, newDecay);
				}

				if (optimalFlowDirections[2])
				{
					Flow(world, x, y, z - 1, newDecay);
				}

				if (optimalFlowDirections[3])
				{
					Flow(world, x, y, z + 1, newDecay);
				}
			}
		}

		private bool[] GetOptimalFlowDirections(Level world, int x, int y, int z)
		{
			int l;
			int x2;

			for (l = 0; l < 4; ++l)
			{
				_flowCost[l] = 1000;
				x2 = x;
				int z2 = z;

				if (l == 0)
				{
					x2 = x - 1;
				}

				if (l == 1)
				{
					++x2;
				}

				if (l == 2)
				{
					z2 = z - 1;
				}

				if (l == 3)
				{
					++z2;
				}

				if (!BlocksFluid(world, x2, y, z2) && (!IsSameMaterial(world.GetBlock(x2, y, z2)) || world.GetBlock(x2, y, z2).Metadata != 0))
				{
					if (BlocksFluid(world, x2, y - 1, z2))
					{
						_flowCost[l] = CalculateFlowCost(world, x2, y, z2, 1, l);
					}
					else
					{
						_flowCost[l] = 0;
					}
				}
			}

			l = _flowCost[0];

			for (x2 = 1; x2 < 4; ++x2)
			{
				if (_flowCost[x2] < l)
				{
					l = _flowCost[x2];
				}
			}

			for (x2 = 0; x2 < 4; ++x2)
			{
				_optimalFlowDirections[x2] = _flowCost[x2] == l;
			}

			return _optimalFlowDirections;
		}

		private int CalculateFlowCost(Level world, int x, int y, int z, int accumulatedCost, int prevDirection)
		{
			int cost = 1000;

			for (int direction = 0; direction < 4; ++direction)
			{
				if ((direction != 0 || prevDirection != 1)
				    && (direction != 1 || prevDirection != 0)
				    && (direction != 2 || prevDirection != 3)
				    && (direction != 3 || prevDirection != 2))
				{
					int x2 = x;
					int z2 = z;

					if (direction == 0)
					{
						x2 = x - 1;
					}

					if (direction == 1)
					{
						++x2;
					}

					if (direction == 2)
					{
						z2 = z - 1;
					}

					if (direction == 3)
					{
						++z2;
					}

					if (!BlocksFluid(world, x2, y, z2) && (!IsSameMaterial(world.GetBlock(x2, y, z2)) || world.GetBlock(x2, y, z2).Metadata != 0))
					{
						if (!BlocksFluid(world, x2, y - 1, z2))
						{
							return accumulatedCost;
						}

						if (accumulatedCost < 4)
						{
							int j2 = CalculateFlowCost(world, x2, y, z2, accumulatedCost + 1, direction);

							if (j2 < cost)
							{
								cost = j2;
							}
						}
					}
				}
			}

			return cost;
		}

		private void Flow(Level world, int x, int y, int z, int decay)
		{
			if (CanBeFlownInto(world, x, y, z))
			{
				//Block block = world.GetBlock(x, y, z);

				//if (this is FlowingLava)
				//{
				//	this.fizz(world, i, j, k);
				//}
				//else
				//{
				//	block.DoDrop(world, i, j, k, world.getData(i, j, k), 0);
				//}

				Block newBlock = BlockFactory.GetBlockById(Id);
				newBlock.Coordinates = new BlockCoordinates(x, y, z);
				newBlock.Metadata = (byte) decay;
				world.SetBlock(newBlock, applyPhysics: true);
				world.ScheduleBlockTick(newBlock, TickRate());
			}
		}

		private bool CanBeFlownInto(Level world, int x, int y, int z)
		{
			Block block = world.GetBlock(x, y, z);

			return !IsSameMaterial(block) && (!(block is FlowingLava) && !(block is StationaryLava)) && !BlocksFluid(block);
		}


		private bool BlocksFluid(Level world, int x, int y, int z)
		{
			Block block = world.GetBlock(x, y, z);

			return BlocksFluid(block);
		}

		private bool BlocksFluid(Block block)
		{
			return block.IsSolid;
			//return block.IsBuildable; // block != Blocks.WOODEN_DOOR && block != Blocks.IRON_DOOR_BLOCK && block != Blocks.SIGN_POST && block != Blocks.LADDER && block != Blocks.SUGAR_CANE_BLOCK ? (block.material == Material.PORTAL ? true : block.material.isSolid()) : true;
		}


		private void SetToStill(Level world, int x, int y, int z)
		{
			byte meta = world.GetBlock(x, y, z).Metadata;

			Block stillBlock = BlockFactory.GetBlockById((byte) (Id + 1));
			stillBlock.Metadata = meta;
			stillBlock.Coordinates = new BlockCoordinates(x, y, z);
			world.SetBlock(stillBlock, applyPhysics: false);
		}

		private int GetSmallestFlowDecay(Level world, int x, int y, int z, int decay)
		{
			int blockDecay = GetFlowDecay(world, x, y, z);

			if (blockDecay < 0)
			{
				return decay;
			}

			if (blockDecay == 0)
			{
				++_adjacentSources;
			}

			if (blockDecay >= 8)
			{
				blockDecay = 0;
			}

			return decay >= 0 && blockDecay >= decay ? decay : blockDecay;
		}

		private int GetFlowDecay(Level world, int x, int y, int z)
		{
			Block block = world.GetBlock(x, y, z);
			return IsSameMaterial(block) ? block.Metadata : -1;
		}

		private bool IsSameMaterial(Block block)
		{
			if (this is FlowingWater && (block is FlowingWater || block is StationaryWater)) return true;
			if (this is FlowingLava && (block is FlowingLava || block is StationaryLava)) return true;

			return false;
		}

		private int TickRate()
		{
			return this is FlowingWater ? 5 : (this is FlowingLava ? 30 : 0);
		}

		private void CheckForHarden(Level world, int x, int y, int z)
		{
			Block block = world.GetBlock(x, y, z);
			{
				bool harden = false;
				if (block is FlowingLava || block is StationaryLava)
				{
					if (IsWater(world, x, y, z - 1))
					{
						harden = true;
					}

					if (harden || IsWater(world, x, y, z + 1))
					{
						harden = true;
					}

					if (harden || IsWater(world, x - 1, y, z))
					{
						harden = true;
					}

					if (harden || IsWater(world, x + 1, y, z))
					{
						harden = true;
					}

					if (harden || IsWater(world, x, y + 1, z))
					{
						harden = true;
					}

					if (harden)
					{
						int meta = block.Metadata;

						if (meta == 0)
						{
							world.SetBlock(new Obsidian { Coordinates = new BlockCoordinates(x, y, z) }, true, false);
						}
						else if (meta <= 4)
						{
							world.SetBlock(new Cobblestone { Coordinates = new BlockCoordinates(x, y, z) }, true, false);
						}
					}
				}
			}
		}

		private bool IsWater(Level world, int x, int y, int z)
		{
			Block block = world.GetBlock(x, y, z);
			return block is FlowingWater || block is StationaryWater;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new Item[0];
		}
	}
}