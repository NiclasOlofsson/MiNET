namespace MiNET.Items
{
	public class ItemGoldenChestplate : Item
	{
		public ItemGoldenChestplate(short metadata) : base(315, metadata)
		{
			ItemMaterial = ItemMaterial.Gold;
			ItemType = ItemType.Chestplate;
		}
	}
}
