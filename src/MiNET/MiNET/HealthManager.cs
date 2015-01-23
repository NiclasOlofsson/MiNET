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
			Player.SendSetHealth(--Health);

			//TODO: damage and armour
			if (Health <= 0)
			{
				Die();
			}
		}

		public void Die()
		{
			Health = 0;
			Player.Die();
		}

		public void Reset()
		{
			Health = 20;
		}
	}
}