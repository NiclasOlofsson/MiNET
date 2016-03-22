using System;
using MiNET.Items;

namespace MiNET.Blocks
{
	public class RedstoneOre : Block
	{
		public RedstoneOre() : this(73)
		{
		}

		public RedstoneOre(byte id) : base(id)
		{
			BlastResistance = 15;
			Hardness = 3;
		}

		public override Item GetDrops()
		{
			var rnd = new Random((int)DateTime.UtcNow.Ticks);
			return ItemFactory.GetItem(331, 0, (byte)(4 + rnd.Next(1)));
		}
	}
}