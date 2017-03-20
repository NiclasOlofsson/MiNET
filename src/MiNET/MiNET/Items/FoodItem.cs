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
		}
	}
}