namespace MiNET.Items
{
	public class ItemWoodenShovel : ItemShovel
	{
		public ItemWoodenShovel(short metadata) : base(269, metadata)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Wood;
			ItemType = ItemType.Shovel;
			FuelEfficiency = 10;
		}
	}
}