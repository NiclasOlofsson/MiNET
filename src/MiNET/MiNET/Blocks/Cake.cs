using MiNET.Items;

namespace MiNET.Blocks
{
	public class Cake : Block
	{
		public Cake() : base(92)
		{
			IsTransparent = true;
			BlastResistance = 2.5f;
			Hardness = 0.5f;
		}

		public override Item[] GetDrops(Item tool)
		{
			if (Metadata == 0) return new Item[] { ItemFactory.GetItem(354, 0, 1) };
			return new Item[0];
		}
	}
}