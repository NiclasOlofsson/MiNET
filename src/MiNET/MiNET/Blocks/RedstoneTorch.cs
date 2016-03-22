using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class RedstoneTorch : UnlitRedstoneTorch
	{
		public RedstoneTorch() : base(76)
		{
			LightLevel = 7;
		}
	}
}