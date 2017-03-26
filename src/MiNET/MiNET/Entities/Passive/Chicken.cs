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

			Behaviors.Add(new PanicBehavior(60, Speed, 1.4));
			Behaviors.Add(new StrollBehavior(60, Speed, 0.7));
			Behaviors.Add(new LookAtPlayerBehavior());
			Behaviors.Add(new RandomLookaroundBehavior());
		}

		public override Item[] GetDrops()
		{
			return new[]
			{
				ItemFactory.GetItem(365)
			};
		}
	}
}