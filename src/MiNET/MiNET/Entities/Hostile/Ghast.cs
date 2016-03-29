using MiNET.Worlds;
using MiNET.Items;

namespace MiNET.Entities.Hostile
{
	public class Ghast : HostileMob
	{
		public Ghast(Level level) : base(EntityType.Ghast, level)
		{
			Width = Length = 4.0;
			Height = 4.0;
			HealthManager.MaxHealth = 100;
			HealthManager.ResetHealth();
		}
	}
}