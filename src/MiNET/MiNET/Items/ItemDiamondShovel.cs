namespace MiNET.Items
{
	public class ItemDiamondShovel : ItemShovel
	{
		public ItemDiamondShovel() : base(277)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Diamond;
			ItemType = ItemType.Shovel;
		}
	}
}