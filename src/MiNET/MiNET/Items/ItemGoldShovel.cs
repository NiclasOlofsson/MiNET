namespace MiNET.Items
{
	public class ItemGoldShovel : ItemShovel
	{
		internal ItemGoldShovel(short metadata) : base(284, metadata)
		{
			ItemMaterial = ItemMaterial.Gold;
			ItemType = ItemType.Shovel;
		}
	}
}