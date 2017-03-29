namespace MiNET.Effects
{
	public class Absorption : Effect
	{
		public Absorption() : base(EffectType.Absorption)
		{
		}

		public override void SendAdd(Player player)
		{
			base.SendAdd(player);
			player.HealthManager.Absorption = 4 * (Level + 1);
			player.SendUpdateAttributes();
		}

		public override void OnTick(Player player)
		{
			if(player.HealthManager.Absorption > 0)
			{
				base.OnTick(player);
			}
			else
			{
				player.RemoveEffect(this);
			}
		}

		public override void SendRemove(Player player)
		{
			base.SendRemove(player);
			player.HealthManager.Absorption = 0;
			player.SendUpdateAttributes();
		}
	}
}