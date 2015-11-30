namespace MiNET.Items
{
	public class ItemLeatherBoots : Item
	{
		public ItemLeatherBoots(short metadata) : base(301, metadata)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Boots;
			ItemMaterial = ItemMaterial.Leather;
		}
	}
}
