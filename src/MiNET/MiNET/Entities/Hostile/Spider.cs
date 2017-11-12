using MiNET.Entities.Behaviors;
using MiNET.Worlds;
using MiNET.Items;

namespace MiNET.Entities.Hostile
{
	public class Spider : HostileMob
	{
		public Spider(Level level) : base((int) EntityType.Spider, level)
		{
			Width = Length = 1.4;
			Height = 0.9;
			NoAi = true;
			Speed = 0.3;

			HealthManager.MaxHealth = 160;
			HealthManager.ResetHealth();

			AttackDamage = 3;

			Behaviors.Add(new WanderBehavior(this, 0.8));
			Behaviors.Add(new LookAtPlayerBehavior(this, 8.0));
			Behaviors.Add(new RandomLookaroundBehavior(this));
		}

		public override Item[] GetDrops()
		{
			return base.GetDrops();
		}

	}
}