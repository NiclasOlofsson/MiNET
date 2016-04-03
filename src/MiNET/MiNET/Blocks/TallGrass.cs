using System;
using MiNET.Items;

namespace MiNET.Blocks
{
	public class TallGrass : Block
	{
		public enum TallGrassTypes
		{
			DeadShrub = 0,
			TallGrass = 1,
			Fern = 2
		}

		public TallGrass() : base(31)
		{
			IsSolid = false;
			IsReplacible = true;
			IsTransparent = true;
		}

		public override Item[] GetDrops()
		{
			// 50% chance to drop seeds.
			var rnd = new Random((int) DateTime.UtcNow.Ticks);
			if (rnd.NextDouble() > 0.5)
			{
				return new[] {ItemFactory.GetItem(295)};
			}

			return new Item[0];
		}
	}
}