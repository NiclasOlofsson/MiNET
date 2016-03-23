namespace MiNET.Blocks
{
	public class FlowingLava : Flowing
	{
		public FlowingLava() : base(10)
		{
			LightLevel = 15;
			BlastResistance = 500;
			Hardness = 100;
		}
	}
}