namespace MiNET.Blocks
{
	public class NetherBrickFence : Fence
	{
		public NetherBrickFence() : base(113)
		{
			IsFlammable = false; // Overrides Wooden Fence, so make sure its not flammable.
			BlastResistance = 30;
			Hardness = 2;
		}
	}
}