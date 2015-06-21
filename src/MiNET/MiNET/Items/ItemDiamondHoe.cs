namespace MiNET.Items
{
	internal class ItemDiamondHoe : ItemHoe
	{
		public ItemDiamondHoe(short metadata) : base(293, metadata)
		{
			ItemMaterial = ItemMaterial.Diamond;
			ItemType = ItemType.Hoe;
		}
	}
}