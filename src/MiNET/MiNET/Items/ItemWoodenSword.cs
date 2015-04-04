namespace MiNET.Items
{
	public class ItemWoodenSword : Item
	{
		public ItemWoodenSword(short metadata) : base(268, metadata)
		{
			ItemMaterial = ItemMaterial.Wood;
			ItemType = ItemType.Sword;
			FuelEfficiency = 10;
		}
	}
}