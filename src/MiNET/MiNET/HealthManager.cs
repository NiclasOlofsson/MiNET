using System;
using System.Diagnostics;

namespace MiNET
{
	public class HealthManager
	{
		public Player Player { get; set; }
		public int Health { get; set; }
		public int Air { get; set; }
		public bool IsDead { get; set; }

		public HealthManager(Player player)
		{
			Player = player;
			ResetHealth();
		}


		public void TakeHit(Player sourcePlayer)
		{
			Player.SendSetHealth(--Health);
		}

		public void KillPlayer()
		{
			IsDead = true;
			Health = 0;
			Player.SendSetHealth(Health);
			Player.Kill();
		}

		public void ResetHealth()
		{
			Health = 20;
			Air = 300;
			IsDead = false;
		}

		public void OnTick()
		{
			if (IsDead) return;

			if (Health <= 0)
			{
				KillPlayer();
			}

			if (IsInWater(Player.KnownPosition))
			{
				Air--;
				Debug.WriteLine("Air: {0}", Air);
				if (Air <= 0)
				{
					if (Math.Abs(Air)%10 == 0)
					{
						Player.SendSetHealth(--Health);
						Player.SendEntityEvent();
					}
				}
			}
			else
			{
				Air = 300;
			}
		}

		private bool IsInWater(PlayerPosition3D playerPosition)
		{
			float y = playerPosition.Y + 1.62f;

			PlayerPosition3D waterPos = new PlayerPosition3D();
			waterPos.X = playerPosition.X;
			waterPos.Y = y;
			waterPos.Z = playerPosition.Z;
			var block = Player.Level.GetBlock(waterPos);

			if (block != null && block.Id == 8)
			{
				return y < Math.Floor(y) + 1 - ((1/9) - 0.1111111);
			}

			return false;
		}
	}
}