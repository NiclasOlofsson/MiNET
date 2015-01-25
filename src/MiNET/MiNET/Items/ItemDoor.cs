using Craft.Net.Common;
using MiNET.Blocks;
using MiNET.Worlds;

namespace MiNET.Items
{
	//A door specifies its hinge side in the block data of its upper block, 
	// and its facing and opened status in the block data of its lower block
	public class ItemDoor : Item
	{
		internal ItemDoor() : base(324)
		{
		}

		public override void UseItem(Level world, Player player, Coordinates3D blockCoordinates, BlockFace face)
		{
			byte direction = player.GetDirection();

			var coordinates = GetNewCoordinatesFromFace(blockCoordinates, face);

			// Base block, meta sets orientation
			Block block = new BlockWoodenDoor();
			block.Coordinates = coordinates;
			block.Metadata = direction;

			int x = blockCoordinates.X;
			int y = blockCoordinates.Y;
			int z = blockCoordinates.Z;

			int xd = 0;
			int zd = 0;

			if (direction == 0) zd = 1;
			if (direction == 1) xd = -1;
			if (direction == 2) zd = -1;
			if (direction == 3) xd = 1;

			int i1 = (world.GetBlock(x - xd, y, z - zd).IsSolid ? 1 : 0) + (world.GetBlock(x - xd, y + 1, z - zd).IsSolid ? 1 : 0);
			int j1 = (world.GetBlock(x + xd, y, z + zd).IsSolid ? 1 : 0) + (world.GetBlock(x + xd, y + 1, z + zd).IsSolid ? 1 : 0);
			bool flag = world.GetBlock(x - xd, y, z - zd).Id == block.Id || world.GetBlock(x - xd, y + 1, z - zd).Id == block.Id;
			bool flag1 = world.GetBlock(x + xd, y, z + zd).Id == block.Id || world.GetBlock(x + xd, y + 1, z + zd).Id == block.Id;
			bool flag2 = false;

			if (flag && !flag1)
			{
				flag2 = true;
			}
			else if (j1 > i1)
			{
				flag2 = true;
			}

			if (!block.CanPlace(world)) return;

			// The upper doore block, meta marks upper and
			// sets orientation based on ajecent blocks
			Block blockUpper = new BlockWoodenDoor();
			blockUpper.Coordinates = coordinates + Level.Up;
			blockUpper.Metadata = (byte) (0x08 | (flag2 ? 1 : 0));

			world.SetBlock(block);
			world.SetBlock(blockUpper);
		}
	}
}