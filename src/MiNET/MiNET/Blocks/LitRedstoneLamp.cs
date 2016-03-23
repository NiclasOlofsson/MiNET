using MiNET.Items;

namespace MiNET.Blocks
{
	public class LitRedstoneLamp : RedstoneLamp
	{
		public LitRedstoneLamp() : base(122)
		{
			LightLevel = 15;
		}
	}
}