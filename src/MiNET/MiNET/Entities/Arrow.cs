using MiNET.Worlds;

namespace MiNET.Entities
{
	public class Arrow : Projectile
	{
		public Arrow(Player shooter, Level level, bool isCritical = false) : base(shooter, 80, level, 2, isCritical)
		{
			Width = 0.5;
			Length = 0.5;
			Height = 0.0;

			Gravity = 0.05;
			Drag = 0.01;

			//OK: Drag = 0.0083;

			HealthManager.IsInvulnerable = true;
			Ttl = 1200;
			DespawnOnImpact = false;
		}
	}
}