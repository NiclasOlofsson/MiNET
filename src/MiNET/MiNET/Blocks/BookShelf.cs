namespace MiNET.Blocks
{
	public class Bookshelf : Block
	{
		public Bookshelf() : base(47)
		{
			FuelEfficiency = 15;
			BlastResistance = 7.5f;
			Hardness = 1.5f;
			IsFlammable = true;
		}
	}
}
