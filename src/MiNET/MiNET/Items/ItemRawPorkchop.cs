namespace MiNET.Items
{
	public class ItemRawPorkchop : Item
	{
		public ItemRawPorkchop() : base(319)
		{
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(320);
		}
	}
}