namespace MiNET.Items
{
	public class ItemIronLeggings : Item
	{
		public ItemIronLeggings() : base(308)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Leggings;
			ItemMaterial = ItemMaterial.Iron;
		}
	}
}