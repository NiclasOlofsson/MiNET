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

		public override Item GetDrops()
		{
			return new ItemBlock(new Dirt(), 0);
		}
	}
}