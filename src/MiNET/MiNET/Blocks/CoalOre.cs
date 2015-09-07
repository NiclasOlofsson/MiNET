using MiNET.Utils;

namespace MiNET.Blocks
{
	public class CoalOre : Block
	{
		public CoalOre() : base(16)
		{
		}

		public override ItemStack GetDrops()
		{
			return new ItemStack(263, 1);
		}
	}
}