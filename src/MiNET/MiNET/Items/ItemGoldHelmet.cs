namespace MiNET.Items
{
	public class ItemGoldHelmet : Item
	{
		public ItemGoldHelmet() : base(314)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Helmet;
			ItemMaterial = ItemMaterial.Gold;
		}
	}
}
