namespace MiNET.Items
{
	public class ItemWoodenSword : Item
	{
		public ItemWoodenSword() : base(268)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Wood;
			ItemType = ItemType.Sword;
			FuelEfficiency = 10;
		}
	}
}