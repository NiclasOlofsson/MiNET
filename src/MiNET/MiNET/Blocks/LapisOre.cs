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

		public override Item GetDrops()
		{
			// Random between 4-8
			var rnd = new Random((int)DateTime.UtcNow.Ticks);
			var plus = rnd.Next(4);
			return ItemFactory.GetItem(341, 0, (byte)(4 + plus));
		}
	}
}