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
			return new Item(3); // Drop dirt block
		}
	}
}