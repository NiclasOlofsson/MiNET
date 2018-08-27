namespace MiNET.Items
{
	public class ItemWoodenAxe : ItemAxe
	{
		public ItemWoodenAxe() : base(271)
		{
			ItemMaterial = ItemMaterial.Wood;
			FuelEfficiency = 10;
		}
	}
}