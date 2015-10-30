namespace MiNET.Items
{
	public class ItemIronPickaxe : ItemShear
	{
		public ItemIronPickaxe(short metadata) : base(359, metadata)
		{
			ItemMaterial = ItemMaterial.Iron;
			ItemType = ItemType.Item;
		}
	}
}
