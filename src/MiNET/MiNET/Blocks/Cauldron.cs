

using MiNET.Items;

namespace MiNET.Blocks
{
	public class Cauldron : Block
	{
		public Cauldron() : base(118)
		{
			IsTransparent = true;
			BlastResistance = 10;
			Hardness = 2;
		}

		public override Item[] GetDrops()
		{
			return new[] {ItemFactory.GetItem(380)};
		}
	}
}