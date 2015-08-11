namespace MiNET.Items
{
	internal class ItemRawPorkchop : Item
	{
		internal ItemRawPorkchop(short metadata) : base(319, metadata)
		{
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(320);
		}
	}
}