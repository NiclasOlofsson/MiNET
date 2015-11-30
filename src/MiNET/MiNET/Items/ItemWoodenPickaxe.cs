namespace MiNET.Items
{
	public class ItemWoodenPickaxe : Item
	{
		public ItemWoodenPickaxe(short metadata) : base(270, metadata)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Wood;
			ItemType = ItemType.PickAxe;
			FuelEfficiency = 10;
		}
	}
}