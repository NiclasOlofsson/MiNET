using MiNET.Items;
using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class Sheep : PassiveMob, IAgeable
	{
		public Sheep(Level level) : base(EntityType.Sheep, level)
		{
			Width = Length = 0.9;
			Height = 1.3;
			HealthManager.MaxHealth = 80;
			HealthManager.ResetHealth();
		}

		public bool IsBaby { get; set; }

		public override Item[] GetDrops()
		{
			return new []
			{
				ItemFactory.GetItem(35, 0, 2)
			};
		}
	}
}