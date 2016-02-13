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
			return ItemFactory.GetItem(332, 0, 4); // Drop snowball
		}
	}
}