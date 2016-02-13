namespace MiNET.Items
{
	public class ItemDiamondHelmet : Item
	{
		public ItemDiamondHelmet() : base(310)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Helmet;
			ItemMaterial = ItemMaterial.Diamond;
		}
	}
}