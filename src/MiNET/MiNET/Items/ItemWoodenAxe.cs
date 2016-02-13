namespace MiNET.Items
{
	public class ItemWoodenAxe : Item
	{
		public ItemWoodenAxe() : base(271)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Wood;
			ItemType = ItemType.Axe;
			FuelEfficiency = 10;
		}
	}
}