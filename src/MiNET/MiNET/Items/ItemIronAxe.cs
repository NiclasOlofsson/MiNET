namespace MiNET.Items
{
	public class ItemIronAxe : Item
	{
		public ItemIronAxe() : base(258)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Iron;
			ItemType = ItemType.Axe;
		}
	}
}