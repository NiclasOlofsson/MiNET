namespace MiNET.Items
{
	public class ItemIronPickaxe : Item
	{
		public ItemIronPickaxe(short metadata) : base(257, metadata)
		{
			ItemMaterial = ItemMaterial.Iron;
			ItemType = ItemType.Pickaxe;
		}
	}
}
