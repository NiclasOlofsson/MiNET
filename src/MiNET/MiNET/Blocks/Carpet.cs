namespace MiNET.Blocks
{
	public class Carpet : Block
	{
		public Carpet() : base(171)
		{
			IsTransparent = true;
			BlastResistance = 0.5f;
			Hardness = 0.1f;
			IsFlammable = true;
		}
	}
}