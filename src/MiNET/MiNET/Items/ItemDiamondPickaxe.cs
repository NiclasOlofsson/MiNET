namespace MiNET.Items
{
	public class ItemDiamondPickaxe : Item
	{
		public ItemDiamondPickaxe(short metadata) : base(278, metadata)
		{
			ItemMaterial = ItemMaterial.Diamond;
			ItemType = ItemType.Pickaxe;
		}
	}
}
