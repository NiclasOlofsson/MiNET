namespace MiNET.Items
{
	public class ItemDiamondHoe : ItemHoe
	{
		public ItemDiamondHoe(short metadata) : base(293, metadata)
		{
			ItemMaterial = ItemMaterial.Diamond;
			ItemType = ItemType.Hoe;
		}
	}
}