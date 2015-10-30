namespace MiNET.Items
{
	public class ItemChainBoots : Item
	{
		public ItemChainBoots(short metadata) : base(305, metadata)
		{
			ItemMaterial = ItemMaterial.Chain;
			ItemType = ItemType.Boots;
		}
	}
}
