using MiNET.Utils;

namespace MiNET.Blocks
{
	public class MonsterSpawner : Block
	{
		public MonsterSpawner() : base (52)
		{
			isSolid = false;
		}
		
		public override ItemStack getDrops(){
			return null; //Drop nothing
		}
	}
	
}