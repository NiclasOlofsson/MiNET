namespace MiNET.Items
{
	public class ItemIronBoots : Item
	{
		public ItemIronBoots() : base(309)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Boots;
			ItemMaterial = ItemMaterial.Iron;
		}
	}
}