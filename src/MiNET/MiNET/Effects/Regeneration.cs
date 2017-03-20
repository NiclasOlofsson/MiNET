using log4net;

namespace MiNET.Effects
{
	public class Regeneration : Effect
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Regeneration));

		public Regeneration() : base(EffectType.Regeneration)
		{
		}


		public override void OnTick(Player player)
		{
			if (Duration%(Level == 1 ? 25 : 50) == 0)
			{
				player.HealthManager.Regen(1);
			}

			base.OnTick(player);
		}
	}
}