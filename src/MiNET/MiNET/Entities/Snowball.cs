using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities
{
	public class Snowball : Entity
	{
		public Player Player { get; set; }

		public Snowball(Player player, Level level) : base(81, level)
		{
			Player = player;
			Width = 0.25;
			Length = 0.25;
			Height = 0.25;

			Gravity = 0.03;
			Drag = 0.01;
		}


		public override void OnTick()
		{
			if (KnownPosition.Y <= 0)
			{
				DespawnEntity();
				return;
			}

			Velocity *= (1.0 - Drag);
			Velocity -= new Vector3(0, Gravity, 0);

			KnownPosition.X += (float) Velocity.X;
			KnownPosition.Y += (float) Velocity.Y;
			KnownPosition.Z += (float) Velocity.Z;

			var bbox = GetBoundingBox();

			Player[] players = Level.GetSpawnedPlayers();
			foreach (var player in players)
			{
				if (player == Player) continue;

				if (player.GetBoundingBox().Intersects(bbox))
				{
					player.HealthManager.TakeHit(this, 1, DamageCause.Projectile);
					player.BroadcastEntityEvent();
					DespawnEntity();
					break;
				}
			}
		}
	}
}