using MiNET.Items;

namespace MiNET.Blocks
{
	public class CoalOre : Block
	{
		public CoalOre() : base(16)
		{
			BlastResistance = 15;
			Hardness = 3;
		}

		public override Item[] GetDrops(Item tool)
		{
			if (tool.ItemMaterial < ItemMaterial.Wood) return new Item[0];

			return new[] {ItemFactory.GetItem(263, 0, 1)};
		}
	}
}