namespace MiNET.Items
{
	public class ItemIronShovel : ItemShovel
	{
		public ItemIronShovel(short metadata) : base(256, metadata)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Iron;
			ItemType = ItemType.Shovel;
		}
	}
}