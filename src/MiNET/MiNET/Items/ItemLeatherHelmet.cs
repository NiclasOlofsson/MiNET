namespace MiNET.Items
{
	public class ItemLeatherHelmet : Item
	{
		public ItemLeatherHelmet() : base(298)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Helmet;
			ItemMaterial = ItemMaterial.Leather;
		}
	}
}
