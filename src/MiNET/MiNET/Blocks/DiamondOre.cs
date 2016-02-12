using MiNET.Items;

namespace MiNET.Blocks
{
	public class DiamondOre : Block
	{
		public DiamondOre() : base(56)
		{
		}

		public override Item GetDrops()
		{
			return new Item(264, 0) {Count = 1};
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(264, 0);
		}
	}
}