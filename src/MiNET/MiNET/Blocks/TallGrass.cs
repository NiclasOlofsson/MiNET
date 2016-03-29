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
			// 1/24 chance of dropping seeds according to the wiki
			var rnd = new Random((int) DateTime.UtcNow.Ticks);
			if (rnd.NextDouble() <= 0.041)
			{
				return new[] {ItemFactory.GetItem(295)};
			}

			return null;
		}
	}
}