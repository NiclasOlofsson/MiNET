namespace MiNET.Items
{
	public class ItemGoldenHelmet : Item
	{
		public ItemGoldenHelmet(short metadata) : base(314, metadata)
		{
			ItemMaterial = ItemMaterial.Gold;
			ItemType = ItemType.Helmet;
		}
	}
}
