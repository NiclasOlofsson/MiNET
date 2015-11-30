namespace MiNET.Items
{
	public class ItemDiamondSword : Item
	{
		public ItemDiamondSword(short metadata) : base(276, metadata)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Sword;
			ItemMaterial = ItemMaterial.Diamond;
		}
	}
}