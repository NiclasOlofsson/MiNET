using MiNET.Worlds;

namespace MiNET.Particles
{
	public class HeartParticle : Particle
	{
		public HeartParticle(Level level, int scale = 0) : base(14, level)
		{
			Data = scale;
		}
	}
}