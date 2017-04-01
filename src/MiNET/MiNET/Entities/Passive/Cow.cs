using MiNET.Entities.Behaviors;
using MiNET.Items;
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

			Behaviors.Add(new PanicBehavior(this, 60, Speed, 2.0));
			Behaviors.Add(new TemptedBehavior(this, typeof(ItemWheat), 10, 1.25));
			Behaviors.Add(new StrollBehavior(this, 60, Speed, 0.7));
			Behaviors.Add(new LookAtPlayerBehavior(this));
			Behaviors.Add(new RandomLookaroundBehavior(this));
		}

		public override Item[] GetDrops()
		{
			return new[]
			{
				ItemFactory.GetItem(363, 0, 2),
				ItemFactory.GetItem(334, 0, 2)
			};
		}
	}
}