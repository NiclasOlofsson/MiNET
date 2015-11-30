namespace MiNET.Items
{
	public class ItemDiamondAxe : Item
	{
		public ItemDiamondAxe(short metadata) : base(279, metadata)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Diamond;
			ItemType = ItemType.Axe;
		}
	}
}