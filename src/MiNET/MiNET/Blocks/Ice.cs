using MiNET.Utils;

namespace MiNET.Blocks
{
	public class Ice : Block
	{
		public Ice() : base(79)
		{
			IsSolid = false;
		}
		
		public override ItemStack GetDrops(){
			return null; //Drop nothing
		}
	}
}