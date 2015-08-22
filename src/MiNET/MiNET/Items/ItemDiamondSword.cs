namespace MiNET.Items
{
	public class ItemDiamondSword : Item
	{
		public ItemDiamondSword(short metadata) : base(276, metadata)
		{
			ItemType = ItemType.Sword;
			ItemMaterial = ItemMaterial.Diamond;
		}
	}
}