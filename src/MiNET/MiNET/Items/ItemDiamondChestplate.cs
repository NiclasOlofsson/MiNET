namespace MiNET.Items
{
	public class ItemDiamondChestplate : Item
	{
		public ItemDiamondChestplate(short metadata) : base(311, metadata)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Chestplate;
			ItemMaterial = ItemMaterial.Diamond;
		}
	}
}