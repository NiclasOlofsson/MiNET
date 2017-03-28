namespace MiNET.Items
{
	public class ItemBone : Item
	{
		public ItemBone() : base(352)
		{
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(364);
		}
	}
}