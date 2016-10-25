using MiNET.Worlds;

namespace MiNET.Entities.Hostile
{
	public class Stray : HostileMob, IAgeable
	{
		public Stray(Level level) : base((int) EntityType.Stray, level)
		{
			Width = Length = 0.6;
			Height = 1.8;
		}
	}
}