using MiNET.Worlds;

namespace MiNET.Entities.Projectiles
{
	public class Egg : Projectile
	{
		public Egg(Player shooter, Level level) : base(shooter, 82, level, 0)
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