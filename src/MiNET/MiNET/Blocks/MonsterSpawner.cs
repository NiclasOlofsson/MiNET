using System;
using MiNET.Items;

namespace MiNET.Blocks
{
	public class MonsterSpawner : Block
	{
		public MonsterSpawner() : base(52)
		{
			IsTransparent = true; // Doesn't block light
			LightLevel = 1;
			BlastResistance = 25;
			Hardness = 5;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new Item[0];
		}

		public override float GetExperiencePoints()
		{
			Random random = new Random();
			return random.Next(15, 44);
		}
	}
}