using MiNET.Entities.Behaviors;
using MiNET.Worlds;

namespace MiNET.Entities.Hostile
{
	public class Creeper : HostileMob
	{
		public Creeper(Level level) : base(EntityType.Creeper, level)
		{
			Width = Length = 0.6;
			Height = 1.8;
			NoAi = true;

			Behaviors.Add(new StrollBehavior(this, 60, Speed, 0.7));
			Behaviors.Add(new LookAtPlayerBehavior(this, 8.0));
			Behaviors.Add(new RandomLookaroundBehavior(this));
		}
	}
}