namespace MiNET.Items
{
	public class ItemLeatherHelmet : Item
	{
		public ItemLeatherHelmet(short metadata) : base(298, metadata)
		{
			ItemType = ItemType.Helmet;
			ItemMaterial = ItemMaterial.Leather;
		}
	}
}
