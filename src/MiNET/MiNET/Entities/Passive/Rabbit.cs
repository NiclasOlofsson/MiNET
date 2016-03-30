using MiNET.Items;
using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class Rabbit : PassiveMob
	{
		public Rabbit(Level level) : base(EntityType.Rabbit, level)
		{
			Width = Length = 0.6;
			Height = 0.7;
			HealthManager.MaxHealth = 100;
			HealthManager.ResetHealth();
		}

		public override Item[] GetDrops()
		{
			return new []
			{
				ItemFactory.GetItem(411)
			};
		}
	}
}