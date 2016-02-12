using MiNET.Items;

namespace MiNET.Blocks
{
	public class Wood : Block
	{
		public Wood() : base(17)
		{
			FuelEfficiency = 15;
		}

		public override Item GetDrops()
		{
			return new Item(Id, (short) (Metadata & 0x03)) {Count = 1};
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(263, 1);
		}
	}
}