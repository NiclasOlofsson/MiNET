using System;
using MiNET.Items;

namespace MiNET.Blocks
{
	public class RedMushroomBlock : Block
	{
		public RedMushroomBlock() : base(100)
		{
			BlastResistance = 1;
			Hardness = 0.2f;
		}

		public override Item[] GetDrops(Item tool)
		{
			var rnd = new Random((int)DateTime.UtcNow.Ticks);
			var next = rnd.Next(3);
			if (next > 0)
			{
				return new[] {ItemFactory.GetItem(40, 0, (byte)next)};
			}
			return new Item[0];
		}
	}
}