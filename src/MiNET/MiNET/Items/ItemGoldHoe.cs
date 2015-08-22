namespace MiNET.Items
{
	public class ItemGoldHoe : ItemHoe
	{
		public ItemGoldHoe(short metadata)
			: base(294, metadata)
		{
			ItemMaterial = ItemMaterial.Gold;
			ItemType = ItemType.Hoe;
		}
	}
}