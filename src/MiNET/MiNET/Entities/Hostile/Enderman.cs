using MiNET.Worlds;
using MiNET.Items;

namespace MiNET.Entities.Hostile
{
	public class Enderman : HostileMob
	{
		public Enderman(Level level) : base(EntityType.Enderman, level)
		{
			Width = Length = 0.6;
			Height = 2.9;
			HealthManager.MaxHealth = 400;
			HealthManager.ResetHealth();
		}
	}
}