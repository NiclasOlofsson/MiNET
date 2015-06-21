namespace MiNET.Items
{
	internal class ItemWoodenHoe : ItemHoe
	{
		public ItemWoodenHoe(short metadata) : base(290, metadata)
		{
			ItemMaterial = ItemMaterial.Wood;
			ItemType = ItemType.Hoe;
			FuelEfficiency = 10;
		}
	}
}