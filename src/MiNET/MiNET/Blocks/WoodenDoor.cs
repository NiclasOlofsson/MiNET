﻿using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	//A door specifies its hinge side in the block data of its upper block, 
	// and its facing and opened status in the block data of its lower block
	public class WoodenDoor : Block
	{
		public WoodenDoor() : base(64)
		{
		}

		protected override bool CanPlace(Level world, BlockCoordinates blockCoordinates, BlockFace face)
		{
			return world.GetBlock(blockCoordinates).IsReplaceable && world.GetBlock(blockCoordinates + Level.Up).IsReplaceable;
		}

		public override void BreakBlock(Level level)
		{
			// Remove door
			if ((Metadata & 0x08) == 0x08) // Is Upper?
			{
				level.SetBlock(new Air {Coordinates = Coordinates + Level.Down});
			}
			else
			{
				level.SetBlock(new Air {Coordinates = Coordinates + Level.Up});
			}

			level.SetBlock(new Air {Coordinates = Coordinates});
		}

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face)
		{
			Block block = this;
			// Remove door
			if ((Metadata & 0x08) == 0x08) // Is Upper?
			{
				block = world.GetBlock(GetNewCoordinatesFromFace(blockCoordinates, BlockFace.Down));
			}

			block.Metadata ^= 0x04;
			world.SetBlock(block);

			return true;
		}
	}
}
