namespace MiNET.Items
{
	public class ItemGoldBoots : Item
	{
		public ItemGoldBoots(short metadata) : base(317, metadata)
		{
			ItemType = ItemType.Boots;
			ItemMaterial = ItemMaterial.Gold;
		}
	}
}
