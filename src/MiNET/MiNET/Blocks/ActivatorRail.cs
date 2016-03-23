namespace MiNET.Blocks
{
	public class ActivatorRail : Block
	{
		public ActivatorRail() : base(126)
		{
			IsSolid = false;
			IsTransparent = true;
			BlastResistance = 3.5f;
			Hardness = 0.7f;
		}
	}
}