using MiNET.Items;

namespace MiNET.Blocks
{
	public class CoalOre : Block
	{
		public CoalOre() : base(16)
		{
			BlastResistance = 15;
			Hardness = 3;
		}

		public override Item GetDrops()
		{
			return ItemFactory.GetItem(263, 0, 1);
		}
	}
}