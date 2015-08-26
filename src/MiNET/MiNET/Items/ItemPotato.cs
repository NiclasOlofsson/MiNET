namespace MiNET.Items
{
	public class ItemPotato : FoodItem
	{
		public ItemPotato() : base(392, 0, 1)
		{
		}

		public override Item GetSmelt()
		{
			return new ItemBakedPotato();
		}
	}
}