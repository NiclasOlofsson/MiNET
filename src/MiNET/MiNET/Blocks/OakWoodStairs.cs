namespace MiNET.Blocks
{
	public class OakWoodStairs : BlockStairs
	{
		public OakWoodStairs() : base(53)
		{
			IsTransparent = true; // Partial - Blocks light.
			BlastResistance = 15;
			Hardness = 2;
			IsFlammable = true;
		}
	}
}