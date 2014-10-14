using MiNET.Network.Blocks;

namespace MiNET.Network.Items
{
	public class ItemFactory
	{
		public static Item GetItem(int id)
		{
			Item item = null;

			if (id <= 255)
			{
				item = new ItemBlock(BlockFactory.GetBlockById((byte) id));
			}
			else if (id == 324)
			{
				item = new ItemDoor();
			}

			return item;
		}
	}
}