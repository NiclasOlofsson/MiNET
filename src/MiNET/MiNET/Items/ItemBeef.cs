namespace MiNET.Items
{
	public class ItemBeef : FoodItem
	{
		public ItemBeef() : base(363, 0, 3, 1.8)
		{
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(364);
		}
	}
}