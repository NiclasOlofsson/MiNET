namespace MiNET.Items
{
	internal class ItemIronLeggings : Item
	{
		internal ItemIronLeggings(short metadata) : base(308, metadata)
		{
			ItemType = ItemType.Leggings;
			ItemMaterial = ItemMaterial.Iron;
		}
	}
}