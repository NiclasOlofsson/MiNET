namespace MiNET.Items
{
	public class ItemWoodenHoe : ItemHoe
	{
		public ItemWoodenHoe() : base(290)
		{
			ItemMaterial = ItemMaterial.Wood;
			FuelEfficiency = 10;
		}
	}
}