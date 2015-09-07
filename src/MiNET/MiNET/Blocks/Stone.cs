using MiNET.Utils;

namespace MiNET.Blocks
{
	public class Stone : Block
	{
		public Stone() : base(1)
		{
		}

		public override ItemStack GetDrops()
		{
			return new ItemStack(4); // Drop cobblestone
		}
	}
}