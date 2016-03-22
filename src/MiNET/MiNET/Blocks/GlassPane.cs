namespace MiNET.Blocks
{
	public class GlassPane : Block
	{
		public GlassPane() : base(102)
		{
			IsTransparent = true;
			BlastResistance = 1.5f;
			Hardness = 0.3f;
		}
	}
}