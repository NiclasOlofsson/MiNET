using MiNET.Worlds;

namespace MiNET.Entities.Hostile
{
	public class Zombie : HostileMob, IAgeable
	{
		public Zombie(Level level) : base((int) EntityType.Zombie, level)
		{
			Width = Length = 0.6;
			Height = 1.8;
			NoAi = true;
			Drag = 0.09;
			Gravity = 0.02;
		}
	}
}