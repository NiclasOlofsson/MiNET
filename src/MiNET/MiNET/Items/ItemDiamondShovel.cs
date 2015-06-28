namespace MiNET.Items
{
	public class ItemDiamondShovel : ItemShovel
	{
		public ItemDiamondShovel(short metadata) : base(277, metadata)
		{
			ItemMaterial = ItemMaterial.Diamond;
			ItemType = ItemType.Shovel;
		}
	}
}