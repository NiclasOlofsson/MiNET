using MiNET.Blocks;

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
}