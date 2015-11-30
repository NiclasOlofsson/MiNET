namespace MiNET.Items
{
	public class ItemGoldBoots : Item
	{
		public ItemGoldBoots(short metadata) : base(317, metadata)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Boots;
			ItemMaterial = ItemMaterial.Gold;
		}
	}
}
