namespace MiNET.Items
{
	public class ItemStonePickaxe : Item
	{
		public ItemStonePickaxe() : base(274)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Stone;
			ItemType = ItemType.PickAxe;
		}
	}
}