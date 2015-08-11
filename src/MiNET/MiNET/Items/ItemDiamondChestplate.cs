namespace MiNET.Items
{
	internal class ItemDiamondChestplate : Item
	{
		public ItemDiamondChestplate(short metadata) : base(311, metadata)
		{
			ItemType = ItemType.Chestplate;
			ItemMaterial = ItemMaterial.Diamond;
		}
	}
}