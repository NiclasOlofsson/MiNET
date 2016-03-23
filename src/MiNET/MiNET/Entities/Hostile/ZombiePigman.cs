using MiNET.Worlds;
using MiNET.Items;

namespace MiNET.Entities.Hostile
{
	public class ZombiePigman : HostileMob
	{
		public ZombiePigman(Level level) : base((int) EntityType.ZombiePigman, level)
		{
			Width = Length = 0.6;
			Height = 1.8;
		}
	}
}