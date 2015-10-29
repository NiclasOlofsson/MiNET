using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Stationary : Block
	{
		internal Stationary(byte id) : base(id)
		{
			IsSolid = false;
			IsBuildable = false;
			IsReplaceable = true;
		}

		public override void DoPhysics(Level level)
		{
			CheckForHarden(level, Coordinates.X, Coordinates.Y, Coordinates.Z);

			if (level.GetBlock(Coordinates).Id == Id)
			{
				SetToFlowing(level);
			}
		}

		private void SetToFlowing(Level world)
		{
			Block flowingBlock = BlockFactory.GetBlockById((byte) (Id - 1));
			flowingBlock.Metadata = Metadata;
			flowingBlock.Coordinates = Coordinates;
			world.SetBlock(flowingBlock, applyPhysics: false);
			world.ScheduleBlockTick(flowingBlock, 5);
		}

		private void CheckForHarden(Level world, int x, int y, int z)
		{
			Block block = world.GetBlock(x, y, z);
			{
				bool harden = false;
				if (block is FlowingLava || block is StationaryLava)
				{
					if (IsWater(world, x, y, z))
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
							world.SetBlock(new Obsidian {Coordinates = new BlockCoordinates(x, y, z)});
						}
						else if (meta <= 4)
						{
							world.SetBlock(new Cobblestone {Coordinates = new BlockCoordinates(x, y, z)});
						}
					}
				}
			}
		}

		private bool IsWater(Level world, int x, int y, int z)
		{
			Block block = world.GetBlock(x, y, z - 1);
			return block is FlowingWater || block is StationaryWater;
		}
	}
}
