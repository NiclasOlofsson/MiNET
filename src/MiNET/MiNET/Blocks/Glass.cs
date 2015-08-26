using MiNET.Utils;

namespace MiNET.Blocks
{
	public class Glass : Block
	{
		public Glass() : base(20)
		{
			IsSolid = false;
		}
		
		public override ItemStack GetDrops(){
			return null;
		}
	}
}