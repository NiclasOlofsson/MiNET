namespace MiNET.Blocks
{
	public class Sandstone : Block
	{
		public Sandstone() : this(24)
		{
			
		}

		public Sandstone(byte id) : base(id)
		{
			BlastResistance = 4;
			Hardness = 0.8f;
		}
	}
}