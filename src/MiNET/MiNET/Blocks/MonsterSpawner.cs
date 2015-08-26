using MiNET.Utils;

namespace MiNET.Blocks
{
	public class MonsterSpawner : Block
	{
		public MonsterSpawner() : base(52)
		{
			
		}
		
		public override ItemStack GetDrops(){
			return null;
		}
	}
}