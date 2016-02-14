namespace MiNET.Items
{
	public class ItemStonePickaxe : Item
	{
		public ItemStonePickaxe() : base(270)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Stone;
			ItemType = ItemType.PickAxe;
		}
	}

	public class ItemDiamondPickaxe : Item
	{
		public ItemDiamondPickaxe() : base(278)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Stone;
			ItemType = ItemType.PickAxe;
		}
	}

}