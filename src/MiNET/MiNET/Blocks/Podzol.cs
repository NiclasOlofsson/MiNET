using MiNET.Utils;

namespace MiNET.Blocks
{
	public class Podzol : Block
	{
		public Podzol() : base(243)
		{
			
		}
		
		public override ItemStack GetDrops(){
			return new ItemStack(3);
		}
	}
}