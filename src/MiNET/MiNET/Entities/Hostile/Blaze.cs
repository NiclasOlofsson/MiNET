using MiNET.Worlds;
using MiNET.Items;

namespace MiNET.Entities.Hostile
{
	public class Blaze : HostileMob
	{
		public Blaze(Level level) : base(EntityType.Blaze, level)
		{
			Width = Length = 0.6;
			Height = 1.8;
		}
	}
}