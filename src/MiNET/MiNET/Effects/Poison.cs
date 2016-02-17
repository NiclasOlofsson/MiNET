namespace MiNET.Effects
{
	public class Poison : Effect
	{
		public Poison() : base(EffectType.Poison)
		{
		}

		public override void OnTick(Player player)
		{
			if (Duration%(Level == 1 ? 25 : 50) == 0)
			{
				if (player.HealthManager.Health > 12)
				{
					player.HealthManager.TakeHit(null, 2, DamageCause.Magic);
				}
			}

			base.OnTick(player);
		}
	}
}