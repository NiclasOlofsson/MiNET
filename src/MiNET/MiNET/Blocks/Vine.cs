namespace MiNET.Blocks
{
	public class Vine : Block
	{
		public Vine() : base(106)
		{
			IsSolid = false;
			IsTransparent = false;
			BlastResistance = 1;
			Hardness = 0.2f;
			IsFlammable = true;
		}
	}
}