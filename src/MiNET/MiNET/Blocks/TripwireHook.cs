using MiNET.Items;

namespace MiNET.Blocks
{
	public class TripwireHook : Block
	{
		public TripwireHook() : base(131)
		{
			IsSolid = false;
			IsTransparent = true;
		}
	}
}