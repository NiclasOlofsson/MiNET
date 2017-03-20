using MiNET.Worlds;

namespace MiNET.Entities.Projectiles
{
	public class Arrow : Projectile
	{
		public Arrow(Player shooter, Level level, int damage = 2, bool isCritical = false) : base(shooter, 80, level, damage, isCritical)
		{
			Width = 0.15;
			Length = 0.15;
			Height = 0.15;

			Gravity = 0.05;
			Drag = 0.01;

			//OK: Drag = 0.0083;

			HealthManager.IsInvulnerable = true;
			Ttl = 1200;
			DespawnOnImpact = false;
		}
	}
}