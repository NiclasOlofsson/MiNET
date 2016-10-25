using MiNET.Worlds;

namespace MiNET.Entities.Hostile
{
	public class WitherSkeleton : HostileMob, IAgeable
	{
		public WitherSkeleton(Level level) : base((int) EntityType.WitherSkeleton, level)
		{
			Width = Length = 0.7;
			Height = 2.4;
		}
	}
}