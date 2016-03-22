using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class RedstoneTorchOff : Block
	{
		public RedstoneTorchOff() : base(75)
		{
			IsTransparent = true;
			IsSolid = false;
			Hardness = 0;
		}
	}
}