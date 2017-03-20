using MiNET.Items;

namespace MiNET.Blocks
{
	public class Mycelium : Block
	{
		public Mycelium() : base(110)
		{
			BlastResistance = 2.5f;
			Hardness = 0.6f;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new[] {new ItemBlock(new Dirt(), 0)};
		}
	}
}