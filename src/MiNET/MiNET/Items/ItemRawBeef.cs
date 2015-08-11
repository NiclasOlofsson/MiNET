namespace MiNET.Items
{
	internal class ItemRawBeef : Item
	{
		internal ItemRawBeef(short metadata) : base(363, metadata)
		{
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(364);
		}
	}
}