using MiNET.Items;

namespace MiNET.Blocks
{
	public class FlowerPot : Block
	{
		public FlowerPot() : base(140)
		{
			IsTransparent = true;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new[] {ItemFactory.GetItem(390, 0, 1)};
		}
	}
}