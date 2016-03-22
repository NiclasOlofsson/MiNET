using MiNET.Items;

namespace MiNET.Blocks
{
	public class Reeds : Block
	{
		public Reeds() : base(83)
		{
			IsSolid = false;
			IsTransparent = true;
		}

		public override Item[] GetDrops()
		{
			return new[] {ItemFactory.GetItem(338, 0, 1)};
		}
	}
}