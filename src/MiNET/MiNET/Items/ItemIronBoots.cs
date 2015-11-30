namespace MiNET.Items
{
	public class ItemIronBoots : Item
	{
		public ItemIronBoots(short metadata) : base(309, metadata)
		{
			MaxStackSize = 1;
			ItemType = ItemType.Boots;
			ItemMaterial = ItemMaterial.Iron;
		}
	}
}