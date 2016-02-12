using System;

namespace MiNET.Items
{
	public abstract class FoodItem : Item
	{
		public int FoodPoints { get; set; }

		public FoodItem(short id, short metadata, int foodPoints) : base(id, metadata)
		{
			FoodPoints = foodPoints;
		}

		public virtual void Consume(Player player)
		{
			var healthManager = player.HealthManager;
			healthManager.Health += FoodPoints*10;
			healthManager.Health = Math.Min(healthManager.Health, 200);
			player.SendSetHealth();
		}
	}
}