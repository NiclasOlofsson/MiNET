namespace MiNET.Effects
{
	public class InstantDamage : Effect
	{
		public InstantDamage() : base(EffectType.InstantDamage)
		{
		}

		public override void SendAdd(Player player)
		{
			player.HealthManager.TakeHit(null, 6*(Level + 1), DamageCause.Magic);
		}

		public override void SendUpdate(Player player)
		{
		}

		public override void SendRemove(Player player)
		{
		}
	}
}