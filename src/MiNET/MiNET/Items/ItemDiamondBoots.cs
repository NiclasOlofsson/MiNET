namespace MiNET.Items
{
	public class ItemDiamondBoots : Item
	{
		public ItemDiamondBoots() : base(313)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Boots;
			ItemMaterial = ItemMaterial.Diamond;
		}
	}
}