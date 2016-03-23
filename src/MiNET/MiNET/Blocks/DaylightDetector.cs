namespace MiNET.Blocks
{
	public class DaylightDetector : Block
	{
		public DaylightDetector() : base(151)
		{
			IsTransparent = true;
			BlastResistance = 1;
			Hardness = 0.2f;
		}
	}
}