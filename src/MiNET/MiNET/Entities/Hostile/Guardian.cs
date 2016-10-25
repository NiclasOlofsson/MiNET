using MiNET.Worlds;

namespace MiNET.Entities.Hostile
{
	public class Guardian : HostileMob, IAgeable
	{
		public Guardian(Level level) : base((int) EntityType.Guardian, level)
		{
			Width = Length = 0.7;
			Height = 2.4;
		}
	}
}