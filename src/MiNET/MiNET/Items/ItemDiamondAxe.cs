namespace MiNET.Items
{
	public class ItemDiamondAxe : Item
	{
		public ItemDiamondAxe(short metadata) : base(279, metadata)
		{
			ItemMaterial = ItemMaterial.Diamond;
			ItemType = ItemType.Axe;
		}
	}
}