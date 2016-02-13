namespace MiNET.Items
{
	public class ItemGoldChestplate : Item
	{
		public ItemGoldChestplate() : base(315)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Chestplate;
			ItemMaterial = ItemMaterial.Gold;
		}
	}
}
