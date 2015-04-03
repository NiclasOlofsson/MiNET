using MiNET.Worlds;

namespace MiNET.Entities
{
	public class Egg : Throwable
	{
		public Egg(Player shooter, Level level) : base(shooter, 82, level)
		{
			Width = 0.25;
			Length = 0.25;
			Height = 0.25;

			Gravity = 0.03;
			Drag = 0.01;

			HealthManager.IsInvulnerable = true;
		}
	}
}