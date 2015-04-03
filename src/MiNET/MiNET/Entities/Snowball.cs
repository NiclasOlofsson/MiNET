using MiNET.Worlds;

namespace MiNET.Entities
{
	public class Snowball : Throwable
	{
		public Snowball(Player shooter, Level level) : base(shooter, 81, level)
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