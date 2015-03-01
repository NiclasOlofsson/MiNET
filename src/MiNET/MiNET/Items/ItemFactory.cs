using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemFactory
	{
		public static Item GetItem(int id)
		{
			return GetItem(id, 0);
		}

		public static Item GetItem(int id, short metadata)
		{
			Item item;

			if (id == 54) item = new ItemChest(metadata);
			else if (id == 44) item = new ItemSlab(id, metadata);
			else if (id == 61) item = new ItemFurnace(metadata);
			else if (id == 158) item = new ItemSlab(id, metadata);
			else if (id == 259) item = new ItemFlintAndSteel(metadata);
			else if (id == 263) item = new ItemCoal(metadata);
			else if (id == 267) item = new ItemIronSword(metadata);
			else if (id == 268) item = new ItemWoodenSword(metadata);
			else if (id == 269) item = new ItemWoodenShovel(metadata);
			else if (id == 270) item = new ItemWoodenPickaxe(metadata);
			else if (id == 271) item = new ItemWoodenAxe(metadata);
			else if (id == 280) item = new ItemStick(metadata);
			else if (id == 323) item = new ItemSign(metadata);
			else if (id == 324) item = new ItemDoor(metadata);
			else if (id == 325) item = new ItemBucket(metadata);
			else if (id == 363) item = new RawBeef(metadata);
			else if (id == 365) item = new RawChicken(metadata);
			else if (id == 319) item = new RawPorkchop(metadata);
			else if (id <= 255) item = new ItemBlock(BlockFactory.GetBlockById((byte) id), metadata);
			else item = new Item(id, metadata);

			return item;
		}
	}

	public class ItemSlab : Item
	{
		public ItemSlab(int id, short metadata) : base(id, metadata)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			// 8 = 1000
			byte upperBit = 0x08;
			byte upperMask = 0x0f;
			// 7 = 0111
			byte materialMask = 0x07;

			byte direction = player.GetDirection();

			Block existingBlock = world.GetBlock(blockCoordinates);

			var coordinates = GetNewCoordinatesFromFace(blockCoordinates, face);
			Block newBlock = world.GetBlock(coordinates);

			if (face == BlockFace.Up && faceCoords.Y == 0.5 && existingBlock.Id == Id && (existingBlock.Metadata & materialMask) == Metadata)
			{
				// Replace with double block
				SetDoubleSlab(world, blockCoordinates);
				return;
			}

			if (face == BlockFace.Down && faceCoords.Y == 0.5 && (existingBlock.Metadata & materialMask) == Metadata)
			{
				// Replace with double block
				SetDoubleSlab(world, blockCoordinates);
				return;
			}

			if (newBlock.Id != Id || (newBlock.Metadata & materialMask) != Metadata)
			{
				Block slab = BlockFactory.GetBlockById((byte) (Id));
				slab.Coordinates = coordinates;
				slab.Metadata = (byte) Metadata;
				if (face != BlockFace.Up && faceCoords.Y > 0.5 || (face == BlockFace.Down && faceCoords.Y == 0.0))
				{
					slab.Metadata |= upperBit;
				}
				world.SetBlock(slab);
				return;
			}

			// Same material in existing block, make double slab

			{
				// Create double slab, replace existing
				SetDoubleSlab(world, coordinates);
			}
		}

		private void SetDoubleSlab(Level world, BlockCoordinates coordinates)
		{
			Block slab = BlockFactory.GetBlockById((byte) (Id - 1));
			slab.Coordinates = coordinates;
			slab.Metadata = (byte) Metadata;
			world.SetBlock(slab);
		}
	}
}