using MiNET.Worlds;

namespace MiNET.Entities.Hostile
{
	public class Husk : HostileMob, IAgeable
	{
		public Husk(Level level) : base((int) EntityType.Husk, level)
		{
			Width = Length = 0.6;
			Height = 1.8;
		}
	}
}