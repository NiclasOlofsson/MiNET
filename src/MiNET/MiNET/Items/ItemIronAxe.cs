namespace MiNET.Items
{
	public class ItemIronAxe : Item
	{
		public ItemIronAxe(short metadata) : base(258, metadata)
		{
			ItemMaterial = ItemMaterial.Iron;
			ItemType = ItemType.Axe;
		}
	}
}