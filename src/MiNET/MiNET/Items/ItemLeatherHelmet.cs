namespace MiNET.Items
{
	public class ItemLeatherHelmet : Item
	{
		public ItemLeatherHelmet(short metadata) : base(298, metadata)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Helmet;
			ItemMaterial = ItemMaterial.Leather;
		}
	}
}
