namespace MiNET.Items
{
	public class ItemWoodenPickaxe : Item
	{
		public ItemWoodenPickaxe(short metadata) : base(270, metadata)
		{
			ItemMaterial = ItemMaterial.Wood;
			ItemType = ItemType.PickAxe;
			FuelEfficiency = 10;
		}
	}
}