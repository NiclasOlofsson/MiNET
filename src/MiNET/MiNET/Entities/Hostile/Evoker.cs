using MiNET.Worlds;

namespace MiNET.Entities.Hostile
{
	public class Evoker : HostileMob
	{
		public Evoker(Level level) : base(EntityType.Evoker, level)
		{
			Width = Length = 0.6;
			Height = 1.95;
			HealthManager.MaxHealth = 240;
			HealthManager.ResetHealth();
		}
	}
}
