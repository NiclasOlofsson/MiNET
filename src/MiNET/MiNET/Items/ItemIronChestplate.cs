namespace MiNET.Items
{
	internal class ItemIronChestplate : Item
	{
		internal ItemIronChestplate(short metadata) : base(307, metadata)
		{
			ItemType = ItemType.Chestplate;
			ItemMaterial = ItemMaterial.Iron;
		}
	}
}