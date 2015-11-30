namespace MiNET.Items
{
	public class ItemIronChestplate : Item
	{
		public ItemIronChestplate(short metadata) : base(307, metadata)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Chestplate;
			ItemMaterial = ItemMaterial.Iron;
		}
	}
}