using MiNET.Items;

namespace MiNET.Blocks
{
	public class Snow : Block
	{
		public Snow() : base(80)
		{
		}

		public override Item GetDrops()
		{
			return new Item(332, 0) {Count = 4}; // Drop snowball
		}
	}
}