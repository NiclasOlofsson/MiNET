namespace MiNET.Items
{
	public class ItemLeatherLeggings : Item
	{
		public ItemLeatherLeggings(short metadata) : base(300, metadata)
		{
			ItemType = ItemType.Leggings;
			ItemMaterial = ItemMaterial.Leather;
		}
	}
}
