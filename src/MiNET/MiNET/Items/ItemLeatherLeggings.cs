namespace MiNET.Items
{
	public class ItemLeatherLeggings : Item
	{
		public ItemLeatherLeggings() : base(300)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Leggings;
			ItemMaterial = ItemMaterial.Leather;
		}
	}
}
