using MiNET.Utils;

namespace MiNET.Blocks
{
	public class Snow : Block
	{
		public Snow() : base(80)
		{
		}

		public override ItemStack GetDrops()
		{
			return new ItemStack(332, 4); // Drop snowball
		}
	}
}