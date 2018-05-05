using MiNET.Blocks;
using MiNET.Worlds;

namespace MiNET.Particles
{
	public class TerrainParticle : Particle
	{
		public TerrainParticle(Level level, Block block) : base(ParticleType.Terrain, level)
		{
            Data = (int)block.GetRuntimeId();
		}
	}
}