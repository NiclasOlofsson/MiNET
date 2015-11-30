namespace MiNET.Items
{
	public class ItemLeatherLeggings : Item
	{
		public ItemLeatherLeggings(short metadata) : base(300, metadata)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Leggings;
			ItemMaterial = ItemMaterial.Leather;
		}
	}
}
