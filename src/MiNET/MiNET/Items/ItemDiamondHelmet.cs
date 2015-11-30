namespace MiNET.Items
{
	public class ItemDiamondHelmet : Item
	{
		public ItemDiamondHelmet(short metadata) : base(310, metadata)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Helmet;
			ItemMaterial = ItemMaterial.Diamond;
		}
	}
}