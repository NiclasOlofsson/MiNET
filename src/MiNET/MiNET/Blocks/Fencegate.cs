namespace MiNET.Blocks
{
	public class FenceGate : Block
	{
		public FenceGate() : this(107)
		{
		}

		public FenceGate(byte id) : base(id)
		{
			FuelEfficiency = 15;
			IsTransparent = true;
			BlastResistance = 15;
			Hardness = 2;
			IsFlammable = true;
		}
	}
}
