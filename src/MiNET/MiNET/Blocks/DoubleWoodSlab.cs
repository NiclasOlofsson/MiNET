using MiNET.Items;

namespace MiNET.Blocks
{
	public class DoubleWoodSlab : Block
	{
		public DoubleWoodSlab() : base(157)
		{
			BlastResistance = 15;
			Hardness = 2;
			IsFlammable = true;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new[] {ItemFactory.GetItem(158, Metadata, 2)};
		}
	}
}