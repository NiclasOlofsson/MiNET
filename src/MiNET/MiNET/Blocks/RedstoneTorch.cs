using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class RedstoneTorch : UnlitRedstoneTorch
	{
		public RedstoneTorch() : base(76)
		{
			LightLevel = 7;
            Power = 15;
            IsConductive = true;
        }
	}
}