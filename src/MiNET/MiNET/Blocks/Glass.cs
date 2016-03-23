namespace MiNET.Blocks
{
	public class Glass : Block
	{
		public Glass() : base(20)
		{
			IsTransparent = true;
			BlastResistance = 1.5f;
			Hardness = 0.3f;
		}
	}
}