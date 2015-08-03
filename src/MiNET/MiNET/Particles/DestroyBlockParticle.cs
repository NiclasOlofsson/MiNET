using MiNET.Blocks;
using MiNET.Net;
using MiNET.Worlds;

namespace MiNET.Particles
{
	public class DestroyBlockParticle : Particle
	{
		public DestroyBlockParticle(Level level, Block block) : base(0, level)
		{
			Data = block.Id + (block.Metadata << 12);
		}

		public override void Spawn()
		{
			McpeLevelEvent particleEvent = McpeLevelEvent.CreateObject();
			particleEvent.eventId = 2001;
			particleEvent.x = (float) Position.X;
			particleEvent.y = (float) Position.Y;
			particleEvent.z = (float) Position.Z;
			particleEvent.data = Data;
			Level.RelayBroadcast(particleEvent);
		}
	}
}