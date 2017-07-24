using System.Numerics;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	//A door specifies its hinge side in the block data of its upper block, 
	// and its facing and opened status in the block data of its lower block
	public class ItemWoodenDoor : Item
	{
		private readonly byte _blockId;

		public ItemWoodenDoor(short itemId = 324, byte blockId = 64) : base(itemId)
		{
			_blockId = blockId;
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			byte direction = player.GetDirection();

			var coordinates = GetNewCoordinatesFromFace(blockCoordinates, face);

			// Base block, meta sets orientation
			Block block = BlockFactory.GetBlockById(_blockId);
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

			if (!block.CanPlace(world, blockCoordinates,  face)) return;

			// The upper doore block, meta marks upper and
			// sets orientation based on ajecent blocks
			Block blockUpper = BlockFactory.GetBlockById(_blockId);
			blockUpper.Coordinates = coordinates + Level.Up;
			blockUpper.Metadata = (byte) (0x08 | (flag2 ? 1 : 0));

			world.SetBlock(block);
			world.SetBlock(blockUpper);
		}
	}

	public class ItemSpruceDoor : ItemWoodenDoor
	{
		public ItemSpruceDoor() : base(427, 193)
		{
		}
	}

	public class ItemBirchDoor : ItemWoodenDoor
	{
		public ItemBirchDoor() : base(428, 194)
		{
		}
	}


	public class ItemJungleDoor : ItemWoodenDoor
	{
		public ItemJungleDoor() : base(429, 195)
		{
		}
	}

	public class ItemAcaciaDoor : ItemWoodenDoor
	{
		public ItemAcaciaDoor() : base(430, 196)
		{
		}
	}

	public class ItemDarkOakDoor : ItemWoodenDoor
	{
		public ItemDarkOakDoor() : base(431, 197)
		{
		}
	}
}