namespace MiNET.Blocks
{
	public class Lever : Block
	{
		public Lever() : base(69)
		{
			IsTransparent = true;
			IsSolid = false;
			BlastResistance = 2.5f;
			Hardness = 0.5f;
		}
	}
}