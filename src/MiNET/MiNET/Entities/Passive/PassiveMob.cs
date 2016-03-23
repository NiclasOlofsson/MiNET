using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public abstract class PassiveMob : Mob
	{
		protected PassiveMob(EntityType type, Level level)
			: base(type, level)
		{
		}
	}
}