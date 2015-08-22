namespace MiNET.Items
{
	public class ItemDiamondHelmet : Item
	{
		public ItemDiamondHelmet(short metadata) : base(310, metadata)
		{
			ItemType = ItemType.Helmet;
			ItemMaterial = ItemMaterial.Diamond;
		}
	}
}