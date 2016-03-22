using System;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Wheat : Block
	{
		public Wheat() : base(59)
		{
			IsSolid = false;
			IsTransparent = true;
		}

		public override Item[] GetDrops()
		{
			if (Metadata == 0x7)
			{
				// Can also return 0-3 seeds at random.
				var rnd = new Random((int)DateTime.UtcNow.Ticks);
				var count = rnd.Next(4);
				if (count > 0)
				{
					return new[]
					{
						ItemFactory.GetItem(296, 0, 1),
						ItemFactory.GetItem(295, 0, (byte)count)
					};
				}
				return new[] { ItemFactory.GetItem(296, 0, 1) };

			}

			return new[] {ItemFactory.GetItem(295, 0, 1)};
		}
	}
}