namespace MiNET.Blocks
{
	public class LightWeightedPressurePlate : Block
	{
		public LightWeightedPressurePlate() : base(147)
		{
			IsSolid = false;
			IsTransparent = true;
			BlastResistance = 2.5f;
			Hardness = 0.5f;
		}
	}
}