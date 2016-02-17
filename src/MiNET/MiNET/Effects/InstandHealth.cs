namespace MiNET.Effects
{
	public class InstandHealth : Effect
	{
		public InstandHealth() : base(EffectType.InstandHealth)
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