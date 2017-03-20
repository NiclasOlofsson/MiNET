using MiNET.Items;

namespace MiNET.Blocks
{
	public class Snow : Block
	{
		public Snow() : base(80)
		{
			BlastResistance = 1;
			Hardness = 0.2f;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new[] {ItemFactory.GetItem(332, 0, 4)}; // Drop snowball
		}
	}
}