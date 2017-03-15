using System;
using MiNET.Items;

namespace MiNET.Blocks
{
	public class ChorusPlant : Block
	{
		public ChorusPlant() : base(240)
		{
			IsTransparent = true;
			BlastResistance = 2;
		}

		public override Item[] GetDrops(Item tool)
		{
			var rnd = new Random((int)DateTime.UtcNow.Ticks);
			if (rnd.Next(2) > 0) // Note that random.Next EXCLUDES the parameter so this is 50/50
			{
				return new Item[] { ItemFactory.GetItem(432, 0, 1)}; // Chorus Fruit
			}

			return new Item[0];
		}
	}
}