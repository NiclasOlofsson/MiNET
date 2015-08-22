namespace MiNET.Items
{
	public class ItemIronChestplate : Item
	{
		public ItemIronChestplate(short metadata) : base(307, metadata)
		{
			ItemType = ItemType.Chestplate;
			ItemMaterial = ItemMaterial.Iron;
		}
	}
}