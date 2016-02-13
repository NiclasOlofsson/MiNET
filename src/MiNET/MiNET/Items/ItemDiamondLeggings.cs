namespace MiNET.Items
{
	public class ItemDiamondLeggings : Item
	{
		public ItemDiamondLeggings() : base(312)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Leggings;
			ItemMaterial = ItemMaterial.Diamond;
		}
	}
}