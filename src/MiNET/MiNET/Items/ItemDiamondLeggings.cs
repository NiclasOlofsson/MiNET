namespace MiNET.Items
{
	internal class ItemDiamondLeggings : Item
	{
		public ItemDiamondLeggings(short metadata) : base(312, metadata)
		{
			ItemType = ItemType.Leggings;
			ItemMaterial = ItemMaterial.Diamond;
		}
	}
}