namespace MiNET.Items
{
	internal class ItemDiamondBoots : Item
	{
		public ItemDiamondBoots(short metadata) : base(313, metadata)
		{
			ItemType = ItemType.Boots;
			ItemMaterial = ItemMaterial.Diamond;
		}
	}
}