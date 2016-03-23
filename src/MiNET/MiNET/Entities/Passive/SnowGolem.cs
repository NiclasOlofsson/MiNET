using MiNET.Items;
using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class SnowGolem : PassiveMob
	{
		public SnowGolem(Level level) : base((int) EntityType.SnowGolem, level)
		{
			Width = Length = 0.7;
			Height = 1.9;
		}

		public override Item[] GetDrops()
		{
			return new[]
			{
				ItemFactory.GetItem(332)
			};
		}
	}
}