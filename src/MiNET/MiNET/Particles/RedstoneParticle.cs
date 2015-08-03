using MiNET.Worlds;

namespace MiNET.Particles
{
	public class RedstoneParticle : Particle
	{
		public RedstoneParticle(Level level, int lifetime = 1) : base(8, level)
		{
			Data = lifetime;
		}
	}
}