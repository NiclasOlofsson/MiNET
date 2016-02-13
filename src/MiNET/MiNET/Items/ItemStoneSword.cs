namespace MiNET.Items
{
	public class ItemStoneSword : Item
	{
		public ItemStoneSword() : base(272)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Stone;
			ItemType = ItemType.Sword;
		}
	}
}