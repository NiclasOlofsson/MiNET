using Craft.Net.Common;
using MiNET.Network.Worlds;

namespace MiNET.Network.Blocks
{
	public class BlockWoodenDoor : Block
	{
		public BlockWoodenDoor() : base(64)
		{
		}

		public override bool CanPlace(Level world, Coordinates3D blockCoordinates)
		{
			return world.GetBlock(blockCoordinates).IsReplacible && world.GetBlock(blockCoordinates + Level.Up).IsReplacible;
		}

		public override void BreakBlock(Level level)
		{
			// Remove door
			if ((Metadata & 0x08) == 0x08) // Is Upper?
			{
				level.SetBlock(new BlockAir {Coordinates = Coordinates + Level.Down});
			}
			else
			{
				level.SetBlock(new BlockAir {Coordinates = Coordinates + Level.Up});
			}

			level.SetBlock(new BlockAir {Coordinates = Coordinates});
		}

		public override bool Interact(Level world, Player player, Coordinates3D blockCoordinates, BlockFace face)
		{
			Block block = this;
			// Remove door
			if ((Metadata & 0x08) == 0x08) // Is Upper?
			{
				block = world.GetBlock(GetNewCoordinatesFromFace(blockCoordinates, BlockFace.NegativeY));
			}

			block.Metadata ^= 0x04;
			world.SetBlock(block);

			return true;
		}
	}
}