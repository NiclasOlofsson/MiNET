namespace MiNET.Items
{
	public class ItemGoldAxe : Item
	{
		public ItemGoldAxe(short metadata) : base(286, metadata)
		{
			ItemMaterial = ItemMaterial.Gold;
			ItemType = ItemType.Axe;
			FuelEfficiency = 10;
		}
	}
}