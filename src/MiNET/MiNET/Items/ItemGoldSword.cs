namespace MiNET.Items
{
	public class ItemGoldSword : Item
	{
		public ItemGoldSword() : base(283)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Gold;
			ItemType = ItemType.Sword;
		}
	}
}