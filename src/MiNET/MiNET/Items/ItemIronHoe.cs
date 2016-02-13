namespace MiNET.Items
{
	public class ItemIronHoe : ItemHoe
	{
		public ItemIronHoe() : base(292)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Iron;
			ItemType = ItemType.Hoe;
		}
	}
}