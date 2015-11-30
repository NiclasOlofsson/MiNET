namespace MiNET.Items
{
	public class ItemIronHoe : ItemHoe
	{
		public ItemIronHoe(short metadata) : base(292, metadata)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Iron;
			ItemType = ItemType.Hoe;
		}
	}
}