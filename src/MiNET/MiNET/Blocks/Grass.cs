using MiNET.Items;

namespace MiNET.Blocks
{
	public class Grass : Block
	{
		public Grass() : base(2)
		{
			BlastResistance = 3;
			Hardness = 0.6f;
		}

		public override Item[] GetDrops()
		{
			return new[] {new ItemBlock(new Dirt(), 0) {Count = 1}}; //Drop dirt block
		}
	}
}