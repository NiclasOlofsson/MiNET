namespace MiNET.Items
{
	public class ItemIronPickaxe : Item
	{
		public ItemIronPickaxe(short metadata) : base(330, metadata)
		{
			ItemMaterial = ItemMaterial.Iron;
			ItemType = ItemType.Item;
		}
	}
}
