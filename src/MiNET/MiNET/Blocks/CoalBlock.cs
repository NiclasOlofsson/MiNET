namespace MiNET.Blocks
{
	public class CoalBlock : Block
	{
		public CoalBlock() : base(173)
		{
			FuelEfficiency = 800;
			BlastResistance = 30;
			Hardness = 5;
			IsFlammable = true;
		}
	}
}
