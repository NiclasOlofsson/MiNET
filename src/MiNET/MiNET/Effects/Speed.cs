using log4net;

namespace MiNET.Effects
{
	public class Speed : Effect
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Speed));

		public Speed() : base(EffectType.Speed)
		{
		}

		public override void SendAdd(Player player)
		{
			player.MovementSpeed = (float) (0.1 + (Level + 1)*0.01);
			player.SendUpdateAttributes();

			base.SendAdd(player);
		}

		public override void SendUpdate(Player player)
		{
			player.MovementSpeed = (float) (0.1 + (Level + 1)*0.01);
			player.SendUpdateAttributes();

			base.SendUpdate(player);
		}

		public override void SendRemove(Player player)
		{
			player.MovementSpeed = 0.1f;
			player.SendUpdateAttributes();

			base.SendRemove(player);
		}

		public override void OnTick(Player player)
		{
			player.MovementSpeed = (float) (0.1 + (Level + 1)*0.01);
			player.SendUpdateAttributes();

			base.OnTick(player);
		}
	}
}