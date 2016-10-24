using MiNET.Items;

namespace MiNET.Blocks
{
	public class LitRedstoneLamp : RedstoneLamp
	{
		public LitRedstoneLamp() : base(124)
		{
			LightLevel = 15;
		}
	}
}