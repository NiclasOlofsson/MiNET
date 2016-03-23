namespace MiNET.Blocks
{
	public class WoodSlab : Block
	{
		public WoodSlab() : base(158)
		{
			BlastResistance = 15;
			Hardness = 2;
			IsFlammable = true;
		}
	}
}