using MiNET.Items;
using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class Ocelot : PassiveMob
	{
		public Ocelot(Level level) : base(EntityType.Ocelot, level)
		{
			Width = Length = 0.6;
			Height = 0.8;
		}
	}
}