using MiNET.Blocks;
using MiNET.Entities.Behaviors;
using MiNET.Worlds;

namespace MiNET.Entities.Hostile
{
	public class Zombie : HostileMob, IAgeable
	{
		public Zombie(Level level) : base((int) EntityType.Zombie, level)
		{
			Width = Length = 0.6;
			Height = 1.95;
			NoAi = true;
			Speed = 0.23;

			Behaviors.Add(new MeeleAttackBehavior(this, 1.0, 40));
			Behaviors.Add(new FindAttackableTargetBehavior(this, 40));
			Behaviors.Add(new StrollBehavior(this, 60, Speed, 1.0));
			Behaviors.Add(new LookAtPlayerBehavior(this, 8.0));
			Behaviors.Add(new RandomLookaroundBehavior(this));
		}

		public override void OnTick()
		{
			base.OnTick();

			Block block = Level.GetBlock(KnownPosition);
			if (!(block is StationaryWater) && !(block is FlowingWater) && block.SkyLight > 7 && (Level.CurrentWorldTime < 12566 || Level.CurrentWorldTime > 23450))
			{
				if (!HealthManager.IsOnFire) HealthManager.Ignite(80);
			}
			else
			{
				if (HealthManager.IsOnFire) HealthManager.Ignite(80); // last kick in the butt
			}
		}
	}
}