using MiNET.Items;

namespace MiNET.Blocks
{
	public class Stone : Block
	{
		public Stone() : base(1)
		{
		}

		public override Item GetDrops()
		{
			return new ItemBlock(new Cobblestone(), 0) {Count = 1}; // Drop cobblestone
		}
	}
}