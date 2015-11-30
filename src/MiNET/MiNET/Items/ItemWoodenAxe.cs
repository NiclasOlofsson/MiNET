namespace MiNET.Items
{
	public class ItemWoodenAxe : Item
	{
		public ItemWoodenAxe(short metadata) : base(271, metadata)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Wood;
			ItemType = ItemType.Axe;
			FuelEfficiency = 10;
		}
	}
}