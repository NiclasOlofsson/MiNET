namespace MiNET.Items
{
	public class ItemLeatherBoots : Item
	{
		public ItemLeatherBoots(short metadata) : base(301, metadata)
		{
			ItemType = ItemType.Boots;
			ItemMaterial = ItemMaterial.Leather;
		}
	}
}
