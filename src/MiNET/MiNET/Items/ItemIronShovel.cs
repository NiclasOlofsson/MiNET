namespace MiNET.Items
{
	public class ItemIronShovel : ItemShovel
	{
		public ItemIronShovel(short metadata) : base(256, metadata)
		{
			ItemMaterial = ItemMaterial.Iron;
			ItemType = ItemType.Shovel;
		}
	}
}