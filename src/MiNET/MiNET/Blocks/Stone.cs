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
			return new Item(4); // Drop cobblestone
		}
	}
}