using MiNET.Utils;

namespace MiNET.Blocks
{
	public class Air : Block
	{
		public Air() : base(0)
		{
			IsReplacible = true;
			IsSolid = false;
			IsBuildable = false;
		}

		public override ItemStack GetDrops()
		{
			return null;
		}
	}
}