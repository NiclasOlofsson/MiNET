using System;
using MiNET.Items;

namespace MiNET.Blocks
{
	public class Carrots : Crops
	{
		public Carrots() : base(141)
		{
		}

		public override Item[] GetDrops(Item tool)
		{
			if (Metadata == 7)
			{
				Random random = new Random();
				return new[] {ItemFactory.GetItem(391, 0, (byte) random.Next(1, 5))};
			}

			return new[] {ItemFactory.GetItem(391, 0, 1)};
		}
	}
}