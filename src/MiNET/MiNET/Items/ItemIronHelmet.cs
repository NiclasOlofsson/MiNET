namespace MiNET.Items
{
	public class ItemIronHelmet : Item
	{
		public ItemIronHelmet() : base(306)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Helmet;
			ItemMaterial = ItemMaterial.Iron;
		}
	}
}