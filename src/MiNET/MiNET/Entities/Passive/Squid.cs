using MiNET.Items;
using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class Squid : PassiveMob
	{
		public Squid(Level level) : base((int) EntityType.Squid, level)
		{
			Width = Length = 0.95;
			Height = 0.95;
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