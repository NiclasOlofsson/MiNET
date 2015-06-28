namespace MiNET.Items
{
	public class ItemStoneShovel : ItemShovel
	{
		public ItemStoneShovel(short metadata) : base(273, metadata)
		{
			ItemMaterial = ItemMaterial.Stone;
			ItemType = ItemType.Shovel;
		}
	}
}