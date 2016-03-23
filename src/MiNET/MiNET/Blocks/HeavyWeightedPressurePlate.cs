namespace MiNET.Blocks
{
	public class HeavyWeightedPressurePlate : Block
	{
		public HeavyWeightedPressurePlate() : base(148)
		{
			IsSolid = false;
			IsTransparent = true;
			BlastResistance = 2.5f;
			Hardness = 0.5f;
		}
	}
}