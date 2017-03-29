using MiNET.Items;

namespace MiNET.Blocks
{
	public class Clay : Block
	{
		public Clay() : base(82)
		{
			BlastResistance = 3;
			Hardness = 0.6f;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new Item[] { ItemFactory.GetItem(337, 0, 4) };
		}
	}
}