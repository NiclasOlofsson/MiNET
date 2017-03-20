using System;
using MiNET.Items;

namespace MiNET.Blocks
{
	public class Beetroot : Crops
	{
		public Beetroot() : base(244)
		{
			MaxGrowth = 4;
		}

		public override Item[] GetDrops(Item tool)
		{
			if (Metadata == MaxGrowth)
			{
				// Can also return 0-3 seeds at random.
				var rnd = new Random();
				var count = rnd.Next(4);
				if (count > 0)
				{
					return new[]
					{
						ItemFactory.GetItem(457, 0, 1),
						ItemFactory.GetItem(458, 0, (byte) count)
					};
				}
				return new[] {ItemFactory.GetItem(457, 0, 1)};
			}

			return new[] {ItemFactory.GetItem(458, 0, 1)};
		}
	}
}