using System;
using System.Diagnostics;

namespace MiNET
{
	public class HealthManager
	{
		public Player Player { get; set; }
		public int Health { get; set; }
		public short Air { get; set; }
		public bool IsDead { get; set; }
		public int FireTick { get; set; }
		public bool IsOnFire { get; set; }

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
			Debug.WriteLine("Killing player: " + Player.Username + " IsDead: " + IsDead);
			if (IsDead) return;

			IsDead = true;
			Health = 0;
			Player.SendSetHealth(Health);
			Player.BroadcastEntityEvent();
			Player.BroadcastSetEntityData();
			Player.Kill();
		}

		public void ResetHealth()
		{
			Health = 20;
			Air = 300;
			IsOnFire = false;
			FireTick = 0;
			IsDead = false;
		}

		public void OnTick()
		{
			if (IsDead) return;

			if (Health <= 0)
			{
				KillPlayer();
				return;
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
						Player.BroadcastEntityEvent();
						Player.BroadcastSetEntityData();
					}
				}

				if (IsOnFire)
				{
					IsOnFire = false;
					FireTick = 0;
					Player.BroadcastSetEntityData();
				}
			}
			else
			{
				Air = 300;
			}

			if (!IsOnFire && IsInLava(Player.KnownPosition))
			{
				FireTick = 300;
				IsOnFire = true;
				Player.BroadcastSetEntityData();
			}

			if (IsOnFire)
			{
				FireTick--;
                if (FireTick <= 0)
                {
                    IsOnFire = false;
                }

				if (Math.Abs(FireTick)%25 == 0)
				{
					Player.SendSetHealth(--Health);
					Player.BroadcastEntityEvent();
					Player.BroadcastSetEntityData();
				}
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

			if (block != null && block.Id == 8 || block != null && block.Id == 9)
			{
				return y < Math.Floor(y) + 1 - ((1/9) - 0.1111111);
			}

			return false;
		}

		private bool IsInLava(PlayerPosition3D playerPosition)
		{
			var block = Player.Level.GetBlock(playerPosition);

			if (block != null && block.Id == 10 || block != null && block.Id == 11)
			{
				return playerPosition.Y < Math.Floor(playerPosition.Y) + 1 - ((1/9) - 0.1111111);
			}

			return false;
		}
	}
}