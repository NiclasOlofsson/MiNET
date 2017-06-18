using MiNET.Worlds;

namespace MiNET.Entities.Hostile
{
	public class Vindicator : HostileMob
	{
		public Vindicator(Level level) : base(EntityType.Vindicator, level)
		{
			Width = Length = 0.6;
			Height = 1.95;
			HealthManager.MaxHealth = 240;
			HealthManager.ResetHealth();
		}
	}
}
