namespace MiNET.Items
{
	public class ItemDiamondBoots : Item
	{
		public ItemDiamondBoots(short metadata) : base(313, metadata)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Boots;
			ItemMaterial = ItemMaterial.Diamond;
		}
	}
}