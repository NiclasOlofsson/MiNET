namespace MiNET.Items
{
	public class ItemLeatherPants : Item
	{
		public ItemLeatherPants(short metadata) : base(300, metadata)
		{
			ItemMaterial = ItemMaterial.Leather;
			ItemType = ItemType.Leggings;
		}
	}
}
