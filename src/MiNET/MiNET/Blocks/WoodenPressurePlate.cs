namespace MiNET.Blocks
{
	public class WoodenPressurePlate : Block
	{
		public WoodenPressurePlate() : base(72)
		{
			IsTransparent = true;
			IsSolid = false;
			BlastResistance = 2.5f;
			Hardness = 0.5f;
		}
	}
}