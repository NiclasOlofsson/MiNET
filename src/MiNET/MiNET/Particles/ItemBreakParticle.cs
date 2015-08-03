using MiNET.Items;
using MiNET.Worlds;

namespace MiNET.Particles
{
	public class ItemBreakParticle : Particle
	{
		public ItemBreakParticle(Level level, Item item) : base(9, level)
		{
			Data = (item.Id << 16) | item.Metadata;
		}
	}
}