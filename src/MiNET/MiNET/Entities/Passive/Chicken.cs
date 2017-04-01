using System;
using MiNET.Entities.Behaviors;
using MiNET.Items;
using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class Chicken : PassiveMob
	{
		public Chicken(Level level) : base(EntityType.Chicken, level)
		{
			Width = Length = 0.4;
			Height = 0.7;
			HealthManager.MaxHealth = 40;
			Drag = 0.2;
			HealthManager.ResetHealth();

			Behaviors.Add(new PanicBehavior(this, 60, Speed, 1.4));
			Behaviors.Add(new TemptedBehavior(this, typeof(ItemWheatSeeds), 10, 1.0));
			Behaviors.Add(new StrollBehavior(this, 60, Speed, 0.7));
			Behaviors.Add(new LookAtPlayerBehavior(this));
			Behaviors.Add(new RandomLookaroundBehavior(this));
		}

		public override Item[] GetDrops()
		{
			Random random = new Random();
			return new[]
			{
				ItemFactory.GetItem(365),
				ItemFactory.GetItem(288, 0, random.Next(1, 3)),
			};
		}
	}
}