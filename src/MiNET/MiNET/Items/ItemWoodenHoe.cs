namespace MiNET.Items
{
	public class ItemWoodenHoe : ItemHoe
	{
		public ItemWoodenHoe() : base(290)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Wood;
			ItemType = ItemType.Hoe;
			FuelEfficiency = 10;
		}
	}
}