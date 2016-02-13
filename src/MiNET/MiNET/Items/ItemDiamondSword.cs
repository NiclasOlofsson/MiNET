namespace MiNET.Items
{
	public class ItemDiamondSword : Item
	{
		public ItemDiamondSword() : base(276)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Sword;
			ItemMaterial = ItemMaterial.Diamond;
		}
	}
}