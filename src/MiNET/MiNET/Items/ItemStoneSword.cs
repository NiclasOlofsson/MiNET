namespace MiNET.Items
{
	public class ItemStoneSword : Item
	{
		public ItemStoneSword(short metadata) : base(272, metadata)
		{
			ItemMaterial = ItemMaterial.Stone;
			ItemType = ItemType.Sword;
		}
	}
}