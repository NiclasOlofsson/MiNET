namespace MiNET.Items
{
	public class ItemIronChestplate : Item
	{
		public ItemIronChestplate() : base(307)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Chestplate;
			ItemMaterial = ItemMaterial.Iron;
		}
	}
}