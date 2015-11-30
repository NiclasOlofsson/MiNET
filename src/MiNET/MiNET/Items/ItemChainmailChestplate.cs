namespace MiNET.Items
{
	public class ItemChainmailChestplate : Item
	{
		public ItemChainmailChestplate(short metadata) : base(303, metadata)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Chestplate;
			ItemMaterial = ItemMaterial.Chain;
		}
	}
}
