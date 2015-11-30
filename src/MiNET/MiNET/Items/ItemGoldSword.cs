namespace MiNET.Items
{
	public class ItemGoldSword : Item
	{
		public ItemGoldSword(short metadata) : base(283, metadata)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Gold;
			ItemType = ItemType.Sword;
		}
	}
}