namespace MiNET.Items
{
	public class ItemDiamondChestplate : Item
	{
		public ItemDiamondChestplate() : base(311)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Chestplate;
			ItemMaterial = ItemMaterial.Diamond;
		}
	}
}