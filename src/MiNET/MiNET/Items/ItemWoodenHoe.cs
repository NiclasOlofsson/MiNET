namespace MiNET.Items
{
	public class ItemWoodenHoe : ItemHoe
	{
		public ItemWoodenHoe(short metadata) : base(290, metadata)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Wood;
			ItemType = ItemType.Hoe;
			FuelEfficiency = 10;
		}
	}
}