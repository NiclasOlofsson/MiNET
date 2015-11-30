namespace MiNET.Items
{
	public class ItemStoneShovel : ItemShovel
	{
		public ItemStoneShovel(short metadata) : base(273, metadata)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Stone;
			ItemType = ItemType.Shovel;
		}
	}
}