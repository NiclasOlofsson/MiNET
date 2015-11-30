namespace MiNET.Items
{
	public class ItemIronSword : Item
	{
		public ItemIronSword(short metadata) : base(267, metadata)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Iron;
			ItemType = ItemType.Sword;
		}
	}
}