namespace MiNET.Items
{
	public class ItemGoldLeggings : Item
	{
		public ItemGoldLeggings(short metadata) : base(316, metadata)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Leggings;
			ItemMaterial = ItemMaterial.Gold;
		}
	}
}
