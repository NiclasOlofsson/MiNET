using MiNET.Worlds;
using MiNET.Items;

namespace MiNET.Entities.Hostile
{
	public class Silverfish : HostileMob
	{
		public Silverfish(Level level) : base((int) EntityType.Silverfish, level)
		{
			Width = Length = 0.4;
			Height = 0.3;
			HealthManager.MaxHealth = 80;
			HealthManager.ResetHealth();
		}
	}
}