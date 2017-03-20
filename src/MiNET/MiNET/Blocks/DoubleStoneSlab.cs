using MiNET.Items;

namespace MiNET.Blocks
{
	public class DoubleStoneSlab : Block
	{
		public DoubleStoneSlab() : base(43)
		{
			BlastResistance = 30;
			Hardness = 2;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new[] {ItemFactory.GetItem(44, Metadata, 2)};
		}
	}
}