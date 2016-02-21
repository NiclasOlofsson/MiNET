using System.Drawing;
using MiNET.Worlds;

namespace MiNET.Particles
{
	public class DustParticle : Particle
	{
		public DustParticle(Level level, Color color) : base(ParticleType.Dust, level)
		{
			byte r = color.R;
			byte g = color.G;
			byte b = color.B;
			byte a = color.A;

			Data = ((a & 0xff) << 24) | ((r & 0xff) << 16) | ((g & 0xff) << 8) | (b & 0xff);
		}
	}
}