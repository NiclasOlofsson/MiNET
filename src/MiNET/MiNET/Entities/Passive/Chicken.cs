using MiNET.Items;
using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class Chicken : PassiveMob
	{
		public Chicken(Level level) : base(EntityType.Chicken, level)
		{
			Width = Length = 0.4;
			Height = 0.7;
			HealthManager.MaxHealth = 40;
			HealthManager.ResetHealth();
		}

		public override Item[] GetDrops()
		{
			return new []
			{
				ItemFactory.GetItem(365)
			};
		}
	}
}