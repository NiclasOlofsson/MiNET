using System;
using MiNET.Items;

namespace MiNET.Blocks
{
	public class Potatoes : Crops
	{
		public Potatoes() : base(142)
		{
		}

		public override Item[] GetDrops(Item tool)
		{
			if (Metadata == 7)
			{
				Random random = new Random();
				return new[] {ItemFactory.GetItem(392, 0, (byte) random.Next(1, 5))};
			}

			return new[] {ItemFactory.GetItem(392, 0, 1)};
		}
	}
}