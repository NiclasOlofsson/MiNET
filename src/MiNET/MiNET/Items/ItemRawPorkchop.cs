namespace MiNET.Items
{
	public class ItemRawPorkchop : Item
	{
		public ItemRawPorkchop(short metadata) : base(319, metadata)
		{
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(320);
		}
	}
}