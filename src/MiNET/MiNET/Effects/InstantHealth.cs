namespace MiNET.Effects
{
	public class InstantHealth : Effect
	{
		public InstantHealth() : base(EffectType.InstantHealth)
		{
		}

		public override void SendAdd(Player player)
		{
			player.HealthManager.Regen(4*(Level + 1));
		}

		public override void SendUpdate(Player player)
		{
		}

		public override void SendRemove(Player player)
		{
		}
	}
}