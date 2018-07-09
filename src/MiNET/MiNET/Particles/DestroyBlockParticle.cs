using MiNET.Blocks;
using MiNET.Net;
using MiNET.Worlds;

namespace MiNET.Particles
{
	public class DestroyBlockParticle : Particle
	{
		public DestroyBlockParticle(Level level, Block block) : base(0, level)
		{
<<<<<<< HEAD
            Data = (int)block.GetRuntimeId();
=======
			Data = (int)block.GetRuntimeId();
>>>>>>> 86f35b43910890e118cedd4a207ba5d5e79c1298
			Position = block.Coordinates;
		}

		public override void Spawn()
		{
			McpeLevelEvent particleEvent = McpeLevelEvent.CreateObject();
			particleEvent.eventId = 2001;
			particleEvent.position = Position;
			particleEvent.data = Data;
			Level.RelayBroadcast(particleEvent);
		}
	}
}