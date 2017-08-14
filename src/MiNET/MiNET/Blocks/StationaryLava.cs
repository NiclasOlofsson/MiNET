namespace MiNET.Blocks
{
	public class StationaryLava : Stationary
	{
		public StationaryLava() : base(11)
		{
			LightLevel = 15;
			BlastResistance = 500;
			Hardness = 100;
		}
	}
}