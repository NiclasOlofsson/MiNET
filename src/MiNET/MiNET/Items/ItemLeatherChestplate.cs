namespace MiNET.Items
{
	public class ItemLeatherChestplate : Item
	{
		public ItemLeatherChestplate(short metadata) : base(299, metadata)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Chestplate;
			ItemMaterial = ItemMaterial.Leather;
		}
	}
}
