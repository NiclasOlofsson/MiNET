namespace MiNET.Items
{
	public class ItemMuttonRaw : FoodItem
	{
		public ItemMuttonRaw() : base(423, 0, 3, 1.8)
		{
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(424);
		}
	}
}