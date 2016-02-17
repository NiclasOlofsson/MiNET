namespace MiNET.Effects
{
	public class Slowness : Effect
	{
		private double _multiplier = 0.015;

		public Slowness() : base(EffectType.Slowness)
		{
		}

		public override void SendAdd(Player player)
		{
			player.MovementSpeed = (float) (0.1 - (Level + 1)*_multiplier);
			player.SendUpdateAttributes();

			base.SendAdd(player);
		}

		public override void SendUpdate(Player player)
		{
			player.MovementSpeed = (float) (0.1 - (Level + 1)*_multiplier);
			player.SendUpdateAttributes();

			base.SendUpdate(player);
		}

		public override void SendRemove(Player player)
		{
			player.MovementSpeed = 0.1f;
			player.SendUpdateAttributes();

			base.SendRemove(player);
		}
	}
}