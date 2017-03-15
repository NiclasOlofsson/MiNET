using MiNET.Items;

namespace MiNET.Blocks
{
	public class GoldOre : Block
	{
		public GoldOre() : base(14)
		{
			BlastResistance = 15;
			Hardness = 3;
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(266, 0);
		}

		public override Item[] GetDrops(Item tool)
		{
			if (tool.ItemMaterial < ItemMaterial.Iron) return new Item[0];

			return base.GetDrops(tool);
		}
	}
}