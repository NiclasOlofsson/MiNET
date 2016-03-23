namespace MiNET.Blocks
{
	public class StonePressurePlate : Block
	{
		public StonePressurePlate() : base(70)
		{
			IsTransparent = true;
			IsSolid = false;
			BlastResistance = 2.5f;
			Hardness = 0.5f;
		}
	}
}