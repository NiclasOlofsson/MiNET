namespace MiNET.Items
{
	public class ItemRawBeef : FoodItem
	{
		public ItemRawBeef() : base(363, 0, 3, 1.8)
		{
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(364);
		}
	}
}