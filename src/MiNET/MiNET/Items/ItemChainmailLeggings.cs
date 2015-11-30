namespace MiNET.Items
{
	public class ItemChainmailLeggings : Item
	{
		public ItemChainmailLeggings(short metadata) : base(304, metadata)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Leggings;
			ItemMaterial = ItemMaterial.Chain;
		}
	}
}
