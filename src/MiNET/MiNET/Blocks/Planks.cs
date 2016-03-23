namespace MiNET.Blocks
{
	public class Planks : Block
	{
		public Planks() : base(5)
		{
			FuelEfficiency = 15;
			BlastResistance = 15;
			Hardness = 2;
			IsFlammable = true;
		}
	}
}
