using System;
using MiNET.Items;

namespace MiNET.Blocks
{
	public class Melon : Block
	{
		public Melon() : base(103)
		{
			Hardness = 1;
			IsTransparent = true;
		}

		public override Item[] GetDrops(Item tool)
		{
			var rnd = new Random((int) DateTime.UtcNow.Ticks);
			return new[] {ItemFactory.GetItem(360, 0, (byte) (3 + rnd.Next(5)))};
		}
	}
}