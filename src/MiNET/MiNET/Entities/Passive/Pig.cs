using MiNET.Items;
using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class Pig : PassiveMob, IAgeable
	{
		public Pig(Level level) : base(EntityType.Pig, level)
		{
			Width = Length = 0.9;
			Height = 0.9;
		}

		public bool IsBaby { get; set; }

		public override Item[] GetDrops()
		{
			return new []
			{
				ItemFactory.GetItem(319, 0, 2)
			};
		}
	}
}