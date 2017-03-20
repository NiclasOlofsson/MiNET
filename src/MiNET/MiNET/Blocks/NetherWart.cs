using System;
using MiNET.Items;

namespace MiNET.Blocks
{
	public class NetherWart : Block
	{
		public NetherWart() : base(115)
		{
			IsTransparent = true;
			IsSolid = false;
		}

		public override Item[] GetDrops(Item tool)
		{
			if (Metadata == 3)
			{
				var rnd = new Random((int)DateTime.UtcNow.Ticks);
				return new[] {ItemFactory.GetItem(372, 0, (byte)(2 + rnd.Next(3)))};
			}

			return new[] {ItemFactory.GetItem(372, 0, 1)};
		}
	}
}