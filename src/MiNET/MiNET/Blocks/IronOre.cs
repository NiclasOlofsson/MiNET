using MiNET.Items;

namespace MiNET.Blocks
{
	public class IronOre : Block
	{
		public IronOre() : base(15)
		{
			BlastResistance = 15;
			Hardness = 3;
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(265, 0);
		}

		public override Item[] GetDrops(Item tool)
		{
			if (tool.ItemMaterial < ItemMaterial.Stone) return new Item[0];

			return base.GetDrops(tool);
		}
	}
}