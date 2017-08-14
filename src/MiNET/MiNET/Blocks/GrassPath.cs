using MiNET.Items;

namespace MiNET.Blocks
{
	public class GrassPath : Block
	{
		public GrassPath() : base(198)
		{
			BlastResistance = 3.25f;
			Hardness = 0.6f;
			IsTransparent = true; // Partial - blocks light.
			IsBlockingSkylight = false; // Partial - blocks light.
		}

		public override Item[] GetDrops(Item tool)
		{
			return new[] {new ItemBlock(new Dirt(), 0) {Count = 1}}; // Drop dirt block
		}
	}
}