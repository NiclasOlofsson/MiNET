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

			Behaviors.Add(new PanicBehavior(60, Speed, 1.25));
			Behaviors.Add(new StrollBehavior(60, Speed, 0.7));
			Behaviors.Add(new LookAtPlayerBehavior());
			Behaviors.Add(new RandomLookaroundBehavior());
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