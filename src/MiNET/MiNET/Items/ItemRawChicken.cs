namespace MiNET.Items
{
	internal class ItemRawChicken : Item
	{
		internal ItemRawChicken(short metadata) : base(365, metadata)
		{
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(366);
		}
	}
}