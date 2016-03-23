using MiNET.Worlds;
using MiNET.Items;

namespace MiNET.Entities.Hostile
{
	public class CaveSpider : HostileMob
	{
		public CaveSpider(Level level) : base((int) EntityType.CaveSpider, level)
		{
			Width = Length = 0.7;
			Height = 0.5;
		}
	}
}