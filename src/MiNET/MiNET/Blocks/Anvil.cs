namespace MiNET.Blocks
{
	public class Anvil : Block
	{
		public Anvil() : base(145)
		{
			IsTransparent = true;
			BlastResistance = 6000;
			Hardness = 5;
		}
	}
}