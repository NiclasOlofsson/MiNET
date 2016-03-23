using MiNET.Items;
using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class IronGolem : PassiveMob
	{
		public IronGolem(Level level) : base((int) EntityType.IronGolem, level)
		{
			Width = Length = 1.4;
			Height = 2.9;
		}

		public override Item[] GetDrops()
		{
			return new[]
			{
				ItemFactory.GetItem(265, 0, 4)
			};
		}
	}
}