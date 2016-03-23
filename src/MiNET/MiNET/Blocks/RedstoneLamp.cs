namespace MiNET.Blocks
{
	public class RedstoneLamp : Block
	{
		public RedstoneLamp() : this(123)
		{
			
		}

		public RedstoneLamp(byte id) : base(id)
		{
			BlastResistance = 1.5f;
			Hardness = 0.3f;
		}
	}
}