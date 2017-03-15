using MiNET.Items;

namespace MiNET.Blocks
{
	public class QuartzOre : Block
	{
		public QuartzOre() : base(153)
		{
			BlastResistance = 15;
			Hardness = 3;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new[] {ItemFactory.GetItem(406, 0, 1)};
		}
	}
}