namespace MiNET.Items
{
	public class ItemWoodenShovel : ItemShovel
	{
		public ItemWoodenShovel(short metadata) : base(269, metadata)
		{
			ItemMaterial = ItemMaterial.Wood;
			ItemType = ItemType.Shovel;
			FuelEfficiency = 10;
		}
	}
}