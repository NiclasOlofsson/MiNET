namespace MiNET.Blocks
{
	public class Beacon : Block
	{
		public Beacon() : base(138)
		{
			LightLevel = 15;
			BlastResistance = 15;
			Hardness = 3;
		}
	}
}