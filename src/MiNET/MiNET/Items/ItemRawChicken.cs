namespace MiNET.Items
{
	public class ItemRawChicken : Item
	{
		public ItemRawChicken(short metadata) : base(365, metadata)
		{
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(366);
		}
	}
}