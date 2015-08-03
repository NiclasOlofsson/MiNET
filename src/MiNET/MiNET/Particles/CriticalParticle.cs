using MiNET.Worlds;

namespace MiNET.Particles
{
	public class CriticalParticle : Particle
	{
		public CriticalParticle(Level level, int scale = 2) : base(2, level)
		{
			Data = scale;
		}
	}
}