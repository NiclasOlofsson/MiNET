

using MiNET.Items;

namespace MiNET.Blocks
{
	public class PistonHead : Block
	{
		public PistonHead() : base(34)
		{
			BlastResistance = 2.5f;
			IsTransparent = true;
		}

		public override Item[] GetDrops()
		{
			return new Item[0];
		}
	}
}