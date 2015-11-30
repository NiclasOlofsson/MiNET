namespace MiNET.Items
{
	public class ItemGoldChestplate : Item
	{
		public ItemGoldChestplate(short metadata) : base(315, metadata)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Chestplate;
			ItemMaterial = ItemMaterial.Gold;
		}
	}
}
