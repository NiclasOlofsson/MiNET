namespace MiNET.Items
{
	public class ItemGoldChestplate : Item
	{
		public ItemGoldChestplate(short metadata) : base(315, metadata)
		{
			ItemType = ItemType.Chestplate;
			ItemMaterial = ItemMaterial.Gold;
		}
	}
}
