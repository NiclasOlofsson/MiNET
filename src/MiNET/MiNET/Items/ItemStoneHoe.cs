namespace MiNET.Items
{
	internal class ItemStoneHoe : ItemHoe
	{
		public ItemStoneHoe(short metadata) : base(291, metadata)
		{
			ItemMaterial = ItemMaterial.Stone;
			ItemType = ItemType.Hoe;
		}
	}
}