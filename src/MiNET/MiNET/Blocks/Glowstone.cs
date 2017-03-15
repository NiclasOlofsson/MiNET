using System;
using MiNET.Items;

namespace MiNET.Blocks
{
	public class Glowstone : Block
	{
		public Glowstone() : base(89)
		{
			//IsTransparent = true;
			LightLevel = 15;
			BlastResistance = 1.5f;
			Hardness = 0.3f;
		}

		public override Item[] GetDrops(Item tool)
		{
			var rnd = new Random((int)DateTime.UtcNow.Ticks);
			return new[] {ItemFactory.GetItem(348, 0, (byte) (2 + rnd.Next(2)))};
		}
	}
}