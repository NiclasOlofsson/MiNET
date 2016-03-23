using MiNET.Worlds;
using MiNET.Items;

namespace MiNET.Entities.Hostile
{
	public class Creeper : HostileMob
	{
		public Creeper(Level level) : base(EntityType.Creeper, level)
		{
			Width = Length = 0.6;
			Height = 1.8;
		}
	}
}