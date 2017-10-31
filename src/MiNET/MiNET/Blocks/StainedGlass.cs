namespace MiNET.Blocks
{
	public class StainedGlass : Glass
	{
		public StainedGlass() : base(241)
		{
			IsTransparent = true;
			BlastResistance = 1.5f;
			Hardness = 0.3f;
			IsBlockingSkylight = false;
		}
	}
}