using MiNET.Effects;

namespace MiNET.Items
{
	public class ItemGoldenApple : FoodItem
	{
		public ItemGoldenApple() : base(322, 0, 4, 9.6)
		{
		}

		public override void Consume(Player player)
		{
			base.Consume(player);
			player.SetEffect(new Absorption() {Duration = 2400});
			player.SetEffect(new Regeneration() { Duration = 100, Level = 1 });
		}
	}
}