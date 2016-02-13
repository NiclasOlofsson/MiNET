namespace MiNET.Items
{
	public class ItemStoneAxe : Item
	{
		public ItemStoneAxe() : base(275)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Stone;
			ItemType = ItemType.Axe;
		}
	}
}