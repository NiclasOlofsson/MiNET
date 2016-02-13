namespace MiNET.Items
{
	public class ItemIronSword : Item
	{
		public ItemIronSword() : base(267)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Iron;
			ItemType = ItemType.Sword;
		}
	}
}