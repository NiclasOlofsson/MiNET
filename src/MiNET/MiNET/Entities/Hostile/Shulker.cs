using MiNET.Worlds;

namespace MiNET.Entities.Hostile
{
	public class Shulker : HostileMob
	{
		public Shulker(Level level) : base((int) EntityType.Shulker, level)
		{
			Width = Length = 0.8;
			Height = 1.8;
		}
	}
}