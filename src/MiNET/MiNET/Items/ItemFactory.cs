using MiNET.Blocks;

namespace MiNET.Items
{
	public class ItemFactory
	{
		public static Item GetItem(int id)
		{
			Item item = new Item(id);

			if (id <= 255)
			{
				item = new ItemBlock (BlockFactory.GetBlockById ((byte)id));
			}
            else if (id == 324)
			{
				item = new ItemDoor ();
			} 
			else if (id == 325)
			{
				item = new ItemBucket ();
			}
            else if (id == 259)
            {
                item = new ItemFlintAndSteel();
            }

			return item;
		}
	}
}