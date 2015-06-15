using MiNET.Worlds;

namespace MiNET.Entities
{
	public class Arrow : Throwable
	{
		public Arrow(Player shooter, Level level) : base(shooter, 80, level)
		{
			Width = 0.5;
			Length = 0.5;
			Height = 0.0;

			Gravity = 0.05;
			Drag = 0.01;

			HealthManager.IsInvulnerable = true;
			Ttl = 1200;
			DespawnOnImpact = false;
		}
	}
}