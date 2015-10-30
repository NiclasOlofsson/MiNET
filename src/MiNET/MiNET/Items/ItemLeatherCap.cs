namespace MiNET.Items
{
	public class ItemLeatherCap : Item
	{
		public ItemLeatherCap(short metadata) : base(298, metadata)
		{
			ItemMaterial = ItemMaterial.Leather;
			ItemType = ItemType.Helmet;
		}
	}
}
