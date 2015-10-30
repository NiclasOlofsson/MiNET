namespace MiNET.Items
{
	public class ItemLeatherTunic : Item
	{
		public ItemLeatherTunic(short metadata) : base(299, metadata)
		{
			ItemMaterial = ItemMaterial.Leather;
			ItemType = ItemType.Chestplate;
		}
	}
}
