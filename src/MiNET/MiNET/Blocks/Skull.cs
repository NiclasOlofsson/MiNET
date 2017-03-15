using MiNET.Items;

namespace MiNET.Blocks
{
	public class Skull : Block
	{
		public Skull() : base(144)
		{
			IsTransparent = true;
			BlastResistance = 5;
			Hardness = 1;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new[] {ItemFactory.GetItem(397, Metadata, 1)};
		}
	}
}