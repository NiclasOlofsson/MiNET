using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class PolarBear : PassiveMob, IAgeable
	{
		public PolarBear(Level level) : base(EntityType.PolarBear, level)
		{
			Width = Length = 1.3;
			Height = 1.4;
			HealthManager.MaxHealth = 100;
			HealthManager.ResetHealth();
		}
	}
}