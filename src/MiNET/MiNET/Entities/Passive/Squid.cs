using MiNET.Items;
using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class Squid : PassiveMob
	{
		public Squid(Level level) : base(EntityType.Squid, level)
		{
			Width = Length = 0.95;
			Height = 0.95;
			HealthManager.MaxHealth = 100;
			HealthManager.ResetHealth();
		}

		public override Item[] GetDrops()
		{
			return new[]
			{
				ItemFactory.GetItem(351)
			};
		}
	}
}