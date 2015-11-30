namespace MiNET.Items
{
	public class ItemStoneSword : Item
	{
		public ItemStoneSword(short metadata) : base(272, metadata)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Stone;
			ItemType = ItemType.Sword;
		}
	}
}