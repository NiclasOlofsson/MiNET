namespace MiNET.Items
{
	public class ItemGoldenBoots : Item
	{
		public ItemGoldenBoots(short metadata) : base(317, metadata)
		{
			ItemMaterial = ItemMaterial.Gold;
			ItemType = ItemType.Boots;
		}
	}
}
