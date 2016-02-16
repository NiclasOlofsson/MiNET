namespace MiNET.Items
{
	public class ItemRawChicken : FoodItem
	{
		public ItemRawChicken() : base(365, 0, 2, 1.2)
		{
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(366);
		}
	}
}