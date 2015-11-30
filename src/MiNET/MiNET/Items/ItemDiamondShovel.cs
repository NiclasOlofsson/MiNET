namespace MiNET.Items
{
	public class ItemDiamondShovel : ItemShovel
	{
		public ItemDiamondShovel(short metadata) : base(277, metadata)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Diamond;
			ItemType = ItemType.Shovel;
		}
	}
}