namespace MiNET.Blocks
{
	public class HayBlock : Block
	{
		public HayBlock() : base(170)
		{
			BlastResistance = 2.5f;
			Hardness = 0.5f;
			IsFlammable = true;
		}
	}
}