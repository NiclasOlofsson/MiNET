namespace MiNET.Blocks
{
	public class Sapling : Block
	{
		public Sapling() : base(6)
		{
			FuelEfficiency = 5;
			IsTransparent = true;
			IsFlammable = true;
		}
	}
}
