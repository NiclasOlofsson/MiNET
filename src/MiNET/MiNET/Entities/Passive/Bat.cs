using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class Bat : PassiveMob
	{
		public Bat(Level level) : base((int) EntityType.Bat, level)
		{
			Width = Length = 0.5;
			Height = 0.9;
		}
	}
}