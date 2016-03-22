namespace MiNET.Blocks
{
	public class Cactus : Block
	{
		public Cactus() : base(81)
		{
			IsTransparent = true;
			BlastResistance = 2;
			Hardness = 0.4f;
		}
	}
}