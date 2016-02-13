namespace MiNET.Items
{
	public class ItemLeatherBoots : Item
	{
		public ItemLeatherBoots() : base(301)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Boots;
			ItemMaterial = ItemMaterial.Leather;
		}
	}
}
