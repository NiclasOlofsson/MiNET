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

		public override Item[] GetDrops(Item tool)
		{
			if (tool.ItemMaterial < ItemMaterial.Iron) return new Item[0];

			var rnd = new Random((int) DateTime.UtcNow.Ticks);
			return new[] {ItemFactory.GetItem(331, 0, (byte) (4 + rnd.Next(1)))};
		}

		public override float GetExperiencePoints()
		{
			Random random = new Random();
			return random.Next(1, 6);
		}
	}
}