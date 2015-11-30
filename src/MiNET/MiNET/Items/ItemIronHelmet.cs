namespace MiNET.Items
{
	public class ItemIronHelmet : Item
	{
		public ItemIronHelmet(short metadata) : base(306, metadata)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Helmet;
			ItemMaterial = ItemMaterial.Iron;
		}
	}
}