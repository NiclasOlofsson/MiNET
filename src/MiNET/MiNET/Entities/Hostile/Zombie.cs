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
			Height = 1.8;
			NoAi = true;

			Behaviors.Add(new MeeleAttackBehavior(this, 0.7, 40));
			Behaviors.Add(new FindAttackableTargetBehavior(this, 40));
			//Behaviors.Add(new StrollBehavior(this, 60, Speed, 0.7));
			Behaviors.Add(new LookAtPlayerBehavior(this, 8.0));
			Behaviors.Add(new RandomLookaroundBehavior(this));
		}

		//public override void OnTick()
		//{
		//	base.OnTick();

		//	Block block = Level.GetBlock(KnownPosition);
		//	if (!(block is StationaryWater) && !(block is FlowingWater) && block.SkyLight > 7 && (Level.CurrentWorldTime > 450 && Level.CurrentWorldTime < 11615))
		//	{
		//		if (!HealthManager.IsOnFire) HealthManager.Ignite(80);
		//	}
		//	else
		//	{
		//		if (HealthManager.IsOnFire) HealthManager.Ignite(80); // last kick in the butt
		//	}
		//}
	}
}