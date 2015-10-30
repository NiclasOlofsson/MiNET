namespace MiNET.Items
{
	public class ItemChainLeggings : Item
	{
		public ItemChainLeggings(short metadata) : base(304, metadata)
		{
			ItemMaterial = ItemMaterial.Chain;
			ItemType = ItemType.Leggings;
		}
	}
}
