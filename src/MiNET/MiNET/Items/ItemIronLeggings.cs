namespace MiNET.Items
{
	public class ItemIronLeggings : Item
	{
		public ItemIronLeggings(short metadata) : base(308, metadata)
		{
			ItemType = ItemType.Leggings;
			ItemMaterial = ItemMaterial.Iron;
		}
	}
}