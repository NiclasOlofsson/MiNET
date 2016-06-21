namespace MiNET.Effects
{
	public class Speed : Effect
	{
		private double _multiplier = 0.02;

		public Speed() : base(EffectType.Speed)
		{
			Particles = false;
		}

		public override void SendAdd(Player player)
		{
			player.MovementSpeed = (float) (0.1 + (Level + 1)*_multiplier);
			player.SendUpdateAttributes();

			base.SendAdd(player);
		}

		public override void SendUpdate(Player player)
		{
			player.MovementSpeed = (float) (0.1 + (Level + 1)*_multiplier);
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