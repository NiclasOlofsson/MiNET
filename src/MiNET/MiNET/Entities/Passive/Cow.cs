﻿using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class Cow : PassiveMob, IAgeable
	{
		public Cow(Level level) : base(EntityType.Cow, level)
		{
			Width = Length = 0.9;
			Height = 1.4;
			HealthManager.MaxHealth = 100;
			HealthManager.ResetHealth();
		}

		public bool IsBaby { get; set; }

		public override Item[] GetDrops()
		{
			return new []
			{
				ItemFactory.GetItem(363, 0, 2),
				ItemFactory.GetItem(334, 0, 2)
			};
		}
	}
}