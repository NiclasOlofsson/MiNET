namespace MiNET.Items
{
	public class ItemGoldShovel : ItemShovel
	{
		public ItemGoldShovel() : base(284)
		{
			MaxStackSize = 1;
			ItemMaterial = ItemMaterial.Gold;
			ItemType = ItemType.Shovel;
		}
	}
}