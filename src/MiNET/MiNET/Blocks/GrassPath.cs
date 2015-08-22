using MiNET.Utils;

namespace MiNET.Blocks
{
	public class GrassPath : Block
	{
		public GrassPath() : base(198)
		{
		}

		public override ItemStack GetDrops()
		{
			return new ItemStack(3); // Drop dirt block
		}
	}
}