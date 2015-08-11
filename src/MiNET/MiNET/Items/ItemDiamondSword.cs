namespace MiNET.Items
{
	internal class ItemDiamondSword : Item
	{
		internal ItemDiamondSword(short metadata) : base(276, metadata)
		{
			ItemType = ItemType.Sword;
			ItemMaterial = ItemMaterial.Diamond;
		}
	}
}