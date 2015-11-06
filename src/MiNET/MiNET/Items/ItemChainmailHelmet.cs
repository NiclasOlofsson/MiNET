namespace MiNET.Items
{
	public class ItemChainmailHelmet : Item
	{
		public ItemChainmailHelmet(short metadata) : base(302, metadata)
		{
			ItemType = ItemType.Helmet;
			ItemMaterial = ItemMaterial.Chain;
		}
	}
}
