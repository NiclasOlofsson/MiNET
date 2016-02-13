namespace MiNET.Items
{
	public class ItemStoneHoe : ItemHoe
	{
		public ItemStoneHoe() : base(291)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Stone;
			ItemType = ItemType.Hoe;
		}
	}
}