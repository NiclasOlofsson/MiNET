#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Numerics;
using log4net;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public abstract class Flowing : Block
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Flowing));

		[StateRange(0, 15)] public virtual int LiquidDepth { get; set; } = 0;

		private int _adjacentSources;
		private int[] _flowCost = new int[4];
		private bool[] _optimalFlowDirections = new bool[4];

		protected Flowing(byte id) : base(id)
		{
			IsSolid = false;
			IsBuildable = false;
			IsReplaceable = true;
			IsTransparent = true;
		}

		public override void BlockAdded(Level level)
		{
			if (!CheckForHarden(level, Coordinates))
			{
				level.ScheduleBlockTick(this, TickRate());
			}
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			if (!CheckForHarden(world, blockCoordinates))
			{
				world.ScheduleBlockTick(this, TickRate());
			}

			return false;
		}

		public override void DoPhysics(Level level)
		{
			CheckForHarden(level, Coordinates);
			level.ScheduleBlockTick(this, TickRate());
		}

		public override void OnTick(Level world, bool isRandom)
		{
			if (isRandom) return;

			var random = new Random();

			int x = Coordinates.X;
			int y = Coordinates.Y;
			int z = Coordinates.Z;
			var current = Coordinates;

			//int currentDecay = GetFlowDecay(world, x, y, z);
			int currentDecay = LiquidDepth;
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
				smallestFlowDecay = GetSmallestFlowDecay(world, current + BlockCoordinates.Left, smallestFlowDecay);
				smallestFlowDecay = GetSmallestFlowDecay(world, current + BlockCoordinates.Right, smallestFlowDecay);
				smallestFlowDecay = GetSmallestFlowDecay(world, current + BlockCoordinates.Backwards, smallestFlowDecay);
				smallestFlowDecay = GetSmallestFlowDecay(world, current + BlockCoordinates.Forwards, smallestFlowDecay);
				int newDecay = smallestFlowDecay + multiplier;
				if (newDecay >= 8 || smallestFlowDecay < 0)
				{
					newDecay = -1;
				}

				if (GetFlowDecay(world, current + BlockCoordinates.Up) >= 0)
				{
					int topFlowDecay = GetFlowDecay(world, current + BlockCoordinates.Up);

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
					if (world.GetBlock(current + BlockCoordinates.Down).IsSolid)
					{
						newDecay = 0;
					}
					else if (IsSameMaterial(world.GetBlock(current + BlockCoordinates.Down)) && GetLiquidDepth(world.GetBlock(current + BlockCoordinates.Down)) == 0)
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
						SetToStill(world, current);
					}
				}
				else
				{
					currentDecay = newDecay;
					if (newDecay < 0)
					{
						world.SetAir(current);
					}
					else
					{
						LiquidDepth = newDecay;
						world.SetBlock(this);
						world.ApplyPhysics(x, y, z);
						world.ScheduleBlockTick(this, tickRate); // Schedule tick
					}
				}
			}
			else
			{
				SetToStill(world, current);
			}

			if (CanBeFlownInto(world, current + BlockCoordinates.Down) /* || world.GetBlock(x, y - 1, z) is Flowing*/)
			{
				if (this is FlowingLava && (world.GetBlock(x, y - 1, z) is FlowingWater || world.GetBlock(x, y - 1, z) is Water))
				{
					world.SetBlock(new Cobblestone {Coordinates = new BlockCoordinates(x, y - 1, z)});
					return;
				}

				if (currentDecay >= 8)
				{
					Flow(world, current + BlockCoordinates.Down, currentDecay);
				}
				else
				{
					Flow(world, current + BlockCoordinates.Down, currentDecay + 8);
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
					//Flow(world, x - 1, y, z, newDecay);
					Flow(world, current + BlockCoordinates.Left, newDecay);
				}

				if (optimalFlowDirections[1])
				{
					Flow(world, current + BlockCoordinates.Right, newDecay);
				}

				if (optimalFlowDirections[2])
				{
					Flow(world, current + BlockCoordinates.Backwards, newDecay);
				}

				if (optimalFlowDirections[3])
				{
					Flow(world, current + BlockCoordinates.Forwards, newDecay);
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

				if (!BlocksFluid(world, x2, y, z2) && (!IsSameMaterial(world.GetBlock(x2, y, z2)) || GetLiquidDepth(world.GetBlock(x2, y, z2)) != 0))
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

		private int GetLiquidDepth(Block block)
		{
			return block switch
			{
				Flowing flowing => flowing.LiquidDepth,
				Stationary stationary => stationary.LiquidDepth,
				_ => -1
			};
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

					if (!BlocksFluid(world, x2, y, z2) && (!IsSameMaterial(world.GetBlock(x2, y, z2)) || GetLiquidDepth(world.GetBlock(x2, y, z2)) != 0))
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

		private void Flow(Level world, BlockCoordinates coord, int decay)
		{
			if (CanBeFlownInto(world, coord))
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

				Flowing newBlock = (Flowing) BlockFactory.GetBlockById(Id);
				newBlock.Coordinates = new BlockCoordinates(coord);
				newBlock.LiquidDepth = decay;
				world.SetBlock(newBlock, applyPhysics: true);
				world.ScheduleBlockTick(newBlock, TickRate());
			}
		}

		private bool CanBeFlownInto(Level world, BlockCoordinates coord)
		{
			Block block = world.GetBlock(coord);

			return !IsSameMaterial(block) && (!(block is FlowingLava) && !(block is Lava)) && !BlocksFluid(block);
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


		private void SetToStill(Level world, BlockCoordinates coord)
		{
			var block = (Flowing) world.GetBlock(coord);

			var stillBlock = (Stationary) BlockFactory.GetBlockById((byte) (Id + 1));
			stillBlock.LiquidDepth = block.LiquidDepth;
			stillBlock.Coordinates = new BlockCoordinates(coord);
			world.SetBlock(stillBlock, applyPhysics: false);
		}

		private int GetSmallestFlowDecay(Level world, BlockCoordinates coord, int decay)
		{
			int blockDecay = GetFlowDecay(world, coord);

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

		private int GetFlowDecay(Level world, BlockCoordinates coord)
		{
			Block block = world.GetBlock(coord);

			int liquidDepth;
			switch (block)
			{
				case Flowing flowing:
					liquidDepth = flowing.LiquidDepth;
					break;
				case Stationary stationary:
					liquidDepth = stationary.LiquidDepth;
					break;
				default:
					return -1;
			}

			return IsSameMaterial(block) ? liquidDepth : -1;
		}


		private bool IsSameMaterial(Block block)
		{
			if (this is FlowingWater && (block is FlowingWater || block is Water)) return true;
			if (this is FlowingLava && (block is FlowingLava || block is Lava)) return true;

			return false;
		}

		private int TickRate()
		{
			return this is FlowingWater ? 5 : (this is FlowingLava ? 30 : 0);
		}

		private bool CheckForHarden(Level world, BlockCoordinates coord)
		{
			var block = world.GetBlock(coord) as Flowing; // This is "this" isn't it?

			bool harden = false;
			if (block is FlowingLava /* || block is Lava*/)
			{
				if (IsWater(world, coord + BlockCoordinates.Backwards))
				{
					harden = true;
				}

				if (harden || IsWater(world, coord + BlockCoordinates.Forwards))
				{
					harden = true;
				}

				if (harden || IsWater(world, coord + BlockCoordinates.Left))
				{
					harden = true;
				}

				if (harden || IsWater(world, coord + BlockCoordinates.Right))
				{
					harden = true;
				}

				if (harden || IsWater(world, coord + BlockCoordinates.Up))
				{
					harden = true;
				}

				if (harden)
				{
					int liquidDepth = block.LiquidDepth;

					if (liquidDepth == 0)
					{
						world.SetBlock(new Obsidian {Coordinates = new BlockCoordinates(coord)}, true, false);
					}
					else if (liquidDepth <= 4)
					{
						world.SetBlock(new Cobblestone {Coordinates = new BlockCoordinates(coord)}, true, false);
					}

					return true;
				}
			}

			return false;
		}

		private bool IsWater(Level world, BlockCoordinates coord)
		{
			Block block = world.GetBlock(coord);
			return block is FlowingWater || block is Water;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new Item[0];
		}
	}
}