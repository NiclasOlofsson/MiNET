namespace MiNET.Items
{
	internal class ItemIronSword : Item
	{
		public ItemIronSword(short metadata) : base(267, metadata)
		{
			ItemMaterial = ItemMaterial.Iron;
			ItemType = ItemType.Sword;
		}
	}
}