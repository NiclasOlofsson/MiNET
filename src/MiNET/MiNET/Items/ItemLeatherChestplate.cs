namespace MiNET.Items
{
	public class ItemLeatherChestplate : Item
	{
		public ItemLeatherChestplate() : base(299)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Chestplate;
			ItemMaterial = ItemMaterial.Leather;
		}
	}
}
