namespace MiNET.Blocks
{
	public class CobblestoneWall : Block
	{
		public CobblestoneWall() : base(139)
		{
			IsTransparent = true;
			BlastResistance = 30;
			Hardness = 2;
		}
	}
}