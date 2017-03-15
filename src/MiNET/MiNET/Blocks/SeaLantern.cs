using System;
using MiNET.Items;

namespace MiNET.Blocks
{
	public class SeaLantern : Block
	{
		public SeaLantern() : base(169)
		{
			LightLevel = 15;
			BlastResistance = 1.5f;
			Hardness = 0.3f;
		}

		public override Item[] GetDrops(Item tool)
		{
			var rnd = new Random((int)DateTime.UtcNow.Ticks);
			return new[] { ItemFactory.GetItem(409, 0, (byte)(rnd.Next(2, 3))) }; //drop prismarine_shard
		}
	}
}
