namespace MiNET.Items
{
	public class ItemGoldenLeggings : Item
	{
		public ItemGoldenLeggings(short metadata) : base(316, metadata)
		{
			ItemMaterial = ItemMaterial.Gold;
			ItemType = ItemType.Leggings;
		}
	}
}
