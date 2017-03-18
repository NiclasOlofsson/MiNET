using System;
using MiNET.Items;

namespace MiNET.Blocks
{
	public class LapisOre : Block
	{
		public LapisOre() : base(21)
		{
			BlastResistance = 15;
			Hardness = 3;
		}

		public override Item[] GetDrops(Item tool)
		{
			if (tool.ItemMaterial < ItemMaterial.Stone) return new Item[0];

			// Random between 4-8
			var rnd = new Random((int) DateTime.UtcNow.Ticks);
			var plus = rnd.Next(4);
			return new[] {ItemFactory.GetItem(351, 4, (byte) (4 + plus))};
		}

		public override float GetExperiencePoints()
		{
			Random random = new Random();
			return random.Next(2, 6);
		}

	}
}