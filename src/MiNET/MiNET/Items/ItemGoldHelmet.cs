namespace MiNET.Items
{
	public class ItemGoldHelmet : Item
	{
		public ItemGoldHelmet(short metadata) : base(314, metadata)
		{
			ItemType = ItemType.Helmet;
			ItemMaterial = ItemMaterial.Gold;
		}
	}
}
