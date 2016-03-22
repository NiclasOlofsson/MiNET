using MiNET.Items;

namespace MiNET.Blocks
{
	public class Farmland : Block
	{
		public Farmland() : base(60)
		{
			IsTransparent = false; // Partial - blocks light.
			BlastResistance = 3;
			Hardness = 0.6f;
		}

		public override Item GetDrops()
		{
			return new ItemBlock(new Dirt(), 0) {Count = 1}; // Drop dirt block
		}
	}
}