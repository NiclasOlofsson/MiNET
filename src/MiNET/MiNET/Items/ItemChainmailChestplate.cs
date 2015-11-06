namespace MiNET.Items
{
	public class ItemChainmailChestplate : Item
	{
		public ItemChainmailChestplate(short metadata) : base(303, metadata)
		{
			ItemType = ItemType.Chestplate;
			ItemMaterial = ItemMaterial.Chain;
		}
	}
}
