using MiNET.Items;
using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class Sheep : PassiveMob, IAgeable
	{
		public Sheep(Level level) : base((int) EntityType.Sheep, level)
		{
			Width = Length = 0.9;
			Height = 1.3;
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