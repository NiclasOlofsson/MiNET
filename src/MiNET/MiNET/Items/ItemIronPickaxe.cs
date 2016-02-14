namespace MiNET.Items
{
	public class ItemIronPickaxe : Item
	{
		public ItemIronPickaxe() : base(257)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Iron;
			ItemType = ItemType.PickAxe;
		}
	}
}