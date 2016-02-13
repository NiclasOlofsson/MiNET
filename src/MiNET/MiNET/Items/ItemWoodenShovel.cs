namespace MiNET.Items
{
	public class ItemWoodenShovel : ItemShovel
	{
		public ItemWoodenShovel() : base(269)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Wood;
			ItemType = ItemType.Shovel;
			FuelEfficiency = 10;
		}
	}
}