using MiNET.Items;

namespace MiNET.Blocks
{
	public class GrassPath : Block
	{
		public GrassPath() : base(198)
		{
		}

		public override Item GetDrops()
		{
			return new ItemBlock(new Dirt(), 0) {Count = 1}; // Drop dirt block
		}
	}
}