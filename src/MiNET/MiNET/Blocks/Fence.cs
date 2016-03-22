namespace MiNET.Blocks
{
	public class Fence : Block
	{
		public Fence() : this(85)
		{
		}

		public Fence(byte id) : base(id)
		{
			FuelEfficiency = 15;
			IsTransparent = true;
			BlastResistance = 15;
			Hardness = 2;
			IsFlammable = true;
		}
	}
}
