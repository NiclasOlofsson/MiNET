using MiNET.Blocks;

namespace MiNET.Items
{
	public class ItemFactory
	{
		public static Item GetItem(int id)
		{
			Item item;

			if (id <= 255) item = new ItemBlock(BlockFactory.GetBlockById((byte) id));
			else if (id == 323) item = new ItemSign();
			else if (id == 324) item = new ItemDoor();
			else if (id == 325) item = new ItemBucket();
			else if (id == 259) item = new ItemFlintAndSteel();
			else if (id == 267) item = new ItemIronSword();
			else item = new Item(id);

			return item;
		}
	}
}