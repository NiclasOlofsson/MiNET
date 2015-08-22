using MiNET.Utils;

namespace MiNET.Blocks
{
	public class Ice : Block
	{
		public Ice() : base(79)
		{
			isSolid = false;
		}
		
		public override ItemStack getDrops(){
			return null; //Drop nothing
		}
	}
}