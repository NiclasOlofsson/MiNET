namespace MiNET
{
	public class HealthManager
	{
		public Player Player { get; set; }
		public int Health { get; set; }

		public HealthManager(Player player, int health = 20)
		{
			Player = player;
			Health = health;
		}


		public void TakeHit(Player sourcePlayer)
		{
			//TODO: damage and armour
			Health--;
			if (Health == 0)
			{
				Die();
			}
			else
			{
				Player.SendSetHealth(Health);
			}
		}

		public void Die()
		{
			Health = 0;
			Player.SendSetHealth(Health);
			Player.Die();
		}

		public void Reset()
		{
			Health = 20;
		}
	}
}