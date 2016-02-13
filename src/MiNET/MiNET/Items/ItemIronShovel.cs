namespace MiNET.Items
{
	public class ItemIronShovel : ItemShovel
	{
		public ItemIronShovel() : base(256)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Iron;
			ItemType = ItemType.Shovel;
		}
	}
}