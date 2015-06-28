using MiNET.Utils;

namespace MiNET.Blocks
{
	internal class GrassPath : Block
	{
		internal GrassPath() : base(198)
		{
		}

		public override ItemStack GetDrops()
		{
			return new ItemStack(3); // Drop dirt block
		}
	}
}