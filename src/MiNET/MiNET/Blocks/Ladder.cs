namespace MiNET.Blocks
{
	public class Ladder : Block
	{
		public Ladder() : base(65)
		{
			IsTransparent = true;
			BlastResistance = 2;
			Hardness = 0.4f;
		}
	}
}