using MiNET.Net;
using MiNET.Worlds;

namespace MiNET.Particles
{
	public class SpawnParticle : Particle
	{
		public SpawnParticle(Level level, int height, int width) : base(0, level)
		{
			Data = (width & 0xff) + ((height & 0xff) << 8);
		}

		public override void Spawn()
		{
			McpeLevelEvent particleEvent = McpeLevelEvent.CreateObject();
			particleEvent.eventId = 2004;
			particleEvent.x = (float) Position.X;
			particleEvent.y = (float) Position.Y;
			particleEvent.z = (float) Position.Z;
			particleEvent.data = Data;
			Level.RelayBroadcast(particleEvent);
		}
	}
}