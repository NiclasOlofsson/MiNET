using MiNET.Utils;

namespace MiNET.Blocks
{
	public class LitFurnace : Furnace
	{
		public LitFurnace() : base(62)
		{
		}
		
		public override ItemStack GetDrops(){
			return new ItemStack(61);
		}
	}
}