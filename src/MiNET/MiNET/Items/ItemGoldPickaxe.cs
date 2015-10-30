namespace MiNET.Items
{
	public class ItemGoldPickaxe : Item
	{
		public ItemGoldPickaxe(short metadata) : base(285, metadata)
		{
			ItemMaterial = ItemMaterial.Gold;
			ItemType = ItemType.Pickaxe;
		}
	}
}
