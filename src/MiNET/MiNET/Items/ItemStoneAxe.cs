namespace MiNET.Items
{
	public class ItemStoneAxe : Item
	{
		public ItemStoneAxe(short metadata) : base(275, metadata)
		{
			ItemMaterial = ItemMaterial.Stone;
			ItemType = ItemType.Axe;
		}
	}
}