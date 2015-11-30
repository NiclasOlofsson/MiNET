namespace MiNET.Items
{
	public class ItemGoldAxe : Item
	{
		public ItemGoldAxe(short metadata) : base(286, metadata)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Gold;
			ItemType = ItemType.Axe;
		}
	}
}