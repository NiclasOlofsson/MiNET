using MiNET.Blocks;
using MiNET.Worlds;

namespace MiNET.Particles
{
	public class TerrainParticle : Particle
	{
		public TerrainParticle(Level level, Block block) : base(ParticleType.Terrain, level)
		{
<<<<<<< HEAD
            Data = (int)block.GetRuntimeId();
=======
			Data = (int)block.GetRuntimeId();
>>>>>>> 86f35b43910890e118cedd4a207ba5d5e79c1298
		}
	}
}