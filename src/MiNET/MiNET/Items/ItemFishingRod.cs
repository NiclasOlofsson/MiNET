namespace MiNET.Items
{
	public class ItemIronPickaxe : ItemFishing
	{
		public ItemIronPickaxe(short metadata) : base(346, metadata)
		{
			ItemMaterial = ItemMaterial.Wood;
			ItemType = ItemType.Item;
		}
	}
}
