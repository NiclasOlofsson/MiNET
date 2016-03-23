namespace MiNET.Blocks
{
	public class StoneSlab : Block
	{
		public StoneSlab() : base(44)
		{
			BlastResistance = 30;
			Hardness = 2;
			IsTransparent = true; // Partial
		}
	}
}