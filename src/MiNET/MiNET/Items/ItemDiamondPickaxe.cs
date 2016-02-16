namespace MiNET.Items
{
	public class ItemDiamondPickaxe : Item
	{
		public ItemDiamondPickaxe() : base(278)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Diamond;
			ItemType = ItemType.PickAxe;
		}
	}
}