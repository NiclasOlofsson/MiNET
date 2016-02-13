namespace MiNET.Items
{
	public class ItemStoneShovel : ItemShovel
	{
		public ItemStoneShovel() : base(273)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Stone;
			ItemType = ItemType.Shovel;
		}
	}
}