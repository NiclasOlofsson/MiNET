namespace MiNET.Items
{
	public class ItemGoldHoe : ItemHoe
	{
		public ItemGoldHoe() : base(294)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Gold;
			ItemType = ItemType.Hoe;
		}
	}
}