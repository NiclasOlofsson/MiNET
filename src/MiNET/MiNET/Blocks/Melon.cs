using System;
using MiNET.Items;

namespace MiNET.Blocks
{
	public class Melon : Block
	{
		public Melon() : base(103)
		{
			BlastResistance = 5;
			Hardness = 1;
		}

		public override Item[] GetDrops()
		{
			var rnd = new Random((int)DateTime.UtcNow.Ticks);
			return new[] {ItemFactory.GetItem(360, 0, (byte) (3 + rnd.Next(5)))};
		}
	}
}