using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemBed : Item
	{
		internal ItemBed(short metadata) : base(355, metadata)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			byte direction = player.GetDirection();

			var coordinates = GetNewCoordinatesFromFace(blockCoordinates, face);

			// Base block, meta sets orientation

			Bed block = new Bed
			{
				Coordinates = coordinates
			};

			switch (direction)
			{
				case 1:
					block.Metadata = 0;
					break; // West
				case 2:
					block.Metadata = 1;
					break; // North
				case 3:
					block.Metadata = 2;
					break; // East
				case 0:
					block.Metadata = 3;
					break; // South 
			}

			if (!block.CanPlace(world)) return;

			BlockFace lowerFace = BlockFace.None;
			switch (block.Metadata)
			{
				case 0:
					lowerFace = (BlockFace) 3;
					break; // West
				case 1:
					lowerFace = (BlockFace) 4;
					break; // North
				case 2:
					lowerFace = (BlockFace) 2;
					break; // East
				case 3:
					lowerFace = (BlockFace) 5;
					break; // South 
			}

			Bed blockUpper = new Bed
			{
				Coordinates = GetNewCoordinatesFromFace(coordinates, lowerFace),
				Metadata = (byte) (block.Metadata | 0x08)
			};

			if (!blockUpper.CanPlace(world)) return;

			//TODO: Check down from both blocks, must be solids

			world.SetBlock(block);
			world.SetBlock(blockUpper);
		}
	}
}