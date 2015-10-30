namespace MiNET.Items
{
	public class ItemLeatherBoots : Item
	{
		public ItemLeatherBoots(short metadata) : base(301, metadata)
		{
			ItemMaterial = ItemMaterial.Leather;
			ItemType = ItemType.Boots;
		}
	}
}
