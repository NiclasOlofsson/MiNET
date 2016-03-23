using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public abstract class PassiveMob : Mob
	{
		protected PassiveMob(int entityTypeId, Level level)
			: base(entityTypeId, level)
		{
		}
	}
}