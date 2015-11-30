namespace MiNET.Items
{
	public class ItemWoodenSword : Item
	{
		public ItemWoodenSword(short metadata) : base(268, metadata)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Wood;
			ItemType = ItemType.Sword;
			FuelEfficiency = 10;
		}
	}
}