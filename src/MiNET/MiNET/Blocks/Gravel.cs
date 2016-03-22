using System;
using MiNET.Items;

namespace MiNET.Blocks
{
	public class Gravel : Block
	{
		public Gravel() : base(13)
		{
			BlastResistance = 3;
			Hardness = 0.6f;
		}

		public override Item GetDrops()
		{
			var rnd = new Random((int)DateTime.UtcNow.Ticks);
			if (rnd.NextDouble() <= 0.1)
			{
				return ItemFactory.GetItem(318);
			}

			return base.GetDrops();
		}
	}
}