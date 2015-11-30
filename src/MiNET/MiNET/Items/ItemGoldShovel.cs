namespace MiNET.Items
{
	public class ItemGoldShovel : ItemShovel
	{
		public ItemGoldShovel(short metadata) : base(284, metadata)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Gold;
			ItemType = ItemType.Shovel;
		}
	}
}