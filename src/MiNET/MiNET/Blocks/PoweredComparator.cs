

namespace MiNET.Blocks
{
	public class PoweredComparator : UnpoweredComparator
	{
		public PoweredComparator() : base(150)
		{
			LightLevel = 7;
		}
	}
}