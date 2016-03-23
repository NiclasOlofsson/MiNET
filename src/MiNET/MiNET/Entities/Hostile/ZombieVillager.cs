using MiNET.Worlds;
using MiNET.Items;

namespace MiNET.Entities.Hostile
{
	public class ZombieVillager : HostileMob
	{
		public ZombieVillager(Level level) : base((int) EntityType.ZombieVillager, level)
		{
			Width = Length = 0.6;
			Height = 1.8;
		}
	}
}