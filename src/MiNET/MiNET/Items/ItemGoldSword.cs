namespace MiNET.Items
{
	public class ItemGoldSword : Item
	{
		public ItemGoldSword(short metadata) : base(283, metadata)
		{
			ItemMaterial = ItemMaterial.Gold;
			ItemType = ItemType.Sword;
			FuelEfficiency = 10;
		}
	}
}