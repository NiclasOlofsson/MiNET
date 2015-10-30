namespace MiNET.Items
{
	public class ItemChainChestplate : Item
	{
		public ItemChainChestplate(short metadata) : base(303, metadata)
		{
			ItemMaterial = ItemMaterial.Chain;
			ItemType = ItemType.Chestplate;
		}
	}
}
