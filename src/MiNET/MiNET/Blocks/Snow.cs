using MiNET.Utils;

namespace MiNET.Blocks
{
	public class Snow : Block
	{
		internal Snow() : base(80)
		{
         isSolid = false;
		}

		public override ItemStack GetDrops()
		{
			return new ItemStack(332, 4); // Drop snowball
		}
	}
}