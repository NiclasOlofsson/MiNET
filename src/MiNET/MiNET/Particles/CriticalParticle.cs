using MiNET.Worlds;

namespace MiNET.Particles
{
	public class CriticalParticle : Particle
	{
		public CriticalParticle(Level level, int scale = 2) : base(ParticleType.Critical, level)
		{
			Data = scale;
		}
	}
}