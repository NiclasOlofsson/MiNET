using System;
using log4net;
using MiNET.Items;

namespace MiNET.Blocks
{
	public class Wheat : Crops
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Wheat));

		public Wheat() : base(59)
		{
		}

		public override Item[] GetDrops(Item tool)
		{
			if (Metadata == 7)
			{
				// Can also return 0-3 seeds at random.
				var rnd = new Random();
				var count = rnd.Next(4);
				if (count > 0)
				{
					return new[]
					{
						ItemFactory.GetItem(296, 0, 1),
						ItemFactory.GetItem(295, 0, (byte) count)
					};
				}
				return new[] {ItemFactory.GetItem(296, 0, 1)};
			}

			return new[] {ItemFactory.GetItem(295, 0, 1)};
		}
	}
}