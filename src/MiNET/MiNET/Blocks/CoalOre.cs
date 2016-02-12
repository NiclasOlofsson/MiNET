using MiNET.Items;

namespace MiNET.Blocks
{
	public class CoalOre : Block
	{
		public CoalOre() : base(16)
		{
		}

		public override Item GetDrops()
		{
			return new Item(263, 0) {Count = 1};
		}
	}
}