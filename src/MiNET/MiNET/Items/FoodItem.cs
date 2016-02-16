using System;

namespace MiNET.Items
{
	public abstract class FoodItem : Item
	{
		public int FoodPoints { get; set; }
		public double SaturationRestore { get; set; }

		public FoodItem(short id, short metadata, int foodPoints, double saturationRestore) : base(id, metadata)
		{
			FoodPoints = foodPoints;
			SaturationRestore = saturationRestore;
		}

		public virtual void Consume(Player player)
		{
			player.HungerManager.IncreaseFoodAndSaturation(this, FoodPoints, SaturationRestore);
			//var healthManager = player.HealthManager;
			//healthManager.Health += FoodPoints*10;
			//healthManager.Health = Math.Min(healthManager.Health, 200);
			//player.SendUpdateAttributes();
		}
	}
}