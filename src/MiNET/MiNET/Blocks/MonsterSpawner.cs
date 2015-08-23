using MiNET.Utils;

namespace MiNET.Blocks
{
	public class MonsterSpawner : Block
	{
		public MonsterSpawner() : base (52)
		{
			IsSolid = false;
		}
		
		public override ItemStack GetDrops(){
			return null; //Drop nothing
		}
	}
	
}