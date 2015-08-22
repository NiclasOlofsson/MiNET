namespace MiNET.Items
{
	public class ItemIronHoe : ItemHoe
	{
		public ItemIronHoe(short metadata)
			: base(292, metadata)
		{
			ItemMaterial = ItemMaterial.Iron;
			ItemType = ItemType.Hoe;
		}
	}
}