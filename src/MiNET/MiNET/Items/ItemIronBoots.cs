namespace MiNET.Items
{
	internal class ItemIronBoots : Item
	{
		internal ItemIronBoots(short metadata) : base(309, metadata)
		{
			ItemType = ItemType.Boots;
			ItemMaterial = ItemMaterial.Iron;
		}
	}
}