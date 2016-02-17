namespace MiNET.Effects
{
	public class Invisibility : Effect
	{
		public Invisibility() : base(EffectType.Invisibility)
		{
		}

		public override void SendAdd(Player player)
		{
			player.IsInvisible = true;
			player.HideNameTag = true;
			player.SendSetEntityData();

			base.SendAdd(player);
		}

		public override void SendUpdate(Player player)
		{
			player.IsInvisible = true;
			player.HideNameTag = true;
			player.SendSetEntityData();

			base.SendUpdate(player);
		}

		public override void SendRemove(Player player)
		{
			player.IsInvisible = false;
			player.HideNameTag = false;
			player.SendSetEntityData();

			base.SendRemove(player);
		}
	}
}