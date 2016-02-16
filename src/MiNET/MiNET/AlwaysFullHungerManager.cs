using MiNET.Items;

namespace MiNET
{
	public class AlwaysFullHungerManager : HungerManager
	{
		public AlwaysFullHungerManager(Player player) : base(player)
		{
		}

		public override void IncreaseExhaustion(float amount)
		{
		}

		public override void IncreaseFoodAndSaturation(Item item, int foodPoints, double saturationRestore)
		{
		}

		public override void Move(double distance)
		{
		}
	}
}