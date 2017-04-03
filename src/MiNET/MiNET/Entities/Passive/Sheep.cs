using System;
using MiNET.Entities.Behaviors;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class Sheep : PassiveMob, IAgeable
	{
		public Sheep(Level level) : base(EntityType.Sheep, level)
		{
			Width = Length = 0.9;
			Height = 1.3;
			HealthManager.MaxHealth = 80;
			HealthManager.ResetHealth();

			Behaviors.Add(new PanicBehavior(this, 60, Speed, 1.25));
			Behaviors.Add(new TemptedBehavior(this, typeof (ItemWheat), 10, 1.1));
			Behaviors.Add(new EatBlockBehavior(this));
			Behaviors.Add(new StrollBehavior(this, 60, Speed, 0.7));
			Behaviors.Add(new LookAtPlayerBehavior(this));
			Behaviors.Add(new RandomLookaroundBehavior(this));
		}

		public override MetadataDictionary GetMetadata()
		{
			var metadata = base.GetMetadata();
			metadata[16] = new MetadataInt(32);
			return metadata;
		}

		public override Item[] GetDrops()
		{
			Random random = new Random();
			return new[]
			{
				ItemFactory.GetItem(35, 0, 1),
				ItemFactory.GetItem(423, 0, random.Next(1, 3)),
			};
		}
	}
}