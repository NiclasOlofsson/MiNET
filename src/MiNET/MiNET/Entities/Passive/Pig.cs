using MiNET.Entities.Behaviors;
using MiNET.Items;
using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class Pig : PassiveMob, IAgeable
	{
		public Pig(Level level) : base(EntityType.Pig, level)
		{
			Width = Length = 0.9;
			Height = 0.9;
			HealthManager.MaxHealth = 100;
			HealthManager.ResetHealth();

			Behaviors.Add(new PanicBehavior(this, 60, Speed, 1.25));
			Behaviors.Add(new TemptedBehavior(this, typeof(ItemCarrot), 10, 1.2));
			//Behaviors.Add(new TemptedBehavior(this, typeof(ItemCarrotStick), 10, 1.2));
			Behaviors.Add(new StrollBehavior(this, 60, Speed, 0.7));
			Behaviors.Add(new LookAtPlayerBehavior(this));
			Behaviors.Add(new RandomLookaroundBehavior(this));
		}

		public override Item[] GetDrops()
		{
			return new[]
			{
				ItemFactory.GetItem(319, 0, 2)
			};
		}
	}
}