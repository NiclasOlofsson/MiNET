using MiNET.Worlds;
using MiNET.Items;

namespace MiNET.Entities.Hostile
{
	public class CaveSpider : HostileMob
	{
		public CaveSpider(Level level) : base(EntityType.CaveSpider, level)
		{
			Width = Length = 0.7;
			Height = 0.5;
			HealthManager.MaxHealth = 120;
			HealthManager.ResetHealth();
		}
	}
}