namespace MiNET.Items
{
	public class ItemGoldHelmet : Item
	{
		public ItemGoldHelmet(short metadata) : base(314, metadata)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Helmet;
			ItemMaterial = ItemMaterial.Gold;
		}
	}
}
