using MiNET.Items;

namespace MiNET.Blocks
{
	public class EmeraldOre : Block
	{
		public EmeraldOre() : base(129)
		{
			BlastResistance = 15;
			Hardness = 3;
		}

		public override Item[] GetDrops()
		{
			return new[] {ItemFactory.GetItem(388, 0, 1)};
		}
	}
}