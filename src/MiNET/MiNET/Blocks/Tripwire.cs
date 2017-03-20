using MiNET.Items;

namespace MiNET.Blocks
{
	public class Tripwire : Block
	{
		public Tripwire() : base(132)
		{
			IsTransparent = true;
			IsSolid = false;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new[] {ItemFactory.GetItem(287, 0, 1)};
		}
	}
}