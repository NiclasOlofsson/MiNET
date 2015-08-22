using MiNET.Utils;

namespace MiNET.Blocks
{
	public class Grass : Block
	{
		public Grass() : base(2)
		{
		}

     
		public override ItemStack GetDrops()
		{
			return new ItemStack(3); //Drop dirt block
		}
	}
}