using MiNET.Worlds;
using MiNET.Items;

namespace MiNET.Entities.Hostile
{
	public class Spider : HostileMob
	{
		public Spider(Level level) : base((int) EntityType.Spider, level)
		{
			Width = Length = 1.4;
			Height = 0.9;
			HealthManager.MaxHealth = 160;
			HealthManager.ResetHealth();
		}

		public override Item[] GetDrops()
		{
			return base.GetDrops();
		}

	}
}