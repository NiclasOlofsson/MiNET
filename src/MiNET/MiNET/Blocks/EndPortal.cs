namespace MiNET.Blocks
{
	public class EndPortal : Block
	{
		public EndPortal() : base(119)
		{
			IsSolid = false;
			BlastResistance = 18000000;
			Hardness = -1;
			LightLevel = 15;
			IsTransparent = true;
		}
	}
}
