namespace MiNET.Items
{
	public class ItemDiamondHoe : ItemHoe
	{
		public ItemDiamondHoe() : base(293)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Diamond;
			ItemType = ItemType.Hoe;
		}
	}
}