using MiNET.Items;
using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class Villager : PassiveMob
	{
		public Villager(Level level) : base(EntityType.Npc, level)
		{
			Width = Length = 0.6;
			Height = 1.8;
		}
	}
}