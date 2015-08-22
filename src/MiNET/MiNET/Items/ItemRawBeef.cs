namespace MiNET.Items
{
	public class ItemRawBeef : Item
	{
		public ItemRawBeef(short metadata) : base(363, metadata)
		{
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(364);
		}
	}
}