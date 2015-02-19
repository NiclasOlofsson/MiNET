namespace MiNET.Items
{
	class RawPorkchop : Item
	{
		internal RawPorkchop(short metadata) : base(319, metadata)
		{
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(320);
		}
	}
}
