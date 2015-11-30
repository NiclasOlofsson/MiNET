namespace MiNET.Items
{
	public class ItemChainmailBoots : Item
	{
		public ItemChainmailBoots(short metadata) : base(305, metadata)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Boots;
			ItemMaterial = ItemMaterial.Chain;
		}
	}
}
