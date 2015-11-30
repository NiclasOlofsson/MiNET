namespace MiNET.Items
{
	public class ItemIronLeggings : Item
	{
		public ItemIronLeggings(short metadata) : base(308, metadata)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Leggings;
			ItemMaterial = ItemMaterial.Iron;
		}
	}
}