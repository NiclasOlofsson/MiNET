using MiNET.Worlds;

namespace MiNET.Entities.Hostile
{
	public class Skeleton : HostileMob
	{
		public Skeleton(Level level) : base((int) EntityType.Skeleton, level)
		{
			Width = Length = 0.6;
			Height = 1.95;
		}
	}
}