namespace MiNET.Items
{
	public class ItemChainHelmet : Item
	{
		public ItemChainHelmet(short metadata) : base(302, metadata)
		{
			ItemMaterial = ItemMaterial.Chain;
			ItemType = ItemType.Helmet;
		}
	}
}
