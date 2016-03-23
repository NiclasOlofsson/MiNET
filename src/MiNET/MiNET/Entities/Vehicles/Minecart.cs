using MiNET.Items;
using MiNET.Utils;

namespace MiNET.Entities.Vehicles
{
	public class Minecart : Vechicle
	{
		public Minecart(MiNET.Worlds.Level level, PlayerLocation position) : base((int) EntityType.Minecart, level)
		{
			KnownPosition = position;
		}

		public override Item[] GetDrops()
		{
			return new[]
			{
				ItemFactory.GetItem(328)
			};
		}
	}
}