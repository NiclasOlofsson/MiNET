namespace MiNET.Items
{
	public class ItemDiamondHoe : ItemHoe
	{
		public ItemDiamondHoe(short metadata) : base(293, metadata)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Diamond;
			ItemType = ItemType.Hoe;
		}
	}
}