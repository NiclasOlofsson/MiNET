using System;
using MiNET.Items;

namespace MiNET.Blocks
{
	public class DiamondOre : Block
	{
		public DiamondOre() : base(56)
		{
			BlastResistance = 15;
			Hardness = 3;
		}

		public override Item[] GetDrops(Item tool)
		{
			if (tool.ItemMaterial < ItemMaterial.Iron) return new Item[0];

			return new[] {ItemFactory.GetItem(264, 0, 1)};
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(264, 0);
		}

		public override float GetExperiencePoints()
		{
			Random random = new Random();
			return random.Next(3, 8);
		}
	}
}