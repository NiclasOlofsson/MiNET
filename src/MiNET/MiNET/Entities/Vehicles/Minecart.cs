using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities.Vehicles
{
	public class Minecart : Vehicle
	{
		public Minecart(Level level, PlayerLocation position) : base(EntityType.Minecart, level)
		{
			KnownPosition = position;
		}

        protected Minecart(EntityType type, Level level, PlayerLocation position) : base(type, level)
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
