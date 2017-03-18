using System;
using MiNET.Items;

namespace MiNET.Blocks
{
	public class EmeraldOre : Block
	{
		public EmeraldOre() : base(129)
		{
			BlastResistance = 15;
			Hardness = 3;
		}

		public override Item[] GetDrops(Item tool)
		{
			if (tool.ItemMaterial < ItemMaterial.Stone) return new Item[0];

			return new[] {ItemFactory.GetItem(388, 0, 1)};
		}

		public override float GetExperiencePoints()
		{
			Random random = new Random();
			return random.Next(3, 8);
		}
	}
}